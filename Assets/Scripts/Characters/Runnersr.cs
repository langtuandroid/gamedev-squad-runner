using JetSystems;
using UnityEngine;

namespace Characters
{
    public class Runnersr : MonoBehaviour
    {
        [SerializeField]
        private Animator _animatorsr;
        [SerializeField]
        private Collider _collidersr;
        [SerializeField]
        private Renderer _renderersr;
        [SerializeField]
        private ParticleSystem _explodeParticlessr;
        [SerializeField]
        private LayerMask _obstaclesLayersr;

        private bool _isTargetedsr;
        
        private void Update()
        {
            if (!_collidersr.enabled) return;
            
            DetectObstaclessr();
        }

        private void DetectObstaclessr()
        {
            if (Physics.OverlapSphere(transform.position, 0.1f, _obstaclesLayersr).Length > 0)
                Explodesr();
        }

        public void StartRunningsr()
        {
            _animatorsr.SetInteger("State", 1);
        }

        public void StopRunningsr()
        {
            _animatorsr.SetInteger("State", 0);
        
        }

        public void SetAsTargetsr()
        {
            _isTargetedsr = true;
        }

        public bool IsTargetedsr()
        {
            return _isTargetedsr;
        }

        public void Explodesr()
        {
            _collidersr.enabled = false;
            _renderersr.enabled = false;
            if (transform.parent != null && transform.parent.childCount <= 1)
            {
                UIManager.setGameoverDelegate?.Invoke();
            }
            transform.parent = null;
            _explodeParticlessr.Play();
            Destroy(gameObject, 3);
        }
        
        private int SumOfDigitssr(int number)
        {
            int sum = 0;
            while (number != 0)
            {
                sum += number % 10;
                number /= 10;
            }
            return sum;
        }
    }
}
