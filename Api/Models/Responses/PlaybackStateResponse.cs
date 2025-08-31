namespace SpotifyEditor.Api.Models.Responses
{
    using SpotifyEditor.Api.Models;
    using SpotifyEditor.Api.Models.Common;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public partial class PlaybackStateResponse
    {
        public Device Device { get; set; }
        public bool ShuffleState { get; set; }
        public bool SmartShuffle { get; set; }
        public string RepeatState { get; set; }
        public bool IsPlaying { get; set; }
        public long Timestamp { get; set; }
        public Context Context { get; set; }
        public long ProgressMs { get; set; }
        [JsonProperty("item")]
        public Track Item { get; set; }
        public string CurrentlyPlayingType { get; set; }
        public Actions Actions { get; set; }
    }

    public partial class Actions
    {
        public Disallows Disallows { get; set; }
    }

    public partial class Disallows
    {
        public bool Resuming { get; set; }
    }

    

}
