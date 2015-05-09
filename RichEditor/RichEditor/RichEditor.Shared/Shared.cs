using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;

public class Shared
{
    public delegate void OpenedEvent(string value);
    public static event OpenedEvent Opened;

    private static string data = string.Empty;

    private void focus(ref RichEditBox display)
    {
        display.Focus(FocusState.Keyboard);
    }

    private static async void read(StorageFile file)
    {
        try
        {
            if (file != null)
            {
                if (Opened != null)
                {
                    Opened(await FileIO.ReadTextAsync(file));
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

    public bool Bold(ref RichEditBox display)
    {
        display.Document.Selection.CharacterFormat.Bold = Windows.UI.Text.FormatEffect.Toggle;
        focus(ref display);
        return display.Document.Selection.CharacterFormat.Bold.Equals(Windows.UI.Text.FormatEffect.On);
    }

    public bool Italic(ref RichEditBox display)
    {
        display.Document.Selection.CharacterFormat.Italic = Windows.UI.Text.FormatEffect.Toggle;
        focus(ref display);
        return display.Document.Selection.CharacterFormat.Italic.Equals(Windows.UI.Text.FormatEffect.On);
    }

    public bool Underline(ref RichEditBox display)
    {
        display.Document.Selection.CharacterFormat.Underline =
            display.Document.Selection.CharacterFormat.Underline.Equals(Windows.UI.Text.UnderlineType.Single) ?
            Windows.UI.Text.UnderlineType.None : Windows.UI.Text.UnderlineType.Single;
        display.Document.Selection.CharacterFormat.Italic = Windows.UI.Text.FormatEffect.Toggle;
        focus(ref display);
        return display.Document.Selection.CharacterFormat.Underline.Equals(Windows.UI.Text.UnderlineType.Single);
    }

    public bool Left(ref RichEditBox display)
    {
        display.Document.Selection.ParagraphFormat.Alignment = Windows.UI.Text.ParagraphAlignment.Left;
        focus(ref display);
        return display.Document.Selection.ParagraphFormat.Alignment.Equals(Windows.UI.Text.ParagraphAlignment.Left);
    }

    public bool Centre(ref RichEditBox display)
    {
        display.Document.Selection.ParagraphFormat.Alignment = Windows.UI.Text.ParagraphAlignment.Center;
        focus(ref display);
        return display.Document.Selection.ParagraphFormat.Alignment.Equals(Windows.UI.Text.ParagraphAlignment.Center);
    }

    public bool Right(ref RichEditBox display)
    {
        display.Document.Selection.ParagraphFormat.Alignment = Windows.UI.Text.ParagraphAlignment.Right;
        focus(ref display);
        return display.Document.Selection.ParagraphFormat.Alignment.Equals(Windows.UI.Text.ParagraphAlignment.Right);
    }

    public void Size(ref RichEditBox display, ref ComboBox value)
    {
        if (value != null)
        {
            string selected = ((ComboBoxItem)value.SelectedItem).Tag.ToString();
            display.Document.Selection.CharacterFormat.Size = float.Parse(selected);
            focus(ref display);
        }
    }

    public void Colour(ref RichEditBox display, ref ComboBox value)
    {
        if (value != null)
        {
            string selected = ((ComboBoxItem)value.SelectedItem).Tag.ToString();
            display.Document.Selection.CharacterFormat.ForegroundColor = Color.FromArgb(
                Byte.Parse(selected.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                Byte.Parse(selected.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                Byte.Parse(selected.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                Byte.Parse(selected.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
            focus(ref display);
        }
    }

    public void Set(ref RichEditBox display, string value)
    {
        display.Document.SetText(TextSetOptions.FormatRtf, value);
        focus(ref display);
    }

    public string Get(ref RichEditBox display)
    {
        string value = string.Empty;
        display.Document.GetText(TextGetOptions.FormatRtf, out value);
        return value;
    }

    public async void New(RichEditBox display)
    {
        if (await Confirm("Create New Document?", "Rich Editor", "Yes", "No"))
        {
            Set(ref display, string.Empty);
        }
    }

    public async void Open()
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".rtf");
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

    public async void Save(string value)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });
            picker.DefaultFileExtension = ".rtf";
            picker.SuggestedFileName = "Document";
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
