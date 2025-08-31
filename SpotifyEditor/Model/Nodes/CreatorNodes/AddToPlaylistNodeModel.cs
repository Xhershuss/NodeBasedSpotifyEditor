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
    public class AddToPlaylistNodeModel : BaseNodeModel
    {


               

        public AddToPlaylistNodeModel()
        {
            Name = "Add To a Playlist Node";
            InputCount = 1;
            OutputCount = 0;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {

            var service = new PlaylistService(SpotifyApiClient._apiClient);

            var id = UrlParser.GetIdFromUrl(this.UserInputText);
            var tracks = DataExecutionContext.DataContextToTrackList(dataContext);  

   

            await service.AddTracksToPlaylist(id, tracks.Select(t => t.Uri).ToList());


            IsExecuted = true;
            ExecuteIndicator();
    
        }
    }

    
}
