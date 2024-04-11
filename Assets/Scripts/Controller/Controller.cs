using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour, IModelObserver, IViewObserver
{
    [SerializeField] private UserInterface _userInterface;
    private DataModel _dataModel;

    void Start()
    {
        _dataModel = new DataModel();
        _dataModel.Attach(this);
        _dataModel.UpdateCityData(Settings.Instance.Year);

        _userInterface.Attach(this);

        StartCoroutine(UpdateModelPeriodically());
        StartCoroutine(UpdateModelPersonaPeriodically());
    }

    private void OnDestroy()
    {
        _dataModel?.Detach(this);
    }

    IEnumerator UpdateModelPeriodically()
    {
        while (true)
        {
            string year = Settings.Instance.Year;
            _dataModel.UpdateUserLocationData();
            _dataModel.UpdateSocioEconomicData(year);
            _dataModel.UpdateNearbyLocationsList();
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator UpdateModelPersonaPeriodically()
    {
        while (true)
        {
            _dataModel.UpdatePersona();
            yield return new WaitForSeconds(45);
        }
    }

    public void OnModelUpdated(UpdateType updateType)
    {
        if (updateType == UpdateType.NearbyLocationsUpdated)
        {
            List<Location> nearbyLocationsList = _dataModel.GetNearbyLocationsList();
            _userInterface.UpdateNearbyLocations(nearbyLocationsList);
            return;
        }

        if (updateType == UpdateType.PersonaUpdated)
        {
            Persona persona = _dataModel.GetPersona();
            _userInterface.SpawnInfoSphere(persona);
            return;
        }

        CityAverageData cityAverageData = _dataModel.GetCityAverageData();
        LocationData locationData = _dataModel.GetLocationData();
        string streetName = _dataModel.GetStreetName();

        _userInterface.UpdateTrafficLights(locationData.TransferRate, locationData.TransferRateUnder15, locationData.UnemploymentRate);
        _userInterface.UpdateCityAverageBars(cityAverageData.AverageTransferRate, cityAverageData.AverageTransferRateUnder15, cityAverageData.AverageUnemploymentRate);
        _userInterface.UpdateDataMenu(locationData.Zone, locationData.TransferRate, locationData.TransferRateUnder15, locationData.UnemploymentRate);
        _userInterface.UpdateStreetName(streetName);
    }

    public void OnViewChanged()
    {
        _dataModel.UpdateCityData(Settings.Instance.Year);
        _dataModel.UpdateSocioEconomicData(Settings.Instance.Year); //force update
    }

}