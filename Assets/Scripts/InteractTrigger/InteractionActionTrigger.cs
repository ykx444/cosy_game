using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//put it at stuff you want to start 
public class InteractionActionTrigger : Interactable
{
    public override void Interact()
    {
        if (LevelManager.instance.LevelNum == 0 && GameManager.instance.customerManager.isOperating)
        {
            GameManager.instance.interactionActionManager.
            ShowInteraction("Do you want to close the shop today?", new List<string> { "Yes", "No" },
            GameManager.instance.interactionActionManager.HandleChoiceClose, null);
        }

        else if (GameManager.instance.timeController.Hours > 17f && LevelManager.instance.LevelNum == 1)
        {
            GameManager.instance.interactionActionManager.
           ShowInteraction("Do you want to skip to next day?", new List<string> { "Yes", "No" },
           GameManager.instance.interactionActionManager.HandleChoiceTimeSkip, null);
        }
        else if (!GameManager.instance.customerManager.isOperating)
        {
            GameManager.instance.interactionActionManager.
           ShowInteraction("Do you want to open the shop?", new List<string> { "Yes", "No" },
           GameManager.instance.interactionActionManager.HandleChoiceOpen, null);
        }
        else return; //dont display anything if otherwise

    }

}