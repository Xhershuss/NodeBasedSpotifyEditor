using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyEditor.Api.Models;

namespace SpotifyEditor.Api.Models.Responses
{
    public partial class TracksResponse
    {
        public List<Track> tracks { get; set; }
    }
}
