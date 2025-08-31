using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models.Common
{
    public partial class ExternalIds
    {
        [JsonProperty("isrc")]
        public string Isrc { get; set; }

        [JsonProperty("ean")]
        public string Ean { get; set; }

        [JsonProperty("upc")]
        public string Upc { get; set; }
    }

}
