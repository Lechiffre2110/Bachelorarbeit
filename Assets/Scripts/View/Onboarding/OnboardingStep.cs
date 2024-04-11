using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OnboardingStep : MonoBehaviour
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _closeButton;
    public delegate void OnboardingBackButtonClicked();
    public static event OnboardingBackButtonClicked OnBackButtonClicked;
    public delegate void OnboardingNextButtonClicked();
    public static event OnboardingNextButtonClicked OnNextButtonClicked;
    public delegate void OnboardingCloseButtonClicked();
    public static event OnboardingCloseButtonClicked OnCloseButtonClicked;
    
    void Start()
    {
        _nextButton.onClick.AddListener(NextButtonClicked);
        _backButton.onClick.AddListener(BackButtonClicked);
        _closeButton.onClick.AddListener(CloseButtonClicked);
    }

    public void NextButtonClicked()
    {
        OnNextButtonClicked?.Invoke();
    }

    public void BackButtonClicked()
    {
        OnBackButtonClicked?.Invoke();
    }

    public void CloseButtonClicked()
    {
        OnCloseButtonClicked?.Invoke();
    }
}
