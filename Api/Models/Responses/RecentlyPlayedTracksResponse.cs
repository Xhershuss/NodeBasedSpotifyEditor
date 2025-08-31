namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;

    public partial class RecentlyPlayedTracksResponse
    {
        public List<Item> Items { get; set; }
        public Uri Next { get; set; }
        public Cursors Cursors { get; set; }
        public long Limit { get; set; }
        public Uri Href { get; set; }
    }

    public partial class Cursors
    {
        public string After { get; set; }
        public string Before { get; set; }
    }

    public partial class Item
    {
        public Track Track { get; set; }
        public DateTimeOffset played_at { get; set; }
        public Context Context { get; set; }
    }

    
}
