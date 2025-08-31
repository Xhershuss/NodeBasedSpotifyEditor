namespace SpotifyEditor.Api.Models.Responses
{
    using System.Collections.Generic;
    using SpotifyEditor.Api.Models.Common;

    public partial class UsersQueueResponse
    {
        public Track CurrentlyPlaying { get; set; }
        public List<Track> Queue { get; set; }
    }

  

}
