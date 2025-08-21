using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using UnityEngine;
using Pom.CharacterActions.Movement;
using Pom.CharacterActions.Combat;
using System.Collections.Generic;

namespace Pom.Units
{
    public abstract class Unit : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; private set; }

        [field: SerializeField] public string DisplayName { get; private set; }

        [field: SerializeField] public Mover Mover { get; private set; }
        [field: SerializeField] public Attacker Attacker { get; private set; }
        [field: SerializeField] public Alliance Alliance { get; private set; }
        protected PathFinder pathFinder => GetComponent<PathFinder>();
    }
}
