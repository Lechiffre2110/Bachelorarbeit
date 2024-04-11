using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
public class PersonaMenu : MonoBehaviour, IMenu
{
    private const SocialStatus _socialStatus = SocialStatus.Low;
    private Persona _persona;
    [SerializeField] private List<Sprite> _memojis;
    [SerializeField] private List<Sprite> _backgrounds;
    [SerializeField] private Image _avatar;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Button _infoButton;
    private string _url = "";

    // Start is called before the first frame update
    void Start()
    {
        Persona _persona = new Persona(_socialStatus); //default persona
        //random avatar from list
        _name.text = _persona._name;
        _avatar.sprite = _memojis[Random.Range(0, _memojis.Count)];
        _description.text = _persona._description;
        _background.sprite = GetBackground(_persona._socialStatus);
        InfoSphere.OnUpdateCurrentPersona += UpdatePersona;
        _infoButton.onClick.AddListener(OnInfoButtonClicked);
    }

    private void UpdatePersona(Persona persona)
    {
        _persona = persona;
        _avatar.sprite = _memojis[Random.Range(0, _memojis.Count)];
        _description.text = persona._description;
        _background.sprite = GetBackground(persona._socialStatus);
        GetLinkFromTag(persona._tag);
        GetButtonTextFromTag(persona._tag);
    }

    private Sprite GetBackground(SocialStatus socialStatus)
    {
        switch (socialStatus)
        {
            case SocialStatus.Transfer:
                return _backgrounds[0];
            case SocialStatus.Low:
                return _backgrounds[1];
            case SocialStatus.Middle:
                return _backgrounds[2];
            case SocialStatus.High:
                return _backgrounds[3];
            default:
                return _backgrounds[0];
        }
    }

    public void OnInfoButtonClicked()
    {
        OpenLink(_url);
    }

    private void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    private void GetLinkFromTag(string tag)
    {
        switch (tag)
        {
            case "Transferbezug":
                _url = "https://www.arbeitsagentur.de/datei/merkblatt-8c-transferleistung_ba034290.pdf";
                break;
            case "Armut":
                _url = "https://www.bpb.de/kurz-knapp/hintergrund-aktuell/516505/armut-in-deutschland-waechst/";
                break;
            case "Mittelschicht":
                _url = "https://www.bpb.de/shop/zeitschriften/apuz/196703/die-mittelschicht-das-unbekannte-wesen/";
                break;
            case "Oberschicht":
                _url = "https://www.bpb.de/system/files/dokument_pdf/_fluter_64_gesamt_web_.pdf";
                break;
            case "Reichtum":
                _url = "https://www.bpb.de/system/files/dokument_pdf/_fluter_64_gesamt_web_.pdf";
                break;
            default:
                _url = "https://www.bpb.de";
                break;
        }
    }

    private void GetButtonTextFromTag(string tag)
    {
        switch (tag)
        {
            case "Transferbezug":
                _infoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mehr Informationen zum Transferbezug";
                break;
            case "Armut":
                _infoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mehr Informationen zur Armut";
                break;
            case "Mittelschicht":
                _infoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mehr Informationen zur Mittelschicht";
                break;
            case "Oberschicht":
                _infoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mehr Informationen zur Oberschicht";
                break;
            case "Reichtum":
                _infoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mehr Informationen zu Reichtum";
                break;
            default:
                _infoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mehr Informationen";
                break;
        }
    }

    public void ToggleMenu()
    {
        if (IsOpen())
        {
            CloseMenu();
        }
        else
        {
            transform.DOLocalMoveX(0, 0.25f);
        }
    }

    public void CloseMenu()
    {
        transform.DOLocalMoveX(1179, 0.25f);
    }

    public bool IsOpen()
    {
        return transform.localPosition.x <= 0;
    }
}