namespace SpotifyEditor.Api.Models
{
    using SpotifyEditor.Api.Models.Common;    
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using SpotifyEditor.Api.Models.Common;

    public partial class Playlist
    {
        [JsonProperty("collaborative")]
        public bool Collaborative { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("snapshot_id")]
        public string SnapshotId { get; set; }

        [JsonProperty("tracks")]
        public PlaylistTracks tracks{ get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    
   public partial class DetailedTrack 
    {
        [JsonProperty("added_at")]
        public string AddedAt { get; set; }

        [JsonProperty("added_by")]
        public Owner AddedBy { get; set; }

        [JsonProperty("is_local")]
        public bool IsLocal { get; set; }

        [JsonProperty("track")]
        public Track Track { get; set; }
        public object PrimaryColor { get; set; }
        public VideoThumbnail VideoThumbnail { get; set; }
    }
    

    public partial class PlaylistTracks
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
        public List<DetailedTrack> Items { get; set; }
    }



    public partial class Playlist
    {
        public static Playlist FromJson(string json) => JsonConvert.DeserializeObject<Playlist>(json, SpotifyEditor.Api.Models.Converter.Settings);
    }
}
