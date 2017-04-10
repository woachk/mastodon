using MastodonAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public Toot()
        {
            this.InitializeComponent();
        }
        public Toot(string username, string display_name, string contents, string avatar, string id)
        {
            this.InitializeComponent();
            content = contents;
            TootContents.NavigateToString(contents);
            UserName.Text = display_name + "\n" + "@" + username;
            toot_id = id;
            name = username;
            displayname = display_name;
            avatarpath = avatar;
            if (avatar[0] == 'h') // HACK!!!
            {
                UserImage.Source = new BitmapImage(new Uri(avatar));
            }
            else
            {
                UserImage.Source = new BitmapImage(new Uri("https://" + MainPage.getServerName() + avatar));
            }
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
            status.content = content;
            status.account.acct = name;
            status.account.display_name = displayname;
            status.account.avatar = avatarpath;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TootDetails), status);
        }
    }
}
