namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;

    public partial class UsersSavedTracksResponse
    {
        public Uri Href { get; set; }
        public List<TracksWithDate> Items { get; set; }
        public long Limit { get; set; }
        public object Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
    }

    

}
