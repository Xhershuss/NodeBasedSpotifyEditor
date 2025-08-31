using SpotifyEditor.Model.Nodes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Helpers
{
    public static class UrlParser
    {
        public static List<string> GetIdsFromBatch(GenericListModel listModel)
        {
            var ids = new List<string>();
            foreach (var item in listModel.Items)
            {
                if (item is string url)
                {
                    var id = GetIdFromUrl(url);
                    if (!string.IsNullOrEmpty(id))
                    {
                        ids.Add(id);
                    }
                }
            }
            return ids;
        }

        public static string GetIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                var uri = new Uri(url);
                var segments = uri.Segments;

                if (segments.Length < 3)
                    return null;

                string idSegment = segments[segments.Length - 1];

                int queryIndex = idSegment.IndexOf('?');
                if (queryIndex != -1)
                {
                    idSegment = idSegment.Substring(0, queryIndex);
                }
                if(idSegment.Length != 22)
                {
                    throw new Exception("The URL seems incomplete. Please check your Spotify link.");
                }
                return idSegment;   
            }
            catch (Exception ex) {
                throw new Exception("The input is not a valid URL. Please enter a correct Spotify link.");
            }
            
        }

    }
}
