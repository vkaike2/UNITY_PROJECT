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

        // Use this for initialization
        void Start()
        {

            UIEventManager.instance.OnPlayerLifeChange.AddListener(OnChangePlayerHp);
        }

        private void OnChangePlayerHp(float percentage)
        {
            _imageBar.fillAmount = percentage;
        }
    }
}