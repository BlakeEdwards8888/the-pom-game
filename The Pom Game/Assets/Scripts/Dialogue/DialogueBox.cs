using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueBox : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] List<string> dialogues = new List<string> ();
        [SerializeField] float typeRate;

        [Header("References")]
        [SerializeField] TMP_Text textBox;

        int dialogueIndex;

        private void Start()
        {
            ShowNextDialogue();
        }

        //called by the "Next" button
        public void ShowNextDialogue()
        {
            if (dialogueIndex == dialogues.Count) return;

            StopAllCoroutines();
            StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
            dialogueIndex++;
        }

        IEnumerator TypeDialogue(string dialogue)
        {
            textBox.text = "";
            char[] dialogueArray = dialogue.ToCharArray();

            foreach (char c in dialogueArray)
            {
                textBox.text += c;
                yield return new WaitForSeconds(char.IsWhiteSpace(c) ? 0 : typeRate);
            }
        }
    }
}
