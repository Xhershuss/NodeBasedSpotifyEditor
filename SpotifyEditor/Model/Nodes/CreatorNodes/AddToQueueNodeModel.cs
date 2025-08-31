using Newtonsoft.Json;
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
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace SpotifyEditor.Model.Nodes.CreatorNodes
{
    public class AddToQueueNodeModel : BaseNodeModel
    {


               

        public AddToQueueNodeModel()
        {
            Name = "Add To Queue Node";
            InputCount = 1;
            OutputCount = 0;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {

            var service = new PlayerService(SpotifyApiClient._apiClient);

            var tracks = DataExecutionContext.DataContextToTrackList(dataContext);

            for (int i = 0; i < tracks.Count; i++)
            {
               if(i % 99 == 0 && i != 0)
                {
                    await Task.Delay(1000);
                }
                await service.AddTrackToQueue(tracks[i].Id);
            }



            IsExecuted = true;
            ExecuteIndicator();
    
        }
    }

    
}
