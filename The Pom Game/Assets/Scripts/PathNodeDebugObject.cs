using TMPro;
using UnityEngine;

public class PathNodeDebugObject : MonoBehaviour
{
    [field: SerializeField] public TMP_Text GScoreText { get; private set; }
    [field: SerializeField] public TMP_Text HScoreText { get; private set; }
    [field: SerializeField] public TMP_Text FScoreText { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
}
