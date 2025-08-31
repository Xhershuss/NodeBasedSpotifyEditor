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
    public class ArtistsTopTracksGeneratorNodeModel : BaseNodeModel
    {

        public ArtistsTopTracksGeneratorNodeModel()
        {
            Name = "Get Artists Top Tracks Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {


            var apiService = new ArtistService(SpotifyApiClient._apiClient);
            var id = UrlParser.GetIdFromUrl(this.UserInputText);
            
            var album = await apiService.GetArtistsTopTracks(id);
            List<object> list = new List<object> { album };
            var dataContext = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
                    { "Type", album.ToString() }
                },
                MultiplicityState = DataExecutionContext.DataMultiplicity.Single
            };

            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(dataContext);

        }
    }
}
