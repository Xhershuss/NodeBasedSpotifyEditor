namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models;
    using SpotifyEditor.Api.Models.Common;


    public partial class NewReleasesResponse
    {
        public Albums Albums { get; set; }
    }

    public partial class Albums
    {
        public Uri Href { get; set; }
        public List<Album> Items { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
    }

 

    public enum AlbumTypeEnum { Album, Ep };

    public enum ArtistType { Artist };

    public enum ReleaseDatePrecision { Day };
}
