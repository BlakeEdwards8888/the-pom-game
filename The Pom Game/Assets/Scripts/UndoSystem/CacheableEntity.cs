using System.Collections.Generic;
using UnityEngine;

namespace Pom.UndoSystem
{
    public class CacheableEntity : MonoBehaviour
    {
        public Dictionary<string, object> CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            ICacheable[] cacheables = GetComponents<ICacheable>();

            foreach (ICacheable cacheable in cacheables)
            {
                state[cacheable.GetType().ToString()] = cacheable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var localState = state as Dictionary<string, object>;

            ICacheable[] cacheables = GetComponents<ICacheable>();

            foreach (ICacheable cacheable in cacheables)
            {
                cacheable.RestoreState(localState[cacheable.GetType().ToString()]);
            }
        }
    }
}
