using System.Collections;
using UnityEngine;

public class InfoSphere : MonoBehaviour
{
    public delegate void OpenPersonaMenu();
    public static event OpenPersonaMenu OnOpenPersonaMenu;

    public delegate void UpdateCurrentPersona(Persona persona);
    public static event UpdateCurrentPersona OnUpdateCurrentPersona;
    private Persona _persona;
    private const float TimeToReturnToPool = 60.0f;

    private void OnEnable()
    {
        StartCoroutine(ReturnToPoolAfterTime());
    }


    public void SetPersona(Persona persona)
    {
        _persona = persona;
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(TimeToReturnToPool);
        ReturnToPool();
    }

    void OnMouseDown()
    {
        OnOpenPersonaMenu();
        OnUpdateCurrentPersona(_persona);
    }
}
