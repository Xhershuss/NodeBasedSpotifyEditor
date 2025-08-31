using SpotifyEditor.Api.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Api.Services
{
    public abstract class BaseService
    {
        protected readonly HttpClient _spotifyApiClient;
        protected readonly SpotifyAuthApi _spotifyAuthApi;

        protected readonly string _userToken;
        protected BaseService(HttpClient spotifyApiClient)
        {
            _spotifyApiClient = spotifyApiClient;
            _spotifyAuthApi = SpotifyAuthApi.GetInstance;
            _userToken = _spotifyAuthApi.RequestUserToken();
        }

        protected async Task EnsureAuthTokens()
        {
            await _spotifyAuthApi.EnsureTokens();
        }


    }
}
