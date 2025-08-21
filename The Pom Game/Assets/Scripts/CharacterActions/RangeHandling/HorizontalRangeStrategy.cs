using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.RangeHandling
{
    [CreateAssetMenu(fileName = "New Horizontal Range Strategy", menuName = "Range Strategies/Horizontal Range Strategy")]
    public class HorizontalRangeStrategy : RangeStrategy
    {
        public override List<PathNode> GetNodesInRange(Vector2 startingGridPosition, Func<PathNode, bool> condition)
        {
            List<PathNode> result = new List<PathNode>();

            for(int i = DeadZone + 1; i <= Range + DeadZone; i++)
            {
                float step = i * GridSystem.Instance.CellSize;

                if (GridSystem.Instance.TryGetGridPosition(new Vector2(startingGridPosition.x - step, startingGridPosition.y), out Vector2 leftPosition))
                {
                    PathNode leftNode = GridSystem.Instance.NavDict[leftPosition];

                    if (condition != null && condition(leftNode))
                        result.Add(leftNode);
                }

                if (GridSystem.Instance.TryGetGridPosition(new Vector2(startingGridPosition.x + step, startingGridPosition.y), out Vector2 rightPosition))
                {
                    PathNode rightNode = GridSystem.Instance.NavDict[rightPosition];

                    if (condition != null && condition(rightNode))
                        result.Add(rightNode);
                }
            }

            return result;
        }

        public override bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition, Func<PathNode, bool> condition)
        {
            if (condition != null && !condition(GridSystem.Instance.NavDict[targetPosition])) return false;
            float horizontalDistance = Mathf.Abs(currentPosition.x - targetPosition.x);
            return currentPosition.y == targetPosition.y && horizontalDistance > DeadZone && horizontalDistance <= Range + DeadZone;
        }
    }
}
