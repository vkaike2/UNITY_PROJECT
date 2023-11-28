using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MouseInteractable
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Animator _animator;

    [Header("CONFIGURATION")]
    [SerializeField]
    private SpawnSpeed _spawnSpeed;

    public ItemData ItemData { get; set; }

    private Rigidbody2D _rigidBody2D;
    private GameManager _gameManager;

    private void Awake()
    {
        _priority = 0;
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void DropItem(DropConfiguration dropConfiguration = DropConfiguration.Player)
    {
        Vector2 force = new Vector2(_spawnSpeed.Horizontal.GetRandom(), _spawnSpeed.Vertical.GetRandom());
        float torque = 100;

        if (dropConfiguration == DropConfiguration.Chest)
        {
            force = new Vector2(force.x*2, force.y);
        }

        _rigidBody2D.AddForce(
            force,
            ForceMode2D.Impulse);

        _rigidBody2D.AddTorque(torque);
    }

    public override void ChangeAnimationOnItemOver(bool isMouseOver)
    {
        //if (!PlayerIsInRange())
        //{
        //    _animator.Play(MyAnimations.Idle.ToString());
        //    return;
        //}

        if (isMouseOver)
        {
            _animator.Play(MyAnimations.Selected.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Idle.ToString());
        }
    }

    public override void InteractWith(CustomMouse mouse)
    {
        if (_gameManager.InventoryIsOpen)
        {
            mouse.StartDragItem(ItemData);
            Destroy(this.gameObject);
            return;
        }

        List<Vector2> itemCoordinates = _gameManager.PlayerInventory.CheckIfCanAddItem(ItemData.Item.InventoryItemLayout);
        List<Vector2> equipCoordinates = null;

        if (ItemData.Item.IsEquipable)
        {
            equipCoordinates = _gameManager.PlayerInventory.CheckIfCanAutoEquip(ItemData.Item.InventoryItemLayout, ItemData);
        }

        if(equipCoordinates != null)
        {
            _gameManager.PlayerInventory.EquipItem(ItemData, equipCoordinates);
            Destroy(this.gameObject);
        }
        else if (itemCoordinates != null)
        {
            _gameManager.PlayerInventory.AddItem(ItemData, itemCoordinates);
            Destroy(this.gameObject);
        }
        else
        {
            this.DropItem();
        }
    }

    public enum DropConfiguration
    {
        Player,
        Chest
    }

    private enum MyAnimations
    {
        Idle,
        Selected
    }

    [Serializable]
    private class SpawnSpeed
    {
        [SerializeField]
        private MinMax _vertical;
        [SerializeField]
        private MinMax _horizontal;

        public MinMax Vertical => _vertical;
        public MinMax Horizontal => _horizontal;
    }
}
