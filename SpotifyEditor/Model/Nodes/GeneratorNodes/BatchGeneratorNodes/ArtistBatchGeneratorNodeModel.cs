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
    public class ArtistBatchGeneratorNodeModel : BaseNodeModel, IBatchNodes
    {

        public GenericListModel ArtistList { get; set; } = new GenericListModel
        {
            ItemType = "Artist"
        };
        public GenericListModel ItemList => ArtistList;


        public ArtistBatchGeneratorNodeModel()
        {
            Name = "Get Artist Batch Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {



            var apiService = new ArtistService(SpotifyApiClient._apiClient);

            var ids = UrlParser.GetIdsFromBatch(this.ArtistList);



            var objects = await apiService.GetArtists(ids);
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
