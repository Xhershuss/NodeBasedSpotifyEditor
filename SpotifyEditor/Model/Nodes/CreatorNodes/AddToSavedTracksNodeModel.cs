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
    public class AddToSavedTracksNodeModel : BaseNodeModel
    {


               

        public AddToSavedTracksNodeModel()
        {
            Name = "Add To Saved Tracks Node (Your Music)";
            InputCount = 1;
            OutputCount = 0;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {

            var service = new TrackService(SpotifyApiClient._apiClient);

     
            var tracks = DataExecutionContext.DataContextToTrackList(dataContext);  

   

            await service.SaveTrackForCurrentUser(tracks.Select(t => t.Uri).ToList());


            IsExecuted = true;
            ExecuteIndicator();
    
        }
    }

    
}
