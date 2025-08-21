using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pom.Navigation
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindFirstObjectByType<GridSystem>();
                }

                return _instance;
            }
            private set { } 
        }

        static GridSystem _instance;

        public Dictionary<Vector2, PathNode> NavDict
        {
            get
            {
                if (_navDict == null) BuildNavDict();

                return _navDict;
            }
            private set { }
        }

        Dictionary<Vector2, PathNode> _navDict;

        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
        [field: SerializeField] public float CellSize { get; private set; }
        [field: SerializeField] public LayerMask ObstacleLayerMask { get; private set; }




        [SerializeField] GameObject nodeSelectableIndicatorPrefab;


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

        private void Start()
        {
            if(_navDict == null) BuildNavDict();
        }

        void BuildNavDict()
        {
            _navDict = new Dictionary<Vector2, PathNode>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2 nodePosition = new Vector2(x * CellSize, y * CellSize);
                    GameObject nodeSelectableIndicator = Instantiate(nodeSelectableIndicatorPrefab, nodePosition, Quaternion.identity);
                    nodeSelectableIndicator.SetActive(false);
                    _navDict[nodePosition] = new PathNode(nodePosition, ObstacleLayerMask, nodeSelectableIndicator);
                }
            }
        }

        public Vector2 GetGridPosition(Vector2 worldPosition)
        {
            Vector2 gridPosition = new Vector2(Mathf.RoundToInt(worldPosition.x) / CellSize,
                Mathf.RoundToInt(worldPosition.y) / CellSize);

            gridPosition.x = Mathf.Clamp(gridPosition.x, 0, Width - 1);
            gridPosition.y = Mathf.Clamp(gridPosition.y, 0, Height - 1);

            return gridPosition;
        }

        public List<PathNode> GetNeighborNodes(PathNode currentNode)
        {
            List<PathNode> resultList = new List<PathNode>
            {
                NavDict[GetGridPosition(new Vector2(currentNode.Position.x + 1, currentNode.Position.y))],
                NavDict[GetGridPosition(new Vector2(currentNode.Position.x - 1, currentNode.Position.y))],
                NavDict[GetGridPosition(new Vector2(currentNode.Position.x, currentNode.Position.y + 1))],
                NavDict[GetGridPosition(new Vector2(currentNode.Position.x, currentNode.Position.y - 1))]
            };

            return resultList;
        }

        public static float GetDistance(Vector2 pointA, Vector2 pointB)
        {
            Vector2 gridDistance = pointA - pointB;

            float xDistance = Mathf.Abs(gridDistance.x);
            float yDistance = Mathf.Abs(gridDistance.y);

            return xDistance + yDistance;
        }
    }
}
