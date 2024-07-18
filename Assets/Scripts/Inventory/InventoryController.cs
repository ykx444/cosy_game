using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject toolbarPanel;

    private void Update()
    {
        //press I, inventory toggle
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
            //toolbar action will update inventory panel
            //if it close and open
            toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
            GameManager.instance.markerManager.Show(false);
        }
    }
}
