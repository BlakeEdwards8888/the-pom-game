using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Units
{
    public class PlayerUnit : Unit
    {
        public bool CanMove { get; private set; } = true;
        public bool CanAttack { get; private set; } = true;

        public bool TryMoveTo(Vector2 destination)
        {
            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            List<PathNode> path = pathFinder.GetPath(currentGridPosition, destination, Mover.GetRange(), PathFinder.RangeOverflowMode.Cancel);

            if (path == null) return false;

            StartCoroutine(Mover.MoveAlongPath(path));

            CanMove = false;
            return true;
        }

        public bool TryAttack(Vector2 mouseGridPosition)
        {
            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            if (!Attacker.IsTargetInRange(currentGridPosition, mouseGridPosition)) return false;

            if (GridSystem.Instance.NavDict[mouseGridPosition].TryGetOccupyingEntity(out Health targetHealth))
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
