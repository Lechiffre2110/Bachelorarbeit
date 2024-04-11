using UnityEngine;

public class Settings
{
    private static readonly object _lock = new object();
    private static Settings _instance;
    public static Settings Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
    }

    public string Year { get; private set; } = "2021";
    public string SocialStatus { get; private set; } = "mittel";
    public bool DisplayPoverty { get; private set; } = true;
    public bool DisplayUnemploymentRate { get; private set; } = true;
    public bool DisplayChildPoverty { get; private set; } = true;
    public bool OnboardingCompleted { get; private set; } = false;

    private Settings()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("Year", Year);
        PlayerPrefs.SetString("SocialStatus", SocialStatus);
        PlayerPrefs.SetInt("DisplayPoverty", DisplayPoverty ? 1 : 0);
        PlayerPrefs.SetInt("DisplayUnemploymentRate", DisplayUnemploymentRate ? 1 : 0);
        PlayerPrefs.SetInt("DisplayChildPoverty", DisplayChildPoverty ? 1 : 0);
        PlayerPrefs.SetInt("OnboardingCompleted", OnboardingCompleted ? 1 : 0);
    }

    public void LoadSettings()
    {
        Year = PlayerPrefs.GetString("Year", "2021");
        SocialStatus = PlayerPrefs.GetString("SocialStatus", "mittel");
        DisplayPoverty = PlayerPrefs.GetInt("DisplayPoverty", 1) == 1;
        DisplayUnemploymentRate = PlayerPrefs.GetInt("DisplayUnemploymentRate", 1) == 1;
        DisplayChildPoverty = PlayerPrefs.GetInt("DisplayChildPoverty", 1) == 1;
        OnboardingCompleted = PlayerPrefs.GetInt("OnboardingCompleted", 0) == 1;
    }

    public void UpdateToggleSettings(bool displayPoverty, bool displayChildPoverty, bool displayUnemploymentRate)
    {
        DisplayPoverty = displayPoverty;
        DisplayUnemploymentRate = displayUnemploymentRate;
        DisplayChildPoverty = displayChildPoverty;
        SaveSettings();
    }

    public void UpdateYear(string year)
    {
        Year = year;
        SaveSettings();
    }

    public void UpdateSocialStatus(string socialStatus)
    {
        SocialStatus = socialStatus;
        SaveSettings();
    }

    public void OnOnboardingCompleted()
    {
        OnboardingCompleted = true;
        SaveSettings();
    }
}