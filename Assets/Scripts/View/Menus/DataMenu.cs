using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DataMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _mssButton;
    [SerializeField] private Button _povertyInfoButton;
    [SerializeField] private Button _childPovertyInfoButton;
    [SerializeField] private Button _unemploymentInfoButton;
    [SerializeField] private List<TextMeshProUGUI> _dataTextFields = new List<TextMeshProUGUI>();

    private const string _mssInfoLink = "https://www.berlin.de/sen/sbw/stadtdaten/stadtwissen/monitoring-soziale-stadtentwicklung/bericht-2021/";
    private const string _povertyInfoLink = "https://www.armuts-und-reichtumsbericht.de/DE/Startseite/start.html";
    private const string _childPovertyInfoLink = "https://www.bertelsmann-stiftung.de/fileadmin/files/BSt/Publikationen/GrauePublikationen/291_2020_BST_Facsheet_Kinderarmut_SGB-II_Daten__ID967.pdf";
    private const string _unemploymentInfoLink = "https://www.bpb.de/kurz-knapp/zahlen-und-fakten/soziale-situation-in-deutschland/61717/arbeitslosigkeit/";

    void Start()
    {
        _mssButton.onClick.AddListener(() => OpenLink(_mssInfoLink));
        _povertyInfoButton.onClick.AddListener(() => OpenLink(_povertyInfoLink));
        _childPovertyInfoButton.onClick.AddListener(() => OpenLink(_childPovertyInfoLink));
        _unemploymentInfoButton.onClick.AddListener(() => OpenLink(_unemploymentInfoLink));
    }

    private void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    public void UpdateDataTextFields(string zone, float transferRate, float transferRateUnder15, float unemploymentRate)
    {
        _dataTextFields[0].text = zone;
        _dataTextFields[1].text = transferRate.ToString("F2") + "%";
        _dataTextFields[2].text = transferRateUnder15.ToString("F2") + "%";
        _dataTextFields[3].text = unemploymentRate.ToString("F2") + "%";
    }

    public void ToggleMenu()
    {
        if (IsOpen())
        {
            CloseMenu();
        }
        else
        {
            transform.DOLocalMoveY(133, 0.25f);
        }
    }

    public void CloseMenu()
    {
        transform.DOLocalMoveY(-2030, 0.25f);
    }

    public bool IsOpen()
    {
        return transform.localPosition.y >= -1000;
    }
}
