using Calcatz.MeshPathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("UI SCENE NAME")]
    [SerializeField]
    private string _scenePlayerUI;

    [Header("PATHFINDING")]
    [SerializeField]
    private Waypoints _waypoints;

    public Player Player { get; private set; }
    public List<Worm> Worms { get; private set; }
    public Waypoints Waypoints => _waypoints;

    private bool _gameIsPaused = false;

    private void Awake()
    {
        Worms = new List<Worm>();
        SceneManager.LoadScene(_scenePlayerUI, LoadSceneMode.Additive);
    }

    #region GET & SET
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
    #endregion

    #region Worms

    public Worm GetNearestWorm(Vector2 position)
    {
        if (Worms.Count == 0) return null;

        return Worms
            .Where(e => e.CurrentBehaviour != Worm.Behaviour.Die)
            .Select(e => new
            {
                Worm = e,
                Distance = Vector2.Distance(position, e.transform.position)
            })
            .OrderBy(e => e.Distance)
            .Select(e => e.Worm)
            .FirstOrDefault();
    }
    #endregion

    #region PAUSE
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
    #endregion
}
