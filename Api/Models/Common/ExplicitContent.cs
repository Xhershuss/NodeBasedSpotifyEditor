using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models.Common
{
    public partial class ExplicitContent
    {
        [JsonProperty("filter_enabled")]
        public bool FilterEnabled { get; set; }

        [JsonProperty("filter_locked")]
        public bool FilterLocked { get; set; }
    }

}
