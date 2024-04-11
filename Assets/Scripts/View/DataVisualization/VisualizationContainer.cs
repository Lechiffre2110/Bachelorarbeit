using TMPro;
using UnityEngine;

public class VisualizationContainer : MonoBehaviour
{
    [SerializeField] private TrafficLight[] _trafficLights;
    [SerializeField] private GameObject[] _trafficLightGroup;
    [SerializeField] private TMP_Dropdown _dataYearDropdown;

    // Threshholds based on MSS report
    // green threshhold calculated as AVG of all data - 0.5 SD of all data; yellow threshhold calculated as AVG + 0.5 SD
    private const float YellowThresholdTransfer = 15.87f; 
    private const float GreenThresholdTransfer = 7.71f;
    private const float YellowThresholdTransferUnder15 = 36.63f;
    private const float GreenThresholdTransferUnder15 = 17.52f;
    private const float YellowThresholdUnemployment = 9.45f;
    private const float GreenThresholdUnemployment = 4.317f;

    public delegate void DataYearChanged();
    public static event DataYearChanged OnDataYearChanged;



    public void UpdateTrafficLights(float transferRate, float transferRateUnder15, float unemploymentRate)
    {
        // Update traffic lights
        foreach (TrafficLight trafficLight in _trafficLights)
        {
            if (trafficLight.name == "Unemployment")
            {
                if (unemploymentRate > YellowThresholdUnemployment)
                {
                    trafficLight.SetColor("red");
                }
                else if (unemploymentRate > GreenThresholdUnemployment)
                {
                    trafficLight.SetColor("yellow");
                }
                else
                {
                    trafficLight.SetColor("green");
                }
            }
            else if (trafficLight.name == "Transfer")
            {
                if (transferRate > YellowThresholdTransfer)
                {
                    trafficLight.SetColor("red");
                }
                else if (transferRate > GreenThresholdTransfer)
                {
                    trafficLight.SetColor("yellow");
                }
                else
                {
                    trafficLight.SetColor("green");
                }
            }
            else if (trafficLight.name == "TransferUnder15")
            {
                if (transferRateUnder15 > YellowThresholdTransferUnder15)
                {
                    trafficLight.SetColor("red");
                }
                else if (transferRateUnder15 > GreenThresholdTransferUnder15)
                {
                    trafficLight.SetColor("yellow");
                }
                else
                {
                    trafficLight.SetColor("green");
                }
            }
        }
    }

    public void UpdateCityAverageBars(float transferRateCityAverage, float transferRateUnder15CityAverage, float unemploymentRateCityAverage)
    {
        foreach (TrafficLight trafficLight in _trafficLights)
        {
            if (trafficLight.name == "Unemployment")
            {
                if (unemploymentRateCityAverage > YellowThresholdUnemployment)
                {
                    trafficLight.SetCityAverageColor("red");
                }
                else if (unemploymentRateCityAverage > GreenThresholdUnemployment)
                {
                    trafficLight.SetCityAverageColor("yellow");
                }
                else
                {
                    trafficLight.SetCityAverageColor("green");
                }
            }
            else if (trafficLight.name == "Transfer")
            {
                if (transferRateCityAverage > YellowThresholdTransfer)
                {
                    trafficLight.SetCityAverageColor("red");
                }
                else if (transferRateCityAverage > GreenThresholdTransfer)
                {
                    trafficLight.SetCityAverageColor("yellow");
                }
                else
                {
                    trafficLight.SetCityAverageColor("green");
                }
            }
            else if (trafficLight.name == "TransferUnder15")
            {
                if (transferRateUnder15CityAverage > YellowThresholdTransferUnder15)
                {
                    trafficLight.SetCityAverageColor("red");
                }
                else if (transferRateUnder15CityAverage > GreenThresholdTransferUnder15)
                {
                    trafficLight.SetCityAverageColor("yellow");
                }
                else
                {
                    trafficLight.SetCityAverageColor("green");
                }
            }
        }
    }

    public void UpdateDisplayedTrafficLights()
    {
        if (Settings.Instance.DisplayPoverty)
        {
            _trafficLightGroup[0].SetActive(true);
        }
        else
        {
            _trafficLightGroup[0].SetActive(false);
        }

        if (Settings.Instance.DisplayChildPoverty)
        {
            _trafficLightGroup[1].SetActive(true);
        }
        else
        {
            _trafficLightGroup[1].SetActive(false);
        }

        if (Settings.Instance.DisplayUnemploymentRate)
        {
            _trafficLightGroup[2].SetActive(true);
        }
        else
        {
            _trafficLightGroup[2].SetActive(false);
        }
    }

    public void UpdateDataYear()
    {
        Settings.Instance.UpdateYear(_dataYearDropdown.options[_dataYearDropdown.value].text);
        OnDataYearChanged();
    }
}