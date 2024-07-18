using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine dialogue;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    dialogueManager.StartDialogue(dialogue);
        //}

        //first part of dialogue ends, add tutorial here
    }
}

