using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Test
{

    public class ReloadSceneTest : MonoBehaviour
    {
        public GameObject buttonGO;
        public GameObject labelGO;

        public TMP_Text damageSourceLabel;

        private GameManager _gameManager;

        private void Awake()
        {
            ActivateSetUp(false);
        }

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _gameManager.OnPlayerDead.AddListener(ActivateButton);
        }

        public void OnClickReload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        private void ActivateButton(string damageSource)
        {
            damageSourceLabel.text = damageSource.ToUpper();
            ActivateSetUp(true);
        }

        private void ActivateSetUp(bool value)
        {
            buttonGO.SetActive(value);
            labelGO.SetActive(value);
        }
    }
}