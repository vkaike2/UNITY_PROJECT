using Calcatz.MeshPathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
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

    //GAME OBJECTS
    public Player Player { get; private set; }
    public PlayerInventory PlayerInventory { get; private set; }

    public List<Worm> Worms { get; private set; }
    public Waypoints Waypoints => _waypoints;

    //UI
    private InventoryUI _inventoryUI;
    public bool InventoryIsOpen => _inventoryUI.IsOpen;

    private bool _gameIsPaused = false;

    private void Awake()
    {
        Worms = new List<Worm>();

        LoadUIScene();
    }

    #region GET & SET
    public void SetPlayer(Player player)
    {
        Player = player;
        PlayerInventory = player.GetComponent<PlayerInventory>();
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

    public void SetInventory(InventoryUI inventoryUI)
    {
        _inventoryUI = inventoryUI;
    }

    public void SetWaypoints(Waypoints waypoints)
    {
        _waypoints = waypoints;
    }
    #endregion

    #region WORMS
    public Worm GetNearestWorm(Vector2 position)
    {
        if (Worms.Count == 0) return null;

        Worm worm = Worms
            .Where(e => !e.IsBeingTargeted)
            .Where(e => e.CurrentBehaviour != Worm.Behaviour.Die && e.CurrentBehaviour != Worm.Behaviour.Reborn)
            .Select(e => new
            {
                Worm = e,
                Distance = Vector2.Distance(position, e.transform.position)
            })
            .OrderBy(e => e.Distance)
            .Select(e => e.Worm)
            .FirstOrDefault();

        if (worm != null)
        {
            worm.IsBeingTargeted = true;
        }

        return worm;
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

    private void LoadUIScene()
    {
        if (!IsSceneLoaded(_scenePlayerUI))
        {
            SceneManager.LoadScene(_scenePlayerUI, LoadSceneMode.Additive);
        }
    }

    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }


}
