using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models.Common
{
    public partial class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("is_private_session")]
        public bool IsPrivateSession { get; set; }

        [JsonProperty("is_restricted")]
        public bool IsRestricted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("volume_percent")]
        public long VolumePercent { get; set; }

        [JsonProperty("supports_volume")]
        public bool SupportsVolume { get; set; }
    }

}
