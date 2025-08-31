namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;

    public partial class CurrentlyPlayingTrackResponse
    {
        public Device Device { get; set; }
        public string RepeatState { get; set; }
        public bool ShuffleState { get; set; }
        public Context Context { get; set; }
        public long Timestamp { get; set; }
        public long ProgressMs { get; set; }
        public bool IsPlaying { get; set; }
        public Track Item { get; set; }
        public string CurrentlyPlayingType { get; set; }
        public Actions Actions { get; set; }
    }

    public partial class Actions
    {
        public bool InterruptingPlayback { get; set; }
        public bool Pausing { get; set; }
        public bool Resuming { get; set; }
        public bool Seeking { get; set; }
        public bool SkippingNext { get; set; }
        public bool SkippingPrev { get; set; }
        public bool TogglingRepeatContext { get; set; }
        public bool TogglingShuffle { get; set; }
        public bool TogglingRepeatTrack { get; set; }
        public bool TransferringPlayback { get; set; }
    }

  
 


}
