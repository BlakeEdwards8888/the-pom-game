using UnityEngine;

namespace Pom.UndoSystem
{
    public interface ICacheable
    {
        public object CaptureState();
        public void RestoreState(object state);
    }
}
