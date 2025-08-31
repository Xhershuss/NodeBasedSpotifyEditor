using SpotifyEditor.Api.Auth;
using System.Configuration;
using System.Data;
using System.Windows;

using SpotifyEditor.Api.Auth;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Api.Models.Common;
using SpotifyEditor.Api.Services;

using SpotifyEditor.Model.Nodes;



namespace SpotifyEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        protected override async void OnStartup(StartupEventArgs e)
        {

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            base.OnStartup(e);


            var clientId = ConfigurationManager.AppSettings["SpotifyClientId"];
            var clientSecret = ConfigurationManager.AppSettings["SpotifyClientSecret"];

            var SpotifyApiClient = SpotifyAuthApi.GetInstance;
            BaseNodeModel.SpotifyApiClient = SpotifyApiClient;

            await SpotifyApiClient.GetSpotifyClient(clientId, clientSecret);

          
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Beklenmeyen bir hata oluştu: {e.Exception.Message}");
            e.Handled = true; 
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Unhandled exception: {(e.ExceptionObject as Exception)?.Message}");
        }



    }

}
