using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Toggle _togglePoverty;
    [SerializeField] private Toggle _toggleChildPoverty;
    [SerializeField] private Toggle _toggleUnemployment;
    [SerializeField] private GameObject _settingsAplliedPopup;
    [SerializeField] private GameObject _settingsDiscardedPopup;
    private bool _povertyBefore;
    private bool _childPovertyBefore;
    private bool _unemploymentBefore;

    public delegate void SaveSettings();
    public static event SaveSettings OnSaveSettings;
    public delegate void CloseSettings();
    public static event CloseSettings OnCloseSettings; //implement in UI Controller

    void Start()
    {
        _povertyBefore = _togglePoverty.isOn;
        _childPovertyBefore = _toggleChildPoverty.isOn;
        _unemploymentBefore = _toggleUnemployment.isOn;
    }

    public void ApplySettings()
    {
        _povertyBefore = _togglePoverty.isOn;
        _childPovertyBefore = _toggleChildPoverty.isOn;
        _unemploymentBefore = _toggleUnemployment.isOn;
        Settings.Instance.UpdateToggleSettings(_togglePoverty.isOn, _toggleChildPoverty.isOn, _toggleUnemployment.isOn);
        Debug.Log("Poverity: " + _togglePoverty.isOn + " Child Poverty: " + _toggleChildPoverty.isOn + " Unemployment: " + _toggleUnemployment.isOn);
        OnSaveSettings();
        OnCloseSettings();
        ShowSettingsAppliedPopup();
    }

    public void CancelSettings()
    {
        _togglePoverty.isOn = _povertyBefore;
        _toggleChildPoverty.isOn = _childPovertyBefore;
        _toggleUnemployment.isOn = _unemploymentBefore;
        OnCloseSettings();
        ShowSettingsDiscardedPopup();
    }

    void ShowSettingsAppliedPopup()
    {
        _settingsAplliedPopup.transform.DOLocalMoveY(-420, 0.25f);
        StartCoroutine(CloseSettingsAppliedPopup());
    }

    IEnumerator CloseSettingsAppliedPopup()
    {
        yield return new WaitForSeconds(2);
        _settingsAplliedPopup.transform.DOLocalMoveY(-1500, 0.25f);
    }

    void ShowSettingsDiscardedPopup()
    {
        _settingsDiscardedPopup.transform.DOLocalMoveY(-420, 0.25f);
        StartCoroutine(CloseSettingsDiscardedPopup());
    }

    IEnumerator CloseSettingsDiscardedPopup()
    {
        yield return new WaitForSeconds(2);
        _settingsDiscardedPopup.transform.DOLocalMoveY(-1500, 0.25f);
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
