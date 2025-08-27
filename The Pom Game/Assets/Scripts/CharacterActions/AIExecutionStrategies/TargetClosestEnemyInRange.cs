using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.Units;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies
{
    [CreateAssetMenu(fileName = "TargetClosestEnemyInRange", menuName = "AI Execution Strategies/TargetClosestEnemyInRange")]
    public class TargetClosestEnemyInRange : ExecutionStrategy
    {
        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            targetPosition = Vector2.zero;

            Unit closestEnemy = FindClosestEnemy(currentUnit);

            if (closestEnemy == null) return false;
            if(!rangeStrategy.IsTargetInRange(currentUnit.Position, closestEnemy.Position)) return false;

            targetPosition = closestEnemy.Position;
            return true;
        }

        private Unit FindClosestEnemy(Unit currentUnit)
        {
            Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);

            Unit closestUnit = null;
            float closestDistance = Mathf.Infinity;

            foreach (Unit sampleUnit in allUnits)
            {
                if (sampleUnit == currentUnit) continue;
                if (sampleUnit.Alliance.AlliedFaction == currentUnit.Alliance.AlliedFaction) continue;

                float distance = GridSystem.GetDistance(currentUnit.Position, sampleUnit.Position);

                if (distance < closestDistance)
                {
                    closestUnit = sampleUnit;
                    closestDistance = distance;
                }
            }

            return closestUnit;
        }
    }
}
