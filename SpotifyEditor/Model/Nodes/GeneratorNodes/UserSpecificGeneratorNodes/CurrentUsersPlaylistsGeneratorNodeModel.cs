using SpotifyEditor.Api.Services;
using SpotifyEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Model.Nodes.GeneratorNodes
{
    public class CurrentUsersPlaylistsGeneratorNodeModel : BaseNodeModel
    {

        public CurrentUsersPlaylistsGeneratorNodeModel()
        {
            Name = "Get Current Users Playlists Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {


            var apiService = new PlaylistService(SpotifyApiClient._apiClient);
           
            var objects = await apiService.GetCurrentUsersPlaylists();
            List<object> list = new List<object> { objects };
            var dataContext = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
                    { "Type", objects.ToString() }
                },
                MultiplicityState = DataExecutionContext.DataMultiplicity.Multiple
            };

            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(dataContext);

        }
    }
}
