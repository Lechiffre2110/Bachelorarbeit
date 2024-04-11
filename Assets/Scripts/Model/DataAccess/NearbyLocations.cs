using System;
using System.Collections.Generic;
using UnityEngine;

public class NearbyLocations
{
    private const string CsvSeparator = ",";
    private List<Location> _locations = new List<Location>();

    public NearbyLocations()
    {
        LoadLocationsFromCSV();
    }
    
    private void LoadLocationsFromCSV()
    {
        //float minDistance = float.MaxValue; 
        TextAsset csvFile = Resources.Load<TextAsset>("locations");
        string[] csvLines = csvFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        for (int i = 1; i < csvLines.Length - 1; i++)
        {
            string[] fields = csvLines[i].Split(CsvSeparator.ToCharArray());

            string name = fields[0];
            string description = fields[1];
            string address = fields[2];
            string phone = fields[3];
            string website = fields[4];
            float latitude = float.Parse(fields[5].Replace(".", ","));
            float longitude = float.Parse(fields[6].Replace(".", ","));

            Location tempLocation = new Location(
                name,
                description,
                address,
                phone,
                website,
                longitude,
                latitude
            );
            _locations.Add(tempLocation);
        }  
    }

    public void UpdateDistances(float latitude, float longitude)
    {
        foreach (Location location in _locations)
        {
            float distance = DistanceUtils.CalculateDistanceBetweenTwoPoints(latitude, longitude, location.Latitude, location.Longitude);
            location.Distance = distance;
        }
        SortLocationsByDistance();
    }

    private void SortLocationsByDistance()
    {
        _locations.Sort((x, y) => x.Distance.CompareTo(y.Distance));
    }

    public List<Location> GetNearbyLocationsList()
    {
        return _locations;
    }
}