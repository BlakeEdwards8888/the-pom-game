using System;
using UnityEngine;

namespace Pom.CaptureSystem
{
    public class CapturableEntity : MonoBehaviour
    {
        public event Action<GameObject> onCaptured;

        public void RaiseCaptureEvent(GameObject capturer)
        {
            onCaptured?.Invoke(capturer);
        }
    }
}
