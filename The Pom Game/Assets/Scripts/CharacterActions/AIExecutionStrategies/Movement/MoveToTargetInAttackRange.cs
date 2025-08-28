using Pom.Alliances;
using Pom.Attributes;
using Pom.CharacterActions.Combat;
using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies.Movement
{
    [CreateAssetMenu(fileName = "MoveToTargetInAttackRange", menuName = "AI Execution Strategies/MoveToTargetInAttackRange")]
    public class MoveToTargetInAttackRange : ExecutionStrategy
    {
        [SerializeField] string otherActionName;

        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            targetPosition = Vector2.zero;

            Health targetHealth = FindClosestEnemyHealth(currentUnit.Position, currentUnit.Alliance.AlliedFaction);

            Vector2 closestEnemyPosition = GridSystem.Instance.GetGridPosition(targetHealth.transform.position);

            ActionExecutor otherAction = currentUnit.GetAction(otherActionName);

            if(otherAction == null)
            {
                Debug.LogError($"{currentUnit.DisplayName} has no action called {otherActionName}");
                return false;
            }

            PathNode targetMovementNode = GetClosestNode(currentUnit, currentUnit.GetComponent<Attacker>().GetNodesInRange(closestEnemyPosition));

            if (targetMovementNode == null)
            {
                Debug.Log($"{currentUnit.name}: No possible walkable node");
                return false;
            }

            targetPosition = targetMovementNode.Position;
            return true;
        }

        private PathNode GetClosestNode(Unit testUnit, List<PathNode> possibleEndingNodes)
        {
            PathNode closestNode = null;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < possibleEndingNodes.Count; i++)
            {
                if (!possibleEndingNodes[i].IsWalkable()) continue;
                if (possibleEndingNodes[i].IsSemipermeable()) continue;
                if (possibleEndingNodes[i].TryGetOccupyingEntity(out Unit unit) && unit != testUnit) continue;

                float distanceToNode = GridSystem.GetDistance(testUnit.Position, possibleEndingNodes[i].Position);
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
