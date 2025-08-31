using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Services
{
    public class UserService : BaseService
    {
        public UserService(HttpClient spotifyApiClient) : base(spotifyApiClient)
        {
        }
        public async Task<User> GetCurrentUser()
        {
            await EnsureAuthTokens();

            var url = $"me";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var user = User.FromJson(json);
            return user;
        }

        public async Task<List<Track>> GetUsersTopTracks(TimeRange tr,int? numberOfTracks = null)
        {

            string timeRange = JsonConvert
                .SerializeObject(tr)   
                .Trim('"');

            await EnsureAuthTokens();

            var tracks = new List<Track>();

            int? requestedCount = numberOfTracks;

            int offset = 0;
            int limit = 50;

            while (true)
            {

                await EnsureAuthTokens();

                var url = $"me/top/tracks?time_range={timeRange}&limit={limit}&offset={offset}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var usersTopTracks = JsonConvert.DeserializeObject<UsersTopTracksResponse>(json);

                var fetchedTracks = usersTopTracks.Items;
                if (fetchedTracks == null || fetchedTracks.Count == 0)
                    break;
                if (numberOfTracks <= 0)
                    break;

                tracks.AddRange(fetchedTracks);
                offset += limit;
                if (numberOfTracks != null) numberOfTracks-=limit;

            }

            if (requestedCount.HasValue)
            {
                int count = Math.Min(requestedCount.Value, tracks.Count);
                return tracks.GetRange(0, count);
            }

            return tracks;



        }

        public async Task<List<Artist>> GetUsersTopArtists(TimeRange tr, int? numberOfArtists = null)
        {

            string timeRange = JsonConvert
                .SerializeObject(tr)
                .Trim('"');

            await EnsureAuthTokens();

            var artists = new List<Artist>();

            int? requestedCount = numberOfArtists;
            int offset = 0;
            int limit = 50;

            while (true)
            {

                await EnsureAuthTokens();

                var url = $"me/top/artists?time_range={timeRange}&limit={limit}&offset={offset}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var usersTopArtists = JsonConvert.DeserializeObject<UsersTopArtistsResponse>(json);

                var fetchedArtists = usersTopArtists.Items;
                if (fetchedArtists == null || fetchedArtists.Count == 0)
                    break;
                if (numberOfArtists <= 0)
                    break;

                artists.AddRange(fetchedArtists);
                offset += limit;
                if (numberOfArtists != null) numberOfArtists -= limit;

            }

            if (requestedCount.HasValue)
            {
                int count = Math.Min(requestedCount.Value, artists.Count);
                return artists.GetRange(0, count);
            }

            return artists;

        }


        public async Task<UserProfile> GetUserProfile(string userId)
        {
            await EnsureAuthTokens();
            var url = $"users/{userId}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var userProfile = UserProfile.FromJson(json);
            return userProfile;
        }


        public async Task FollowPlaylist(string playlistId, bool isPublic = false)
        {
            await EnsureAuthTokens();
            var url = $"playlists/{playlistId}/followers";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(new { public_ = isPublic }), Encoding.UTF8, "application/json");
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }



        public async Task UnfollowPlaylist(string playlistId)
        {
            await EnsureAuthTokens();
            var url = $"playlists/{playlistId}/followers";
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Artist>> GetFollowedArtists()
        {
            int limit = 50;
            string? after = null;

            var artists = new List<Artist>();

            while (true) {

                await EnsureAuthTokens();
                var url = $"me/following?type=artist&limit={limit}&after={after}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
                var response = await _spotifyApiClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var followedArtists = JsonConvert.DeserializeObject<FollowedArtistsResponse>(json);

                artists.AddRange(followedArtists.Artists.Items);

                if (string.IsNullOrEmpty(followedArtists.Artists.Next))
                    return artists;

                var uri = new Uri(followedArtists.Artists.Next);
                var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
                string? newAfter = queryParams.Get("after");
                
                after = newAfter;

            }
        }

        public async Task FollowArtists(List<string> artistIds)
        {
            if (artistIds.Count > 50)
            {
                throw new ArgumentException("You can only follow up to 50 artist at a time.");
            }
            await EnsureAuthTokens();
            var url = "me/following?type=artist";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(new { ids = artistIds }), Encoding.UTF8, "application/json");
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnfollowArtists(List<string> artistIds)
        {
            if (artistIds.Count > 50)
            {
                throw new ArgumentException("You can only unfollow up to 50 artist at a time.");
            }
            await EnsureAuthTokens();
            var url = "me/following?type=artist";
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(new { ids = artistIds }), Encoding.UTF8, "application/json");
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<bool>> CheckIfUserFollowsArtists(List<string> artistIds)
        {
            await EnsureAuthTokens();
            if (artistIds.Count > 50)
            {
                throw new ArgumentException("You can only check up to 50 artists at a time.");
            }
            var ids = string.Join(",", artistIds);
            var url = $"me/following/contains?type=user&ids={ids}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<bool>>(json);

        }

        public async Task FollowUsers(List<string> userIds)
        {
            if (userIds.Count > 50)
            {
                throw new ArgumentException("You can only follow up to 50 users at a time.");
            }
            await EnsureAuthTokens();
            var url = "me/following?type=user";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(new { ids = userIds }), Encoding.UTF8, "application/json");
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnfollowUsers(List<string> userIds)
        {
            if (userIds.Count > 50)
            {
                throw new ArgumentException("You can only unfollow up to 50 users at a time.");
            }
            await EnsureAuthTokens();
            var url = "me/following?type=user";
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(new { ids = userIds }), Encoding.UTF8, "application/json");
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }


        public async Task<List<bool>> CheckIfUserFollowsUsers(List<string> userIds)
        {
            await EnsureAuthTokens();
            if (userIds.Count > 50)
            {
                throw new ArgumentException("You can only check up to 50 users at a time.");
            }
            var ids = string.Join(",", userIds);
            var url = $"me/following/contains?type=user&ids={ids}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<bool>>(json);

        }


        public async Task<bool> CheckIfUserFollowsPlaylist(string playlistId)
        {
            await EnsureAuthTokens();
            var url = $"playlists/{playlistId}/followers/contains";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<bool>>(json).FirstOrDefault();
        }

    }

   
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeRange
    {
        [EnumMember(Value = "short_term")]
        ShortTerm, // Approximately last 4 weeks

        [EnumMember(Value = "medium_term")]
        MediumTerm, // Approximately last 6 months

        [EnumMember(Value = "long_term")]
        LongTerm // Approximately last 1 year
    }
}
