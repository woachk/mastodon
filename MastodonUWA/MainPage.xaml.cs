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
            AuthenticateClass token = new AuthenticateClass();
            token.appname = null;
            var authfile = ApplicationData.Current.LocalFolder.GetFileAsync("auth.txt");
            authfile.AsTask().Wait();
            var tokenfile = authfile.GetResults();
            var ioop = FileIO.ReadTextAsync(tokenfile);
            ioop.AsTask().Wait();
            token.token = ioop.GetResults();
            token.server = getServerName();
            List<StatusClass> tootlist= StatusClass.getTimeline(token);
            for (int i = 0;  i < tootlist.Count; i++)
            {
                Toot toot;
                if (tootlist[i].account.acct != null)
                {
                    toot = new Toot(tootlist[i].account.acct, tootlist[i].account.display_name, tootlist[i].content, tootlist[i].account.avatar, tootlist[i].uri);
                    Toots.Children.Add(toot);
                }
            }
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
}
