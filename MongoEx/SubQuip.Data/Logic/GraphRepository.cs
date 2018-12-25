using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models.Graph;

namespace SubQuip.Data.Logic
{

    public class GraphRepository : IGraphRepository
    {
        protected readonly HttpClient Client = new HttpClient();
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        protected AuthenticationContext _authContext;

        /// <summary>
        /// Initializes a new instance of the GraphRepository
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public GraphRepository(IConfiguration configuration, ILogger<GraphRepository> logger)
        {
            _config = configuration;
            _authContext = new AuthenticationContext(Authority());
            _logger = logger;
        }

        #region Azure AD

        /// <summary>
        /// Get External User Details.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Users> FindExternalUser(string email)
        {
            var authenticationResult = GetAccessToken().GetAwaiter().GetResult();
            var requestUrl = $"https://graph.microsoft.com/v1.0/users?$filter=mail%20eq%20'{email}'";

            var users = await GetGraphObject<Users>(authenticationResult, requestUrl);

            var user = users.Value.First();
            var groupsUrl = $"https://graph.microsoft.com/v1.0/users/{user.Id}/memberOf";
            var groups = await GetGraphObject<Groups>(authenticationResult, groupsUrl);

            user.IsAdmin = groups.Value.Any(g => g.DisplayName == "SubquipAdmin");

            user.Company = groups.Value.FirstOrDefault(g => (g.DisplayName ?? "").StartsWith("Partners"))?.DisplayName.Substring("Partners".Length);

            return users;
        }

        public async Task<User> RequestUserDetails(string email)
        {
            var authenticationResult = GetAccessToken().GetAwaiter().GetResult();
            var requestUrl = $"https://graph.microsoft.com/v1.0/users?$filter=mail%20eq%20'{email}'";

            var users = await GetGraphObject<Users>(authenticationResult, requestUrl);
            User user = null;
            if (users.Value.Any())
            {
                user = users.Value.First();
                var groupsUrl = $"https://graph.microsoft.com/v1.0/users/{user.Id}/memberOf";
                var groups = await GetGraphObject<Groups>(authenticationResult, groupsUrl);

                user.IsAdmin = groups.Value.Any(g => g.DisplayName == "SubquipAdmin");

                user.Company = groups.Value.FirstOrDefault(g => (g.DisplayName ?? "").StartsWith("Partners"))?.DisplayName.Substring("Partners".Length);
            }
            return user;
        }

        /// <summary>
        /// Get list of application users
        /// </summary>
        /// <returns></returns>
        public async Task<Users> ApplicationUsers()
        {
            var authenticationResult = GetAccessToken().GetAwaiter().GetResult();
            var requestUrl = $"https://graph.microsoft.com/v1.0/users";

            var users = await GetGraphObject<Users>(authenticationResult, requestUrl);
            return users;
        }

        #endregion

        protected async Task<AuthenticationResult> GetAccessToken(string resourceString)
        {
            AuthenticationResult result = null;
            var retryCount = 0;
            bool retry;

            do
            {
                retry = false;

                try
                {   // ADAL includes an in-memory cache, so this call will only send a message to the server if the cached token is expired.
                    var cred = new ClientCredential(ClientId(), ClientSecret());
                    var currentAuthority = _authContext.Authority;
                    var configAuthority = Authority();
                    if (!currentAuthority.TrimEnd('/').Equals(configAuthority)) // bug? 
                    {
                        _authContext = new AuthenticationContext(Authority());
                    }
                    result = await _authContext.AcquireTokenAsync(resourceString, cred);
                }
                catch (AdalException ex)
                {
                    if (ex.ErrorCode == "temporarily_unavailable")
                    {
                        retry = true;
                        retryCount++;
                        Thread.Sleep(3000);
                    }

                    _logger.LogError(
                        string.Format("An error occurred while acquiring a token\nTime: {0}\nError: {1}\nRetry: {2}\n",
                            DateTime.Now.ToString(CultureInfo.InvariantCulture),
                            ex.ToString(),
                            retry.ToString()));
                }

            } while (retry && retryCount < 3);
            return result;
        }

        public async Task<AuthenticationResult> GetAccessToken()
        {
            return await GetAccessToken(GraphUrl());
        }

        protected async Task<T> GetGraphObject<T>(AuthenticationResult result, string request)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            var req = new HttpRequestMessage(HttpMethod.Get, request);
            req.Headers.Add("Accept", AcceptHeader());
            using (var res = await Client.SendAsync(req))
            {
                _logger.LogDebug("GetGraphObject<T>() -> await client");
                using (var content = res.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        var resultObject = JsonConvert.DeserializeObject<T>(data);
                        return resultObject;
                    }

                    _logger.LogError("Error in getting GetGraphObject from the graph api");
                    throw new Exception("Error in getting GetGraphObject from the graph api");
                }
            }
        }


        protected string GraphUrl()
        {
            return "https://graph.microsoft.com";
        }

        protected string ClientId()
        {
            return _config["AzureAD:Audience"]; ;
        }
        protected string ClientSecret()
        {
            return _config["AzureAD:ClientSecret"];
        }
        protected string Authority()
        {
            return string.Format(_config["AzureAd:AadInstance"], _config["AzureAd:Tenant"]);
        }

        protected string AcceptHeader()
        {
            return "application/json;odata.metadata=full;odata.streaming=false";
        }

    }
}