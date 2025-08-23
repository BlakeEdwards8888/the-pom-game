using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using UnityEngine;
using Pom.CharacterActions.Movement;
using Pom.CharacterActions.Combat;
using System.Collections.Generic;
using System.Collections;

namespace Pom.Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; private set; }

        [field: SerializeField] public string DisplayName { get; private set; }

        [field: SerializeField] public Mover Mover { get; private set; }
        [field: SerializeField] public Attacker Attacker { get; private set; }
        [field: SerializeField] public Alliance Alliance { get; private set; }

        public bool CanMove { get; private set; } = true;
        public bool CanAttack { get; private set; } = true;

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
            CanMove = true;
            CanAttack = true;
        }

        public bool CanMoveTo(Vector2 destination, out List<PathNode> path, PathFinder.RangeOverflowMode rangeOverflowMode)
        {
            path = null;

            if (!CanMove) return false;

            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            path = pathFinder.GetPath(currentGridPosition, destination, Mover.GetRange(), rangeOverflowMode);

            if (path == null) return false;

            return true;
        }

        public IEnumerator MoveAlongPath(List<PathNode> path)
        {
            CanMove = false;
            yield return Mover.MoveAlongPath(path);
        }

        public bool TryAttack(Vector2 destination)
        {
            if (!CanAttack) return false;

            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            if (!Attacker.IsTargetInRange(currentGridPosition, destination)) return false;

            if (GridSystem.Instance.NavDict[destination].TryGetOccupyingEntity(out Health targetHealth))
            {
                if (targetHealth.TryGetComponent(out Alliance targetAlliance) && targetAlliance.AlliedFaction == Alliance.AlliedFaction) return false;
                Attacker.Attack(targetHealth);
                CanAttack = false;
                return true;
            }

            return false;
        }
    }
}
