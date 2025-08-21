using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Units
{
    public class EnemyUnit : Unit
    {
        Health target;

        Vector2 currentGridPosition;
        Vector2 targetGridPosition;

        public IEnumerator TakeTurn()
        {
            currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            SetTarget();

            yield return HandleMovementAction();

            HandleAttackAction();
        }

        private IEnumerator HandleMovementAction()
        {
            List<PathNode> possibleEndingNodes = Attacker.GetNodesInRange(targetGridPosition);

            PathNode targetEndingNode = GetClosestNode(currentGridPosition, possibleEndingNodes);

            if (targetEndingNode == null)
            {
                Debug.Log($"{gameObject.name}: No possible walkable node");
                yield break;
            }

            List<PathNode> path = pathFinder.GetPath(currentGridPosition, targetEndingNode.Position, Mover.GetRange(), PathFinder.RangeOverflowMode.Truncate);

            yield return Mover.MoveAlongPath(path);
        }

        void HandleAttackAction()
        {
            currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            if (!Attacker.IsTargetInRange(currentGridPosition, targetGridPosition)) return;

            Attacker.Attack(target);
        }

        private void SetTarget()
        {
            float closestTargetDistance = Mathf.Infinity;


            foreach(Alliance alliance in FindObjectsByType<Alliance>(FindObjectsSortMode.None))
            {
                if (alliance.AlliedFaction == Alliance.AlliedFaction) continue;

                Vector2 allianceGridPosition = GridSystem.Instance.GetGridPosition(alliance.transform.position);
                float distance = GridSystem.GetDistance(currentGridPosition, allianceGridPosition);

                if (distance < closestTargetDistance)
                {
                    target = alliance.GetComponent<Health>();
                    closestTargetDistance = distance;
                }
            }

            targetGridPosition = GridSystem.Instance.GetGridPosition(target.transform.position);
        }

        private PathNode GetClosestNode(Vector2 startingGridPosition, List<PathNode> possibleEndingNodes)
        {
            PathNode closestNode = null;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < possibleEndingNodes.Count; i++)
            {
                if (!possibleEndingNodes[i].IsWalkable()) continue;

                float distanceToNode = GridSystem.GetDistance(startingGridPosition, possibleEndingNodes[i].Position);
                if (distanceToNode < closestDistance)
                {
                    closestNode = possibleEndingNodes[i];
                    closestDistance = distanceToNode;
                }
            }

            return closestNode;
        }
    }
}
