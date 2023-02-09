using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Code.UI
{
    public class PlayerLifeBarUI : MonoBehaviour
    {
        [Header("components")]
        [SerializeField]
        private Image _imageBar;
        [SerializeField]
        private GameObject _backgroundBar;

        private UIEventManager _uiEventManager;

        // Use this for initialization
        void Start()
        {
            _uiEventManager = GameObject.FindObjectOfType<UIEventManager>();

            _uiEventManager.OnPlayerLifeChange.AddListener(OnChangePlayerHp);
        }

        private void OnChangePlayerHp(float percentage)
        {
            _imageBar.fillAmount = percentage;
        }
    }
}