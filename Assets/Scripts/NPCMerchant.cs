using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMerchant : Interactable
{
    public Sprite speakerImage;
    public override void Interact()
    {
        //open dialogue
        GameManager.instance.interactionActionManager.
            ShowInteraction("Do you want to buy something?", new List<string> { "Yes", "No" },
            GameManager.instance.interactionActionManager.HandleShopOpen, speakerImage);
    }
}
