namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;
    using SpotifyEditor.Api.Models;

    public partial class UsersTopTracksResponse
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public Uri Previous { get; set; }
        public long Total { get; set; }
        public List<Track> Items { get; set; }
    }

  
}
