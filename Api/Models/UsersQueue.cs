namespace SpotifyEditor.Api.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using SpotifyEditor.Api.Models.Common;
    public partial class UsersQueue
    {
        [JsonProperty("currently_playing")]
        public CurrentlyPlaying CurrentlyPlaying { get; set; }

        [JsonProperty("queue")]
        public List<CurrentlyPlaying> Queue { get; set; }
    }



    public partial class UsersQueue
    {
        public static UsersQueue FromJson(string json) => JsonConvert.DeserializeObject<UsersQueue>(json, SpotifyEditor.Api.Models.Converter.Settings);
    }


}
