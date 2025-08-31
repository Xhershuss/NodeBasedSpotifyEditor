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
    public class PopularityFilterNodeModel : BaseNodeModel
    {

        private int popularityScore;

        public int PopularityScore
        {
            get { return popularityScore; }
            set { popularityScore = value; OnPropertyChange(); OnPropertyChange(nameof(PopularityString)); }
        }


        public string PopularityString
        {
            get
            {
                var populartyString = $"Min Popularity Score: {PopularityScore}";
                return populartyString;
            }
        }

        public PopularityFilterNodeModel()
        {
            Name = "Popularity Filter Node";
            InputCount = 1;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {


            var apiService = new AlbumService(SpotifyApiClient._apiClient);

            var dict = DataExecutionContext.DataContextToFilterData(dataContext);

            dict.TryGetValue("track", out var trackObj);
            dict.TryGetValue("artist", out var artistObj);

            var tracks = trackObj as List<Track>;
            var artists = artistObj as List<Artist>;

            var filteredTracks = tracks?.Where(t => t.Popularity >= PopularityScore).ToList();
            var filteredArtists = artists?.Where(a => a.Popularity >= PopularityScore).ToList();

            List<object> list = new List<object> { filteredTracks,filteredArtists };

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
