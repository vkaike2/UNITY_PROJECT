using UnityEngine;

public abstract class MouseInteractable : MonoBehaviour
{
    [Header("INTERACTABLE")]
    [SerializeField]
    protected float _playerInteractableRange = 5;

    public int Priority => _priority;


    protected int _priority = 10;
    protected GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public abstract void ChangeAnimationOnItemOver(bool isMouseOver);
    public abstract void InteractWith(CustomMouse mouse);

    protected bool PlayerIsInRange()
    {
        //Debug.Log(Vector2.Distance(_gameManager.Player.transform.position, this.transform.position));
        return Vector2.Distance(_gameManager.Player.transform.position, this.transform.position) <= _playerInteractableRange;
    }
}
