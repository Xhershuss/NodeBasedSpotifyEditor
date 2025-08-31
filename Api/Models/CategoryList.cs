namespace SpotifyEditor.Api.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CategoryList
    {
        [JsonProperty("categories")]
        public Categories Categories { get; set; }
    }

    public partial class Categories
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
        public List<Category> Items { get; set; }
    }

    public partial class CategoryList
    {
        public static CategoryList FromJson(string json) => JsonConvert.DeserializeObject<CategoryList>(json, SpotifyEditor.Api.Models.Converter.Settings);
    }

}
