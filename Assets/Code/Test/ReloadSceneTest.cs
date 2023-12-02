using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Test
{

    public class ReloadSceneTest : MonoBehaviour
    {
        public GameObject buttonGO;

        private GameManager _gameManager;

        private void Awake()
        {
            buttonGO.SetActive(false);
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


        private void ActivateButton() => buttonGO.SetActive(true);
    }
}