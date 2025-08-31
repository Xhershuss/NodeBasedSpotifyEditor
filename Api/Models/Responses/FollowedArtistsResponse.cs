namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;
    using SpotifyEditor.Api.Models.Responses;

    public partial class FollowedArtistsResponse
    {
        public ArtistsForFollow Artists { get; set; }
    }

    public partial class ArtistsForFollow
    {
        public string Href { get; set; }
        public long Limit { get; set; }
        public string Next { get; set; }
        public Cursors Cursors { get; set; }
        public long Total { get; set; }
        public List<Artist> Items { get; set; }
    }


}