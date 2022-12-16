using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport.Application.Contract.Dtos
{
    public class GitHubUser
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "login")]
        public string? Login { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "company")]
        public string? Company { get; set; }
        
        [JsonProperty(PropertyName = "followers")]
        public int? NumberOfFollowers { get; set; }

        [JsonProperty(PropertyName = "public_repos")]
        public int? NumberOfPublicRepositories { get; set; }

    }
}
