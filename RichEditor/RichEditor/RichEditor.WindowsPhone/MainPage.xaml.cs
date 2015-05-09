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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RichEditor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        public Shared Shared = new Shared();

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            Bold.IsChecked = Shared.Bold(ref Display);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            Italic.IsChecked = Shared.Italic(ref Display);
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            Underline.IsChecked = Shared.Underline(ref Display);
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = Shared.Left(ref Display);
            Centre.IsChecked = false;
            Right.IsChecked = false;
        }

        private void Centre_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = false;
            Centre.IsChecked = Shared.Centre(ref Display);
            Right.IsChecked = false;
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = false;
            Centre.IsChecked = false;
            Right.IsChecked = Shared.Right(ref Display);
        }

        private void Size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Shared.Size(ref Display, ref Size);
        }

        private void Colour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Shared.Colour(ref Display, ref Colour);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Shared.New(Display);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Shared.Opened += (string value) =>
            {
                Shared.Set(ref Display, value);
            };
            Shared.Open();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Shared.Save(Shared.Get(ref Display));
        }
    }
}
