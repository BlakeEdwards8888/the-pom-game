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
    [CreateAssetMenu(fileName = "New Move To Closest Target In Action Range", menuName = "AI Execution Strategies/Move To Closest Target In Action Range")]
    public class MoveToClosestTargetInActionRange : ExecutionStrategy
    {
        [SerializeField] string otherActionName;

        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            targetPosition = Vector2.zero;

            Health targetHealth = FindClosestEnemyHealth(currentUnit.Position, currentUnit.Alliance.AlliedFaction);

            if (targetHealth == null) return false;

            Vector2 closestEnemyPosition = GridSystem.Instance.GetGridPosition(targetHealth.transform.position);

            ActionExecutor otherAction = currentUnit.GetAction(otherActionName);

            if(otherAction == null)
            {
                Debug.LogError($"{currentUnit.DisplayName} has no action called {otherActionName}");
                return false;
            }

            PathNode targetMovementNode = GetClosestNode(currentUnit, otherAction.GetNodesInRange(closestEnemyPosition));

            if (targetMovementNode == null)
            {
                Debug.Log($"{currentUnit.name}: No possible walkable node");
                return false;
            }

            targetPosition = targetMovementNode.Position;
            return true;
        }
    }
}
