using Pom.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] DialogueBox dialogueBox;
        [SerializeField] DialogueConfig dialogueConfig;

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TriggerDialogue();
            }
        }

        public void TriggerDialogue()
        {
            dialogueBox.GetComponent<UIToggler>().ToggleUI(true);
            dialogueBox.SetDialogue(dialogueConfig);
            dialogueBox.ShowNextDialogue();
        }
    }
}
