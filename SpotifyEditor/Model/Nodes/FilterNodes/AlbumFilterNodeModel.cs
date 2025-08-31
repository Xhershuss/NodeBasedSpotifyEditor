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
    public class AlbumFilterNodeModel : BaseNodeModel, IBatchNodes
    {

        public GenericListModel AlbumList { get; set; } = new GenericListModel
        {
            ItemType = "Album"
        };
        public GenericListModel ItemList => AlbumList;

        private bool isReverse = false;

        public bool IsReverse
        {
            get { return isReverse; }
            set { isReverse = value; OnPropertyChange(); }
        }



        public AlbumFilterNodeModel()
        {
            Name = "Album Filter Node";
            InputCount = 1;
            OutputCount = 1;
        
        }

        public override async Task Process(DataExecutionContext dataContext)
        {



            var apiService = new AlbumService(SpotifyApiClient._apiClient);

            var ids = UrlParser.GetIdsFromBatch(this.AlbumList);


            var albumFilters = await apiService.GetAlbums(ids);
            var filterIds = albumFilters.Select(a => a.Id).ToHashSet();

            var tracks = DataExecutionContext.DataContextToTrackList(dataContext);

          
            var filteredTracks = new List<Track>();
   

            if (!IsReverse)
            {
                if(tracks != null)
                    filteredTracks = tracks?
                            .Where(t => filterIds.Contains(t.Album.Id))
                            .ToList();
               
            }
            else
            {
                if (tracks != null)
                    filteredTracks = tracks?
                             .Where(t => !filterIds.Contains(t.Album.Id))
                            .ToList();
              
            }



            var safeTracks = filteredTracks ?? new List<Track>();
 

            var list = new List<object> { safeTracks };

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
