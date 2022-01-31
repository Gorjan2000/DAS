using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domasna3.Models
{
    public class Map
    {
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string city { get; set; }

        public Map(string name, double lat, double lon, string city)
        {
            this.name = name;
            this.lat = lat;
            this.lon = lon;
            this.city = city;
        }
    }
    
}