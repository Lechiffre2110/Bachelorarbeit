using UnityEngine;
using System.Collections.Generic;

public class DataModel
{
    private DataAccess _dataAccess;
    private Geolocator _geolocator;
    private NearbyLocations _nearbyLocations;
    private float _latitude;
    private float _longitude;
    private string _streetName;
    private CityAverageData _cityAverageData;
    private LocationData _locationData;
    private Persona _persona;

    //Observer pattern implementation
    private List<IModelObserver> _observers = new List<IModelObserver>();
    public void Attach(IModelObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IModelObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.OnModelUpdated();
        }
    }

    protected void NotifyObserversAboutNearbyLocationsUpdate()
    {
        foreach (var observer in _observers)
        {
            observer.OnModelUpdated(UpdateType.NearbyLocationsUpdated);
        }
    }

    protected void NotifyObserversAboutPersonaUpdate()
    {
        foreach (var observer in _observers)
        {
            observer.OnModelUpdated(UpdateType.PersonaUpdated);
        }
    }

    public DataModel()
    {
        _dataAccess = new DataAccess();
        _geolocator = new Geolocator();
        _nearbyLocations = new NearbyLocations();
    }

    public async void UpdateUserLocationData()
    {
        string previousStreetName = _streetName;
        (_latitude, _longitude) = await _geolocator.GetGeoLocationAsync();
        _streetName = await _geolocator.GetStreetNameFromGeoLocationAsync(_latitude, _longitude);
        if (previousStreetName != _streetName)
        {
            NotifyObservers();
        }
    }

    public void UpdateCityData(string year)
    {
        _cityAverageData = _dataAccess.GetCityAverageData(year);
    }

    public void UpdateSocioEconomicData(string year)
    {
        LocationData previousLocationData = _locationData;
        _locationData = _dataAccess.GetDataForGeolocation(_latitude, _longitude, year);
        
        if (previousLocationData.UnemploymentRate != _locationData.UnemploymentRate || previousLocationData.TransferRate != _locationData.TransferRate || previousLocationData.TransferRateUnder15 != _locationData.TransferRateUnder15)
        {
            NotifyObservers();
        }
    }

    public void UpdateNearbyLocationsList()
    {
        _nearbyLocations.UpdateDistances(_latitude, _longitude);
        NotifyObserversAboutNearbyLocationsUpdate();
    }

    public void UpdatePersona()
    {
        string socialStatus = Settings.Instance.SocialStatus;
        switch (socialStatus)
        {
            case "hoch":
                _persona = new Persona(SocialStatus.VeryHigh);
                break;
            case "mittel":
                _persona = new Persona(SocialStatus.Middle);
                break;
            case "niedrig":
                _persona = new Persona(SocialStatus.Low);
                break;
            default:
                _persona = new Persona(SocialStatus.Transfer);
                break;
        }
        NotifyObserversAboutPersonaUpdate();
    }


    public string GetStreetName()
    {
        return _streetName;
    }

    public LocationData GetLocationData()
    {
        return _locationData;
    }

    public CityAverageData GetCityAverageData()
    {
        return _cityAverageData;
    }

    public List<Location> GetNearbyLocationsList()
    {
        return _nearbyLocations.GetNearbyLocationsList();
    }

    public Persona GetPersona()
    {
        return _persona;
    }
}