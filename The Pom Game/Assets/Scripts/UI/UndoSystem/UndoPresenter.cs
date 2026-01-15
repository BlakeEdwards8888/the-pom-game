using Pom.UndoSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Pom.UI.UndoSystem
{
    public class UndoPresenter : MonoBehaviour
    {
        [SerializeField] BoardStateCache boardStateCache;
        [SerializeField] Button undoButton;

        private void OnEnable()
        {
            boardStateCache.onStateStackUpdated += HandleStateStackUpdated;
        }

        private void Start()
        {
            HandleStateStackUpdated();
        }

        private void HandleStateStackUpdated()
        {
            undoButton.interactable = boardStateCache.StateStackCount > 0;
        }

        private void OnDisable()
        {
            boardStateCache.onStateStackUpdated -= HandleStateStackUpdated;
        }
    }
}