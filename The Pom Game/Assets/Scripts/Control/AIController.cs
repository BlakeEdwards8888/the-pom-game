using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using Pom.TurnSystem;
using Pom.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Control
{
    public class AIController : Controller
    {
        const float DELAY_BETWEEN_ACTIONS = 0.5f;

        IEnumerator TurnCoroutine()
        {
            foreach (Unit unit in controllableUnits)
            {
                Health targetHealth = GetClosestEnemyHealth(unit);

                yield return ExecuteMovementAction(unit, targetHealth);

                HandleAttackAction(unit, targetHealth);

                yield return new WaitForSeconds(DELAY_BETWEEN_ACTIONS);
            }

            TurnShifter.Instance.ShiftTurns();
        }

        IEnumerator ExecuteMovementAction(Unit unit, Health targetHealth)
        {
            Vector2 targetPosition = GridSystem.Instance.GetGridPosition(targetHealth.transform.position);

            PathNode targetMovementNode = GetClosestNode(unit, unit.Attacker.GetNodesInRange(targetPosition));

            if (targetMovementNode == null)
            {
                Debug.Log($"{unit.name}: No possible walkable node");
                yield break;
            }

            if (unit.CanMoveTo(targetMovementNode.Position, out List<PathNode> path, PathFinder.RangeOverflowMode.Truncate))
            {
                yield return unit.MoveAlongPath(path);
            }
        }

        void HandleAttackAction(Unit unit, Health targetHealth)
        {
            Vector2 targetPosition = GridSystem.Instance.GetGridPosition(targetHealth.transform.position);

            if (!unit.Attacker.IsTargetInRange(unit.Position, targetPosition)) return;

            unit.Attacker.Attack(targetHealth);
        }

        Health GetClosestEnemyHealth(Unit unit)
        {
            Health closestEnemyHealth = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (Health targetHealth in FindObjectsByType<Health>(FindObjectsSortMode.None))
            {
                if (targetHealth.GetComponent<Alliance>().AlliedFaction == alliance.AlliedFaction) continue;

                Vector2 healthGridPosition = GridSystem.Instance.GetGridPosition(targetHealth.transform.position);
                float distance = GridSystem.GetDistance(unit.Position, healthGridPosition);

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

        public override void InitiateTurn()
        {
            base.InitiateTurn();

            StartCoroutine(TurnCoroutine());
        }
    }
}
