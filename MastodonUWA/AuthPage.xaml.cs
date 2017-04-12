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
using Windows.Security.Authentication.Web;
/*  Copyright (C) 2017  my123 (@never_released)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
            string token = null;
            ApplicationTokenClass apptoken = new ApplicationTokenClass(servername, "Mastodon for Windows 10", "read write follow", "urn:ietf:wg:oauth:2.0:oob");
            string starturi = "https://" + ServerBox.Text + "/oauth/authorize?response_type=code&client_id=" + apptoken.client_id+"&client_secret="+ apptoken.client_secret + "&redirect_uri=urn:ietf:wg:oauth:2.0:oob" + "&username=" + UserNameBox.Text +"&";
            WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None,new Uri(starturi), new System.Uri("https://"+ ServerBox.Text + "/oauth/authorize/"));
            if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                token = WebAuthenticationResult.ResponseData.ToString();
                string[] tokensplit = token.Split('/');
                token = tokensplit[tokensplit.Length - 1];
                AuthenticateClass aclass = new AuthenticateClass(apptoken, token);
                token = aclass.token;
            }
            else
            {
                return; // login failure
            }
            var clientid = await ApplicationData.Current.LocalFolder.CreateFileAsync("apptoken_clientid.txt");
            var clientsecret = await ApplicationData.Current.LocalFolder.CreateFileAsync("apptoken_clientsecret.txt");
            var authfile = await ApplicationData.Current.LocalFolder.CreateFileAsync("auth.txt");
            var serverfile = await ApplicationData.Current.LocalFolder.CreateFileAsync("server.txt");
            await FileIO.WriteTextAsync(clientid, apptoken.client_id);
            await FileIO.WriteTextAsync(clientsecret, apptoken.client_secret);
            await FileIO.WriteTextAsync(authfile, token);
            await FileIO.WriteTextAsync(serverfile, servername);
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }
    }
}
