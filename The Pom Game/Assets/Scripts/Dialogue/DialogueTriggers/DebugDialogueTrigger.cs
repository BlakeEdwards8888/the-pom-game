using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Dialogue.DialogueTriggers
{
    public class DebugDialogueTrigger : DialogueTrigger
    {
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TriggerDialogue();
            }
        }

    }
}
