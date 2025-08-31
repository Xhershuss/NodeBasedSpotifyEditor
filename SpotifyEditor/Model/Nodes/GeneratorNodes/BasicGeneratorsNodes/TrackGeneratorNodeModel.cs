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
    public class TrackGeneratorNodeModel : BaseNodeModel
    {

        public TrackGeneratorNodeModel()
        {
            Name = "Get Track Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {

            var apiService = new TrackService(SpotifyApiClient._apiClient);
            var id = UrlParser.GetIdFromUrl(this.UserInputText);
            //var id = UrlParser.GetIdFromUrl("https://open.spotify.com/intl-tr/track/0LPMWPwCHrIorXSdpnPVyv?si=1fad9aeab24b4847");
            var track = await apiService.GetTrack(id);
            List<object> list = new List<object> { track };
            var dataContext = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
                    { "Type", track.ToString() }
                },
                MultiplicityState = DataExecutionContext.DataMultiplicity.Single
            };

            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(dataContext);

        }

    }
}
