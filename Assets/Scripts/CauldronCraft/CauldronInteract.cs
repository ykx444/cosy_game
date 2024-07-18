using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronInteract : Interactable
{
    public ItemContainer cauldronInventory;
    public Image progressBar;
    public GameObject progressBarContainer;
    public Image resultSlot;

    private float timeToProcess;
    private float cookingTimeElapsed;
    private bool isCooking;
    public Item cookingItem;
    //public List<Item> itemsInPot = new List<Item>();


    public override void Interact()
    {
        //  slotImages = GameManager.instance.slotImages;
        resultSlot = GameManager.instance.resultSlot;
        GameManager.instance.cauldronPanelObject.SetActive(true);
        GameManager.instance.uIController.inventory.SetActive(true);
        GameManager.instance.recipeUIPanel.Open();
    }


    public void SetUpInitialValues(float processTime, float elapsed)
    {
        timeToProcess = processTime;
        cookingTimeElapsed = elapsed;
    }

    public void ToggleBarVisibility(bool isVisible)
    {
        progressBarContainer.SetActive(isVisible);
    }

    public void SetCurrentTime(float elapsed)
    {
        cookingTimeElapsed = elapsed;
    }

    public void UpdateBar()
    {
        progressBar.fillAmount = cookingTimeElapsed / timeToProcess;
    }

    public void StartCookingProcess(Item item)
    {
        if (!isCooking)
        {
            cookingItem = item;
            cookingTimeElapsed = 0;
            GameManager.instance.cauldronPanelObject.SetActive(false);
            StartCoroutine(CookingProcess());
        }
    }

    private IEnumerator CookingProcess()
    {
        isCooking = true;
        ToggleBarVisibility(true);

        while (cookingTimeElapsed < timeToProcess)
        {
            cookingTimeElapsed += Time.deltaTime;
            UpdateBar();
            yield return null;
        }

        isCooking = false;
        ToggleBarVisibility(false);

        // Drop the item or move to inventory, adjust as needed
        SpawnDroppedItemManager.instance.SpawnItem(GameManager.instance.player.transform.position, cookingItem, 1);
        GameManager.instance.audioManager.PlaySoundEffect("cook finish");
        ClearAllItems();
    }

    public void ClearAllItems()
    {
        //itemsInPot.Clear();
        cauldronInventory.RemoveAll();
        resultSlot.sprite = null;
    }

    //public void AddItem(Item item)
    //{
    //    itemsInPot.Add(item);
    //}


}
