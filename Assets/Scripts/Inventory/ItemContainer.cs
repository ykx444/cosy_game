using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    //stores reference to item in the itemContainer
    public Item item;
    public int count;
    public int maxCount = 99;


    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
        maxCount = slot.maxCount;
        
    }

    public void CopyAmount(ItemSlot slot, int amount)
    {
        item = slot.item;
        count = amount;
        maxCount = slot.maxCount;

    }

    public void Clear()
    {
        item = null;
        count = 0;
        maxCount = 99;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }
}

[CreateAssetMenu(menuName = "Data/ItemContainer")]

public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;
    public CollectableItem itemProcessing;


    public bool Add(Item item, int count = 1)
    {
        if(itemProcessing!=null)
        itemProcessing.item = item;
        
        if (item == null)
        {
            Debug.Log("Item is null.");
            return false;
        }

        if (item.stackable)
        {
            // Check if there's an existing slot with the same item that can take more of this item
            ItemSlot existingSlot = slots.Find(slot => slot.item != null && slot.item == item && slot.count < 99);
            if (existingSlot != null)
            {
                int availableSpace = 99 - existingSlot.count;
                if (count <= availableSpace)
                {
                    existingSlot.count += count;

                    return true;
                }
                else
                {
                    existingSlot.count = 99;
                    count -= availableSpace;
                }
            }

            // If there's still count left, try to find an empty slot
            while (count > 0)
            {
                ItemSlot emptySlot = slots.Find(slot => slot.item == null);
                if (emptySlot != null)
                {
                    int addCount = Mathf.Min(count, 99);
                    emptySlot.Set(item, addCount);
                    count -= addCount;
                }
                else
                {
                    // No more empty slots available
                    return false;
                }
            }
            return true;
        }
        else
        {
            // Non-stackable item
            ItemSlot emptySlot = slots.Find(slot => slot.item == null);
            if (emptySlot != null)
            {
                emptySlot.Set(item, count);
                return true;
            }
            else
            {
                // No empty slots available
                return false;
            }
        }
    }

    public void RemoveItem(Item itemToRemove, int count = 1)
    {
        //Find the storage containing the item and minus qty
        if (itemToRemove.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
            itemSlot.count -= count;
            if (itemSlot.count <= 0 && itemSlot != null)
            {
                itemSlot.Clear();
            }
        }
        else
        {
            while (count > 0)
            {
                count -= 1;
                ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
                if (itemSlot != null) itemSlot.Clear();
            }
        }
    }

    //check if we can deduct them altogether
    public bool CheckRemove(Item item, int count = 1)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == item);
        if (item.stackable)
        {
            return itemSlot.count >= count;
        }
        return false;
    }

    public void RemoveAll()
    {
        foreach (ItemSlot slot in slots)
        {
            slot.Clear();
        }
    }
    public void OrganiseInventory()
    {
        // Create a dictionary to hold items and their corresponding slots
        Dictionary<Item, List<ItemSlot>> itemSlotsDict = new Dictionary<Item, List<ItemSlot>>();

        // Populate the dictionary
        foreach (ItemSlot slot in slots)
        {
            if (slot.item != null && slot.item.stackable)
            {
                if (!itemSlotsDict.ContainsKey(slot.item))
                {
                    itemSlotsDict[slot.item] = new List<ItemSlot>();
                }
                itemSlotsDict[slot.item].Add(slot);
            }
        }

        // Loop through each item in the dictionary
        foreach (var itemEntry in itemSlotsDict)
        {
            List<ItemSlot> itemSlotList = itemEntry.Value;

            // Sort the list of slots by count in descending order
            itemSlotList.Sort((slot1, slot2) => slot2.count.CompareTo(slot1.count));

            for (int i = itemSlotList.Count - 1; i > 0; i--)
            {
                ItemSlot slotWithFewItems = itemSlotList[i];
                if (slotWithFewItems.count == 0) continue;

                for (int j = 0; j < i; j++)
                {
                    ItemSlot slotWithMoreItems = itemSlotList[j];
                    if (slotWithMoreItems.count < slotWithMoreItems.maxCount)
                    {
                        int availableSpace = slotWithMoreItems.maxCount - slotWithMoreItems.count;
                        int itemsToTransfer = Mathf.Min(slotWithFewItems.count, availableSpace);

                        slotWithMoreItems.count += itemsToTransfer;
                        slotWithFewItems.count -= itemsToTransfer;

                        if (slotWithFewItems.count == 0)
                        {
                            slotWithFewItems.Clear();
                            break;
                        }
                    }
                }
            }
        }

        // Display the organized slots for debugging purposes
        foreach (ItemSlot slot in slots)
        {
            if (slot.item != null)
            {
                Debug.Log($"Item: {slot.item.name}, Count: {slot.count}");
            }
        }
    }


}
