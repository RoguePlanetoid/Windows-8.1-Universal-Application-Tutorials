﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

public class Shared
{
    public delegate void OpenedEvent(List<CheckBox> value);
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

    public void Add(ref ListBox display, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                CheckBox item = new CheckBox();
                item.Content = value;
                item.Foreground = display.Foreground;
                if (display.SelectedIndex > -1)
                {
                    // Insert after Selected
                    display.Items.Insert(display.SelectedIndex, item);
                }
                else
                {
                    // Add after End
                    display.Items.Add(item);
                }
            }
            catch
            {

            }
            display.Focus(FocusState.Keyboard);
        }
    }

    public void Remove(ref ListBox display)
    {
        if (display.SelectedIndex > -1)
        {
            display.Items.RemoveAt(display.SelectedIndex);
        }
    }

    public async void New(ListBox display)
    {
        if (await Confirm("Create New List?", "Task Editor", "Yes", "No"))
        {
            display.Items.Clear();
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
                    string value = await FileIO.ReadTextAsync(file);
                    List<CheckBox> list = new List<CheckBox>();
                    XElement xml = XElement.Parse(value);
                    if (xml.Name.LocalName == "tasklist")
                    {
                        foreach (XElement task in xml.Descendants("task"))
                        {
                            CheckBox item = new CheckBox();
                            item.IsChecked = task.FirstAttribute.Value.ToLower() == "checked";
                            item.Content = task.Value;
                            list.Add(item);
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

    public async void Open()
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".tsk");
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

    public async void Save(ListBox display)
    {
        try
        {
            XElement items = new XElement("tasklist");
            foreach (CheckBox item in display.Items)
            {
                items.Add(new XElement("task", item.Content, new XAttribute("value",
                ((bool)item.IsChecked ? "checked" : "unchecked"))));
            }
            string value = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), items).ToString();
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Task List", new List<string>() { ".tsk" });
            picker.DefaultFileExtension = ".tsk";
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
