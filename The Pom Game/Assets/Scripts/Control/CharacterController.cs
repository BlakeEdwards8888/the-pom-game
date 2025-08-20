using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation.Presentation;
using Pom.Navigation;
using UnityEngine;
using Pom.CharacterActions.Movement;
using Pom.CharacterActions.Combat;

namespace Pom.Control
{
    public abstract class CharacterController : MonoBehaviour
    {
        protected Mover mover => GetComponent<Mover>();
        protected Attacker attacker => GetComponent<Attacker>();
        protected PathFinder pathFinder => GetComponent<PathFinder>();
        protected RangePresenter rangePresenter => GetComponent<RangePresenter>();
        protected Alliance alliance => GetComponent<Alliance>();
    }
}
