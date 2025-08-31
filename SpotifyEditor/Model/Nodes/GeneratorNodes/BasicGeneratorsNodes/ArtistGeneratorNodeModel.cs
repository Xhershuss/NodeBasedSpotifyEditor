using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Services;
using SpotifyEditor.Helpers;

namespace SpotifyEditor.Model.Nodes.GeneratorNodes
{   
    public class ArtistGeneratorNodeModel : BaseNodeModel
    {

        public string ArtistId { get; set; }

        public ArtistGeneratorNodeModel()
        {
            Name = "Get Artist Node";
            InputCount = 0;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext data)
        {

            var artistService = new ArtistService(SpotifyApiClient._apiClient);
            var id = UrlParser.GetIdFromUrl(this.UserInputText);
            //var id = UrlParser.GetIdFromUrl("https://open.spotify.com/intl-tr/artist/1dfeR4HaWDbWqFHLkxsg1d?si=756acd3ccbe04f98");
            var artist = await artistService.GetArtist(id);
            List<object> list = new List<object> { artist };
            var artistData = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
                    { "Type", artist.ToString() }
                }, 
                MultiplicityState = DataExecutionContext.DataMultiplicity.Single 
            };

            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(artistData);

        }

        
    }
}

