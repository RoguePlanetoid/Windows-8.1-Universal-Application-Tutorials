using Windows.Storage;

public class Shared
{
    public string LoadSetting(string key)
    {
        if (ApplicationData.Current.LocalSettings.Values[key] != null)
        {
            return ApplicationData.Current.LocalSettings.Values[key].ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    public void SaveSetting(string key, string value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = value;
    }
}

