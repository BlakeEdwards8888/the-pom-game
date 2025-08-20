using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Navigation.Presentation
{
    public class RangePresenter : MonoBehaviour
    {
        List<PathNode> currentlySelectableNodes = new List<PathNode>();

        public void ShowSelectableNodes(List<PathNode> nodesInRange)
        {
            ClearSelectableNodes();

            foreach (PathNode node in nodesInRange)
            {
                currentlySelectableNodes.Add(node);
                node.ToggleSelectableIndicator(true);
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
