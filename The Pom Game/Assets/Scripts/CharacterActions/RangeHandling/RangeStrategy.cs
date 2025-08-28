using NUnit.Framework;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.RangeHandling
{
    public abstract class RangeStrategy : ScriptableObject
    {
        [field: SerializeField] public int Range { get; private set; }
        [field: SerializeField] public int DeadZone { get; private set; }

        public abstract bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition, Func<PathNode, bool> condition = null);

        public abstract List<PathNode> GetNodesInRange(Vector2 startingGridPosition, Func<PathNode, bool> condition = null);
    }
}
