namespace SpotifyEditor.Api.Models.Responses
{
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    using SpotifyEditor.Api.Models;
    public partial class AlbumsResponse
    {
        public List<Album> Albums { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum
    {
        [EnumMember(Value = "artist")]
        Artist,

        [EnumMember(Value = "track")]
        Track
    }
}
