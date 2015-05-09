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

namespace MediaPlayer
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
            Shared.Init();
            Shared.Playing += () =>
            {
                Position.Value = (int)Display.Position.TotalMilliseconds;
            };
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        public Shared Shared = new Shared();

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Display.CurrentState == MediaElementState.Playing)
            {
                Display.Pause();
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
            }
            else
            {
                Display.Play();
                Play.Icon = new SymbolIcon(Symbol.Pause);
                Play.Label = "Pause";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Display.Stop();
        }

        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Shared.Go(ref Display, Value.Text, e);
        }

        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Volume != null)
            {
                Display.Volume = (double)Volume.Value;
            }
        }

        private void Display_MediaOpened(object sender, RoutedEventArgs e)
        {
            Position.Maximum = (int)Display.NaturalDuration.TimeSpan.TotalMilliseconds;
            Display.Play();
            Play.Icon = new SymbolIcon(Symbol.Pause);
            Play.Label = "Pause";
        }

        private void Display_MediaEnded(object sender, RoutedEventArgs e)
        {
            Play.Icon = new SymbolIcon(Symbol.Play);
            Play.Label = "Play";
            Display.Stop();
            Position.Value = 0;
        }

        private void Display_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Shared.Timer(Display.CurrentState == MediaElementState.Playing);
        }
    }
}
