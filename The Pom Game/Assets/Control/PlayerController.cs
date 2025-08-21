using Pom.Navigation;
using Pom.Navigation.Presentation;
using Pom.Units;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Pom.Control
{
    public class PlayerController : MonoBehaviour
    {
        public enum PlayerState
        {
            Idle,
            Move,
            Attack
        }

        public static PlayerController Instance 
        { 
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<PlayerController>();

                return _instance;
            }
            private set { } 
        }

        static PlayerController _instance;

        RangePresenter rangePresenter => GetComponent<RangePresenter>();
        PlayerUnit activeUnit;
        PlayerState currentState = PlayerState.Idle;

        public event Action<Unit> onUnitSelected;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (HandleUnitSelection()) return;

            switch (currentState) 
            {
                case PlayerState.Move:
                    HandleMovement();
                    break;
                case PlayerState.Attack:
                    HandleAttack();
                    break;
            }
        }

        private bool HandleUnitSelection()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (GridSystem.Instance.NavDict[GetMouseGridPosition()].TryGetOccupyingEntity(out PlayerUnit unit))
                {
                    SwitchState(PlayerState.Idle);
                    activeUnit = unit;
                    onUnitSelected?.Invoke(unit);
                    return true;
                }
            }

            return false;
        }

        private void HandleMovement()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                activeUnit.MoveTo(GetMouseGridPosition());
            }
        }

        void HandleAttack()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                activeUnit.Attack(GetMouseGridPosition());
            }
        }

        public void ClearActiveUnit()
        {
            activeUnit = null;
        }

        public void SwitchState(PlayerState newState)
        {
            HandleExitState();
            HandleEnterState(newState);

            currentState = newState;
        }

        void HandleEnterState(PlayerState newState)
        {
            Vector2 activeUnitGridPosition = Vector2.zero;

            if(activeUnit != null) activeUnitGridPosition = GridSystem.Instance.GetGridPosition(activeUnit.transform.position);

            switch (newState)
            {
                case PlayerState.Idle:
                    ClearActiveUnit();
                    break;
                case PlayerState.Move:
                    rangePresenter.ShowSelectableNodes(activeUnit.Mover.GetNodesInRange(activeUnitGridPosition, (node) => { return node.IsWalkable(); }));
                    break;
                case PlayerState.Attack:
                    rangePresenter.ShowSelectableNodes(activeUnit.Attacker.GetNodesInRange(activeUnitGridPosition,
                        (node) =>
                        {
                            if (!node.IsWalkable()) return false;

                            if (Physics2D.Linecast(node.Position, activeUnitGridPosition, GridSystem.Instance.ObstacleLayerMask)) return false;

                            return true;
                        }));
                    break;
            }
        }

        void HandleExitState()
        {
            rangePresenter.ClearSelectableNodes();

            switch (currentState)
            {
                case PlayerState.Idle:
                    break;
                case PlayerState.Move:
                    break;
                case PlayerState.Attack:
                    break;
            }
        }

        private Vector2 GetMouseGridPosition()
        {
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return GridSystem.Instance.GetGridPosition(worldMousePosition);
        }
    }
}
