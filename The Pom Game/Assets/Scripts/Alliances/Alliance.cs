using UnityEngine;

namespace Pom.Alliances
{
    public class Alliance : MonoBehaviour
    {
        [field: SerializeField] public Faction AlliedFaction { get; private set; }
    }
}
