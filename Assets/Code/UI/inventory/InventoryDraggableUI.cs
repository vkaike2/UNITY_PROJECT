using System.Linq;
using UnityEngine;

public class InventoryDraggableUI : MonoBehaviour
{
    private bool _isBeingDragged = false;
    private InventoryItemUI _activePrefab;
    private InventoryUI _inventoryUI;

    private void Start()
    {
        _inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
    }

    private void Update()
    {
        if (!_isBeingDragged) return;
        this.transform.position = Input.mousePosition;
    }

    public void StartDragItem(ScriptableItem item)
    {
        _activePrefab = Instantiate(item.PrefabUI, this.transform);
        _activePrefab.transform.position = this.transform.position;
        _activePrefab.SetItem(item);
        _activePrefab.IsBeingDragged = true;

        _isBeingDragged = true;
    }

    public void StopDragItem()
    {
        _activePrefab.StopDrag();

        if (!TryToAddItemToInventory())
        {
            Destroy(_activePrefab.gameObject);
        }

        _activePrefab = null;
        _isBeingDragged = false;
    }

    private bool TryToAddItemToInventory()
    {
        InventorySlotUI slot = RaycastUtils.GetComponentsUnderMouseUI<InventorySlotUI>().FirstOrDefault();
        if (slot == null) return false;

        bool canFit = _activePrefab.CheckIfCanFit();
        if (!canFit) return false;

        _inventoryUI.AddItem(_activePrefab);

        return true;
    }

}
