using Pom.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Navigation
{
    public class PathFinder : MonoBehaviour
    {
        public enum RangeOverflowMode
        {
            Cancel,
            Truncate
        }

        public List<PathNode> GetPath(Vector2 startingPosition, Vector2 endingPosition, int range, RangeOverflowMode rangeOverflowMode)
        {
            Dictionary<Vector2, PathNode> navDict = GridSystem.Instance.NavDict;

            if (!navDict[endingPosition].IsWalkable()) return null;
            if (navDict[endingPosition].TryGetOccupyingEntity(out Health health))
            {
                Debug.Log($"Found entity with health: {health.name} position: {endingPosition}");
                return null;
            }

            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            openList.Add(navDict[startingPosition]);

            foreach (KeyValuePair<Vector2, PathNode> entry in navDict)
            {
                entry.Value.Reset();
            }

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestScoringNode(openList, startingPosition, endingPosition);

                if (currentNode.Position == navDict[endingPosition].Position)
                {
                    //end of path reached
                    return GenerateFinalPath(navDict[endingPosition], range, rangeOverflowMode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighborNode in GridSystem.Instance.GetNeighborNodes(currentNode))
                {
                    if (neighborNode == null) continue;
                    if (closedList.Contains(neighborNode)) continue;
                    if (!neighborNode.IsWalkable()) continue;

                    neighborNode.SetPreviousNode(currentNode);

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }

            Debug.LogWarning($"{gameObject.name}: No Path Found from {startingPosition} to {endingPosition}");
            return null;
        }

        public PathNode GetLowestScoringNode(List<PathNode> sampleList, Vector2 startingPosition, Vector2 endingPosition)
    {
            if (sampleList.Count == 1) return sampleList[0];

            PathNode lowestScoringNode = sampleList[0];

            for (int i = 1; i < sampleList.Count; i++)
            {
                if (sampleList[i].GetScore(startingPosition, endingPosition) < lowestScoringNode.GetScore(startingPosition, endingPosition))
                {
                    lowestScoringNode = sampleList[i];
                }
            }

            return lowestScoringNode;
        }

        private List<PathNode> GenerateFinalPath(PathNode endNode, int range, RangeOverflowMode rangeOverflowMode)
        {
            List<PathNode> pathNodeList = new List<PathNode>{endNode};

            PathNode currentNode = endNode;

            while(currentNode.GetPreviousNode() != null)
            {
                pathNodeList.Add(currentNode.GetPreviousNode());
                currentNode = currentNode.GetPreviousNode();
            }

            pathNodeList.Reverse();

            if (pathNodeList.Count > range + 1)
            {
                switch (rangeOverflowMode) {
                    case RangeOverflowMode.Truncate:
                        pathNodeList.RemoveRange(range + 1, pathNodeList.Count - (range + 1));
                        break;
                    case RangeOverflowMode.Cancel:
                        return null;
            }
            }

                return pathNodeList;
        }
    }
}
