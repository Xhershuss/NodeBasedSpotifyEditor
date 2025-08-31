using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models.Common
{
    public partial class ExternalUrls
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }
}
