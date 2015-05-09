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

namespace TaskEditor
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

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Shared.New(Display);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Shared.Opened += (List<CheckBox> value) =>
            {
                Display.Items.Clear();
                foreach (CheckBox item in value)
                {
                    item.Foreground = Display.Foreground;
                    Display.Items.Add(item);
                }
            };
            Shared.Open();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Shared.Save(Display);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Shared.Remove(ref Display);
        }

        private void Add_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Shared.Add(ref Display, Value.Text, e);
        }
    }
}
