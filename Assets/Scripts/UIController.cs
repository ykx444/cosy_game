using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject inventory;
    private void Start()
    {
        CloseInventory();
    }
    public void CloseInventory()
    {
        inventory.SetActive(false);
        GameManager.instance.toolbarPanel.gameObject.SetActive(true);
    }

    public void CloseCauldronPanel()
    {
        foreach (ItemSlot item in GameManager.instance.currentCauldron.cauldronInventory.slots)
        {
            if(item!=null)
            GameManager.instance.playerInventory.Add(item.item);
        }
        //update UI
        GameManager.instance.toolbarPanel.Show();
        GameManager.instance.inventoryPanel.Show();

        GameManager.instance.currentCauldron.ClearAllItems();
        GameManager.instance.currentCauldron = null;
        GameManager.instance.cauldronPanelObject.SetActive(false);

        GameManager.instance.recipeUIPanel.Close();
    }

}
