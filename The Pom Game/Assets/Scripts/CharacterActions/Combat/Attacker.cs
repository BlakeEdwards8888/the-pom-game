using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.Combat
{
    public class Attacker : ActionExecutor
    {
        [SerializeField] int damage;

        public override string GetDisplayName()
        {
            return "Attack";
        }

        public void Attack(Health target)
        {
            target.TakeDamage(damage, gameObject);
        }

        public override bool TryExecute(Vector2 gridPosition, List<ActionExecutionArg> executionArgs, Action finished)
        {
            if (IsUsed) return false;

            if (!IsTargetInRange(transform.position, gridPosition)) return false;

            if (GridSystem.Instance.NavDict[gridPosition].TryGetOccupyingEntity(out Health targetHealth))
            {
                if (targetHealth.TryGetComponent(out Alliance targetAlliance) && targetAlliance.AlliedFaction == GetComponent<Alliance>().AlliedFaction) return false;
                Execute(targetHealth, finished);
                return true;
            }

            return false;
        }

        protected override void Execute(object args, Action finished)
        {
            base.Execute(args, finished);

            Health targetHealth = args as Health;

            Attack(targetHealth);
            finished?.Invoke();
        }
    }
}
