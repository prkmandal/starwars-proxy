using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Models.ViewModel
{
    public class FilmViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public int Episode_id { get; set; }
        public string Opening_crawl { get; set; } = default!;
        public string Director { get; set; } = default!;
        public string Producer { get; set; } = default!;
        public DateTime? Rlease_date { get; set; } = default!;
        public int[] Starships { get; set; }
    }

    
}
