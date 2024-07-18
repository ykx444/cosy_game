using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronCraftManager : MonoBehaviour
{
    [SerializeField] int maxItemCount = 3; // how many cauldron can hold

    CauldronInteract currentCauldron;
    public Button confirmButton;

    private void Start()
    {
        confirmButton.onClick.AddListener(StartProcessing);
    }
    public void AddItem(CauldronInteract targetCauldron)
    {
        CheckCurrentRecipe(targetCauldron);
        //if (targetCauldron.itemsInPot.Count < maxItemCount)
        //{
        //    if (item.itemType == ItemType.ingredient)
        //    {
        //        //targetCauldron.AddItem(item);
        //        CheckCurrentRecipe(targetCauldron);

        //    }
        //}
    }


    //public void ClearCauldron(CauldronInteract cauldron)
    //{

    //    if (cauldron != null)
    //    {
    //        currentCauldron.resultSlot.sprite = null;
    //        cauldron.ClearAllItems();
    //    }

    //}

    // Method to check the current recipe
    public void CheckCurrentRecipe(CauldronInteract cauldron)
    {
        CauldronRecipe result = GameManager.instance.recipeManager.CheckRecipe(cauldron.cauldronInventory);
        if (result != null)
        {
            cauldron.resultSlot.gameObject.SetActive(true);
            cauldron.resultSlot.sprite = result.output.icon;
            cauldron.SetUpInitialValues(result.processTime, 0);
            cauldron.cookingItem = result.output;
        }
        else
        {
            cauldron.resultSlot.gameObject.SetActive(false);
            cauldron.cookingItem = null;
            Debug.Log("No matching recipe.");
        }
    }

    // Button press
    public void StartProcessing()
    {
        currentCauldron = GameManager.instance.currentCauldron;
        if (currentCauldron.cookingItem != null)
        {
            currentCauldron.StartCookingProcess(currentCauldron.cookingItem);
        }
    }

}
