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
    public class TrackBatchGeneratorNodeModel : BaseNodeModel,IBatchNodes
    {

        public GenericListModel TrackList { get; set; } = new GenericListModel
        {
            ItemType = "Track"
        };
        public GenericListModel ItemList => TrackList;
      

        public TrackBatchGeneratorNodeModel()
        {
            Name = "Get Track Batch Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {

            

            var apiService = new TrackService(SpotifyApiClient._apiClient);

            var ids = UrlParser.GetIdsFromBatch(this.TrackList);
           

         
            var objects = await apiService.GetTracks(ids);
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
