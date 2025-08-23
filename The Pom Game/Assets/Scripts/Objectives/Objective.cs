using System;
using UnityEngine;

namespace Pom.Objectives
{
    [CreateAssetMenu(fileName = "Objective", menuName = "Objective")]
    public class Objective : ScriptableObject
    {
        [field: SerializeField] public string Description { get; private set; }
    }
}
