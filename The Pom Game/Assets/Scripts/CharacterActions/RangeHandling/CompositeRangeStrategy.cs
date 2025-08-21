using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.RangeHandling
{
    [CreateAssetMenu(fileName = "New Composite Range Strategy", menuName = "Range Strategies/Composite Range Strategy")]
    public class CompositeRangeStrategy : RangeStrategy
    {
        [SerializeField] List<RangeStrategy> rangeStrategies = new List<RangeStrategy>();

        public override List<PathNode> GetNodesInRange(Vector2 startingGridPosition, Func<PathNode, bool> condition)
        {
            List<PathNode> result = new List<PathNode>();

            foreach (RangeStrategy rangeStrategy in rangeStrategies)
            {
                foreach (PathNode node in rangeStrategy.GetNodesInRange(startingGridPosition, condition))
                {
                    result.Add(node);
                }
            }

            return result;
        }

        public override bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition, Func<PathNode, bool> condition)
        {
            foreach (RangeStrategy rangeStrategy in rangeStrategies)
            {
                if (rangeStrategy.IsTargetInRange(currentPosition, targetPosition, condition)) return true;
            }

            return false;
        }
    }
}
