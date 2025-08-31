using SpotifyEditor.Api.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Services
{
    public class SearchService : BaseService
    {

        public SearchService(HttpClient spotifyApiClient) : base(spotifyApiClient) { }


        public async Task<SearchForItemResponse> SearchForItem(string q,List<string> types,int limit = 20,int offset = 0 )
        {
            //For usage , see https://stackoverflow.com/questions/73680222/what-is-the-correct-format-of-the-query-for-search-requests-to-spotifys-web-api

            var args = new List<string>
            {
                    $"q={Uri.EscapeDataString(q)}",
                    $"type={string.Join(",", types)}"
                };

        
            var url = $"search?{string.Join("&", args)}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();


            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SearchForItemResponse>(json);

            return result;
        }


    }
}
