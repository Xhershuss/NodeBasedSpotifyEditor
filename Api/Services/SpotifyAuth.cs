using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SpotifyEditor.Api.Listener;


namespace SpotifyEditor.Api.Auth
{
    public sealed class SpotifyAuthApi
    {
        // lazy singleton
        private static readonly Lazy<SpotifyAuthApi> _lazy = new Lazy<SpotifyAuthApi>(() => new SpotifyAuthApi());
        public static SpotifyAuthApi GetInstance => _lazy.Value;

        public HttpClient _apiClient = new HttpClient();
        private HttpClient _httpClient = new HttpClient();

        private string _clientId;
        private string _clientSecret;

        private string _accessToken;
        private DateTime _accessTokenExpireDate;

        private string _userToken;
        private DateTime _userTokenExpireDate;
        private string _refreshToken;

        private string baseUri = "https://api.spotify.com/v1/";
        private string baseUserUri = "https://api.spotify.com/v1/me/";
        private string tokenUri = "https://accounts.spotify.com/api/token";
        public string redirectUri = "http://localhost:8888/callback";

        private SpotifyAuthApi() { }


        public async Task GetSpotifyClient(string clientId, string clientSecret)
        {

            _clientId = clientId;
            _clientSecret = clientSecret;

            await GetAccessToken();
            await GetUserToken();

            //Console.WriteLine("acc  " + _accessToken);
            //Console.WriteLine("user  " + _userToken);

            _apiClient.BaseAddress = new Uri(baseUri);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            //await EnsureTokens();

            //Console.WriteLine("-------------------");
            //Console.WriteLine("acc  " + _accessToken);
            //Console.WriteLine("user  " + _userToken);




        }

        internal string RequestUserToken()
        {
            return _userToken;
        }

        public void SetAuthHeader(bool useUserToken = false)
        {
            string token = (useUserToken) ? _userToken ! : _accessToken;
            string uri = (useUserToken) ? baseUserUri! : baseUri;

            _apiClient.BaseAddress = new Uri(uri);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task EnsureTokens()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                if (DateTime.UtcNow >= _accessTokenExpireDate)
                {
                    await GetAccessToken();

                }
            }
            if (!string.IsNullOrWhiteSpace(_userToken) && !string.IsNullOrWhiteSpace(_refreshToken))
            {
                if (DateTime.UtcNow >= _userTokenExpireDate)
                {
                    await RefreshUserToken();

                }
            }

        }

        private async Task RefreshUserToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, tokenUri);

            request.Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"grant_type","refresh_token" },
                    {"refresh_token", _refreshToken},
                }
                   );

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var creds = $"{_clientId}:{_clientSecret}";
            var credsIn64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(creds));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credsIn64);


            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var userTokenResponse = JsonConvert.DeserializeObject<UserTokenResponse>(json);

            _userToken = userTokenResponse.access_token;
            _userTokenExpireDate = DateTime.UtcNow.AddSeconds(userTokenResponse.expires_in - 60);
            _refreshToken = userTokenResponse.refresh_token;

        }



        private async Task GetUserToken()
        {
            //User token => 2 OAuth
            string responseType = "code";
            string[] scopes = new string[]
            {
                "user-library-read",
                "user-library-modify",
                "user-read-playback-state",
                "user-modify-playback-state",
                "user-read-recently-played",
                "user-read-currently-playing",
                "playlist-modify-public",
                "playlist-modify-private",
                "playlist-read-private",
                "playlist-read-collaborative",
                "ugc-image-upload",
                "user-top-read",
                "user-follow-read",
                "user-follow-modify"
            };
            string scope = string.Join(" ", scopes);
            string state = Guid.NewGuid().ToString();


            var query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = _clientId;
            query["response_type"] = responseType;
            query["redirect_uri"] = redirectUri;
            query["scope"] = scope;
            query["state"] = state;
            query["show_dialog"] = "true";

            string authorizeUrl = "https://accounts.spotify.com/authorize/?" + query.ToString();


            // Tarayıcıda URL'yi aç (Desktop App için)
            Process.Start(new ProcessStartInfo
            {
                FileName = authorizeUrl,
                UseShellExecute = true //varsayılan program ile açar
            });

            var listener = new CallbackListerner(redirectUri);

            var callbackQuery = new CallbackQuery();
            (callbackQuery.code, callbackQuery.state) = await listener.ListenForCallback();


            using var user_request = new HttpRequestMessage(HttpMethod.Post, tokenUri);


            user_request.Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"grant_type","authorization_code" },
                    {"code", callbackQuery.code },
                    {"redirect_uri" ,redirectUri}
                }
                   );

            var creds = $"{_clientId}:{_clientSecret}";
            var credsIn64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(creds));
            user_request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credsIn64);

            user_request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            var user_response = await _httpClient.SendAsync(user_request);
            user_response.EnsureSuccessStatusCode();

            var user_json = await user_response.Content.ReadAsStringAsync();
            var userTokenResponse = JsonConvert.DeserializeObject<UserTokenResponse>(user_json);

            _userToken = userTokenResponse.access_token;
            _userTokenExpireDate = DateTime.UtcNow.AddSeconds(userTokenResponse.expires_in - 60);
            _refreshToken = userTokenResponse.refresh_token;
        }

        private async Task GetAccessToken()
        {
            using var accessRequest = new HttpRequestMessage(HttpMethod.Post, tokenUri);

            accessRequest.Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"grant_type","client_credentials" },
                    {"client_id", _clientId },
                    {"client_secret" ,_clientSecret}
                }
                   );


            var response = await _httpClient.SendAsync(accessRequest);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var accessTokenResoponse = JsonConvert.DeserializeObject<AccessTokenResponse>(json);


            _accessToken = accessTokenResoponse.access_token;
            _accessTokenExpireDate = DateTime.UtcNow.AddSeconds(accessTokenResoponse.expires_in - 60);
        }

        internal class UserTokenResponse
        {

            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }

        }


        internal class CallbackQuery
        {
            public string code { get; set; }
            public string state { get; set; }
        }

        internal class AccessTokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }

        }


    }


   

}