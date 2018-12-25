using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SubQuip.Entity.Models.Graph
{
	public class Group
	{      
		[JsonProperty("id")]
		public string Id { get; set; }


		[JsonProperty("description")]
		public string Description { get; set; }



		[JsonProperty("displayName")]
		public string DisplayName { get; set; }


		[JsonProperty("securityEnabled")]
		public string SecurityEnabled { get; set; }      
	}

	public class Groups
    {
        [JsonProperty("value")]
		public List<Group> Value { get; set; }
    }
}
