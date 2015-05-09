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

namespace VideoRecorder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        public Shared Shared = new Shared();

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            if (Shared.Recording)
            {
                Preview.Source = null;
                Shared.Stop();
                Record.Icon = new SymbolIcon(Symbol.Video);
            }
            else
            {
                Display.Source = null;
                Shared.Record(Preview);
                Record.Icon = new SymbolIcon(Symbol.VideoChat);
            }
        }

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            await Shared.Play(Dispatcher, Display);
        }
    }
}
