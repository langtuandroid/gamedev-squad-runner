using Characters;
using JetSystems;
using UnityEngine;

namespace Gameplay
{
    public class SquadDetectionsr : MonoBehaviour
    {
        [Header(" Managers ")]
        [SerializeField]
        private SquadFormationsr _squadFormationsr;
        
        [Header(" Settings ")]
        [SerializeField]
        private LayerMask _doorLayersr;
        [SerializeField]
        private LayerMask _finishLayersr;
        [SerializeField]
        private LayerMask _obstacleLayersr;
    
        private Runnersr _runnersr;
    
        private void Update()
        {
            if (UIManager.IsGame())
            {
                DetectDoorssr();
                DetectFinishLinesr();
                DetectionObstaclessr();
            }
        }

        public void SetCharactersr(GameObject runner)
        {
            _runnersr = runner.GetComponent<Runnersr>();
        }

        private void DetectDoorssr()
        {
            Collider[] detectedDoors = Physics.OverlapSphere(transform.position, _squadFormationsr.GetSquadRadiussr(), _doorLayersr);

            if (detectedDoors.Length <= 0) return;

            Collider collidedDoorCollider = detectedDoors[0];
            Doorsr collidedDoorsr = collidedDoorCollider.GetComponentInParent<Doorsr>();

            int runnersAmountToAdd = collidedDoorsr.GetRunnersAmountToAddsr(collidedDoorCollider, _squadFormationsr.transform.childCount);
            _squadFormationsr.AddRunnerssr(runnersAmountToAdd);
        
        }

        private void DetectFinishLinesr()
        {
            if (Physics.OverlapSphere(transform.position, 1, _finishLayersr).Length > 0)
            {
                FindObjectOfType<FinishLinesr>().PlayConfettiParticlessr();
                SetLevelCompletesr();
            }
        }

        private void DetectionObstaclessr()
        {
            if (Physics.OverlapSphere(transform.position, 0.5f, _obstacleLayersr).Length > 0)
            {
                if (_runnersr != null)
                {
                    _runnersr.Explodesr();
                }
            }
        } 
     
        private void SetLevelCompletesr()
        {
            UIManager.setLevelCompleteDelegate?.Invoke(3);
        
        }
        
        private bool IsPositivesr(int number)
        {
            return number > 0;
        }
    }
}
