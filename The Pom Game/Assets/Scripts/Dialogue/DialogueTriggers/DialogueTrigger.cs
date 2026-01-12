using Pom.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Dialogue.DialogueTriggers
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] DialogueBox dialogueBox;
        [SerializeField] DialogueConfig dialogueConfig;

        public void TriggerDialogue()
        {
            dialogueBox.GetComponent<UIToggler>().ToggleUI(true);
            dialogueBox.SetDialogue(dialogueConfig);
            dialogueBox.ShowNextDialogue();
        }
    }
}
