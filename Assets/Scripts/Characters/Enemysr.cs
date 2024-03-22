using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
    public class Enemysr : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _runnersLayersr;
        [SerializeField]
        private float _detectionDistancesr;
        [SerializeField]
        private float _moveSpeedsr;
        [SerializeField]
        private Animator _animatorsr;
        
        [SerializeField]
        private List<GameObject> _enemyModels;
        
        private Runnersr _targetRunnersr;
        
        private void Update()
        {
            if (_targetRunnersr == null)
            {
                FindTargetRunnersr();
            }
            else
            {
                AttackRunnersr();
            }
        }

        private void Start()
        {
            InitEnemy();
        }

        private void InitEnemy()
        {
            int randome = Random.Range(0, _enemyModels.Count);
            _enemyModels[randome].SetActive(true);
        }

        private void FindTargetRunnersr()
        {
            Collider[] detectedRunners = Physics.OverlapSphere(transform.position, _detectionDistancesr, _runnersLayersr);

            if (detectedRunners.Length <= 0) return;

            for (int i = 0; i < detectedRunners.Length; i++)
            {
                Runnersr currentRunnersr = detectedRunners[i].GetComponent<Runnersr>();
                if (currentRunnersr.IsTargetedsr()) continue;

                currentRunnersr.SetAsTargetsr();
                _targetRunnersr = currentRunnersr;
                StartMovingsr();
                break;
            }
        }

        private void AttackRunnersr()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetRunnersr.transform.position, _moveSpeedsr * Time.deltaTime);
            transform.forward = (_targetRunnersr.transform.position - transform.position).normalized;

            if(Vector3.Distance(transform.position, _targetRunnersr.transform.position) < 1f)
            {
                _targetRunnersr.Explodesr();
                Explodesr();
            }
        }

        private void StartMovingsr()
        {
            _animatorsr.SetInteger("State", 1);
            transform.parent = null;
        }
        
        private void Explodesr()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionDistancesr);
        }
        
        private List<T> RemoveDuplicatessr<T>(List<T> list)
        {
            return list.Distinct().ToList();
        }
    }
}
