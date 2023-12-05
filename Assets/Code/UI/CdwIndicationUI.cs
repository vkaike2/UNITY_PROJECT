using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CdwIndicationUI : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Image _image;

    [Header("CONFIGURATION")]
    [SerializeField]
    private float _cdwToHide = 0.5f;

    private Coroutine _cdwCoroutine;

    private void Awake()
    {
        _image.enabled = false;
    }

    public void StartCdw(float cdw)
    {
        _cdwCoroutine = StartCoroutine(CalculateCdw(cdw));
    }
    
    public void ForceEndCdw()
    {
        if(_cdwCoroutine != null)
        {
            StopCoroutine(_cdwCoroutine);
        }

        StartCoroutine(HideBarAfterCdw(_cdwToHide));
    }


    private IEnumerator CalculateCdw(float seconds)
    {
        _image.fillAmount = 0;

        float cdw = 0;
        float percentage = 0;
        _image.enabled = true;

        while (cdw <= seconds)
        {
            cdw += Time.deltaTime;
            percentage = cdw / seconds;
            _image.fillAmount = percentage;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HideBarAfterCdw(_cdwToHide));
    }

    IEnumerator HideBarAfterCdw(float cdwToHide)
    {
        yield return new WaitForSeconds(cdwToHide);
        _image.enabled = false;
    }
}
