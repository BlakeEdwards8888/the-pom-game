using Pom.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Navigation
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] bool ignoreSemipermeable = false;

        public enum RangeOverflowMode
        {
            Cancel,
            Truncate
        }

        public List<PathNode> GetPath(Vector2 startingPosition, Vector2 endingPosition, int range, RangeOverflowMode rangeOverflowMode, List<PathNode> blacklist = null)
        {
            if(blacklist == null) blacklist = new List<PathNode>();
            Dictionary<Vector2, PathNode> navDict = GridSystem.Instance.NavDict;

            if (!navDict[endingPosition].IsWalkable()) return null;
            if (!ignoreSemipermeable && navDict[endingPosition].IsSemipermeable()) return null;

            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>(blacklist);

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
                    List<PathNode> prospectivePath = GenerateFinalPath(navDict[endingPosition], range, rangeOverflowMode);

                    if (prospectivePath == null) return null;

                    //If the final path ends on a space that contains a unit as a result of
                    //the range overflow mode, then we have to recalculate the path and ignore that node
                    //to prevent units from overlapping, but still alowing them to pass through
                    //each other on their way to their final destination
                    if (!ignoreSemipermeable && prospectivePath[prospectivePath.Count - 1].IsSemipermeable())
                    {
                        blacklist.Add(prospectivePath[prospectivePath.Count - 1]);
                        return GetPath(startingPosition, endingPosition, range, rangeOverflowMode, blacklist);
                    }
                    else
                    {
                        return prospectivePath;
                    }
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
                switch (rangeOverflowMode) 
                {
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
