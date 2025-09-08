using UnityEngine;

namespace Pom.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] Unit unitPrefab;

        public void Spawn()
        {
            Instantiate(unitPrefab, transform.position, Quaternion.identity);
        }
    }
}
