using System;
namespace SubQuip.Entity.Models.Graph
{
	using System;
	using System.Collections.Generic;
	using System.Dynamic;
	using System.Reflection;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

	public class Users
	{
		[JsonProperty("value")]
		public List<User> Value { get; set; }
        
	}

	public class User
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }
		[JsonProperty("givenName")]
		public string GivenName { get; set; }
		[JsonProperty("mail")]
		public string Mail { get; set; }

		[JsonProperty("surname")]
		public string Surname { get; set; }
		[JsonProperty("userPrincipalName")]
		public string UserPrincipalName { get; set; }
  
		public bool IsAdmin { get; set; }

		public string Company { get; set; }
	}

}


