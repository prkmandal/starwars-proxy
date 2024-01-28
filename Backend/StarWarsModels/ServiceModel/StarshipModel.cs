using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Models.ServiceModel
{
    public class StarshipModel : ReturnModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = default!;
        [JsonProperty("model")]
        public string Model { get; set; } = default!;
        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; } = default!;
        [JsonProperty("cost_in_credits")]
        public string Cost_in_credits { get; set; } = default!;
        [JsonProperty("length")]
        public string Length { get; set; } = default!;
        [JsonProperty("max_atmosphering_speed")]
        public string Max_atmosphering_speed { get; set; } = default!;
        [JsonProperty("crew")]
        public string Crew { get; set; } = default!;
        [JsonProperty("passengers")]
        public string Passengers { get; set; } = default!;
        public string Cargo_capacity { get; set; } = default!;
        [JsonProperty("consumables")]
        public string Consumables { get; set; } = default!;
        public string Hyperdrive_rating { get; set; } = default!;
        public string MGLT { get; set; } = default!;
        public string Starship_class { get; set; } = default!;
        public List<string> Pilots { get; set; }
        [JsonProperty("films")]
        public List<string> Films { get; set; }
        //public DateTime Created { get ; set ; }
        //public DateTime Edited { get ; set ; }
        //public string Url { get ; set ; }
    }
}
