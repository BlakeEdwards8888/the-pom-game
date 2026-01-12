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
            float localTypeRate = typeRate;

            for(int i = 0; i < dialogueArray.Length; i++)
            {
                if (dialogueArray[i] == '<')
                {
                    string formattingString = "<";

                    for(int j = i + 1; j < dialogueArray.Length; j++)
                    {
                        formattingString += dialogueArray[j];
                        if(dialogueArray[j] == '>')
                        {
                            textBox.text += formattingString;
                            i = j + 1;
                            break;
                        }
                    }
                }

                textBox.text += dialogueArray[i];
                yield return new WaitForSecondsRealtime(char.IsWhiteSpace(dialogueArray[i]) ? 0 : typeRate);
            }

            currentCoroutine = null;
            finished?.Invoke();
        }
    }
}
