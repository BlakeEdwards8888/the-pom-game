using Unity.VisualScripting;
using UnityEngine;

namespace Pom.Navigation
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance { get
            {
                if(_instance == null)
                {
                    _instance = FindFirstObjectByType<GridSystem>();
                }

                return _instance;
            }
            private set { } }

        static GridSystem _instance;

        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }

        [field: SerializeField] public float CellSize { get; private set; }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public Vector2 GetGridPosition(Vector2 worldPosition)
        {
            return new Vector2(Mathf.RoundToInt(worldPosition.x) / CellSize,
                Mathf.RoundToInt(worldPosition.y) / CellSize);
        }
    }
}
