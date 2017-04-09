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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
using MastodonAPI;

namespace MastodonUWA
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string getServerName()
        {
            var serverfile = ApplicationData.Current.LocalFolder.GetFileAsync("server.txt");
            serverfile.AsTask().Wait();
            var serverfile_o = serverfile.GetResults();
            var ioop = FileIO.ReadTextAsync(serverfile_o);
            ioop.AsTask().Wait();
            return ioop.GetResults();
        }
        public MainPage()
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var settings = (string)e.Parameter;
            AuthenticateClass token = new AuthenticateClass();
            token.appname = null;
            var authfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            authfile.AsTask().Wait();
            var tokenfile = authfile.GetResults();
            var ioop = FileIO.ReadTextAsync(tokenfile);
            ioop.AsTask().Wait();
            token.token = ioop.GetResults();
            token.server = getServerName();
            List<StatusClass> tootlist = null;
            if (settings != "notifications")
            {
                if (settings == "PublicTimeline")
                {
                    tootlist = StatusClass.getPublicTimeline(token);
                }
                else if (settings == "LocalPublicTimeline")
                {
                    tootlist = StatusClass.getLocalPublicTimeline(token);
                }
                else
                {
                    tootlist = StatusClass.getTimeline(token);
                }
                for (int i = 0; i < tootlist.Count; i++)
                {
                    Toot toot;
                    if (tootlist[i].account.acct != null)
                    {
                        toot = new Toot(tootlist[i].account.acct, tootlist[i].account.display_name, tootlist[i].content, tootlist[i].account.avatar, tootlist[i].uri);
                        TootContainer.Items.Add(toot);
                    }
                }
            }
            else
            {
                List<NotificationClass> notifications = NotificationClass.getNotifications(token);
                for (int i = 0; i < notifications.Count; i++)
                {
                    if (notifications[i].id != null)
                    {
                        if (notifications[i].type == "favourite")
                        {
                            TextBlock block = new TextBlock();
                            Toot toot;
                            block.Text = notifications[i].account.display_name + " favourited your post.";
                            toot = new Toot(notifications[i].account.acct, notifications[i].account.display_name, notifications[i].status.content, notifications[i].account.avatar, notifications[i].status.uri);
                            TootContainer.Items.Add(block);
                            TootContainer.Items.Add(toot);
                        }
                        if (notifications[i].type == "reblog")
                        {
                            TextBlock block = new TextBlock();
                            Toot toot;
                            block.Text = notifications[i].account.display_name + " boosted your post.";
                            toot = new Toot(notifications[i].account.acct, notifications[i].account.display_name, notifications[i].status.content, notifications[i].account.avatar, notifications[i].status.uri);
                            TootContainer.Items.Add(block);
                            TootContainer.Items.Add(toot);
                        }
                        if (notifications[i].type == "follow")
                        {
                            TextBlock block = new TextBlock();
                            block.Text = notifications[i].account.display_name + " now follows you.";
                            TootContainer.Items.Add(block);
                        }
                        if (notifications[i].type == "mention")
                        {
                            Toot toot;
                            toot = new Toot(notifications[i].account.acct, notifications[i].account.display_name, notifications[i].status.content, notifications[i].account.avatar, notifications[i].status.uri);
                            TootContainer.Items.Add(toot);
                        }
                    }
                }
            }
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
    }
}
