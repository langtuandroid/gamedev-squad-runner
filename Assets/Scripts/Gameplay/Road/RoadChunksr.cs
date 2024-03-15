using UnityEngine;

namespace Gameplay.Road
{
    public class RoadChunksr : MonoBehaviour
    {
        [SerializeField]
        private  float _lengthsr = 20;
        [SerializeField]
        private Color _gizmosColorsr = Color.red;

        public float Lengthsr
        {
            get => _lengthsr;
            set => _lengthsr = value;
        }

        private void OnDrawGizmos()
        {
            Vector3 center = transform.position + (Lengthsr / 2 * Vector3.forward);
            Vector3 size = new Vector3(20, 20, Lengthsr);

            Gizmos.color = _gizmosColorsr;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
