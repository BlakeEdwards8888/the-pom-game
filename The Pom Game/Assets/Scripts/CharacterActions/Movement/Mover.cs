using NUnit.Framework;
using Pom.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.Movement
{
    public class Mover : ActionExecutor
    {
        const float MINIMUM_STOPPING_DISTANCE = 0.1f;

        [SerializeField] float moveSpeed;
        public IEnumerator MoveAlongPath(List<PathNode> path)
        {
            foreach (PathNode pathNode in path)
            {
                yield return MoveToNewDestination(pathNode.Position);
            }
        }

        public IEnumerator MoveToNewDestination(Vector2 destination)
        {
            while (Vector2.Distance(transform.position, destination) > MINIMUM_STOPPING_DISTANCE)
            {
                Vector2 direction = (destination - (Vector2)transform.position).normalized;
                Vector2 movement = direction * moveSpeed * Time.deltaTime;

                transform.Translate(movement);

                yield return null;
            }

            transform.position = destination;
        }

        public override List<PathNode> GetNodesInRange(Vector2 startingGridPosition)
        {
            List<PathNode> nodesInRange = new List<PathNode>();
            Dictionary<Vector2, PathNode> navDict = GridSystem.Instance.NavDict;

            nodesInRange.Add(navDict[startingGridPosition]);
            List<PathNode> neighborNodes = GridSystem.Instance.GetNeighborNodes(navDict[startingGridPosition]);

            for (int i = 0; i < Range; i++)
            {
                List<PathNode> neighborNodeCache = new List<PathNode>(neighborNodes);

                foreach (PathNode node in neighborNodeCache)
                {
                    if (nodesInRange.Contains(node)) continue;
                    if (!node.IsWalkable()) continue;

                    neighborNodes.Remove(node);
                    nodesInRange.Add(node);

                    foreach (PathNode neighborNode in GridSystem.Instance.GetNeighborNodes(node))
                    {
                        neighborNodes.Add(neighborNode);
                    }
                }
            }

            nodesInRange.Remove(navDict[startingGridPosition]);
            return nodesInRange;
        }

    }
}
