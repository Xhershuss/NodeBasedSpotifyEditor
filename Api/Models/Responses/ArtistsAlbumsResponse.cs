namespace SpotifyEditor.Api.Models.Responses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ArtistsAlbumsResponse
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("previous")]
        public Uri Previous { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("items")]
        public List<Album> Items { get; set; }
    }


   
}
