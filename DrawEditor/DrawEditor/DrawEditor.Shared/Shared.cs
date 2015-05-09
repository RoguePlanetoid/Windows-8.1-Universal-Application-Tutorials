using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Shared
{
    public struct Ink
    {
        public int Size;
        public Color Colour;
        public Point Point;
    }

    public delegate void OpenedEvent(List<Ink> value);
    public static event OpenedEvent Opened;

    private static string data = string.Empty;
    private static List<Ink> list = new List<Ink>();

    private void add(ref Canvas display, Ink item)
    {
        Ellipse circle = new Ellipse();
        circle.Height = item.Size;
        circle.Width = item.Size;
        circle.Fill = new SolidColorBrush(item.Colour);
        Canvas.SetTop(circle, item.Point.Y);
        Canvas.SetLeft(circle, item.Point.X);
        display.Children.Add(circle);
    }

    private void display(ref Canvas display, bool last = false)
    {
        if (last)
        {
            add(ref display, list.LastOrDefault());
        }
        else
        {
            foreach (Ink item in list)
            {
                add(ref display, item);
            }
        }
    }

    private static string pointToString(Point value)
    {
        return string.Format("{0},{1}", value.X, value.Y);
    }

    private static Point stringToPoint(string value)
    {
        return new Point(
            double.Parse(value.Split(',').First()),
            double.Parse(value.Split(',').Last()));
    }

    private static string colourToString(Color value)
    {
        return String.Format("{0:X2}{1:X2}{2:X2}{3:X2}",
            value.A, value.R, value.G, value.B);
    }

    private static Color stringToColour(string value)
    {
        return Color.FromArgb(
        Byte.Parse(value.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
        Byte.Parse(value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
        Byte.Parse(value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
        Byte.Parse(value.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
    }

    private static async void read(StorageFile file)
    {
        try
        {
            if (file != null)
            {
                if (Opened != null)
                {
                    string value = await FileIO.ReadTextAsync(file);
                    list.Clear();
                    XElement xml = XElement.Parse(value);
                    if (xml.Name.LocalName == "drawing")
                    {
                        foreach (XElement element in xml.Descendants("ink"))
                        {
                            Ink ink = new Ink();
                            ink.Size = int.Parse(element.Attribute("size").Value);
                            ink.Colour = stringToColour(element.Attribute("colour").Value);
                            ink.Point = stringToPoint(element.Attribute("point").Value);
                            list.Add(ink);
                        }
                    }
                    Opened(list);
                }
            }
        }
        catch
        {

        }
    }

    private static async void write(StorageFile file, string value)
    {
        try
        {
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, value);
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
        catch
        {

        }
    }

    public async Task<bool> Confirm(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    public void Draw(ref Canvas value, ref ComboBox size, ref ComboBox colour, PointerRoutedEventArgs args)
    {
        if (args.GetCurrentPoint(value).Properties.IsLeftButtonPressed)
        {
            Ink item = new Ink();
            string selectedSize = ((ComboBoxItem)size.SelectedItem).Tag.ToString();
            string selectedColour = ((ComboBoxItem)colour.SelectedItem).Tag.ToString();
            item.Size = int.Parse(selectedSize);
            item.Colour = stringToColour(selectedColour);
            item.Point = args.GetCurrentPoint(value).Position;
            list.Add(item);
            display(ref value, true);
        }
    }

    public void Set(ref Canvas canvas, List<Ink> value)
    {
        list = value;
        display(ref canvas);
    }

    public async void New(Canvas display)
    {
        if (await Confirm("Create New Drawing?", "Draw Editor", "Yes", "No"))
        {
            list.Clear();
            display.Children.Clear();
        }
    }

    public async void Open()
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".drw");
#if WINDOWS_PHONE_APP
            picker.PickSingleFileAndContinue();
            await Task.Delay(0);
#else
            read(await picker.PickSingleFileAsync());
#endif
        }
        catch
        {

        }
    }

    public async void Save()
    {
        try
        {
            XElement items = new XElement("drawing");
            foreach (Ink item in list)
            {
                XElement ink = new XElement("ink");
                ink.Add(new XAttribute("size", item.Size));
                ink.Add(new XAttribute("colour", colourToString(item.Colour)));
                ink.Add(new XAttribute("point", pointToString(item.Point)));
                items.Add(ink);
            }
            string value = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), items).ToString();
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Drawing", new List<string>() { ".drw" });
            picker.DefaultFileExtension = ".drw";
            picker.SuggestedFileName = "Drawing";
#if WINDOWS_PHONE_APP
            data = value;
            picker.PickSaveFileAndContinue();
            await Task.Delay(0);
#else
            write(await picker.PickSaveFileAsync(), value);
#endif
        }
        catch
        {

        }
    }

#if WINDOWS_PHONE_APP
    public static void Continue(IContinuationActivatedEventArgs args)
    {
        if (args != null)
        {
            switch (args.Kind)
            {
                case ActivationKind.PickFileContinuation:
                    read((args as FileOpenPickerContinuationEventArgs).Files[0]);
                    break;
                case ActivationKind.PickSaveFileContinuation:
                    write((args as FileSavePickerContinuationEventArgs).File, data);
                    break;
            }
        }
    }
#endif
}
