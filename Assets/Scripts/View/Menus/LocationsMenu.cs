using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class LocationsMenu : MonoBehaviour, IMenu
{
    [SerializeField] private GameObject _locationItemPrefab;
    [SerializeField] private  Transform _contentPanel;
    [SerializeField] private Geolocator _geolocator;

    public void UpdateNearbyLocations(List<Location> nearbyLocationsList)
    {
        foreach (Transform child in _contentPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (Location location in nearbyLocationsList)
        {
            GameObject newItem = Instantiate(_locationItemPrefab, _contentPanel); 

            TextMeshProUGUI nameText = newItem.transform.Find("Location Name").GetComponent<TextMeshProUGUI>();
            nameText.text = location.Name;

            TextMeshProUGUI distanceText = newItem.transform.Find("Location Distance").GetComponent<TextMeshProUGUI>();
            distanceText.text = location.Distance.ToString("F2") + "km";

            TextMeshProUGUI phoneText = newItem.transform.Find("Location Phone").GetComponent<TextMeshProUGUI>();
            phoneText.text = "Telefon: \n" + location.Phone;

            TextMeshProUGUI addressText = newItem.transform.Find("Location Address").GetComponent<TextMeshProUGUI>();
            addressText.text = "Adresse: \n" + location.Address;

            Button websiteButton = newItem.transform.Find("Location Website").GetComponent<Button>();
            string link = location.Website;
            websiteButton.onClick.RemoveAllListeners();
            websiteButton.onClick.AddListener(() => OpenLink(link));

            Button directionsButton = newItem.transform.Find("Location Directions").GetComponent<Button>();
            directionsButton.onClick.RemoveAllListeners();
            directionsButton.onClick.AddListener(() => OpenAppleMapsWithDirections(location.Latitude, location.Longitude));
        }
    }

    void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    void OpenAppleMapsWithDirections(float destinationLat, float destinationLng)
    {
        string url = $"http://maps.apple.com/?daddr={destinationLat},{destinationLng}";
        Application.OpenURL(url);
    }


    public void ToggleMenu()
    {
        if (IsOpen())
        {
            CloseMenu();
        }
        else
        {
            transform.DOLocalMoveY(-205, 0.25f);
        }
    }

    public void CloseMenu()
    {
        transform.DOLocalMoveY(-1750, 0.25f);
    }

    public bool IsOpen()
    {
        return transform.localPosition.y >= -1000;
    }
}
