using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanel : MonoBehaviour
{
    public ItemContainer inventory;
    public List<InventoryButton> buttons;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetIndex();
    }

    public void OnEnabled()
    {
        Show();
    }

    private void SetIndex()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            buttons[i].SetIndex(i); //give each button in panel a id
        }
    }

    private void OnEnable()
    {
        Show();
    }
    public virtual void Show()
    {
        inventory.OrganiseInventory();
        //set button if it has item, else clean it
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            if (inventory.slots[i].item == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(inventory.slots[i]);
            }
        }
    }

   
    public virtual void OnClick(int id, string name){}
    public virtual void OnClick(int id) { }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
