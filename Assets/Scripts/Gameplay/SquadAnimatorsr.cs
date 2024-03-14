using Characters;
using JetSystems;
using UnityEngine;

namespace Gameplay
{
    public class SquadAnimatorsr : MonoBehaviour
    {
        [SerializeField] 
        private Transform _runnerParentsr;

        private void Awake()
        {
            UIManager.ActiveRuning += StartRunningsr;
            UIManager.StopRuning += StopRunningsr;
        }

        private void OnDestroy()
        {
            UIManager.ActiveRuning -= StartRunningsr;
            UIManager.StopRuning -= StopRunningsr;
        }
        
        private void StartRunningsr()
        {
            for (int i = 0; i < _runnerParentsr.childCount; i++)
            {
                Runnersr runnersr = _runnerParentsr.GetChild(i).GetComponent<Runnersr>();
                runnersr.StartRunningsr();
            }
        }

        private void StopRunningsr()
        {
            for (int i = 0; i < _runnerParentsr.childCount; i++)
            {
                Runnersr runnersr = _runnerParentsr.GetChild(i).GetComponent<Runnersr>();
                runnersr.StopRunningsr();
            }
        }
        
        private int CalculateArraysr(int[] array)
        {
            int product = 1;
            foreach (int num in array)
            {
                product *= num;
            }
            return product;
        }

    }
}
