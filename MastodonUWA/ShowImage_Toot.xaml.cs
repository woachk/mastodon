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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Boîte de dialogue de contenu, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace MastodonUWA
{
    public sealed partial class ShowImage_Toot : ContentDialog
    {
        public ShowImage_Toot()
        {
            this.InitializeComponent();
        }
        public ShowImage_Toot(dynamic toot)
        {
            this.InitializeComponent();
            if (toot.media_attachments.Count == 0)
            {
                Title = "No image attached to this toot";
            }
            else
            {
                TootImage.Source = new BitmapImage(new Uri(toot.media_attachments.preview_url));
            }
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }
    }
}
