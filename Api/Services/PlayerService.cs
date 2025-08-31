using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;

namespace SpotifyEditor.Api.Services
{
    public class PlayerService : BaseService
    {

        public PlayerService(HttpClient spotifyApiClient) : base(spotifyApiClient) { }

        public async Task<NowPlayingInfo> GetPlaybackState()
        {
            await EnsureAuthTokens();


            var url = $"me/player";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var playbackState = JsonConvert.DeserializeObject<PlaybackStateResponse>(json);

            if (playbackState != null)
            {
                NowPlayingInfo info = new NowPlayingInfo()
                {
                    Device = playbackState.Device,
                    ShuffleState = playbackState.ShuffleState,
                    SmartShuffle = playbackState.SmartShuffle,
                    RepeatState = playbackState.RepeatState,
                    IsPlaying = playbackState.IsPlaying,
                    Timestamp = playbackState.Timestamp,
                    Context = playbackState.Context,
                    ProgressMs = playbackState.ProgressMs,
                    Track = playbackState.Item,
                    CurrentlyPlayingType = playbackState.CurrentlyPlayingType,
                    Resuming = playbackState.Actions?.Disallows?.Resuming ?? false
                };

                return info;
            }

            return null;


        }

        public async Task<List<Device>> GetAvailableDevices()
        {
            await EnsureAuthTokens();

            var url = $"me/player/devices";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var devicesResponse = JsonConvert.DeserializeObject<AvailableDevicesResponse>(json);



            return devicesResponse.Devices;
        }

        public async Task TransferPlayback(string deviceId, bool play = false)
        {
            await EnsureAuthTokens();
            var url = $"me/player";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var content = new
            {
                device_ids = new List<string> { deviceId },
                play
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<NowPlayingInfo> GetCurrentlyPlayingTrack()
        {

            await EnsureAuthTokens();
            var url = $"me/player/currently-playing";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var currentlyPlayingTrack = JsonConvert.DeserializeObject<CurrentlyPlayingTrackResponse>(json);

            NowPlayingInfo info = new NowPlayingInfo()
            {
                Device = currentlyPlayingTrack.Device,
                ShuffleState = currentlyPlayingTrack.ShuffleState,
                RepeatState = currentlyPlayingTrack.RepeatState,
                IsPlaying = currentlyPlayingTrack.IsPlaying,
                Timestamp = currentlyPlayingTrack.Timestamp,
                Context = currentlyPlayingTrack.Context,
                ProgressMs = currentlyPlayingTrack.ProgressMs,
                Track = currentlyPlayingTrack.Item,
                CurrentlyPlayingType = currentlyPlayingTrack.CurrentlyPlayingType
            };

            return info;
        }


        public async Task StartResumePlayback(string deviceId, string contextUri = null, List<string> uris = null, OffsetFormat offset = null, int positionMs = 0)
        {

            //uris can only list of tracks while context_uri can be a playlist or album
            //you can not use both at the same time
            //offset can oyly be used with context_uri
            //offset can be in {"position": 5} or {"uri": "spotify:track:TRACK_ID"} format
            await EnsureAuthTokens();
            var url = $"me/player/play?device_id={deviceId}";

            object content = null;

            if (uris == null && contextUri == null)
            {
                throw new ArgumentException("Either uris or context_uri must be provided.");
            }
            if (uris != null && contextUri != null)
            {
                throw new ArgumentException("You cannot use both uris and context_uri at the same time.");
            }
            if (contextUri != null)
            {
                object offsetType = null;

                if (offset != null)
                {
                    if (offset.Uri != null)
                    {
                        offsetType = new { uri = offset.Uri };
                    }
                    else if (offset.Position != null)
                    {
                        offsetType = new { position = offset.Position };
                    }
                }

                content = new
                {
                    context_uri = contextUri,
                    offset = offsetType,
                    position_ms = positionMs
                };
            }
            else if (uris != null)
            {
                content = new
                {
                    uris = uris,
                    position_ms = positionMs
                };
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(content, settings);

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

        }



        public async Task PausePlayback(string deviceId)
        {
            await EnsureAuthTokens();
            var url = $"me/player/pause?device_id={deviceId}";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();



        }

        public async Task SkipToNext(string deviceId)
        {
            await EnsureAuthTokens();
            var url = $"me/player/next?device_id={deviceId}";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task SkipToPrevious(string deviceId)
        {
            await EnsureAuthTokens();
            var url = $"me/player/previous?device_id={deviceId}";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }


        public async Task SeekThePosition(int positionMs, string deviceId = null)
        {
            //if device id is not supplied targets the current device
            //if position ms is too much for the track player goes to next track
            await EnsureAuthTokens();
            var url = $"me/player/seek?position_ms={positionMs}";

            if (deviceId != null) { url += $"&device_id={deviceId}"; }

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task SetRepeatMode(RepeatState state, string deviceId = null)
        {
            //track will repeat the current track.
            //context will repeat the current context.
            //off will turn repeat off.

            await EnsureAuthTokens();
            var url = $"me/player/repeat?state={state.ToString().ToLowerInvariant()}";

            if (deviceId != null) { url += $"&device_id={deviceId}"; }

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }


        public async Task SetPlaybackVolume(int volumePercent, string deviceId = null)
        {
            //volume percent must be between 0 and 100
            if (volumePercent < 0 || volumePercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(volumePercent), "Volume percent must be between 0 and 100.");
            }
            await EnsureAuthTokens();
            var url = $"me/player/volume?volume_percent={volumePercent}";
            if (deviceId != null) { url += $"&device_id={deviceId}"; }
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task ToggleShuffleMode(bool state, string deviceId = null)
        {
            //true will enable shuffle, false will disable it.
            await EnsureAuthTokens();
            var url = $"me/player/shuffle?state={state.ToString().ToLowerInvariant()}";
            if (deviceId != null) { url += $"&device_id={deviceId}"; }
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Track>> GetRecentlyPlayedTracks(int? limit)
        {
            if (limit == null)
            {
                limit = 49;
            }

            if (limit <= 0 || limit > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 50.");
            }

            List<Track> tracks = new List<Track>();
            await EnsureAuthTokens();


            var url = $"me/player/recently-played?limit={limit}";


            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var recentlyPlayedResponse = JsonConvert.DeserializeObject<RecentlyPlayedTracksResponse>(json);

            foreach (var item in recentlyPlayedResponse.Items)
            {
                tracks.Add(item.Track);
            }

            return tracks;

        }


        public async Task<List<Track>> GetUsersQueue()
        {
            await EnsureAuthTokens();
            var url = $"me/player/queue";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var queueResponse = JsonConvert.DeserializeObject<UsersQueueResponse>(json);

            return queueResponse.Queue;

        }

        public async Task AddTrackToQueue(string uri, string deviceId = null)
        {
            //uri must be a track
            await EnsureAuthTokens();
            var url = $"me/player/queue?uri=spotify%3Atrack%3A{HttpUtility.UrlEncode(uri)}";
            if (deviceId != null) { url += $"&device_id={deviceId}"; }
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", base._userToken);
            var response = await _spotifyApiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();


        }

    }
}
