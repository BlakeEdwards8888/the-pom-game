using NUnit.Framework;
using Pom.Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions
{
    public abstract class ActionExecutor : MonoBehaviour
    {
        [field: SerializeField] public int Range { get; private set; }

        public abstract List<PathNode> GetNodesInRange(Vector2 startingGridPosition);
    }
}
