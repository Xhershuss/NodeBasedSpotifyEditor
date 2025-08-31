namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;
    using SpotifyEditor.Api.Models;

    public partial class UsersPlaylistsResponse
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public object Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
        public List<Playlist> Items { get; set; }
    }



  
 

    public enum Description { Despspspps, Empty };

    public enum OwnerType { User };

    public enum ItemType { Playlist };
}
