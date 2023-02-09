using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Code.MANAGER
{
    public class GameManager : MonoBehaviour
    {
        [Header("ui scene name")]
        [SerializeField]
        private string _scenePlayerUI;

        public Player Player { get; private set; }

        private bool _gameIsPaused = false;

        private void Awake()
        {
            SceneManager.LoadScene(_scenePlayerUI, LoadSceneMode.Additive);
        }

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void RemovePlayer()
        {
            Player = null;
        }


        public void OnPauseGameInput(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;

            _gameIsPaused = !_gameIsPaused;
            PauseGame(_gameIsPaused);
        }


        private void PauseGame(bool value)
        {
            if (value)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}