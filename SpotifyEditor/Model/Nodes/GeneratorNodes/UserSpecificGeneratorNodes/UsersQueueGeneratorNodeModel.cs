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
using System.Windows.Controls;

namespace SpotifyEditor.Model.Nodes.GeneratorNodes
{
    public class UsersQueueGeneratorNodeModel : BaseNodeModel
    {

  


        public UsersQueueGeneratorNodeModel()
        {
            Name = "Get Users Queue Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {



            var apiService = new PlayerService(SpotifyApiClient._apiClient);



            var objects = await apiService.GetUsersQueue();
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
