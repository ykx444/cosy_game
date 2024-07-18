using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField]
    List<CauldronRecipe> recipes;

    // Method to check if a set of items in the ItemContainer matches any recipe
    public CauldronRecipe CheckRecipe(ItemContainer cauldronInventory)
    {
        foreach (CauldronRecipe recipe in recipes)
        {
            if (IsRecipeMatch(recipe, cauldronInventory))
            {
                return recipe;
            }
        }
        return null; // No matching recipe found
    }

    private bool IsRecipeMatch(CauldronRecipe recipe, ItemContainer cauldronInventory)
    {
        // Get item counts from the cauldron inventory
        Dictionary<Item, int> potItemCounts = GetItemCountsFromContainer(cauldronInventory);

        // Get item counts from the recipe
        Dictionary<Item, int> recipeItemCounts = GetItemCounts(recipe.requiredMaterials);

        // Compare the dictionaries
        foreach (KeyValuePair<Item, int> kvp in recipeItemCounts)
        {
            if (!potItemCounts.ContainsKey(kvp.Key) || potItemCounts[kvp.Key] != kvp.Value)
            {
                return false;
            }
        }

        return true; // All items match
    }

    private Dictionary<Item, int> GetItemCounts(List<Item> items)
    {
        Dictionary<Item, int> itemCounts = new Dictionary<Item, int>();
        foreach (Item item in items)
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item]++;
            }
            else
            {
                itemCounts[item] = 1;
            }
        }
        return itemCounts;
    }

    private Dictionary<Item, int> GetItemCountsFromContainer(ItemContainer container)
    {
        Dictionary<Item, int> itemCounts = new Dictionary<Item, int>();
        foreach (ItemSlot slot in container.slots)
        {
            if (slot.item != null)
            {
                if (itemCounts.ContainsKey(slot.item))
                {
                    itemCounts[slot.item] += slot.count;
                }
                else
                {
                    itemCounts[slot.item] = slot.count;
                }
            }
        }
        return itemCounts;
    }
}
