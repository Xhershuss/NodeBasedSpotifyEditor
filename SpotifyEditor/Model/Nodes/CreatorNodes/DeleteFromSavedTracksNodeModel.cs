using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Models.Responses;
using SpotifyEditor.Api.Services;
using SpotifyEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpotifyEditor.Model.Nodes.CreatorNodes
{
    public class DeleteFromSavedTracksNodeModel : BaseNodeModel
    {


               

        public DeleteFromSavedTracksNodeModel()
        {
            Name = "Delete From Saved Tracks Node";
            InputCount = 1;
            OutputCount = 0;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {

            var service = new TrackService(SpotifyApiClient._apiClient);

            var dict = DataExecutionContext.DataContextToFilterData(dataContext);

            dict.TryGetValue("track", out var trackObj);
            dict.TryGetValue("artist", out var artistObj);

            var tracks = trackObj as List<Track>;
            var artists = artistObj as List<Artist>;




            var playlist = await service.GetCurrentUsersSavedTracks();
             if (playlist == null)
             {
                throw new Exception("Playlist not found");
             }
             var toDelete = new List<string>();

            foreach (var track in playlist)
            {
                if(tracks != null &&  tracks.Any(tr => track.Track.Id == tr.Id))
                {
                    toDelete.Add(track.Track.Id);
                }

                if(artists != null && artists.Any(ar => track.Track.Artists.Any(tar => tar.Id == ar.Id)))
                {
                    toDelete.Add(track.Track.Id);
                }
            }

            if (toDelete.Count == 0)
            {
                throw new Exception("No matching tracks found in the playlist");
            }

            await service.RemoveUsersSavedTracks(toDelete);

            IsExecuted = true;
            ExecuteIndicator();
    
        }
    }

    
}
