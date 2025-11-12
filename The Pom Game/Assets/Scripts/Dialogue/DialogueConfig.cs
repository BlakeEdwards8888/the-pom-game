using UnityEngine;

namespace Pom.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class DialogueConfig : ScriptableObject
    {
        [field: SerializeField] public string[] Dialogues { get; private set; }
    }
}
