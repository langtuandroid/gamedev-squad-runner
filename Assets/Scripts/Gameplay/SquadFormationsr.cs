using System.Linq;
using Characters;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class SquadFormationsr : MonoBehaviour
    {
        [Header(" Components ")]
        [SerializeField]
        private TextMeshPro _squadAmountTextsr;
        
        [Header(" Formation Settings ")]
        [Range(0f, 1f)]
        [SerializeField] 
        private float _radiusFactorsr = 0.324f;
        [Range(0f, 1f)]
        [SerializeField]
        private float _angleFactorsr = 1f;
    
        private Runnersr _runnersrPrefabsr;

        private void Update()
        {
            FermatSpiralPlacementsr();
            _squadAmountTextsr.text = transform.childCount.ToString();
        }

        public void SetCharacter(GameObject runner)
        {
            _runnersrPrefabsr = runner.GetComponent<Runnersr>();
        }

        private void FermatSpiralPlacementsr()
        {
            float goldenAngle = 137.5f * _angleFactorsr;  

            for (int i = 0; i < transform.childCount; i++)
            {
                float x = _radiusFactorsr * Mathf.Sqrt(i+1) * Mathf.Cos(Mathf.Deg2Rad * goldenAngle * (i+1));
                float z = _radiusFactorsr * Mathf.Sqrt(i+1) * Mathf.Sin(Mathf.Deg2Rad * goldenAngle * (i+1));

                Vector3 runnerLocalPosition = new Vector3(x, 0, z);
                transform.GetChild(i).localPosition = Vector3.Lerp(transform.GetChild(i).localPosition, runnerLocalPosition, 0.1f);
            }
        }

        public float GetSquadRadiussr()
        {
            return _radiusFactorsr * Mathf.Sqrt(transform.childCount);
        }

        public void AddRunnerssr(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Runnersr runnersrInstance = Instantiate(_runnersrPrefabsr, transform);
                runnersrInstance.StartRunningsr();
                runnersrInstance.name = "Runner_" + runnersrInstance.transform.GetSiblingIndex();
            }
        }
    
        private string ReverseStringsr(string str)
        {
            return new string(str.Reverse().ToArray());
        }
    }
}
