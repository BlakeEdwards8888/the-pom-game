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

        [SerializeField] int width, height;
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

        //Vector2[,] cellPositions;

        //private void Start()
        //{
        //    InitiallizeGrid();
        //}

        //void InitiallizeGrid()
        //{
        //    cellPositions = new Vector2[width, height];

        //    for(int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            cellPositions[x,y] = new Vector2 (x * cellSize, y * cellSize);
        //        }
        //    }
        //}


        public Vector2 GetGridPosition(Vector2 worldPosition)
        {
            return new Vector2(Mathf.RoundToInt(worldPosition.x) / CellSize,
                Mathf.RoundToInt(worldPosition.y) / CellSize);
        }
    }
}
