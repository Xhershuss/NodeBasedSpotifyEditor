using SpotifyEditor.Api.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Models
{
    public class NowPlayingInfo
    {
        public Device Device { get; set; }
        public bool ShuffleState { get; set; }
        public bool SmartShuffle { get; set; }
        public string RepeatState { get; set; }
        public bool IsPlaying { get; set; }
        public long Timestamp { get; set; }
        public Context Context { get; set; }
        public long ProgressMs { get; set; }
        public Track Track { get; set; }
        public string CurrentlyPlayingType { get; set; }
        public bool Resuming { get; set; }
    }


}
