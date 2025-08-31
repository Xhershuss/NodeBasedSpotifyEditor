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
    public class UserTopTracksGeneratorNodeModel : BaseNodeModel
    {

  

        private double timeDuration;

        public double TimeDuration
        {
            get { return timeDuration; }
            set { timeDuration = value; OnPropertyChange(); }
        }

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


        public TimeRange TimeDurationString
        {
            get
            {
                return timeDuration switch
                {
                    0 => TimeRange.ShortTerm,
                    1 => TimeRange.MediumTerm,
                    2 => TimeRange.LongTerm,
                    _ => TimeRange.MediumTerm
                };
            }
        }


        public UserTopTracksGeneratorNodeModel()
        {
            Name = "Get Users Top Tracks Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {



            var apiService = new UserService(SpotifyApiClient._apiClient);



            var objects = await apiService.GetUsersTopTracks(TimeDurationString, NumberOfTracks);
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
