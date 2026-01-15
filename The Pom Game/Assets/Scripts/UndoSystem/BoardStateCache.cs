using Pom.Control;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.UndoSystem
{
    public class BoardStateCache : MonoBehaviour
    {
        public int StateStackCount => stateStack.Count;

        Stack<Dictionary<CacheableEntity, object>> stateStack = new Stack<Dictionary<CacheableEntity, object>> ();

        public event Action onStateRestored;
        public event Action onStateStackUpdated;

        public void CaptureState()
        {
            Dictionary<CacheableEntity, object> state = new Dictionary<CacheableEntity, object>();

            CacheableEntity[] cachableEntities = FindObjectsByType<CacheableEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var entity in cachableEntities)
            {
                state[entity] = entity.CaptureState();
            }

            stateStack.Push(state);

            onStateStackUpdated?.Invoke();
        }

        public void RestoreState()
        {
            if (stateStack.Count == 0) return; 
            
            Dictionary<CacheableEntity, object> state = stateStack.Pop();

            foreach(KeyValuePair<CacheableEntity, object> kvp in state)
            {
                kvp.Key.RestoreState(kvp.Value);
            }

            onStateRestored?.Invoke();
            onStateStackUpdated?.Invoke();
        }

        public void ClearStateStack()
        {
            stateStack.Clear();
            onStateStackUpdated?.Invoke();
        }
    }
}
