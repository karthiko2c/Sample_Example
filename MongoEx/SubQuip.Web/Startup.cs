﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.DotNet.PlatformAbstractions;
using SubQuip.Data;
using SubQuip.Data.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace SubQuip.WebApi
{
    /// <summary>
    /// Web api startup
    /// </summary>
    public class Startup
    {
		private const string Swaggersecret = "fe9f6272-8a49-464c-96cf-d342bc9c1bab";
       
        private const string DefaultCorsPolicyName = "localhost";

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            //Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"secrets/appsettings.secrets.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            RuntimeEnvironment = Configuration.GetSection("Environment")["ASPNETCore_Global"];
            DefaultThreshold = Configuration.GetSection("DefaultThreshold")["DefaultThreshold"];
        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; }
        public static string RuntimeEnvironment { get; set; }
        public static string DefaultThreshold { get; set; }
        public static bool UseMocks { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
			services.AddSingleton(Configuration);
           // services.AddDbContext<RSContext>(option => option.UseSqlServer(Configuration.GetConnectionString("RSConnection")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Add all Transient dependencies
            services = BuildUnityContainer.RegisterAddTransient(services);

            //Configure CORS for angular2 UI
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, p =>
                {
                    //todo: Get from confiuration
                    p.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "SubQuip API v1.0", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                // add documentation to Swagger api
                var basePath = ApplicationEnvironment.ApplicationBasePath.ToLower();
                c.IncludeXmlComments(Path.Combine(basePath, "subquip.webapi.xml"));

                // Filter for file upload control to show on swagger ui
                c.OperationFilter<FileUploadOperation>();
            });
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
			{
				if (Configuration["AuthMethod"] == "ad")
				{
					options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = string.Format(Configuration["AzureAd:Issuer"], Configuration["AzureAd:Audience"]),
                        ValidAudience = Configuration["AzureAD:Audience"],
						ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                                            
                    };
                    options.Authority = string.Format(Configuration["AzureAd:AadInstance"], Configuration["AzureAd:Tenant"]);
                    options.Audience = Configuration["AzureAD:Audience"];
				}
				else if (Configuration["AuthMethod"] == "local")
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateAudience = false,
						//ValidAudience = "the audience you want to validate",
						ValidateIssuer = false,
						//ValidIssuer = "the isser you want to validate",

						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SigningKey"])),

						ValidateLifetime = true, //validate the expiration and not before values in the token
						ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
					};
				} else {
					throw new Exception("unknown authentication method: " + Configuration["AuthMethod"]);
				}
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(DefaultCorsPolicyName); //Enable CORS!
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //temporary simple authenticaion scheme to access swagger
            //TODO: remove swagger for non-local builds
            app.Use( async (context, next) => {

				if (context.Request.Path.StartsWithSegments("/api/swagger"))
                {   
                    var ok = true;
                    var authHeader = context.Request.Headers["Authorization"].ToString();
                    if (authHeader != null 
                        && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase)
                        && !(context.Request.Cookies["SWAGGERAUTH"] != null
					         && IsValid(context.Request.Cookies["SWAGGERAUTH"]))
					   )
                    {
                        var token = authHeader.Substring("Basic ".Length).Trim();
                        var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                        var credentials = credentialstring.Split(':');
                        if(credentials[0] == "swg" && credentials[1] == "JMM?km=9XM!#,Lt!")
                        {
							var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
							var hash = "";
							using(var sha256 = System.Security.Cryptography.SHA256.Create())  
                            {  
								var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(now+Swaggersecret));  
                                hash = BitConverter.ToString(hashedBytes);  
                            } 
							string cookie = now+"|"+hash;
                            CookieOptions option = new CookieOptions();
                            option.Expires = DateTime.Now.AddMinutes(15);
							context.Response.Cookies.Append("SWAGGERAUTH", cookie, option);
                            context.Response.Headers["Location"] = "/api/swagger";
                            context.Response.StatusCode = 302;
                            return;
                        }
                        else 
                        {
							ok = false;
                        }
                    } else
                    {
						ok = IsValid(context.Request.Cookies["SWAGGERAUTH"]);
                    }

                    if(!ok)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
                        return;
                    }
                }
                await next.Invoke();
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c  => {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1.0/swagger.json", "Versioned API v1.0");
                c.RoutePrefix = "api/swagger";
                c.DocExpansion("none");
            });
            
            app.UseAuthentication();
            app.UseMvc();
            app.UseDefaultFiles();
			app.UseStaticFiles();
        }


		private bool IsValid(string cookie){
			if (cookie == null || !cookie.Contains("|")) return false;
			bool retval = true;
			cookie = Uri.UnescapeDataString(cookie);
			var timestamp = cookie.Split("|")[0];
            var hash = cookie.Split("|")[1];

			var control = "";
			using(var sha256 = System.Security.Cryptography.SHA256.Create())  
            {  
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(timestamp+Swaggersecret));  
				control = BitConverter.ToString(hashedBytes);  
            }
            

			var cookietime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp));

			if (DateTimeOffset.UtcNow > cookietime.AddMinutes(15))
			{
				retval = false;
			}

			if(hash != control)
			{
				retval = false;
			}

			return retval;
		}
    }
}
