using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Navigation
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] LayerMask obstacleLayerMask;
        [SerializeField] PathNodeDebugObject pathNodeDebugObjectPrefab;

        Dictionary<Vector2, PathNode> navDict;

        private void Start()
        {
            BuildNavDict();
        }

        void BuildNavDict()
        {
            navDict = new Dictionary<Vector2, PathNode>();

            for (int x = 0; x < GridSystem.Instance.Width; x++)
            {
                for (int y = 0; y < GridSystem.Instance.Height; y++)
                {
                    Vector2 nodePosition = new Vector2(x, y);
                    navDict[nodePosition] = new PathNode(nodePosition, obstacleLayerMask);
                }
            }
        }

        public List<PathNode> GetPath(Vector2 startingPosition, Vector2 endingPosition)
        {
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
                    return GenerateFinalPath(navDict[endingPosition]);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighborNode in GetNeighborNodes(currentNode))
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

        private PathNode GetLowestScoringNode(List<PathNode> sampleList, Vector2 startingPosition, Vector2 endingPosition)
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


        List<PathNode> GetNeighborNodes(PathNode currentNode)
        {
            List<PathNode> resultList = new List<PathNode>
            {
                navDict[new Vector2(currentNode.Position.x + 1, currentNode.Position.y)],
                navDict[new Vector2(currentNode.Position.x - 1, currentNode.Position.y)],
                navDict[new Vector2(currentNode.Position.x, currentNode.Position.y + 1)],
                navDict[new Vector2(currentNode.Position.x, currentNode.Position.y - 1)]
            };

            return resultList;
        }

        private List<PathNode> GenerateFinalPath(PathNode endNode)
        {
            List<PathNode> pathNodeList = new List<PathNode>{endNode};

            PathNode currentNode = endNode;

            while(currentNode.GetPreviousNode() != null)
            {
                pathNodeList.Add(currentNode.GetPreviousNode());
                currentNode = currentNode.GetPreviousNode();
            }

            pathNodeList.Reverse();

            return pathNodeList;
        }
    }
}
