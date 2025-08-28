using Pom.Alliances;
using Pom.Attributes;
using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.Units;
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
    }
}
