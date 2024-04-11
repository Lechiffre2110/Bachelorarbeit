using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Onboarding : MonoBehaviour
{
    [SerializeField] private List<GameObject> _onboardingSteps;
    private int _currentStep = 0;

    public delegate void ShowDataContainer(bool show);
    public static event ShowDataContainer OnShowDataContainer;
    [SerializeField] private GameObject _onboardingClosedPopup;

    void Start()
    {
        OnboardingStep.OnNextButtonClicked += NextStep;
        OnboardingStep.OnBackButtonClicked += PreviousStep;
        OnboardingStep.OnCloseButtonClicked += CloseOnboarding;
        UserInterface.OnStartOnboarding += StartOnboarding;
    }

    public void NextStep()
    {
        if (_currentStep < _onboardingSteps.Count - 1)
        {
            _onboardingSteps[_currentStep].SetActive(false);
            _currentStep++;

            if (_currentStep == 2)
            {
                OnShowDataContainer?.Invoke(false);
            }

            if (_currentStep < _onboardingSteps.Count)
            {
                _onboardingSteps[_currentStep].SetActive(true);
            }
        }
        else
        {
            CloseOnboarding();
        }
    }

    public void StartOnboarding()
    {
        _currentStep = 0;
        _onboardingSteps[0].SetActive(true);
    }

    public void PreviousStep()
    {
        _onboardingSteps[_currentStep].SetActive(false);
        _currentStep--;
        if (_currentStep >= 0)
        {
            _onboardingSteps[_currentStep].SetActive(true);
        }

        if (_currentStep == 1)
        {
            OnShowDataContainer?.Invoke(true);
        }
    }

    public void CloseOnboarding()
    {
        _onboardingSteps[_currentStep].SetActive(false);
        _currentStep = 0;
        OnShowDataContainer?.Invoke(true);
        ShowOnboardingClosedPopup();
    }

    private void ShowOnboardingClosedPopup()
    {
        _onboardingClosedPopup.transform.DOLocalMoveY(-420, 0.25f);
        StartCoroutine(CloseOnboardingClosedPopup());
    }

    private IEnumerator CloseOnboardingClosedPopup()
    {
        yield return new WaitForSeconds(2);
        _onboardingClosedPopup.transform.DOLocalMoveY(-1500, 0.25f);
    }
}
