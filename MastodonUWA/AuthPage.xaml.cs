using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using MastodonAPI;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace MastodonUWA
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string servername = ServerBox.Text;
            ApplicationTokenClass apptoken = new ApplicationTokenClass(servername, "MastodonClient", "read write follow", "urn:ietf:wg:oauth:2.0:oob");
            AuthenticateClass oauth_token = new AuthenticateClass(apptoken, UserNameBox.Text, PasswordHereBox.Password);
            if (apptoken.client_id == null)
            {
                FailedTextBox.Text = "Login failed.";
                return;
            }
            if (oauth_token.token == null)
            {
                FailedTextBox.Text = "Login failed.";
                return;
            }
            var clientid = await ApplicationData.Current.LocalFolder.CreateFileAsync("apptoken_clientid.txt");
            var clientsecret = await ApplicationData.Current.LocalFolder.CreateFileAsync("apptoken_clientsecret.txt");
            var authfile = await ApplicationData.Current.LocalFolder.CreateFileAsync("auth.txt");
            await FileIO.WriteTextAsync(clientid, apptoken.client_id);
            await FileIO.WriteTextAsync(clientsecret, apptoken.client_secret);
            await FileIO.WriteTextAsync(authfile, oauth_token.token);
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }
    }
}
