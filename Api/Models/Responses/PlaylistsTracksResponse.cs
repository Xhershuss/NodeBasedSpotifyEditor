namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;
    using SpotifyEditor.Api.Models;

    public partial class PlaylistsTracksResponse
    {
        public Uri Href { get; set; }
        public List<DetailedTrack> Items { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public Uri Previous { get; set; }
        public long Total { get; set; }
    }


    public partial class AddedBy
    {
        public ExternalUrls ExternalUrls { get; set; }
        public Uri Href { get; set; }
        public string Id { get; set; }
        public AddedByType Type { get; set; }
        public string Uri { get; set; }
        public string Name { get; set; }
    }

    public enum AddedByType { Artist, User };

  

  
}
