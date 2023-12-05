using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    [Header("COMPONENTS")]
    [SerializeField]
    private Image _imageBar;
    [SerializeField]
    private GameObject _backgroundBar;

    [Header("CONFIGURATION")]
    [SerializeField]
    private bool _startActive = false;
    [SerializeField]
    private float _cdwToHide = 0.5f;

    public SetBehaviour OnSetBehaviour { get; private set; }


    private void Awake()
    {
        OnSetBehaviour = new SetBehaviour();

        _backgroundBar.SetActive(_startActive);

        OnSetBehaviour.AddListener(StartProgressBar);
    }

    private void StartProgressBar(float value, Behaviour behaviour)
    {
        StopAllCoroutines();

        switch (behaviour)
        {
            case Behaviour.ProgressBar_Hide:
                StartCoroutine(ManageProgressBar(value));
                break;
            case Behaviour.LifeBar_Hide:
                ManageLifeBar(value);
                break;
            default:
                break;
        }

    }

    IEnumerator ManageProgressBar(float completionCdw)
    {
        _imageBar.fillAmount = 0;
        _backgroundBar.SetActive(true);

        float cdw = 0;
        float percentage = 0;

        while (cdw <= completionCdw)
        {
            cdw += Time.deltaTime;
            percentage = cdw / completionCdw;
            _imageBar.fillAmount = percentage;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(HideBarAfterCdw(_cdwToHide));
    }

    private void ManageLifeBar(float percentage)
    {
        _imageBar.fillAmount = percentage;

        _backgroundBar.SetActive(true);

        StartCoroutine(HideBarAfterCdw(_cdwToHide));
    }

    IEnumerator HideBarAfterCdw(float cdwToHide)
    {
        yield return new WaitForSeconds(cdwToHide);
        _backgroundBar.SetActive(false);
    }

    public enum Behaviour
    {
        ProgressBar_Hide,
        LifeBar_Hide
    }

    /// <summary>
    /// float -> used in different ways depending on the behaviour
    /// Behaviour -> what it will do after fills
    /// </summary>
    public class SetBehaviour : UnityEvent<float, Behaviour> { }
}
