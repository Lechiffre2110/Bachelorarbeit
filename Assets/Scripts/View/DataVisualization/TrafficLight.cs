using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] private Image _topLight;
    [SerializeField] private Image _middleLight;
    [SerializeField] private Image _bottomLight;
    [SerializeField] private Image _cityAverageBar;

    private Color _gray = new Color(0.6784314f, 0.6784314f, 0.6784314f);


    private Color _yellow = new Color(0.9647059f, 0.6823529f, 0.1764706f);
    private Color _green = new Color(0.3294118f, 0.6000000f, 0.4078431f);

    public void SetColor(string color)
    {
        _topLight.color = _gray;
        _middleLight.color = _gray;
        _bottomLight.color = _gray;

        switch (color)
        {
            case "red":
                _topLight.color = Color.red;
                break;
            case "yellow":
                _middleLight.color = _yellow;
                break;
            case "green":
                _bottomLight.color = _green;
                break;
        }
    }

    public void SetCityAverageColor(string color)
    {
        switch (color)
        {
            case "red":
                _cityAverageBar.color = Color.red;
                break;
            case "yellow":
                _cityAverageBar.color = _yellow;
                break;
            case "green":
                _cityAverageBar.color = _green;
                break;
        }
    }
}
