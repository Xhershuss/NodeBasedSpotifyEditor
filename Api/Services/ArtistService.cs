using SpotifyEditor.Api.Models;
using Newtonsoft.Json;
using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models.Responses;
using System.Text;
using System.Runtime.InteropServices;

namespace SpotifyEditor.Api.Services
{
    public class ArtistService:BaseService
    {
      
        public ArtistService(HttpClient spotifyApiClient) : base(spotifyApiClient) { }
      
        
        
        public async Task<Artist> GetArtist(string artistId)
        {

            await EnsureAuthTokens();

            var url = $"artists/{artistId}";
            var request = new HttpRequestMessage(HttpMethod.Get,url);
            
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Artist artist = JsonConvert.DeserializeObject<Artist>(json);

            return artist;
        }

        public async Task<List<Artist>> GetArtists(List<string> artistIds)
        {
            await EnsureAuthTokens();

            if(artistIds.Count == 0)
                throw new ArgumentException("Artist Batch List is empty");

            var ids = string.Join(",", artistIds);
            var response = await _spotifyApiClient.GetAsync($"artists?ids={ids}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var artistsResponse = JsonConvert.DeserializeObject<ArtistsResponse>(json);

            List<Artist> artists= artistsResponse.Artists;
            return artists;
        }

        public async Task<List<Album>> GetArtistsAlbums(string artistId,List<string> includeGroups)
        {

            //VALID: album,single,appears_on,compilation
            var include = string.Join (",", includeGroups);

            var albums = new List<Album>();

            int limit = 20;
            int offset = 0;

            while(true)
            {
                await EnsureAuthTokens();
                var url = $"artists/{artistId}/albums?include_groups={include}&limit={limit}&offset={offset}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var artistsAlbums = JsonConvert.DeserializeObject<ArtistsAlbumsResponse>(json);

                var fetchedAlbums = artistsAlbums.Items;
                if (fetchedAlbums == null || fetchedAlbums.Count == 0)
                    break;


                albums.AddRange(fetchedAlbums);
                offset += limit;
            }

            return albums;
        }

        public async Task<List<Track>> GetArtistsTopTracks(string artistId)
        {

            await EnsureAuthTokens();

            var url = $"artists/{artistId}/top-tracks";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var artistsTopTracks = JsonConvert.DeserializeObject<ArtistsTopTracksResponse>(json);

            return artistsTopTracks.Tracks;

        }

        public void PrintArtist(Artist artist)
        {
            Console.WriteLine(artist.Name);
        }
         
    }
}
