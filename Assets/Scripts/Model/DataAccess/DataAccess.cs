using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DataAccess
{
    private const string AverageDataSetName = "mss-data-city-averages";
    private const string CsvSeparator = ",";
    private CultureInfo Culture = CultureInfo.InvariantCulture; //make const 
    private Dictionary<string, string> DataSets = new Dictionary<string, string> //make const
    {
        { "2013", "mss-data-geo-2013" },
        { "2015", "mss-data-geo-2015" },
        { "2017", "mss-data-geo-2017" },
        { "2019", "mss-data-geo-2019" },
        { "2021", "mss-data-geo-2021" }
    };


    public CityAverageData GetCityAverageData(string year)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(AverageDataSetName);
        string[] csvLines = csvFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        CityAverageData cityAverageData = new CityAverageData();

        for (int i = 1; i < csvLines.Length - 1; i++)
        {
            string[] fields = csvLines[i].Split(CsvSeparator.ToCharArray());

            try
            {
                if (fields[0] == year)
                {
                    cityAverageData.AverageUnemploymentRate = float.Parse(fields[1], Culture);
                    cityAverageData.AverageTransferRate = float.Parse(fields[2], Culture);
                    cityAverageData.AverageTransferRateUnder15 = float.Parse(fields[3], Culture);
                    return cityAverageData;
                }
            }
            catch (FormatException)
            {
                Debug.LogError("Error parsing CSV line: " + csvLines[i]);
                continue;
            }
        }
        return cityAverageData;
    }

    public LocationData GetDataForGeolocation(float latitude, float longitude, string year)
    {
        string dataSetName = DataSets[year];
        TextAsset csvFile = Resources.Load<TextAsset>(dataSetName);
        string[] csvLines = csvFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        float minDistance = float.MaxValue;
        LocationData closestData = new LocationData();

        for (int i = 1; i < csvLines.Length - 1; i++)
        {
            string[] fields = csvLines[i].Split(CsvSeparator.ToCharArray());

            try
            {
                float csvLatitude = float.Parse(fields[fields.Length - 2], Culture);
                float csvLongitude = float.Parse(fields[fields.Length - 1], Culture);

                if (csvLatitude == 0f || csvLongitude == 0f)
                {
                    continue;
                }

                float distance = DistanceUtils.CalculateDistanceBetweenTwoPoints(latitude, longitude, csvLatitude, csvLongitude);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestData.Zone = fields[1];
                    closestData.UnemploymentRate = float.Parse(fields[2], Culture);
                    closestData.TransferRate = float.Parse(fields[3], Culture);
                    closestData.TransferRateUnder15 = float.Parse(fields[4], Culture);
                    closestData.SocialStatus = fields[5];
                    closestData.Year = year;
                }
            }
            catch (FormatException)
            {
                Debug.LogError("Error parsing CSV line: " + csvLines[i]);
                continue;
            }
        }
        Settings.Instance.UpdateSocialStatus(closestData.SocialStatus);
        return closestData;
    }
}