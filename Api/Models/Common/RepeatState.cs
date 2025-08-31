
namespace SpotifyEditor.Api.Models.Common
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RepeatState
    {
        //[EnumMember(Value = "track")]
        Track,

        //[EnumMember(Value = "context")]
        Context,

        //[EnumMember(Value = "off")]
        Off
    }

}
