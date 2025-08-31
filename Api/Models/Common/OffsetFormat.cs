using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models.Common
{
    public class OffsetFormat
    {

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public int? Position { get; set; }

        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        public string? Uri { get; set; }

        public OffsetFormat(int position)
        {
            Position = position;
            Uri = null;
        }

        public OffsetFormat(string uri)
        {
            Uri = uri;
            Position = null;
        }


    }
}
