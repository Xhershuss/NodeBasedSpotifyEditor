using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models.Common
{
    public partial class TracksWithDate
    {
        [JsonProperty("added_at")]
        public DateTimeOffset AddedAt { get; set; }
        public Track Track { get; set; }
    }

}
