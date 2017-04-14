using MastodonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mastodon.WPF
{
    /// <summary>
    /// Logique d'interaction pour Toot.xaml
    /// </summary>
    public partial class Toot : UserControl
    {
        public string toot_id;
        public string content;
        int tapdisabled = 0;
        StatusClass_new toot;
        public Toot(StatusClass_new status)
        {
            Width = 400;
            this.InitializeComponent();
            toot = status;
            string acct = status.account.acct;
            string dname = status.account.display_name;
            UserName.Text = dname + "\n" + acct;
            TootContents.NavigateToString(WebContentHelper.WrapHtml((string)status.content, 350, 400));
            string avatar = status.account.avatar;
            string reblogged = status.reblogged;
            string favourited = status.favourited;
            if (reblogged == "1")
            {
                Retoot.Foreground = new SolidColorBrush(Colors.Green);
            }
            if (favourited == "1")
            {
                Favorites.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            if (avatar != null)
            {
                if (avatar[0] == 'h') // HACK!!!
                {
                    UserImage.Source = new BitmapImage(new Uri(avatar));
                }
                else
                {
                    UserImage.Source = new BitmapImage(new Uri("https://" + GetToken.getServerName() + avatar));
                }
            }
        }
        private async void Answer_Click(object sender, RoutedEventArgs e)
        {
            /* var contentDialog = new WritingToot(toot_id);
            await contentDialog.ShowAsync(); */
        }

        private void TootContents_PointerPressed(object sender, EventArgs e)
        {
            /* Frame rootFrame = Current.Content as Frame;
            rootFrame.Navigate(typeof(TootDetails), toot); */
        }

        private void Grid_Tapped(object sender, EventArgs e)
        {

        }

        private void TootContents_NavigationStarting(object sender, NavigatingCancelEventArgs args)
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
            Retoot.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void Favorites_Click(object sender, RoutedEventArgs e)
        {
            StatusClass.favourite_toot(toot.id, GetToken.getAuthClass());
            Favorites.Foreground = new SolidColorBrush(Colors.Yellow);
        }
        private async void TootContents_NavigationCompleted(object sender, NavigationEventArgs
args)
        {
           // object returnStr =
           //TootContents.InvokeScript("eval", new string[] { SetBodyOverFlowHiddenString });
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
        private async void RecWeb_Tapped(object sender, EventArgs e)
        {
           /*  Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TootDetails), toot); */
        }

        private async void ImageShow_Click(object sender, RoutedEventArgs e)
        {
            /* StatusClass_new st = StatusClass_new.GetStatus(new HttpConnectionClass(GetToken.getAuthClass()), toot);
            var contentDialog = new ShowImage_Toot(st);
            await contentDialog.ShowAsync(); */
        }

        private void UserImage_Tapped(object sender, EventArgs e)
        {
           /* MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.Content = UserDetails;
            parentWindow.Navigate(typeof(UserDetails), toot.account); */
        }
    }
}

