using System.Collections.Generic;
using System;
using UnityEngine;

public class Persona
{

    //Todo: make them private and readonly
    public FamilyStatus _familyStatus;
    public SocialStatus _socialStatus;

    public string _name = "Manfred";

    public int _income;
    public string _description;
    public int avatar;
    public string _tag;

    public Persona(SocialStatus socialStatus)
    {
        _socialStatus = socialStatus;
        _familyStatus = GetRandomFamilyStatus();
        _income = GenerateIncome();
        _description = GetDescription();
        _tag = GetTags();
    }

    /// <summary>
    /// Function to generate a random FamilyStatus
    /// </summary>
    /// <returns>The generated FamilyStatus</returns>
    private FamilyStatus GetRandomFamilyStatus()
    {
        Array values = Enum.GetValues(typeof(FamilyStatus));
        return (FamilyStatus)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    /// <summary>
    /// Function to generate a randomized income based on the SocialStatus and FamilyStatus
    /// </summary>
    /// <returns>The generated income</returns>
    private int GenerateIncome()
    {
        int income = 0;
        switch (_socialStatus)
        {
            case SocialStatus.Transfer:
                income = UnityEngine.Random.Range(1000, 1500);
                break;
            case SocialStatus.Low:
                income = UnityEngine.Random.Range(1000, 2000);
                break;
            case SocialStatus.Middle:
                income = UnityEngine.Random.Range(4000, 8000);
                break;
            case SocialStatus.High:
                income = UnityEngine.Random.Range(8000, 15000);
                break;
            case SocialStatus.VeryHigh:
                income = UnityEngine.Random.Range(15000, 100000);
                break;
        }

        switch (_familyStatus)
        {
            case FamilyStatus.SingleParentMale or FamilyStatus.SingleParentFemale:
                income *= 1;
                break;
            case FamilyStatus.Family:
                income *= 2;
                break;
        }
        return income;
    }

    /// <summary>
    /// Function to generate a description based on the SocialStatus and FamilyStatus
    /// </summary>
    /// <returns>The generated description</returns>
    private string GetDescription()
    {
        string description = "";
        switch (_socialStatus)
        {
            case SocialStatus.Transfer:
                description = $"Das ist {_name}. \n {_name} ist Teil der ärmeren Gruppe der Einwohner von Berlin. {_name} empfängt Transferleistungen und hat monatlich ein Einkommen von {_income}€. Nach Abzug aller Fixkosten, einschließlich Miete und Heizung, bleibt {_name} nur wenig Geld zum Leben.";
                break;
            case SocialStatus.Low:
                description = $"Das ist {_name}. \n {_name} gehört zur Gruppe der Geringverdiener in Berlin. \n{_name} hat ein monatliches Einkommen von {_income}€, was es schwierig macht, nach Abzug aller Fixkosten, einschließlich Miete und Heizung, über die Runden zu kommen.";
                break;
            case SocialStatus.Middle:
                description = $"Das ist {_name}. \n {_name} gehört zur Mittelschicht der Bevölkerung in Berlin. \n{_name} hat ein stabiles Einkommen von {_income}€, mit dem er/sie einen komfortablen Lebensstandard aufrechterhalten kann, einschließlich Freizeitaktivitäten und Urlaub.";
                break;
            case SocialStatus.High:
                description = $"Das ist {_name}. \n {_name} gehört zur oberen Mittelschicht in Berlin. \n{_name} genießt ein hohes Einkommen von {_income}€, das es ihm/ihr ermöglicht, ein luxuriöses Leben zu führen, einschließlich hochwertiger Wohnverhältnisse und regelmäßiger exklusiver Urlaube.";
                break;
            case SocialStatus.VeryHigh:
                description = $"Das ist {_name}. \n {_name} zählt zu den Spitzenverdienern in Berlin. \nMit einem Einkommen von {_income}€ hat {_name} Zugang zu erstklassigen Lebensbedingungen, kann in Luxus investieren und hat eine signifikante finanzielle Sicherheit.";
                break;
        }

        switch (_familyStatus)
        {
            case FamilyStatus.SingleParentMale:
                description += " and single";
                break;
        }
        return description;
    }

    private string GetTags()
    {
        switch (_socialStatus)
        {
            case SocialStatus.Transfer:
                _tag = "Transferbezug";
                break;
            case SocialStatus.Low:
                _tag = "Armut";
                break;
            case SocialStatus.Middle:
                _tag = "Mittelschicht";
                break;
            case SocialStatus.High:
                _tag = "Oberschicht";
                break;
            case SocialStatus.VeryHigh:
                _tag = "Reichtum";
                break;
        }

        // override tag if an especially relevant combination of social status and family status is present (single parent and low income/transfer)
        if (_familyStatus == FamilyStatus.SingleParentMale || _familyStatus == FamilyStatus.SingleParentFemale)
        {
            if (_socialStatus == SocialStatus.Transfer || _socialStatus == SocialStatus.Low)
            {
                _tag = "Armut Alleinerziehende";
            }
        }

        return _tag;
    }
}

public enum SocialStatus
{
    Transfer,
    Low,
    Middle,
    High,
    VeryHigh
}

public enum FamilyStatus
{
    SingleMale,
    SingleFemale,
    SingleNonBinary,
    Family,
    SingleParentMale,
    SingleParentFemale
}