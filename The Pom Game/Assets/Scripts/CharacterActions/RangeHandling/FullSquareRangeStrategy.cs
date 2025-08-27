using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace Pom.CharacterActions.RangeHandling
{
    [CreateAssetMenu(fileName = "New Full Square Range Strategy", menuName = "Range Strategies/Full Square Range Strategy")]
    public class FullSquareRangeStrategy : RangeStrategy
    {
        public override List<PathNode> GetNodesInRange(Vector2 startingGridPosition, Func<PathNode, bool> condition = null)
        {
            List<PathNode> result = new List<PathNode>();
            float cellSize = GridSystem.Instance.CellSize;

            float length = (Range + DeadZone) * GridSystem.Instance.CellSize;

            Vector2 origin = new Vector2(startingGridPosition.x - length, startingGridPosition.y - length);
            Vector2 endingGridPosition = new Vector2(startingGridPosition.x + length, startingGridPosition.y + length);

            float xLength = Mathf.Abs(endingGridPosition.x - origin.x)/cellSize;
            float yLength = Mathf.Abs(endingGridPosition.y - origin.y)/cellSize;

            for (int i = 0; i <= xLength; i++)
            {
                for (int j = 0; j <= yLength; j++)
                {
                    Vector2 samplePosition = origin + new Vector2(i * GridSystem.Instance.CellSize, j * GridSystem.Instance.CellSize);

                    if (samplePosition == startingGridPosition) continue;
                    
                    Vector2 distance = samplePosition - startingGridPosition;
                    float xDistance = Mathf.Abs(distance.x) / cellSize;
                    float yDistance = Mathf.Abs(distance.y) / cellSize;

                    if (xDistance <= DeadZone && yDistance <= DeadZone) continue;
                    
                    if(GridSystem.Instance.TryGetGridPosition(samplePosition, out Vector2 gridPosition))
                    {
                        result.Add(GridSystem.Instance.NavDict[gridPosition]);
                    }
                }
            }

            return result;
        }

        public override bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition, Func<PathNode, bool> condition = null)
        {
            float length = (Range + DeadZone) * GridSystem.Instance.CellSize;
            float deadZoneLength = DeadZone * GridSystem.Instance.CellSize;

            Vector2 origin = new Vector2(currentPosition.x - length, currentPosition.y - length);
            Vector2 endingGridPosition = new Vector2(currentPosition.x + length, currentPosition.y + length);

            Vector2 deadzoneOrigin = new Vector2(currentPosition.x - deadZoneLength, currentPosition.y - deadZoneLength);
            Vector2 deadzoneEndPosition = new Vector2(currentPosition.x + deadZoneLength, currentPosition.y + deadZoneLength);

            return condition(GridSystem.Instance.NavDict[targetPosition])
                && IsTargetWithinBoxDimensions(targetPosition, origin, endingGridPosition)
                && !IsTargetWithinBoxDimensions(targetPosition, deadzoneOrigin, deadzoneEndPosition);
        }

        private static bool IsTargetWithinBoxDimensions(Vector2 targetPosition, Vector2 origin, Vector2 endingGridPosition)
        {
            return targetPosition.x >= origin.x && targetPosition.x <= endingGridPosition.x
                && targetPosition.y >= origin.y && targetPosition.y <= endingGridPosition.y;
        }
    }
}
