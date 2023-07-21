using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoopBarUI : MonoBehaviour
{
    [Header("components")]
    [SerializeField]
    private Image _imageBar;

    private UIEventManager _uiEventManager;

    // Use this for initialization
    void Start()
    {
        _uiEventManager = GameObject.FindObjectOfType<UIEventManager>();

        _uiEventManager.OnPlayerPoopProgressBar.AddListener(OnChangePlayerHp);
    }

    private void OnChangePlayerHp(float percentage)
    {
        _imageBar.fillAmount = percentage;
    }
}
