using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image highlight;

    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    //only need to be called once, this is to assign each slot an index.
    public void Set(ItemSlot slot)
    {
        icon.sprite = slot.item.icon;
        if (slot.item.stackable == true)
        {
            text.gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
            text.text = slot.count.ToString();
        }
        else
        {
            text.gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
            text.gameObject.SetActive(false);
        }
    }

    public void Clean()
    {
        icon.sprite = null;
        text.text = "";
        icon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ////send itemselot associated with this button to itemManager.cs
        //ItemContainer inventory = GameManager.instance.playerInventory;
        //GameManager.instance.itemController.OnClick(inventory.slots[myIndex]);
        //GameManager.instance.inventoryPanel.Show();//update ui

        //get itempanel from parent object

        Debug.Log(eventData.pointerEnter.transform.parent.gameObject.transform.parent.gameObject.name);
        ToolBarPanel toolbar = transform.parent.GetComponent<ToolBarPanel>();
        toolbar.OnClick(myIndex, eventData.pointerEnter.transform.parent.gameObject.transform.parent.gameObject.name);
        toolbar.OnClick(myIndex);
    }

    public void Highlight(bool b)
    {
        highlight.gameObject.SetActive(b);
    }
}
