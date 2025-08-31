using SpotifyEditor.Api.Services;
using SpotifyEditor.Helpers;
using SpotifyEditor.Model.Nodes.Common;
using SpotifyEditor.Model.Nodes.GeneratorNodes.BatchGeneratorNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Model.Nodes.GeneratorNodes
{
    public class RecentlyPlayedTracksGeneratorNodeModel : BaseNodeModel
    {

        private string numberOfTracksText;
        public string NumberOfTracksText
        {
            get => numberOfTracksText;
            set
            {
                numberOfTracksText = value;
                OnPropertyChange();

                if (int.TryParse(value, out int num))
                    NumberOfTracks = num;
                else
                    NumberOfTracks = null;
            }
        }

        public int? NumberOfTracks { get; private set; }



        public RecentlyPlayedTracksGeneratorNodeModel()
        {
            Name = "Get Recently Played Tracks Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {

            

            var apiService = new PlayerService(SpotifyApiClient._apiClient);

                   
            var objects = await apiService.GetRecentlyPlayedTracks(NumberOfTracks);
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
