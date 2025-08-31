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
    public class PlaylistCreatorNodeModel:BaseNodeModel
    {

        public string UserUrl { get; set; }
        public string PlaylistTitle { get; set; }
        public string PlaylistDescription { get; set; }
        public string ImagePath { get; set; }
        
        
        
       

        public PlaylistCreatorNodeModel()
        {
            Name = "Playlist Creator Node";
            InputCount = 1;
            OutputCount = 0;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {

            var service = new PlaylistService(SpotifyApiClient._apiClient);

            var tracks = DataExecutionContext.DataContextToTrackList(dataContext);  

            var playlist = await service.CreatePlaylist(
                UrlParser.GetIdFromUrl(UserUrl),
                PlaylistTitle,
                true,
                PlaylistDescription
                );

            if (!string.IsNullOrEmpty(ImagePath) )
            {
                service.UploadCustomPlaylistImage(playlist.Id, ImagePath);
            }

            await service.AddTracksToPlaylist(playlist.Id, tracks.Select(t => t.Uri).ToList());


            IsExecuted = true;
            ExecuteIndicator();
    
        }
    }

    
}
