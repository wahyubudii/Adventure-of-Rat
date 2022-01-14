using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public int slotNum;
    public InventoryItem inventoryItem;

    public Button slotBtn;
    public Image icon;
    public Text num;

    public void AddItem(InventoryItem newItem) {
        inventoryItem = newItem;
        icon.sprite = inventoryItem.item.icon;

        slotBtn.enabled = true;
        icon.enabled = true;
        num.enabled = true;
        UpdateNumText();
    }

    public void ClearSlot() {
        inventoryItem = null;
        icon.sprite = null;

        slotBtn.enabled = false;
        icon.enabled = false;
        num.enabled = false;
    }

    //dipanggil saat jumlah item dalam UI inventory atau tas berubah --> text diubah
    public void UpdateNumText() {
        num.text = inventoryItem.NumPerCell.ToString();
        num.enabled = true;
    }

    public void OnSlotBtn(int slotNum) {
        if(inventoryItem != null)
            Inventory.Instance.inventoryUIScript.ShowItemInform(slotNum);
    }
}