using System.Collections;
using UnityEngine;


public class NextWaveButtonUI : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private GameObject _buttonGameObject;

    private UIEventManager _UIEventManager;

    void Start()
    {
        _UIEventManager = GameObject.FindObjectOfType<UIEventManager>();
        _UIEventManager.OnActivateNextWaveButton.AddListener(ActivateButton);

        StartCoroutine(WaitForOneFrameThenDeactivate());
    }


    public void OnClick()
    {
        _UIEventManager.OnClickNextWaveButton.Invoke();
        DeactivateButton();
    }

    private void ActivateButton()
    {
        _buttonGameObject.SetActive(true);
    }

    private void DeactivateButton()
    {
        _buttonGameObject.SetActive(false);
    }

    private IEnumerator WaitForOneFrameThenDeactivate()
    {
        yield return new WaitForEndOfFrame();
        DeactivateButton();
    }
}
