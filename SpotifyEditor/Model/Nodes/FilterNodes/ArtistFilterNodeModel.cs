using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Models.Responses;
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
using System.Windows.Documents;
using System.Xml.Serialization;


namespace SpotifyEditor.Model.Nodes.FilterNodes
{
    public class ArtistFilterNodeModel : BaseNodeModel, IBatchNodes
    {

        public GenericListModel ArtistList { get; set; } = new GenericListModel
        {
            ItemType = "Artist"
        };
        public GenericListModel ItemList => ArtistList;

        private bool isReverse = false;

        public bool IsReverse
        {
            get { return isReverse; }
            set { isReverse = value; OnPropertyChange(); }
        }



        public ArtistFilterNodeModel()
        {
            Name = "Artist Filter Node";
            InputCount = 1;
            OutputCount = 1;
        
        }

        public override async Task Process(DataExecutionContext dataContext)
        {



            var apiService = new ArtistService(SpotifyApiClient._apiClient);

            var ids = UrlParser.GetIdsFromBatch(this.ArtistList);


            var artistFilters = await apiService.GetArtists(ids);
            var filterIds = artistFilters.Select(a => a.Id).ToHashSet();

            var dict = DataExecutionContext.DataContextToFilterData(dataContext);

            dict.TryGetValue("track", out var trackObj);
            dict.TryGetValue("artist", out var artistObj);

            var tracks = trackObj as List<Track>;
            var artists = artistObj as List<Artist>;

            var filteredTracks = new List<Track>();
            var filteredArtists = new List<Artist>();

            if (!IsReverse)
            {
                if(tracks != null)
                    filteredTracks = tracks?
                            .Where(t => t.Artists.Any(a => filterIds.Contains(a.Id)))
                            .ToList();
                if (artists != null)
                    filteredArtists = artists?.Where(a => filterIds.Contains(a.Id)).ToList();
            }
            else
            {
                if (tracks != null)
                    filteredTracks = tracks?
                            .Where(t => t.Artists.All(a => !filterIds.Contains(a.Id)))
                            .ToList();
                if (artists != null)
                    filteredArtists = artists?.Where(a => !filterIds.Contains(a.Id)).ToList();
            }



            var safeTracks = filteredTracks ?? new List<Track>();
            var safeArtists = filteredArtists ?? new List<Artist>();

            var list = new List<object> { safeTracks, safeArtists };

            var processedData = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
      
                },
                MultiplicityState = DataExecutionContext.DataMultiplicity.Multiple
            };


            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(processedData);

        }
    }
}
