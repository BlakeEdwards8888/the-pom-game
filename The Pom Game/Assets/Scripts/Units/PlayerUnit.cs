using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Units
{
    public class PlayerUnit : Unit
    {
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                //rangePresenter.ShowSelectableNodes(mover.GetNodesInRange(currentGridPosition, (node) => { return node.IsWalkable(); }));
            }

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                //rangePresenter.ShowSelectableNodes(attacker.GetNodesInRange(currentGridPosition,
                //    (node) =>
                //    {
                //        if (!node.IsWalkable()) return false;

                //        if (Physics2D.Linecast(node.Position, currentGridPosition, GridSystem.Instance.ObstacleLayerMask)) return false;

                //        return true;
                //    }));
            }
        }

        public void MoveTo(Vector2 destination)
        {
            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            List<PathNode> path = pathFinder.GetPath(currentGridPosition, destination, Mover.GetRange(), PathFinder.RangeOverflowMode.Cancel);
            //rangePresenter.ClearSelectableNodes();

            if (path == null) return;

            StartCoroutine(Mover.MoveAlongPath(path));
        }

        public void Attack(Vector2 mouseGridPosition)
        {
            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            if (!Attacker.IsTargetInRange(currentGridPosition, mouseGridPosition)) return;

            if (GridSystem.Instance.NavDict[mouseGridPosition].TryGetOccupyingEntity(out Health targetHealth))
            {
                if (targetHealth.TryGetComponent(out Alliance targetAlliance) && targetAlliance.AlliedFaction == alliance.AlliedFaction) return;
                Attacker.Attack(targetHealth);
            }
        }
    }
}
