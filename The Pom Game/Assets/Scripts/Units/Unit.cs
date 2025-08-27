using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using UnityEngine;
using Pom.CharacterActions.Movement;
using Pom.CharacterActions.Combat;
using System.Collections.Generic;
using Pom.CharacterActions;
using Pom.CharacterActions.AIExecutionStrategies;

namespace Pom.Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Alliance Alliance { get; private set; }
        [field: SerializeField] public List<ActionExecutor> Actions { get; private set; }

        public Vector2 Position
        {
            get
            {
                return GridSystem.Instance.GetGridPosition(transform.position);
            }
            private set { }
        }

        protected PathFinder pathFinder => GetComponent<PathFinder>();

        public void ResetActionStates()
        {
            foreach (ActionExecutor action in Actions)
            {
                action.ResetUsageState();
            }
        }
    }
}
