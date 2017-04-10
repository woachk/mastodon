using MastodonAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MastodonUWA
{
    public sealed partial class Toot : UserControl
    {
        public string toot_id;
        public string content;
        string name;
        string displayname;
        string avatarpath;
        string reblog;
        string fav;
        public Toot()
        {
            this.InitializeComponent();
        }
        public Toot(string username, string display_name, string contents, string avatar, string id, string reblogged, string favourited)
        {
            this.InitializeComponent();
            content = contents;
            string htcontent = WebContentHelper.WrapHtml(content, TootContents.Width, TootContents.Height);
            TootContents.NavigateToString(htcontent);
            UserName.Text = display_name + "\n" + "@" + username;
            toot_id = id;
            reblog = reblogged;
            fav = favourited;
            name = username;
            displayname = display_name;
            avatarpath = avatar;
            if (reblogged != null)
            {
                Retoot.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            if (favourited != null)
            {
                Favorites.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
            }
            if (avatar != null)
            {
                if (avatar[0] == 'h') // HACK!!!
                {
                    UserImage.Source = new BitmapImage(new Uri(avatar));
                }
                else
                {
                    UserImage.Source = new BitmapImage(new Uri("https://" + MainPage.getServerName() + avatar));
                }
            }
            TootContents.Settings.IsJavaScriptEnabled = false;
            TootContents.Settings.IsIndexedDBEnabled = false;
        }

        private async void Answer_Click(object sender, RoutedEventArgs e)
        {
            var contentDialog = new WritingToot(toot_id);
            await contentDialog.ShowAsync();
        }

        private void TootContents_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            StatusClass status = new StatusClass();
            status.id = toot_id;
            status.content = content;
            status.account.acct = name;
            status.account.display_name = displayname;
            status.account.avatar = avatarpath;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TootDetails), status);
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StatusClass status = new StatusClass();
            status.account = new AccountClass();
            status.id = toot_id;
            status.reblogged = reblog;
            status.favourited = fav;
            status.content = content;
            status.account.acct = name;
            status.account.display_name = displayname;
            status.account.avatar = avatarpath;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TootDetails), status);
        }

        private void TootContents_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri == null)
            {

            }
            else
            {
                args.Cancel = true;
            }
        }

        private void Retoot_Click(object sender, RoutedEventArgs e)
        {
            var authfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            authfile.AsTask().Wait();
            var tokenfile = authfile.GetResults();
            var ioop = FileIO.ReadTextAsync(tokenfile);
            AuthenticateClass token = new AuthenticateClass();
            ioop.AsTask().Wait();
            token.token = ioop.GetResults();
            token.server = MainPage.getServerName();
            StatusClass.boost_toot(toot_id, token);
            Retoot.Background = new SolidColorBrush(Windows.UI.Colors.Green);
        }

        private void Favorites_Click(object sender, RoutedEventArgs e)
        {
            var authfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            authfile.AsTask().Wait();
            var tokenfile = authfile.GetResults();
            var ioop = FileIO.ReadTextAsync(tokenfile);
            AuthenticateClass token = new AuthenticateClass();
            ioop.AsTask().Wait();
            token.token = ioop.GetResults();
            token.server = MainPage.getServerName();
            StatusClass.favourite_toot(toot_id, token);
            Favorites.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
        }
        private async void TootContents_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs
args)
        {
            string returnStr = await
           TootContents.InvokeScriptAsync("eval", new string[] {
SetBodyOverFlowHiddenString });
        }
        string SetBodyOverFlowHiddenString =
       @"function SetBodyOverFlowHidden()
        {
            document.body.style.overflow =
       'hidden';
            return 'Set Style to hidden';
        }
        // now call the function!
        SetBodyOverFlowHidden();";
    }
}
