using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SpotifyEditor.Api.Services
{
    public class PlaylistService : BaseService
    {

        public PlaylistService(HttpClient spotifyApiClient) : base(spotifyApiClient) { }



        public async Task<Playlist> GetPlaylist(string playlistId)
        {

            await EnsureAuthTokens();
            var response = await _spotifyApiClient.GetAsync($"playlists/{playlistId}");
            if(!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to fetch playlist. May playlist is private");
                return null;
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var playlistResponse = JsonConvert.DeserializeObject<Playlist>(json);
                                  


            var tracks = await GetAllPlaylistTracksAsync(playlistId);


            playlistResponse.tracks.Items = tracks;

            return playlistResponse;
        }

        private async Task<List<DetailedTrack>> GetAllPlaylistTracksAsync(string playlistId)
        {


            var tracks = new List<DetailedTrack>();
            const int limit = 50;
            int offset = 0;

            while (true)
            {
                await EnsureAuthTokens();
                var url = $"playlists/{playlistId}/tracks?limit={limit}&offset={offset}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);


                var trackResponse = await _spotifyApiClient.SendAsync(request);
                trackResponse.EnsureSuccessStatusCode();

                var trackJson = await trackResponse.Content.ReadAsStringAsync();
                var playlistsTrackResponse = JsonConvert.DeserializeObject<PlaylistsTracksResponse>(trackJson);

                var fetchedTracks = playlistsTrackResponse?.Items;
                if (fetchedTracks == null || fetchedTracks.Count == 0)
                    break;

                tracks.AddRange(fetchedTracks);
                offset += limit;
            }

            return tracks;
        }

        public async Task ChangePlaylistDetails(string playlistId,string newName,string newDescription = "", bool newPublicState = false , bool newCollabState = false)
        {
            if (newCollabState && newPublicState)
            {
                throw new ArgumentException("You can only set collaborative to true on non-public playlists.");
            }

            await EnsureAuthTokens();
            var url = $"playlists/{playlistId}";

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var content = new
            {
                name = newName,
                @public = newPublicState,
                collaborative = newCollabState,
                description = newDescription

            };
            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddTracksToPlaylist(string playlistId, List<string> uris, int? position = null)
        {
            // Can add up to 100 items in one request
            

            await EnsureAuthTokens();
           
          
            var url = $"playlists/{playlistId}/tracks";

            for (int i = 0; i < Math.Ceiling(uris.Count / 100.0); i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

                var content = new
                {
                    position = position,
                    uris = uris.Skip(i * 100).Take(100).ToList()
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");


                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }

           

        }



        public async Task RemovePlaylistTracks(string playlistId,List<string> tracksIds)
        {
            await EnsureAuthTokens();

           

            var uris = tracksIds.Select(id => new { uri = $"spotify:track:{id}" }).ToList();

            var url = $"playlists/{playlistId}/tracks";

            

            for (int i = 0; i < Math.Ceiling(uris.Count / 100.0); i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

                var content = new
                {
                    tracks = uris.Skip(i * 100).Take(100).ToList()
                };

                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");


                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        
          
        }


        public async Task ReorderPlaylistTracks(string playlistId,int rangeStart, int insertBefore, int rangeLength = 1)
        {
            /* 
                rangeStart : The position of the first item to be reordered.
                insertBefore : The position where the items should be inserted.
                rangeLength : The amount of items to be reordered.

            for 5 track playlist : 
            Spotify representation =>    1- A     1- A
            Apply Reorder(1,2,5)         2- B     2- D     
                                         3- C =>  3- E
                                         4- D     4- C
                                         5- E     5- D
            */

            await EnsureAuthTokens();
            var url = $"playlists/{playlistId}/tracks";

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var content = new
            {
                range_start = rangeStart,
                range_length = rangeLength,
                insert_before = insertBefore
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");


            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }


        public async Task ReplacePlaylistItems(string playlistId,List<string> uris)
        {
            //this function deletes all songs in the playlists
            //and adds the songs in the uris

            if (uris.Count >= 100)
            {
                throw new ArgumentException("A maximum of 100 items can be added in one request.", nameof(uris));
            }

            var prefix = new List<string>();
            foreach (var uri in uris) { 
            prefix.Add($"spotify:track:{uri}");
            }


            await EnsureAuthTokens();

            var url = $"playlists/{playlistId}/tracks";

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var content = new
            {
                uris = prefix
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");


            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }


        public async Task<List<Playlist>> GetCurrentUsersPlaylists()
        {

            var playlists = new List<Playlist>();

            int limit = 50;
            int offset = 0;


            while (true)
            {
          

                var url = $"me/playlists?limit={limit}&offset={offset}";

                await EnsureAuthTokens();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var usersPlaylistsResponse = JsonConvert.DeserializeObject<UsersPlaylistsResponse>(json);


                var fetchedTracks = usersPlaylistsResponse.Items;
                if (fetchedTracks == null || fetchedTracks.Count == 0)
                    break;


                playlists.AddRange(fetchedTracks);
                offset += limit;

            }
            await AddTracksToPlaylists(playlists);
            return playlists;

        }


        public async Task<List<Playlist>> GetUsersPlaylists(string userId)
        {

            var playlists = new List<Playlist>();

            int limit = 50;
            int offset = 0;

            while (true)
            {

                var url = $"users/{userId}/playlists?limit={limit}&offset={offset}";

                await EnsureAuthTokens();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var usersPlaylistsResponse = JsonConvert.DeserializeObject<UsersPlaylistsResponse>(json);


                var fetchedTracks = usersPlaylistsResponse.Items;
                if (fetchedTracks == null || fetchedTracks.Count == 0)
                    break;


                playlists.AddRange(fetchedTracks);
                offset += limit;

            }
            await AddTracksToPlaylists(playlists);
            return playlists;

        }


        public async Task<Playlist> CreatePlaylist(string userId,string name,bool publicState = false ,string description= "")
        {
            // Returns created playlist

            await EnsureAuthTokens();

            var url = $"users/{userId}/playlists";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var content = new
            {
                name = name,
                description = description,
                @public = publicState
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");


            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var playlist = JsonConvert.DeserializeObject<Playlist>(json);

            return playlist;

        }


        public async Task<List<Image>> GetPlaylistsImage(string playlistId)
        {
            await EnsureAuthTokens();

            var url = $"playlists/{playlistId}/images";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var images = JsonConvert.DeserializeObject<List<Image>>(json);

            return images;

        }


        public async Task UploadCustomPlaylistImage(string playlistId, string imageFilePath)
        {

            if (!File.Exists(imageFilePath))
                throw new FileNotFoundException($"Belirtilen dosya bulunamadı: {imageFilePath}");

            byte[] imageBytes = await File.ReadAllBytesAsync(imageFilePath);

            // max size 256 KB
            if (imageBytes.Length > 256 * 1024)
                throw new ArgumentException("Image exceeds 256 KB limit.", nameof(imageFilePath));


            string base64 = Convert.ToBase64String(imageBytes);


            var bodyBytes = Encoding.UTF8.GetBytes(base64);
            var content = new ByteArrayContent(bodyBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");


            await EnsureAuthTokens();
            var url = $"playlists/{playlistId}/images";
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }


        public async Task AddTracksToPlaylists(List<Playlist> playlists)
        {
            await EnsureAuthTokens();

            for(int i = 0; i < playlists.Count; i++)
            {
                var tracks = await GetAllPlaylistTracksAsync(playlists[i].Id);
                playlists[i].tracks.Items = tracks;
            }
        }
    }
}
