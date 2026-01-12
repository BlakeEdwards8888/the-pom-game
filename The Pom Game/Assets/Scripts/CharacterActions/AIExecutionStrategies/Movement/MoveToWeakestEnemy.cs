using Pom.Alliances;
using Pom.Attributes;
using Pom.CharacterActions.Combat;
using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies.Movement
{
    [CreateAssetMenu(fileName = "New Move To Weakest Enemy In Action Range Strategy", menuName = "AI Execution Strategies/Move To Weakest Enemy In Action Range Strategy")]
    public class MoveToWeakestEnemyInActionRange : ExecutionStrategy
    {
        [SerializeField] string otherActionName;

        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            targetPosition = Vector2.zero;

            Health targetHealth = FindWeakestEnemyHealth(currentUnit.Position, currentUnit.Alliance.AlliedFaction);

            if (targetHealth == null) return false;

            Vector2 weakestEnemyPosition = targetHealth.transform.position;

            ActionExecutor otherAction = currentUnit.GetAction(otherActionName);

            if (otherAction == null)
            {
                Debug.LogError($"{currentUnit.DisplayName} has no action called {otherActionName}");
                return false;
            }

            PathNode targetMovementNode = GetClosestNode(currentUnit, otherAction.GetNodesInRange(weakestEnemyPosition));

            if (targetMovementNode == null)
            {
                Debug.Log($"{currentUnit.name}: No possible walkable node");
                return false;
            }

            targetPosition = targetMovementNode.Position;

            return true;
        }

        Health FindWeakestEnemyHealth(Vector2 currentPosition, Faction alliedFaction)
        {
            List<Health> weakestEnemyHealths = new List<Health>();

            foreach (Health targetHealth in FindObjectsByType<Health>(FindObjectsSortMode.None))
            {
                if (!targetHealth.TryGetComponent(out Alliance targetAlliance)) continue;
                if (targetAlliance.AlliedFaction == alliedFaction) continue;

                if (weakestEnemyHealths.Count == 0 || targetHealth.CurrentHealth < weakestEnemyHealths[0].CurrentHealth)
                {
                    weakestEnemyHealths.Clear();
                    weakestEnemyHealths.Add(targetHealth);
                }else if(targetHealth.CurrentHealth == weakestEnemyHealths[0].CurrentHealth)
                {
                    weakestEnemyHealths.Add(targetHealth);
                }
            }

            return GetClosestWeakHealth(currentPosition, weakestEnemyHealths);
        }

        private Health GetClosestWeakHealth(Vector2 currentPosition, List<Health> weakestEnemyHealths)
        {
            float closestDistance = Mathf.Infinity;
            Health closestWeakHealth = null;

            foreach(Health targetHealth in weakestEnemyHealths)
            {
                float distance = GridSystem.GetDistance(currentPosition, targetHealth.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestWeakHealth = targetHealth;
                }
            }

            return closestWeakHealth;
        }
    }
}
