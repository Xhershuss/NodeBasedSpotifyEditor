namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models;

    public partial class UsersSavedAlbumsResponse
    {
        public Uri Href { get; set; }
        public List<DetailedAlbum> Items { get; set; }
        public long Limit { get; set; }
        public object Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
    }

    public partial class DetailedAlbum
    {
        public DateTimeOffset AddedAt { get; set; }
        public Album Album { get; set; }
    }

   
}
