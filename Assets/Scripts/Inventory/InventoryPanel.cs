using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : ToolBarPanel
{
    public override void OnClick(int id, string name)
    {
        ItemContainer inventory = GameManager.instance.playerInventory;

        GameManager.instance.itemController.OnClick(inventory.slots[id], name);

        GameManager.instance.inventoryPanel.Show();//update ui
    }
}

