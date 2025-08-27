using System;
using UnityEngine;

namespace Pom.CharacterActions
{
    [Serializable]
    public struct ActionExecutionArg
    {
        public string predicate;
        public string value;
    }
}
