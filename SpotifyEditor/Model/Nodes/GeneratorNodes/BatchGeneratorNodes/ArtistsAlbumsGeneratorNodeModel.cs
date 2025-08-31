using SpotifyEditor.Api.Services;
using SpotifyEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Model.Nodes.GeneratorNodes
{
    public class ArtistsAlbumsGeneratorNodeModel:BaseNodeModel
    {

        public ArtistsAlbumsGeneratorNodeModel()
        {
            Name = "Get Artists Albums Node";
            InputCount = 0;
            OutputCount = 1;
        }


        private bool album = true;

        public bool Album
        {
            get {return album; }
            set { album = value; OnPropertyChange(); ValidateOptions(); }
        }

        private bool single;
        public bool Single
        {
            get { return single; }
            set { single = value; OnPropertyChange(); ValidateOptions(); }
        }
        private bool appears_on;
        public bool Appears_on
        {
            get {return appears_on; }
            set { appears_on = value; OnPropertyChange(); ValidateOptions(); }
        }
        
        private bool compilation;
        public bool Compilation
        {
            get { return compilation; }
            set { compilation = value; OnPropertyChange(); ValidateOptions(); }
        }

        private void ValidateOptions()
        {

            if (!Album && !Single&& !Appears_on && !Compilation)
            {
                Album = true;
                Single = true;
                Appears_on = true;
                Compilation = true;
            }
        }


        public List<string> IncludeGroups
        {
            get
            {
                var groups = new List<string>();
                if (Album) groups.Add("album");
                if (Single) groups.Add("single");
                if (Appears_on) groups.Add("appears_on");
                if (Compilation) groups.Add("compilation");
                return groups;
            }
        }

        public override async Task Process(DataExecutionContext data)
        {


            var apiService = new ArtistService(SpotifyApiClient._apiClient);
            var id = UrlParser.GetIdFromUrl(this.UserInputText);

            
            var objects = await apiService.GetArtistsAlbums(id, IncludeGroups);
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
