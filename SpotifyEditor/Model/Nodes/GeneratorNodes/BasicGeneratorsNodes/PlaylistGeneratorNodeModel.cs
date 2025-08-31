using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Services;
using SpotifyEditor.Helpers;

namespace SpotifyEditor.Model.Nodes.GeneratorNodes
{   
    public class PlaylistGeneratorNodeModel : BaseNodeModel
    {

        public string PlaylistId { get; set; }

        public PlaylistGeneratorNodeModel()
        {
            Name = "Get Playlist Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {

            var service = new PlaylistService(SpotifyApiClient._apiClient);
            var id = UrlParser.GetIdFromUrl(this.UserInputText);
           
            var objects = await service.GetPlaylist(id);
            List<object> list = new List<object> { objects };
            var artistData = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
                    { "Type", objects.ToString() }
                }, 
                MultiplicityState = DataExecutionContext.DataMultiplicity.Single 
            };

            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(artistData);

        }

        
    }
}

