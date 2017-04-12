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
        IAsyncAction tootrefresh;
        dynamic toot;

        public Toot()
        {
            this.InitializeComponent();
        }
        public Toot(dynamic status, int principal)
        {
            this.InitializeComponent();
            toot = status;
            string acct = status.account.acct;
            string dname = status.account.display_name;
            string content = status.content;
            string avatar = status.account.avatar;
            string id = ((int)status.id).ToString();
            string reblogged = status.reblogged;
            string favourited = status.favourited;
            if (reblogged == "1")
            {
                Retoot.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            if (favourited == "1")
            {
                Favorites.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
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
            if (principal != 1)
            {
                var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
                // doesn't work
                if (!isDark)
                {
                    Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
                }
                else
                {
                    Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                }
            }
            else
            {
                TootContents.NavigationCompleted -= TootContents_NavigationCompleted;
                TootContents.NavigationCompleted += TootContents_NavigationCompletedPrimaryToot;
                RecWeb.Visibility = Visibility.Collapsed;
            }
        }
        public Toot(dynamic status)
        {
            this.InitializeComponent();
            toot = status;
            string acct = status.account.acct;
            string dname = status.account.display_name;
            string content = status.content;
            string avatar = status.account.avatar;
            string id = ((int)status.id).ToString();
            string reblogged = status.reblogged;
            string favourited = status.favourited;
            if (reblogged == "1")
            {
                Retoot.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            if (favourited == "1")
            {
                Favorites.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
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
            if (tootrefresh != null)
            {
                tootrefresh.Cancel();
            }
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TootDetails), toot);
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TootDetails), toot);
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
            StatusClass.boost_toot(toot_id, GetToken.getAuthClass());
            Retoot.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
        }

        private void Favorites_Click(object sender, RoutedEventArgs e)
        {
            StatusClass.favourite_toot(toot.id, GetToken.getAuthClass());
            Favorites.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
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
        private async void RecWeb_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
        private async void TootContents_NavigationCompletedPrimaryToot(WebView sender, WebViewNavigationCompletedEventArgs
args)
        {
            return;
        }

        private async void ImageShow_Click(object sender, RoutedEventArgs e)
        {
            dynamic st = StatusClass_new.GetStatus(GetToken.getAuthClass(), toot);
            var contentDialog = new ShowImage_Toot(st);
            await contentDialog.ShowAsync();
        }
    }
}

