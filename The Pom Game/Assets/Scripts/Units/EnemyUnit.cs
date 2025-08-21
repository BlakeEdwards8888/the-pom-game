using NUnit.Framework;
using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Units
{
    public class EnemyUnit : Unit
    {
        Health target;

        Vector2 currentGridPosition;
        Vector2 targetGridPosition;

        public void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                TakeTurn();
            }

            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                //rangePresenter.ShowSelectableNodes(mover.GetNodesInRange(currentGridPosition));
            }
        }

        private void TakeTurn()
        {
            //rangePresenter.ClearSelectableNodes();
            currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            SetTarget();

            HandleMovementAction();
        }

        private void HandleMovementAction()
        {
            List<PathNode> possibleEndingNodes = Attacker.GetNodesInRange(targetGridPosition);

            PathNode targetEndingNode = GetClosestNode(currentGridPosition, possibleEndingNodes);

            if (targetEndingNode == null)
            {
                Debug.Log("No possible walkable node");
                return;
            }

            List<PathNode> path = pathFinder.GetPath(currentGridPosition, targetEndingNode.Position, Mover.GetRange(), PathFinder.RangeOverflowMode.Truncate);

            StartCoroutine(Mover.MoveAlongPath(path, HandleAttackAction));
        }

        private void SetTarget()
        {
            float closestTargetDistance = Mathf.Infinity;


            foreach(Alliance alliance in FindObjectsByType<Alliance>(FindObjectsSortMode.None))
            {
                if (alliance.AlliedFaction == this.alliance.AlliedFaction) continue;

                Vector2 allianceGridPosition = GridSystem.Instance.GetGridPosition(alliance.transform.position);
                float distance = GridSystem.GetDistance(currentGridPosition, allianceGridPosition);

                if (distance < closestTargetDistance)
                {
                    target = alliance.GetComponent<Health>();
                    closestTargetDistance = distance;
                }
            }

            targetGridPosition = GridSystem.Instance.GetGridPosition(target.transform.position);
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

        void HandleAttackAction()
        {
            currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            if (!Attacker.IsTargetInRange(currentGridPosition, targetGridPosition)) return;

            Attacker.Attack(target);
        }

    }
}
