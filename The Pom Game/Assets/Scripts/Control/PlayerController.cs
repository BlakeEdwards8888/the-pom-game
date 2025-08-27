using Pom.CharacterActions;
using Pom.Navigation;
using Pom.Navigation.Presentation;
using Pom.Objectives;
using Pom.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class PlayerController : Controller
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
        Unit activeUnit;
        ActionExecutor activeAction;
        bool hasCurrentTurn;

        public event Action<Unit> onUnitSelected;
        public event Action onActiveUnitCleared;


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

            HandleAction();
        }

        private bool HandleUnitSelection()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (GridSystem.Instance.NavDict[GetMouseGridPosition()].TryGetOccupyingEntity(out Unit unit))
                {
                    if (unit.Alliance.AlliedFaction != alliance.AlliedFaction) return false;
                    SetActiveUnit(unit);
                    return true;
                }
            }

            return false;
        }

        private void HandleAction()
        {
            if (activeAction == null) return;
            if (activeUnit == null) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!hasCurrentTurn) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mouseGridPosition = GetMouseGridPosition();
                ActionExecutor usingAction = activeAction;
                SetActiveAction(null);
                usingAction.TryExecute(mouseGridPosition, actionArgs);
            }
        }

        public void SetActiveAction(ActionExecutor action)
        {
            if (action == activeAction) return;

            activeAction = action;

            if(activeAction == null || activeAction.IsUsed)
            {
                rangePresenter.ClearSelectableNodes();
            }
            else
            {
                rangePresenter.ShowSelectableNodes(activeAction.GetNodesInRange(activeUnit.Position));
            }
        }

        public void ClearActiveUnit()
        {
            activeUnit = null;
            SetActiveAction(null);
            onActiveUnitCleared?.Invoke();
        }

        private void HandleUnitDeath()
        {
            FindControllableUnits();

            if(controllableUnits.Count == 0)
            {
                ObjectivesList.Instance.TriggerFailureState();
            }
        }

        private Vector2 GetMouseGridPosition()
        {
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            GridSystem.Instance.TryGetGridPosition(worldMousePosition, out Vector2 mouseGridPosition);
            return mouseGridPosition;
        }

        public override void InitiateTurn()
        {
            base.InitiateTurn();

            hasCurrentTurn = true;
        }

        public override void ExitTurn()
        {
            base.ExitTurn();

            SetActiveAction(null);
            hasCurrentTurn = false;
        }

        protected override void FindControllableUnits()
        {
            List<Unit> controllableUnitsCache = new List<Unit>(controllableUnits);

            base.FindControllableUnits();

            foreach (Unit unit in controllableUnits)
            {
                if (controllableUnitsCache.Contains(unit)) continue;
                unit.Health.onDeath.AddListener(HandleUnitDeath);
            }
        }

        public override void SetActiveUnit(Unit unit)
        {
            if (!controllableUnits.Contains(unit))
            {
                controllableUnits.Add(unit);
                unit.Health.onDeath.AddListener(HandleUnitDeath);
            }

            activeUnit = unit;
            SetActiveAction(unit.Actions[0]);
            onUnitSelected?.Invoke(activeUnit);
        }
    }
}
