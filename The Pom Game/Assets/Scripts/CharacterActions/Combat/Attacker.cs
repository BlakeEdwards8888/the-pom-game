using Pom.Attributes;
using Pom.Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.Combat
{
    public class Attacker : ActionExecutor
    {
        [SerializeField] int damage;

        public bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition)
        {
            return GridSystem.GetDistance(currentPosition, targetPosition) <= Range;
        }

        public void Attack(Health target)
        {
            target.TakeDamage(damage);
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
