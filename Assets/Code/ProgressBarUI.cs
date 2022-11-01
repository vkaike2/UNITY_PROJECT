using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    [Header("configuration")]
    [SerializeField]
    private bool _startActive = false;
    [SerializeField]
    private float _cdwToHide = 0.5f;

    public StartEvent OnStart { get; private set; }
    
    private Image _imageBar;

    private void Awake()
    {
        OnStart = new StartEvent();

        _imageBar = GetComponent<Image>();
        _imageBar.enabled = _startActive;

        OnStart.AddListener(StartProgressBar);
    }

    private void StartProgressBar(float cdw, Behaviour behaviour)
    {
        StopAllCoroutines();

        StartCoroutine(ManageProgress(cdw));
    }

    IEnumerator ManageProgress(float completionCdw)
    {
        _imageBar.fillAmount = 0;
        _imageBar.enabled = true;

        float cdw = 0;
        float percentage = 0;

        while (cdw <= completionCdw)
        {
            cdw += Time.deltaTime;
            percentage = cdw / completionCdw;
            _imageBar.fillAmount = percentage;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HideBarAfertCdw());
    }

    IEnumerator HideBarAfertCdw()
    {
        yield return new WaitForSeconds(_cdwToHide);
        _imageBar.enabled = false;
    }


    public enum Behaviour
    {
        Hide_After_Completion
    }

    public class StartEvent : UnityEvent<float, Behaviour> { }
}
