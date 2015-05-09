using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

public class Shared
{
    public delegate void OpenedEvent(string value);
    public static event OpenedEvent Opened;

    private static string data = string.Empty;

    public async Task<bool> Confirm(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    public async void New(TextBox display)
    {
        if (await Confirm("Create New Document?", "Text Editor", "Yes", "No"))
        {
            display.Text = string.Empty;
        }
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

    public async void Open()
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");
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
            picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            picker.DefaultFileExtension = ".txt";
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
