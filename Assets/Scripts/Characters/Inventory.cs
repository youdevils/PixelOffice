using UnityEngine;
using GameDataDictionary;
using System.Collections.Generic;

public class Inventory {
    //Inventory
    public int maxInventorySlots = 20;
    public int usedInventorySlots;
    public Dictionary<int, ItemData> slots = new Dictionary<int, ItemData>();
    #region Inventory Methods
    public bool AddInventoryItem(ItemData _data) {
        if (usedInventorySlots < maxInventorySlots) {
            int id = GetSlotID();
            if (id != -1) {
                usedInventorySlots++;
                int slotid = GetSlotID();
                slots.Add(slotid, _data);
                UIManager.instance.AddInventoryItem(_data, slotid);
                return true;
            } else {
                Debug.LogError("Item Not Added. GetSlotID Error.");
                return false;
            }
        } else {
            Debug.Log("Inventory Full.");
            return false;
        }
    }

    private int GetSlotID() {
        if (usedInventorySlots == 0)
            return 0;

        for (int i = 0; i < maxInventorySlots; i++) {
            if (!slots.ContainsKey(i))
                return i;
        }
        return -1;
    }

    public void RemoveInventoryItem(int _slotID) {
        if (slots.ContainsKey(_slotID)) {
            slots.Remove(_slotID);
            UIManager.instance.RemoveItemFromSlot(_slotID);
            return;
        }
        Debug.LogError("Cannot find item that matches ID:" + _slotID);
        return;
    }

    public void ClearInventory() {
        slots.Clear();
        usedInventorySlots = 0;
    }

    public void RefreshInventoryUI() {
        UIManager.instance.ClearAllItemObjects();
        foreach (int key in Player.instance.Inventory.slots.Keys) {
            UIManager.instance.AddInventoryItem(Player.instance.Inventory.slots[key], key);
        }
    }
    #endregion
}
