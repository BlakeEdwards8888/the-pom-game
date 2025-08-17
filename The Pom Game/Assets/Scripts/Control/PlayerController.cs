using Pom.Movement;
using Pom.Navigation;
using Pom.Navigation.Presentation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class PlayerController : MonoBehaviour
    {

        Mover mover => GetComponent<Mover>();
        PathFinder pathFinder => GetComponent<PathFinder>();
        RangePresenter rangePresenter => GetComponent<RangePresenter>();

        private void Update()
        {
            HandleMovement();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                rangePresenter.ShowSelectableNodes(transform.position, mover.MaximumDistance);
            }
        }

        private void HandleMovement()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 mouseGridPosition = GridSystem.Instance.GetGridPosition(worldMousePosition);
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                List<PathNode> path = pathFinder.GetPath(currentGridPosition, mouseGridPosition, mover.MaximumDistance);
                rangePresenter.ClearSelectableNodes();

                if (path == null) return;

                StartCoroutine(mover.MoveAlongPath(path));
            }
        }
    }
}
