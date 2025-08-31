using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;    
using SpotifyEditor.Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpotifyEditor.Api.Services
{
    public class TrackService : BaseService
    {
        public TrackService(HttpClient spotifyApiClient) : base(spotifyApiClient)
        {
        }

        public async Task<Track> GetTrack(string trackId)
        {
            await EnsureAuthTokens();

            var response = await _spotifyApiClient.GetAsync($"tracks/{trackId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var track = Track.FromJson(json);
            return track;
        }

        public async Task<List<Track>> GetTracks(List<string> trackIds)
        {
            await EnsureAuthTokens();

            var ids = string.Join(",", trackIds);
            var response = await _spotifyApiClient.GetAsync($"tracks?ids={ids}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var trackResponse = JsonConvert.DeserializeObject<TracksResponse>(json);
            List<Track> tracks = trackResponse.tracks;
            return tracks;
        }


        public async Task<List<TracksWithDate>> GetCurrentUsersSavedTracks()
        {
            var tracks = new List<TracksWithDate>();
            int limit = 50;
            int offset = 0;


            while (true)
            {

                await EnsureAuthTokens();

                var url = $"me/tracks?limit={limit}&offset={offset}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var usersSavedTracksResponse = JsonConvert.DeserializeObject<UsersSavedTracksResponse>(json);

                var fetchedTracks = usersSavedTracksResponse.Items;
                if (fetchedTracks == null || fetchedTracks.Count == 0)
                    break;


                tracks.AddRange(fetchedTracks);
                offset += limit;

            }

            return tracks;
        }


        public async Task SaveTrackForCurrentUser(List<string> uris)
        {
            await EnsureAuthTokens();

            var ids = uris.Select(uri => uri.Split(':').Last()).ToList();

            var url = $"me/tracks";

            for (int i = 0; i < Math.Ceiling(uris.Count / 50.0); i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);


                string ts = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'");
                string timestampParam = $"&timestamp={Uri.EscapeDataString(ts)}";


                var content = new
                {
                    timestamped_ids = ids.Skip(i * 50).Take(50).Select(id => new
                    {
                        id = id,
                        added_at = ts
                    }).ToList()
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            


        }

        public async Task RemoveUsersSavedTracks(List<string> uris)
        {
            await EnsureAuthTokens();

            var ids = uris.Select(uri => uri.Split(':').Last()).ToList();


            var url = $"me/tracks";

            for (int i = 0; i < Math.Ceiling(uris.Count / 50.0); i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);


                var content = new
                {
                    ids = ids.Skip(i * 50).Take(50).Select(id => id).ToList()
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();


            }



        }

        public async Task<List<bool>> CheckUsersSavedTracks(List<string> uris)
        {
            await EnsureAuthTokens();
            if (uris.Count > 50)
            {
                throw new ArgumentException("Limit is 50.", nameof(uris));
            }
            var url = $"me/tracks/contains?ids={string.Join(",", uris)}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<bool>>(json);
            return result;




        }
    }
}
    
