
using Pom.Alliances;
using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies.Movement
{
    [CreateAssetMenu(fileName = "New Flee From Enemy Strategy", menuName = "AI Execution Strategies/Flee From Enemy Strategy")]
    public class FleeFromEnemy : ExecutionStrategy
    {
        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            targetPosition = Vector2.zero;

            List<PathNode> nodesInRange = rangeStrategy.GetNodesInRange(currentUnit.Position);

            if (nodesInRange.Count == 0) return false;

            PathNode farthestNode = CalculateFarthestNode(nodesInRange, currentUnit);
            
            targetPosition = farthestNode.Position;

            if(targetPosition == currentUnit.Position) return false;

            return true;
        }

        private PathNode CalculateFarthestNode(List<PathNode> nodesInRange, Unit currentUnit)
        {
            nodesInRange.Add(GridSystem.Instance.NavDict[currentUnit.Position]);

            float farthestDistance = Mathf.NegativeInfinity;
            PathNode farthestNode = null;
            List<Unit> enemyUnits = FindEnemyUnits(currentUnit.Alliance.AlliedFaction);

            foreach (PathNode node in nodesInRange)
            {
                float distance = 0;

                foreach (Unit enemyUnit in enemyUnits)
                {
                    distance += GridSystem.GetDistance(node.Position, enemyUnit.Position);
                }

                if(distance > farthestDistance)
                {
                    farthestDistance = distance;
                    farthestNode = node;
                }
            }

            return farthestNode;
        }

        private List<Unit> FindEnemyUnits(Faction alliedFaction)
        {
            Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);
            List<Unit> enemyUnits = new List<Unit>();

            foreach (Unit unit in allUnits)
            {
                if (unit.Alliance.AlliedFaction == alliedFaction) continue;
                enemyUnits.Add(unit);
            }

            return enemyUnits;
        }
    }
}
