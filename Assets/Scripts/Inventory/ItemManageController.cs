using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManageController : MonoBehaviour
{
    //manage shifting or dropping item in a slot

    [SerializeField] ItemSlot itemSlot; //current item to be dragged
    [SerializeField] ItemSlot originalSlot;
    [SerializeField] GameObject dragitemIcon;

    //store transform to move the icon
    RectTransform iconTransform;
    Image itemIconImage;


    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = dragitemIcon.GetComponent<RectTransform>();
        itemIconImage = dragitemIcon.GetComponent<Image>();
    }

    private void Update()
    {
        if (dragitemIcon.activeInHierarchy)
        {
            iconTransform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                // feature to allow user to drop item out of inventory
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //************drop item
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //z index has to be 0 to see the object
                    worldPos.z = 0;

                    //SpawnDroppedItemManager.instance.SpawnItem(worldPos, itemSlot.item, itemSlot.count);
                    //clear the slot after spawn
                    itemSlot.Clear();//for inventory shift, clear

                    dragitemIcon.SetActive(false);
                }
            }
            //player close the inventory, while operating on item
            if (!GameManager.instance.inventoryPanel.isActiveAndEnabled)
            {
                ReturnItemToSlot();
            }
        }
    }
    internal void OnClick(ItemSlot itemSlot, string name)
    {
        //see what panel button it is, 
        if (name.Equals("PotionCraftPanel"))
        {
            Debug.Log("You are clicking on potion panel");
            if (this.itemSlot == null)
            {
                originalSlot = itemSlot;
                this.itemSlot.Copy(itemSlot);
            }
            else
            {
                //exchange items
                itemSlot.CopyAmount(this.itemSlot, 1);
                if (this.itemSlot.count <= 1)
                {
                   
                    //copy passed itemslot content into a temp slot, clear the passed itemselot
                    GameManager.instance.cauldronManager.AddItem(GameManager.instance.currentCauldron);
                    this.itemSlot = null;
                    dragitemIcon.SetActive(false);
                    return;
                }
                    
               
                this.itemSlot.Set(this.itemSlot.item, this.itemSlot.count - 1);

            }
        }
        else
        {
            if (this.itemSlot == null)
            {
                originalSlot = itemSlot;
                this.itemSlot.Copy(itemSlot);

                itemSlot.Clear();
            }

            else
            {
                //exchange items
                Item item = itemSlot.item;
                int count = itemSlot.count;
                itemSlot.Copy(this.itemSlot); //assign the item being dragged into the inventory item slot
                this.itemSlot.Set(item, count); //exchange procedure of 2 slots

            }
        }
        //copy passed itemslot content into a temp slot, clear the passed itemselot
        if (GameManager.instance.cauldronPanelObject.activeInHierarchy)
            GameManager.instance.cauldronManager.AddItem(GameManager.instance.currentCauldron);
        UpdateIcon();
        //update UI
        GameManager.instance.toolbarPanel.Show();
        GameManager.instance.inventoryPanel.Show();
    }

    private void ReturnItemToSlot()
    {
        if (itemSlot.item != null)
        {
            originalSlot.Copy(itemSlot);
            itemSlot.Clear();
            UpdateIcon();
        }
    }
    private void UpdateIcon()
    {
        if (itemSlot.item == null)
        {
            dragitemIcon.SetActive(false);
        }
        else
        {
            dragitemIcon.SetActive(true);
            itemIconImage.sprite = this.itemSlot.item.icon;
        }
    }
}
