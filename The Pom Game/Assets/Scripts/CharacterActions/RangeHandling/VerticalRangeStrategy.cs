using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.RangeHandling
{
    [CreateAssetMenu(fileName = "New Vertical Range Strategy", menuName = "Range Strategies/Vertical Range Strategy")]
    public class VerticalRangeStrategy : RangeStrategy
    {
        public override List<PathNode> GetNodesInRange(Vector2 startingGridPosition, Func<PathNode, bool> condition)
        {
            List<PathNode> result = new List<PathNode>();

            for (int i = DeadZone + 1; i <= Range + DeadZone; i++)
            {
                float step = i * GridSystem.Instance.CellSize;

                if (GridSystem.Instance.TryGetGridPosition(new Vector2(startingGridPosition.x, startingGridPosition.y - step), out Vector2 bottomPosition))
                {
                    PathNode bottomNode = GridSystem.Instance.NavDict[bottomPosition];

                    if (condition != null && condition(bottomNode))
                        result.Add(bottomNode);
                }

                if (GridSystem.Instance.TryGetGridPosition(new Vector2(startingGridPosition.x, startingGridPosition.y + step), out Vector2 topPosition))
                {
                    PathNode topNode = GridSystem.Instance.NavDict[topPosition];

                    if (condition != null && condition(topNode))
                        result.Add(topNode);
                }
            }

            return result;
        }

        public override bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition, Func<PathNode, bool> condition)
        {
            if (condition != null && !condition(GridSystem.Instance.NavDict[targetPosition])) return false;
            float verticalDistance = Mathf.Abs(currentPosition.y - targetPosition.y);
            return currentPosition.x == targetPosition.x && verticalDistance > DeadZone && verticalDistance <= Range + DeadZone;
        }
    }
}
