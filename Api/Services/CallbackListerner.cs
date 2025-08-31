using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpotifyEditor.Api.Listener
{
    public class CallbackListerner
    {
        private string _redirectUri;

        public CallbackListerner(string redirectUri)
        {
            _redirectUri = redirectUri + "/";
        }
        public async Task<(string,string)> ListenForCallback()
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(_redirectUri);
            httpListener.Start();

            var context = await httpListener.GetContextAsync();

            
            string code = context.Request.QueryString["code"];
            string state = context.Request.QueryString["state"];
            string error = context.Request.QueryString["error"];

            if (code != null)
            {
                string responseText = @"
    <html>
        <head>
            <title>Authentication Success</title>
            <style>
                body {
                    background-color: #1DB954; 
                    color: white;
                    font-family: Arial, sans-serif;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                    margin: 0;
                }
                .container {
                    text-align: center;
                    padding: 20px 40px;
                    background-color: rgba(0,0,0,0.7);
                    border-radius: 10px;
                    box-shadow: 0 0 15px rgba(0,0,0,0.5);
                }
                h1 {
                    margin-bottom: 10px;
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Authentication Success!</h1>
                <p>You can now return to the application.</p>
            </div>
        </body>
    </html>";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
                httpListener.Stop();
                return (code, state);
            }
            else
            {
                string responseText = $@"
    <html>
        <head>
            <title>Authentication Failed</title>
            <style>
                body {{background-color: #FF4C4C; 
                    color: white;
                    font-family: Arial, sans-serif;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                    margin: 0;
                }}
                .container {{text-align: center;
                    padding: 20px 40px;
                    background-color: rgba(0,0,0,0.7);
                    border-radius: 10px;
                    box-shadow: 0 0 15px rgba(0,0,0,0.5);
                }}
                h1 {{margin-bottom: 10px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Authentication Failed!</h1>
                <p>Error: {error}</p>
                <p>Please try again.</p>
            </div>
        </body>
    </html>";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
                httpListener.Stop();
                throw new Exception($"Yetkilendirme başarısız: {error}");
            }






        }

    }
}
