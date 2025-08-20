using NUnit.Framework;
using Pom.Alliances;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class EnemyController : CharacterController
    {
        Transform target;

        public void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {

                TakeTurn();
            }

            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                rangePresenter.ShowSelectableNodes(mover.GetNodesInRange(currentGridPosition));
            }
        }

        private void TakeTurn()
        {
            rangePresenter.ClearSelectableNodes();

            //Movement Action
            SetTarget();

            Vector2 targetGridPosition = GridSystem.Instance.GetGridPosition(target.position);
            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            List<PathNode> possibleEndingNodes = attacker.GetNodesInRange(targetGridPosition);

            PathNode targetEndingNode = GetClosestNode(currentGridPosition, possibleEndingNodes);

            if(targetEndingNode == null)
            {
                Debug.Log("No possible walkable node");
                return;
            }

            List<PathNode> path = pathFinder.GetPath(currentGridPosition, targetEndingNode.Position, mover.Range, PathFinder.RangeOverflowMode.Truncate);

            StartCoroutine(mover.MoveAlongPath(path));

            //Attack Action
        }

        private void SetTarget()
        {
            float closestTargetDistance = Mathf.Infinity;
            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            foreach(Alliance alliance in FindObjectsByType<Alliance>(FindObjectsSortMode.None))
            {
                if (alliance.AlliedFaction == this.alliance.AlliedFaction) continue;

                Vector2 allianceGridPosition = GridSystem.Instance.GetGridPosition(alliance.transform.position);
                float distance = GridSystem.GetDistance(currentGridPosition, allianceGridPosition);

                if (distance < closestTargetDistance)
                {
                    target = alliance.transform;
                    closestTargetDistance = distance;
                }
            }
        }

        private PathNode GetClosestNode(Vector2 startingGridPosition, List<PathNode> possibleEndingNodes)
        {
            PathNode closestNode = null;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < possibleEndingNodes.Count; i++)
            {
                if (!possibleEndingNodes[i].IsWalkable()) continue;

                float distanceToNode = GridSystem.GetDistance(startingGridPosition, possibleEndingNodes[i].Position);
                if (distanceToNode < closestDistance)
                {
                    closestNode = possibleEndingNodes[i];
                    closestDistance = distanceToNode;
                }
            }

            return closestNode;
        }

    }
}
