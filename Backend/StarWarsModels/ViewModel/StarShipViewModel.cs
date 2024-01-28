using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Models.ViewModel
{
    public class StarShipViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Model { get; set; }
        public string Manufacturer { get; set; } = default!;
        public string Consumables { get; set; } = default!;
        public int[] Films { get; set; } = default!; 

    }
}
