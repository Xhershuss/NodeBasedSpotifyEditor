using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models.Common
{
    public partial class Restrictions
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
