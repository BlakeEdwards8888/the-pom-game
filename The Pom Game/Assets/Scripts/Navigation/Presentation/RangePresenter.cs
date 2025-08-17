using System.Collections.Generic;
using UnityEngine;

namespace Pom.Navigation.Presentation
{
    public class RangePresenter : MonoBehaviour
    {
        List<PathNode> currentlySelectableNodes = new List<PathNode>();

        public void ShowSelectableNodes(Vector2 startingPosition, int maximumDistance)
        {
            Dictionary<Vector2, PathNode> navDict = GridSystem.Instance.NavDict;

            currentlySelectableNodes.Add(navDict[startingPosition]);
            List<PathNode> neighborNodes = GridSystem.Instance.GetNeighborNodes(navDict[startingPosition]);

            for (int i = 0; i < maximumDistance; i++)
            {
                List<PathNode> neighborNodeCache = new List<PathNode>(neighborNodes);

                foreach (PathNode node in neighborNodeCache)
                {
                    if (currentlySelectableNodes.Contains(node)) continue;
                    if (!node.IsWalkable()) continue;

                    node.ToggleSelectableIndicator(true);
                    neighborNodes.Remove(node);
                    currentlySelectableNodes.Add(node);

                    foreach (PathNode neighborNode in GridSystem.Instance.GetNeighborNodes(node))
                    {
                        neighborNodes.Add(neighborNode);
                    }
                }
            }
        }

        public void ClearSelectableNodes()
        {
            foreach (PathNode node in currentlySelectableNodes)
            {
                node.ToggleSelectableIndicator(false);
            }

            currentlySelectableNodes.Clear();
        }

    }
}
