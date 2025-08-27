using Pom.CharacterActions.RangeHandling;
using Pom.Units;
using System;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies
{
    public abstract class ExecutionStrategy : ScriptableObject
    {
        public abstract bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy);
    }
}
