using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
namespace MastodonUWA
{
    public sealed partial class SettingsPage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            if (isDark)
            {
                SPanel.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            }
        }
            public SettingsPage()
        {
            this.InitializeComponent();
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

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            var files = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFilesAsync();
            for (int i = 0; i < files.Count; i++)
            {
                await files[i].DeleteAsync();
            }
            Application.Current.Exit();
        }
    }
}
