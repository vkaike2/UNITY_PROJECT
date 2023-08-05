using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoopBarUI : MonoBehaviour
{
    [Header("components")]
    [SerializeField]
    private Image _imageBar;

    // Use this for initialization
    void Start()
    {
        UIEventManager.instance.OnPlayerPoopProgressBar.AddListener(OnChangePlayerHp);
    }

    private void OnChangePlayerHp(float percentage)
    {
        _imageBar.fillAmount = percentage;
    }
}
