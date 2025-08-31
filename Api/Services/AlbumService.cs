using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;   
using SpotifyEditor.Api.Models.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SpotifyEditor.Api.Services
{


    public class AlbumService : BaseService
    {
     

        public AlbumService(HttpClient spotifyApiClient) : base(spotifyApiClient) { }
       
        public async Task<Album> GetAlbum(string albumId)
        {
            await EnsureAuthTokens();
           

            var response = await _spotifyApiClient.GetAsync($"albums/{albumId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var  album = Album.FromJson(json);

            var tracks = await  GetAlbumTracks(albumId);
            album.Tracks.Items = tracks;

            return album;
        }

       
        public void PrintAlbumDetails(Album album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album), "Album cannot be null.");
            }
            Console.WriteLine($"Album Name: {album.Name}");
       
        }

        public async Task<List<Album>> GetAlbums(List<string> albumIds)
        {
            await EnsureAuthTokens();
           
        
            var ids = string.Join(",", albumIds);
            var response = await _spotifyApiClient.GetAsync($"albums?ids={ids}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var albumResponse = JsonConvert.DeserializeObject<AlbumsResponse>(json);

            List<Album> albums = albumResponse.Albums;
            return albums;
        }

        public async Task<List<Track>> GetAlbumTracks(string albumId)
        {
            var tracks = new List<Track>();

            int limit = 50;
            int offset = 0;
            

            while (true)
            {
                var url = $"albums/{albumId}/tracks?limit={limit}&offset={offset}";
                await EnsureAuthTokens();

                var response = await _spotifyApiClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var albumTracksResponse = JsonConvert.DeserializeObject<AlbumTracksResponse>(json);

                var fetchedTracks = albumTracksResponse.Items;
                if (fetchedTracks == null || fetchedTracks.Count == 0)
                    break;


                tracks.AddRange(fetchedTracks);
                offset += limit;


            }

            return tracks;


        }

        public async Task<List<DetailedAlbum>> GetUsersSavedAlbums()
        {
            var albums = new List<DetailedAlbum>();
            int limit = 50; int offset = 0;
            while (true) { 
           
                var url = $"me/albums?limit={limit}&offset={offset}";

                await EnsureAuthTokens();

                var request = new HttpRequestMessage(HttpMethod.Get, url) ;
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
                var response = await _spotifyApiClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var usersSavedAlbums = JsonConvert.DeserializeObject<UsersSavedAlbumsResponse>(json);

                var fetchedAlbums = usersSavedAlbums.Items;
                if (fetchedAlbums == null || fetchedAlbums.Count == 0)
                    break;


                albums.AddRange(fetchedAlbums);
                offset += limit;

            }
            return albums;
        }

        public async Task SaveAlbumsForUser(List<string> albumIds)
        {
            await EnsureAuthTokens();


            var url = $"me/albums";

            var requestBody = JsonConvert.SerializeObject(new { ids = albumIds });
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");


            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = content;

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteAlbumsForUser(List<string> albumIds)
        {
            await EnsureAuthTokens();
            

            var url = "me/albums";
            var requestBody = JsonConvert.SerializeObject(new { ids = albumIds });
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = content;

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }

        public async Task<List<Boolean>> CheckUsersSavedAlbums(List<string> albumIds)
        {
            await EnsureAuthTokens();

            var ids = string.Join(",", albumIds);
            var url = $"me/albums/contains?ids={ids}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            List<Boolean> result = JsonConvert.DeserializeObject<List<Boolean>>(json);
            return result;
        }

        public async Task<List<Album>> GetNewReleases(int limit = 20, int offset = 0)
        {
            await EnsureAuthTokens();

            var response = await _spotifyApiClient.GetAsync($"browse/new-releases?offset={offset}&limit={limit}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var newReleases = JsonConvert.DeserializeObject<NewReleasesResponse>(json);

            List<Album> albumList = newReleases.Albums.Items;
            return albumList;
        }

    }

}