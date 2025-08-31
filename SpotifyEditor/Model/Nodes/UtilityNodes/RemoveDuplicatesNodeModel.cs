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


namespace SpotifyEditor.Model.Nodes.UtilityNodes
{
    public class RemoveDuplicatesNodeModel : BaseNodeModel
    {

       


        public RemoveDuplicatesNodeModel()
        {
            Name = "Remove Duplicates Node";
            InputCount = 0;
            OutputCount = 0;
        
        }

        public override async Task Process(DataExecutionContext dataContext)
        {

            var apiService = new PlaylistService(SpotifyApiClient._apiClient);

           
            var id = UrlParser.GetIdFromUrl(this.UserInputText);
          



            var playlist = await apiService.GetPlaylist(id);
   
            var counts = playlist.tracks.Items
                .Where(i => i?.Track?.Id != null)
                .GroupBy(i => i.Track.Id)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToList();


            var duplicateIds = counts.Where(x => x.Count > 1).Select(x => x.Id).ToList();

            await apiService.RemovePlaylistTracks(playlist.Id, duplicateIds);
            await apiService.AddTracksToPlaylist(playlist.Id, duplicateIds.Select(t => $"spotify:track:{t}").ToList());



            IsExecuted = true;
            ExecuteIndicator();

            

        }
    }
}
