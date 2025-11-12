using Pom.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Pom.Dialogue
{
    public class DialogueBox : MonoBehaviour
    {
        [Header("Settings")]
        //[SerializeField] List<string> dialogues = new List<string> ();
        [SerializeField] float typeRate;

        [Header("References")]
        [SerializeField] TMP_Text textBox;

        int dialogueIndex;
        Coroutine currentCoroutine;
        string[] dialogues;

        public void SetDialogue(DialogueConfig config)
        {
            dialogues = config.Dialogues;
            dialogueIndex = 0;
        }

        //called by the "Next" button
        public void ShowNextDialogue()
        {
            if (dialogueIndex >= dialogues.Length && currentCoroutine == null)
            {
                GetComponent<UIToggler>().ToggleUI(false);
                return;
            }

            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(TypeDialogue(dialogues[dialogueIndex], () => { dialogueIndex++; }));
            }
            else
            {
                StopCoroutine(currentCoroutine);
                textBox.text = dialogues[dialogueIndex];
                currentCoroutine = null;
                dialogueIndex++;
            }
        }

        IEnumerator TypeDialogue(string dialogue, Action finished = null)
        {
            textBox.text = "";
            char[] dialogueArray = dialogue.ToCharArray();

            foreach (char c in dialogueArray)
            {
                textBox.text += c;
                yield return new WaitForSeconds(char.IsWhiteSpace(c) ? 0 : typeRate);
            }

            currentCoroutine = null;
            finished?.Invoke();
        }
    }
}
