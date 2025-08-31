using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
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
    public class AlbumReleaseDateFilterNodeModel : BaseNodeModel
    {

        private int minDate;

        public int MinDate
        {
            get { return minDate; }
            set { minDate = value; OnPropertyChange(); OnPropertyChange(nameof(DateString)); }
        }

        private int maxDate;

        public int MaxDate
        {
            get { return maxDate; }
            set { maxDate = value; OnPropertyChange(); OnPropertyChange(nameof(DateString)); }
        }



    
        public string DateString
        {
            get
            {
                var dateString = $"Between {MinDate} and {MaxDate}";
                return dateString;
            }
        }
        


        public AlbumReleaseDateFilterNodeModel()
        {
            Name = "Album Release Date Filter Node";
            InputCount = 1;
            OutputCount = 1;
        }

        public override async Task Process(DataExecutionContext dataContext)
        {
            if (MinDate > MaxDate)
            {
                throw new Exception("Min Date must be less than Max Date");
            }


            IEnumerable<object> items;
            try
            {
                items = DataExecutionContext.TransformData<object>(dataContext);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid data format");
            }

           

            var albums = new List<Album>();
            foreach (var item in items)
            {
                switch (item)
                {
                    
                    case Album album:
                        albums.Add(album);
                        break;
                    case List<Album> albumList:
                        albums.AddRange(albumList);
                        break;
                    case List<DetailedAlbum> detailedAlbumList:
                        albums.AddRange(detailedAlbumList.Select(album => album.Album));
                        break;
                    default:
                        throw new Exception("Invalid data type. Expected Track, List<Track> or Album, List<Album>.");
                }
            }

            var filteredAlbums = albums?
                 .Where(a =>
                 {
                  
                     var yearPart = a.ReleaseDate.Split('-').FirstOrDefault();

        
                     if (int.TryParse(yearPart, out int releaseYear))
                     {
                         return releaseYear >= MinDate && releaseYear <= MaxDate;
                     }

   
                     return false;
                 }).ToList();


            List<object> list = new List<object> { filteredAlbums };

            var processedData = new DataExecutionContext
            {
                Data = list,
                Metadata = new Dictionary<string, object>
                {
                    { "Type", list.ToString() }
                },
                MultiplicityState = DataExecutionContext.DataMultiplicity.Multiple
            };


            IsExecuted = true;
            ExecuteIndicator();

            WriteToOutputPorts(processedData);

        }
    }
}
