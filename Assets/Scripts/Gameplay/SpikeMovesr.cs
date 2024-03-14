using System;
using System.Linq;
using JetSystems;
using UnityEngine;

namespace Gameplay
{
    public class SpikeMovesr : MonoBehaviour
    {
        [SerializeField] 
        private Vector2 minMaxX;
        [SerializeField]
        private float patrolDuration;
        private Vector3 _targetPositionsr;

        
        private void Start()
        {
            transform.position = transform.position.With(x: minMaxX.x);
            _targetPositionsr = transform.position.With(x: minMaxX.y);
            MoveToTargetPositionsr();
        }
    
        private void MoveToTargetPositionsr()
        {
            LeanTween.move(gameObject, _targetPositionsr, patrolDuration).setOnComplete(SetNextTargetPositionsr);
        }

        private void SetNextTargetPositionsr()
        {
            if (_targetPositionsr.x == minMaxX.x)
                _targetPositionsr.x = minMaxX.y;
            else
                _targetPositionsr.x = minMaxX.x;

            MoveToTargetPositionsr();
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 positionSpike = transform.position;
            positionSpike.x = minMaxX.x;
            Vector3 posCentre = positionSpike;
            posCentre.x = minMaxX.y;
            float cubeSize = 0.5f;
            Gizmos.DrawCube(positionSpike, cubeSize * Vector3.one);
            Gizmos.DrawCube(posCentre, cubeSize * Vector3.one);
        }
        
        private bool IsPalindromsr(string str)
        {
            string reversed = new string(str.Reverse().ToArray());
            return str.Equals(reversed, StringComparison.OrdinalIgnoreCase);
        }
    }
}
