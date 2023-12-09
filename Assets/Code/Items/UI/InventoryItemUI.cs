using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItemUI : MouseOver
{

    [Header("COMPONENTS")]
    [SerializeField]
    private Animator _animator;

    public bool IsBeingDragged { get; set; }

    public ItemData ItemData { get; set; }
    public SlotType SlotType { get; set; }
    public InventoryUI Inventory { get; private set; }  


    private List<InventoryItemSlotUI> _slots;
    private List<InventorySlotUI> _tempInventorySlots = new List<InventorySlotUI>();
    private RectTransform _rectTransform;

    private void Awake()
    {
        _slots = GetComponentsInChildren<InventoryItemSlotUI>().ToList();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Inventory = GameObject.FindObjectOfType<InventoryUI>();
    }

    private void Update()
    {
        ManageDrag();
    }

    public bool CheckIfCanFit()
    {
        List<InventorySlotUI> slotsUnderItem = GetEveryInventorySlotUnderItem();
        slotsUnderItem = slotsUnderItem.Where(e => e.CanReceiveANewItem()).ToList();

        return slotsUnderItem.Count == _slots.Count;
    }

    public bool CheckIfCanSwap()
    {
        List<InventorySlotUI> slotsUnderItem = GetEveryInventorySlotUnderItem();
        slotsUnderItem = slotsUnderItem.Where(e => e.CanReceiveANewItem()).ToList();

        if (slotsUnderItem.Count != _slots.Count) return false;
        if (slotsUnderItem.Select(e => e.ItemUI.ItemData.Id).Distinct().Count() > 1) return false;

        return true;
    }

    /// <summary>
    ///     Used when you add an item with drag and drop
    /// </summary>
    /// <param name="itemParent"></param>
    /// <returns></returns>
    public List<InventorySlotUI> SetPositionInsideInventory(Transform itemParent)
    {
        List<InventorySlotUI> inventorySlotsUnderItem = GetEveryInventorySlotUnderItem();
        List<Vector2> everySlotUnderItemPosition = inventorySlotsUnderItem.Select(e => (Vector2)e.transform.position).ToList();

        SlotType = inventorySlotsUnderItem.FirstOrDefault().Type;

        RectTransform rectTransform = GetComponent<RectTransform>();
        this.transform.position = (Vector2)everySlotUnderItemPosition.CalculateMiddlePosition();

        this.transform.SetParent(itemParent);
        _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.x, _rectTransform.localPosition.y, 0f);

        return inventorySlotsUnderItem;
    }

    public void RemoveFromInventory()
    {
        Inventory.RemoveItem(ItemData.Id);
        Destroy(this.gameObject);
    }

    public void StopDrag()
    {
        IsBeingDragged = false;

        foreach (var slot in GetEveryInventorySlotUnderItem())
        {
            slot.ChangeAnimationOnItemOver(false);
        }
    }

    public List<InventorySlotUI> GetEveryInventorySlotUnderItem()
    {
        List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
        foreach (InventoryItemSlotUI slot in _slots)
        {
            InventorySlotUI inventorySlot = RaycastUtils
                .GetComponentsUnderPositionUI<InventorySlotUI>(slot.transform.position, new List<RaycastUtils.Excluding>() { RaycastUtils.Excluding.Children })
                .FirstOrDefault();

            if (inventorySlot == null) continue;
            inventorySlots.Add(inventorySlot);
        }
        return inventorySlots;
    }

    public InventoryItemUI GetItemToSwap()
    {
        List<InventorySlotUI> slotsUnderItem = GetEveryInventorySlotUnderItem();
        return slotsUnderItem.Where(e => e.HasItem).Select(e => e.ItemUI).FirstOrDefault();
    }


    private void ManageDrag()
    {
        if (!IsBeingDragged) return;

        List<InventorySlotUI> inventorySlotsUnderItem = GetEveryInventorySlotUnderItem();

        foreach (InventorySlotUI slot in _tempInventorySlots.Where(e => inventorySlotsUnderItem.FirstOrDefault(s => s.GetInstanceID() == e.GetInstanceID()) == null))
        {
            slot.ChangeAnimationOnItemOver(false);
        }

        _tempInventorySlots.Clear();
        _tempInventorySlots.AddRange(inventorySlotsUnderItem);

        foreach (InventorySlotUI slot in _tempInventorySlots)
        {
            slot.ChangeAnimationOnItemOver(true);
        }
    }

    public override void ChangeAnimationOnItemOver(bool isMouseOver)
    {
        if (_animator == null) return;

        if (isMouseOver)
        {
            _animator.Play(BasicAnimations.MouseOver.ToString());
        }
        else
        {
            _animator.Play(BasicAnimations.Idle.ToString());
        }
    }


    private enum BasicAnimations
    {
        Idle,
        MouseOver
    }
}
