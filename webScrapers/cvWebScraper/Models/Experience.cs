using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cvWebScraper.Models
{
    public class Experience
    {
        [JsonPropertyName("years")]
        public string Years { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
