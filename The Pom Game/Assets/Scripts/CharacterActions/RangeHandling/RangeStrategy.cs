using NUnit.Framework;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.RangeHandling
{
    public abstract class RangeStrategy : ScriptableObject
    {
        enum Filter
        {
            Unwalkable,
            Semipermeable
        }

        [field: SerializeField] public int Range { get; private set; }
        [field: SerializeField] public int DeadZone { get; private set; }

        [SerializeField] List<Filter> exclusions = new List<Filter>();

        public abstract bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition);

        public abstract List<PathNode> GetNodesInRange(Vector2 startingGridPosition);

        protected bool CheckAgainstFilter(PathNode node)
        {
            if (exclusions.Contains(Filter.Unwalkable) && !node.IsWalkable()) return false;
            if (exclusions.Contains(Filter.Semipermeable) && node.IsSemipermeable()) return false;

            return true;
        }
    }
}
