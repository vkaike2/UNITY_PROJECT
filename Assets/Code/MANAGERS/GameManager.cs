using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("ui scene name")]
    [SerializeField]
    private string _scenePlayerUI;

    public Player Player { get; private set; }
    public List<Worm> Worms { get; private set; }

    private bool _gameIsPaused = false;

    private void Awake()
    {
        Worms = new List<Worm>();
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

    public void SetWorm(Worm worm)
    {
        Worms.Add(worm);
    }

    public void RemoveWorm(Worm worm)
    {
        Worms.Remove(worm);
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
