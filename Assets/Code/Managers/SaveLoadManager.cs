using System.Collections;
using UnityEngine;


public static class SaveLoadManager 
{
    // Inventory
    public static InventoryData InventoryData;
    public static InventoryData EquipData;
   
    public static bool HasInventoryInfo()
    {
        return InventoryData != null && EquipData != null;
    }
}
