using Pom.Attributes;
using Pom.CharacterActions.AIExecutionStrategies;
using Pom.CharacterActions.RangeHandling;
using Pom.Units;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies.Movement
{
    [CreateAssetMenu(fileName = "New Closest Enemy Execution Strategy", menuName = "Execution Strategies/Closest Enemy Strategy")]
    public class MoveToEnemy : ExecutionStrategy
    {
        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            Health closestEnemy = FindClosestEnemyHealth(currentUnit.Position, currentUnit.Alliance.AlliedFaction);

            targetPosition = closestEnemy.transform.position;
            return true;
        }
    }
}
