using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Responses;
using SpotifyEditor.Api.Services;
using SpotifyEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;


namespace SpotifyEditor.Model.Nodes.FilterNodes
{
    public class DurationFilterNodeModel : BaseNodeModel
    {

        private int minTrackDuration;

        public int MinTrackDuration
        {
            get { return minTrackDuration; }
            set { minTrackDuration = value; OnPropertyChange(); OnPropertyChange(nameof(DurationString)); }
        }

        private int maxTrackDuration;

        public int MaxTrackDuration
        {
            get { return maxTrackDuration; }
            set { maxTrackDuration = value; OnPropertyChange(); OnPropertyChange(nameof(DurationString)); }
        }



    
        public string DurationString
        {
            get {
                string minMinutes = (MinTrackDuration / 60).ToString("D2");
                string minSeconds = (MinTrackDuration % 60).ToString("D2");
                string maxMinutes = (MaxTrackDuration / 60).ToString("D2");
                string maxSeconds = (MaxTrackDuration % 60).ToString("D2");
                var durationString = $"Between {minMinutes}:{minSeconds} and {maxMinutes}:{maxSeconds}";
                return durationString; }
        }


        public DurationFilterNodeModel()
        {
            Name = "Track Duration Filter Node";
            InputCount = 1;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {
            if (MinTrackDuration > MaxTrackDuration)
            {
                throw new Exception("Min Track Duration must be less than Max Track Duration");
            }

      

            var tracks = DataExecutionContext.DataContextToTrackList(dataContext);

            // Filter tracks based on duration in milliseconds
            var filteredTracks = tracks?
                .Where(t => t.DurationMs >= MinTrackDuration*1000 && t.DurationMs<= MaxTrackDuration*1000).ToList();

            List<object> list = new List<object> { filteredTracks};

            var processedData = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
                    { "Type", tracks.ToString() }
                },
                MultiplicityState = DataExecutionContext.DataMultiplicity.Multiple
            };


            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(processedData);

        }
    }
}
