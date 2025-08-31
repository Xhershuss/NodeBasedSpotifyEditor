namespace SpotifyEditor.Api.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using SpotifyEditor.Api.Models.Common;
    public partial class Player
    {
        [JsonProperty("device")]
        public Device Device { get; set; }

        [JsonProperty("repeat_state")]
        public string RepeatState { get; set; }

        [JsonProperty("shuffle_state")]
        public bool ShuffleState { get; set; }

        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("progress_ms")]
        public long ProgressMs { get; set; }

        [JsonProperty("is_playing")]
        public bool IsPlaying { get; set; }

        [JsonProperty("item")]
        public Track Item { get; set; }

        [JsonProperty("currently_playing_type")]
        public string CurrentlyPlayingType { get; set; }

        [JsonProperty("actions")]
        public Actions Actions { get; set; }
    }

    public partial class Actions
    {
        [JsonProperty("interrupting_playback")]
        public bool InterruptingPlayback { get; set; }

        [JsonProperty("pausing")]
        public bool Pausing { get; set; }

        [JsonProperty("resuming")]
        public bool Resuming { get; set; }

        [JsonProperty("seeking")]
        public bool Seeking { get; set; }

        [JsonProperty("skipping_next")]
        public bool SkippingNext { get; set; }

        [JsonProperty("skipping_prev")]
        public bool SkippingPrev { get; set; }

        [JsonProperty("toggling_repeat_context")]
        public bool TogglingRepeatContext { get; set; }

        [JsonProperty("toggling_shuffle")]
        public bool TogglingShuffle { get; set; }

        [JsonProperty("toggling_repeat_track")]
        public bool TogglingRepeatTrack { get; set; }

        [JsonProperty("transferring_playback")]
        public bool TransferringPlayback { get; set; }
    }

    public partial class Context
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }







    public partial class Player
    {
        public static Player FromJson(string json) => JsonConvert.DeserializeObject<Player>(json, SpotifyEditor.Api.Models.Converter.Settings);
    }

}
