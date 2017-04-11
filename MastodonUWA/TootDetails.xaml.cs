using MastodonAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238
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
namespace MastodonUWA
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class TootDetails : Page
    {
        public TootDetails()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            if (isDark)
            {
                SPanel.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            }
            StatusClass toot_id = (StatusClass)e.Parameter;
            var authfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            authfile.AsTask().Wait();
            var tokenfile = authfile.GetResults();
            var ioop = FileIO.ReadTextAsync(tokenfile);
            AuthenticateClass token = new AuthenticateClass();
            ioop.AsTask().Wait();
            token.token = ioop.GetResults();
            token.server = MainPage.getServerName();
            List<StatusClass>[] statuses = StatusClass.GetStatusContext(token, toot_id);
            List<StatusClass> tootlist = statuses[0];
            for (int i = 0; i < tootlist.Count; i++)
            {
                Toot toot;
                if (tootlist[i].account.acct != null)
                {
                    toot = new Toot(tootlist[i].account.acct, tootlist[i].account.display_name, tootlist[i].content, tootlist[i].account.avatar, tootlist[i].id, tootlist[i].reblogged, tootlist[i].favourited,0);
                    TootContainer.Items.Add(toot);
                }
            }
            tootlist = statuses[1];
            //toot_id = StatusClass.GetStatus(token,toot_id);
            Toot firstoot = new Toot(toot_id.account.acct, toot_id.account.display_name, toot_id.content, toot_id.account.avatar, toot_id.id, toot_id.reblogged, toot_id.favourited,1);
            firstoot.Height = 300;
            TootContainer.Items.Add(firstoot);
            for (int i = 0; i < tootlist.Count; i++)
            {
                Toot toot;
                if (tootlist[i].account.acct != null)
                {
                    toot = new Toot(tootlist[i].account.acct, tootlist[i].account.display_name, tootlist[i].content, tootlist[i].account.avatar, tootlist[i].id, tootlist[i].reblogged, tootlist[i].favourited,0);
                    TootContainer.Items.Add(toot);
                }
            }
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
        private void MenuButton4_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "PublicTimeline");
        }
        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "LocalPublicTimeline");
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "notifications");
        }

        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), "Timeline");
        }

        private void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(SettingsPage), e);
        }
        private async void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            var contentDialog = new WritingToot();
            await contentDialog.ShowAsync();
        }
    }
}
