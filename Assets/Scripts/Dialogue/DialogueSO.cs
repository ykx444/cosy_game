using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueChoice[] choices;
    public bool hasTutorial;
    public DialogueSO nextDialogue;
    //public int choiceTriggerLineIndex; // Add this field
}

//DIALOGUE SO contains Dialogue