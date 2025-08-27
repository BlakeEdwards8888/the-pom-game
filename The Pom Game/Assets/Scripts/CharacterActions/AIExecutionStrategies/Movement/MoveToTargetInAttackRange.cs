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
        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            Health targetHealth = FindClosestEnemyHealth(currentUnit.Position, currentUnit.Alliance.AlliedFaction);

            Vector2 closestEnemyPosition = GridSystem.Instance.GetGridPosition(targetHealth.transform.position);

            PathNode targetMovementNode = GetClosestNode(currentUnit, currentUnit.GetComponent<Attacker>().GetNodesInRange(closestEnemyPosition));

            if (targetMovementNode == null)
            {
                Debug.Log($"{currentUnit.name}: No possible walkable node");
                targetPosition = Vector2.zero;
                return false;
            }

            targetPosition = targetMovementNode.Position;
            return true;
        }

        private Health FindClosestEnemyHealth(Vector2 currentPosition, Faction alliedFaction)
        {
            Health closestEnemyHealth = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (Health targetHealth in FindObjectsByType<Health>(FindObjectsSortMode.None))
            {
                if (targetHealth.GetComponent<Alliance>().AlliedFaction == alliedFaction) continue;

                Vector2 healthGridPosition = GridSystem.Instance.GetGridPosition(targetHealth.transform.position);
                float distance = GridSystem.GetDistance(currentPosition, healthGridPosition);

                if (distance < closestTargetDistance)
                {
                    closestEnemyHealth = targetHealth;
                    closestTargetDistance = distance;
                }
            }

            return closestEnemyHealth;
        }

        private PathNode GetClosestNode(Unit testUnit, List<PathNode> possibleEndingNodes)
        {
            PathNode closestNode = null;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < possibleEndingNodes.Count; i++)
            {
                if (!possibleEndingNodes[i].IsWalkable()) continue;
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
