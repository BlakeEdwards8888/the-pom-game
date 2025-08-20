using Pom.Alliances;
using Pom.Attributes;
using Pom.Navigation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class PlayerController : CharacterController
    {
        private void Update()
        {
            HandleMovement();
            HandleAttack();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                rangePresenter.ShowSelectableNodes(mover.GetNodesInRange(currentGridPosition));
            }

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                rangePresenter.ShowSelectableNodes(attacker.GetNodesInRange(currentGridPosition));
            }
        }

        private void HandleMovement()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mouseGridPosition = GetMouseGridPosition();
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                List<PathNode> path = pathFinder.GetPath(currentGridPosition, mouseGridPosition, mover.Range, PathFinder.RangeOverflowMode.Cancel);
                rangePresenter.ClearSelectableNodes();

                if (path == null) return;

                StartCoroutine(mover.MoveAlongPath(path));
            }
        }

        void HandleAttack()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Vector2 mouseGridPosition = GetMouseGridPosition();
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                if (!attacker.IsTargetInRange(currentGridPosition, mouseGridPosition)) return;

                if (GridSystem.Instance.NavDict[mouseGridPosition].TryGetOccupyingEntity(out Health targetHealth))
                {
                    if (targetHealth.TryGetComponent(out Alliance targetAlliance) && targetAlliance.AlliedFaction == alliance.AlliedFaction) return;
                    attacker.Attack(targetHealth);
                }
            }
        }

        private Vector2 GetMouseGridPosition()
        {
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return GridSystem.Instance.GetGridPosition(worldMousePosition);
        }
    }
}
