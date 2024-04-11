using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private VisualizationContainer _visualizationContainer;

    [SerializeField] private DataMenu _dataMenu;
    [SerializeField] private LocationsMenu _locationsMenu;
    [SerializeField] private SettingsMenu _settingsMenu;
    [SerializeField] private HelpMenu _helpMenu;
    [SerializeField] private PersonaMenu _personaMenu;
    [SerializeField] private Image _settingsButtonImage;
    [SerializeField] private Image _locationsButtonImage;
    [SerializeField] private Image _helpButtonImage;
    [SerializeField] private Image _homeButtonImage;
    [SerializeField] private GameObject _dataContainer;
    [SerializeField] private TMPro.TextMeshProUGUI _streetNameText;
    [SerializeField] private InfoSpawner _infoSpawner;

    private Color defaultColor = new Color(0.29f, 0.29f, 0.29f, 1f);
    private Color highlightedColor = new Color(0.98f, 0.13f, 0.23f, 1f);

    public delegate void StartOnboarding();
    public static event StartOnboarding OnStartOnboarding;

    //Observer pattern implementation
    private List<IViewObserver> _observers = new List<IViewObserver>();
    public void Attach(IViewObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IViewObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.OnViewChanged();
        }
    }

    void Start()
    {
        Onboarding.OnShowDataContainer += ShowDataContainer;
        InfoSphere.OnOpenPersonaMenu += TogglePersonaMenu;
        SettingsMenu.OnCloseSettings += ToggleSettingsMenu;
        SettingsMenu.OnSaveSettings += UpdateDisplayedTrafficLights;
        VisualizationContainer.OnDataYearChanged += NotifyObservers;
        if (Settings.Instance.OnboardingCompleted == false) 
        {
            RestartOnboarding();
            Settings.Instance.OnOnboardingCompleted();
        }
        RestartOnboarding();
    }
    void OnDestroy()
    {
        Onboarding.OnShowDataContainer -= ShowDataContainer;
        InfoSphere.OnOpenPersonaMenu -= TogglePersonaMenu;
        SettingsMenu.OnCloseSettings -= ToggleSettingsMenu;
        SettingsMenu.OnSaveSettings -= UpdateDisplayedTrafficLights;
        VisualizationContainer.OnDataYearChanged -= NotifyObservers;
    }

    public void UpdateTrafficLights(float transferRate, float transferRateUnder15, float unemploymentRate)
    {
        _visualizationContainer.UpdateTrafficLights(transferRate, transferRateUnder15, unemploymentRate);
    }

    public void UpdateCityAverageBars(float transferRateCityAverage, float transferRateUnder15CityAverage, float unemploymentRateCityAverage)
    {
        _visualizationContainer.UpdateCityAverageBars(transferRateCityAverage, transferRateUnder15CityAverage, unemploymentRateCityAverage);
    }

    public void UpdateDataMenu(string zone, float transferRate, float transferRateUnder15, float unemploymentRate)
    {
        _dataMenu.UpdateDataTextFields(zone, transferRate, transferRateUnder15, unemploymentRate);
    }

    public void UpdateNearbyLocations(List<Location> nearbyLocationsList)
    {
        _locationsMenu.UpdateNearbyLocations(nearbyLocationsList);
    }

    public void UpdateStreetName(string streetName)
    {
        _streetNameText.text = streetName;    
    }

    private void CloseAllMenus()
    {
        _dataMenu.CloseMenu();
        _locationsMenu.CloseMenu();
        _settingsMenu.CloseMenu();
        _helpMenu.CloseMenu();
        _personaMenu.CloseMenu();
        HighlightClickedButton(_homeButtonImage);
    }


    public void ToggleDataMenu()
    {
        CloseAllMenus();
        _dataMenu.ToggleMenu();
        HighlightClickedButton(_homeButtonImage);
    }

    public void ToggleSettingsMenu()
    {
        CloseAllMenus();
        _settingsMenu.ToggleMenu();
        HighlightClickedButton(_settingsButtonImage);
    }

    public void ToggleLocationsMenu()
    {
        CloseAllMenus();
        _locationsMenu.ToggleMenu();
        HighlightClickedButton(_locationsButtonImage);
    }

    public void ToggleHelpMenu()
    {
        CloseAllMenus();
        _helpMenu.ToggleMenu();
        HighlightClickedButton(_helpButtonImage);
    }

    private void ClosePersonaMenu()
    {
        CloseAllMenus();
        _personaMenu.CloseMenu();
        HighlightClickedButton(_homeButtonImage);
    }

    public void TogglePersonaMenu()
    {
        CloseAllMenus();
        _personaMenu.ToggleMenu();
        HighlightClickedButton(_homeButtonImage);
    }

    private void HighlightClickedButton(Image image)
    {
        _settingsButtonImage.color = defaultColor;
        _locationsButtonImage.color = defaultColor;
        _helpButtonImage.color = defaultColor;
        _homeButtonImage.color = defaultColor;
        image.color = highlightedColor;
    }

    private void ShowDataContainer(bool show)
    {
        _dataContainer.SetActive(show);
    }

    private void UpdateDisplayedTrafficLights()
    {
        _visualizationContainer.UpdateDisplayedTrafficLights();
    }

    public void RestartOnboarding()
    {
        CloseAllMenus();
        OnStartOnboarding?.Invoke();
    }

    public void SpawnInfoSphere(Persona persona)
    {
       _infoSpawner.SpawnInfoSphere(persona);
    }
}
