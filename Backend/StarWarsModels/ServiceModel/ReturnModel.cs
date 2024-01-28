
using Newtonsoft.Json;

namespace StarWars.Models.ServiceModel
{
    public class ReturnModel
    {
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("edited")]
        public DateTime Edited { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
