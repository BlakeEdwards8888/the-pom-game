using System.Collections;
using UnityEngine;

namespace Pom.Dialogue.DialogueTriggers
{
    public class StartDialogueTrigger : DialogueTrigger
    {
        [SerializeField] float delay;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            TriggerDialogue();
        }
    }
}
