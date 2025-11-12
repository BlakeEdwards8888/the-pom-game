using Pom.Alliances;
using Pom.Attributes;
using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies
{
    public abstract class ExecutionStrategy : ScriptableObject
    {
        public abstract bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy);

        protected Health FindClosestEnemyHealth(Vector2 currentPosition, Faction alliedFaction)
        {
            Health closestEnemyHealth = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (Health targetHealth in FindObjectsByType<Health>(FindObjectsSortMode.None))
            {
                if (!targetHealth.TryGetComponent(out Alliance targetAlliance)) continue;
                if (targetAlliance.AlliedFaction == alliedFaction) continue;

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

        protected PathNode GetClosestNode(Unit testUnit, List<PathNode> possibleEndingNodes)
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
