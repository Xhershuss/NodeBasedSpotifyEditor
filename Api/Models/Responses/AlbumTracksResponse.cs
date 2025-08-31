namespace SpotifyEditor.Api.Models.Responses
{
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    public partial class AlbumTracksResponse
    {
        public Uri Href { get; set; }
        public List<Track> Items { get; set; }
        public long Limit { get; set; }
        public object Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
    }

}
