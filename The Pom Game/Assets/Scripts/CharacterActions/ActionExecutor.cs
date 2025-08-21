using NUnit.Framework;
using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions
{
    public abstract class ActionExecutor : MonoBehaviour
    {
        [SerializeField] RangeStrategy rangeStrategy;

        public bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition, Func<PathNode, bool> condition = null)
        {
            return rangeStrategy.IsTargetInRange(currentPosition, targetPosition, condition != null ? condition : (node) => { return true; });
        }

        public List<PathNode> GetNodesInRange(Vector2 startingGridPosition, Func<PathNode, bool> condition = null)
        {
            return rangeStrategy.GetNodesInRange(startingGridPosition, condition != null ? condition : (node) => { return true; });
        }

        public int GetRange()
        {
            return rangeStrategy.Range;
        }
    }
}
