namespace SpotifyEditor.Api.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using SpotifyEditor.Api.Models.Common;

    public partial class Category
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("icons")]
        public List<Icon> Icons { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }


    public partial class Category
    {
        public static Category FromJson(string json) => JsonConvert.DeserializeObject<Category>(json, SpotifyEditor.Api.Models.Converter.Settings);
    }

}
