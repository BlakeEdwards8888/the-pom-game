using Pom.Attributes;
using UnityEngine;

namespace Pom.CharacterActions.Combat
{
    public class Attacker : ActionExecutor
    {
        [SerializeField] int damage;

        public void Attack(Health target)
        {
            target.TakeDamage(damage);
        }
    }
}
