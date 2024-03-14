using JetSystems;
using UnityEngine;

namespace Gameplay
{
    public class CircularSawsr : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField]
        private Transform renderer;
        [SerializeField]
        private float rotationSpeed;

        [Header("Movement")]
        [SerializeField]
        private Vector2 minMaxX;
        [SerializeField]
        private float patrolDuration;
        
        private Vector3 targetPosition;
        
        private void Start()
        {
            transform.position = transform.position.With(x: minMaxX.x);
            targetPosition = transform.position.With(x: minMaxX.y);
            MoveToTargetPosition();
        }
        
        private void Update()
        {
            Rotate();
        }

        private void MoveToTargetPosition()
        {
            LeanTween.move(gameObject, targetPosition, patrolDuration).setOnComplete(SetNextTargetPosition);
        }

        private void SetNextTargetPosition()
        {
            if (targetPosition.x == minMaxX.x)
                targetPosition.x = minMaxX.y;
            else
                targetPosition.x = minMaxX.x;

            MoveToTargetPosition();
        }

        private void Rotate()
        {
            renderer.RotateAround(Vector3.forward, Time.deltaTime * rotationSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Vector3 position = transform.position;
            position.x = minMaxX.x;
            Vector3 posmax = position;
            posmax.x = minMaxX.y;
            float cubeSize = 0.5f;
            Gizmos.DrawCube(position, cubeSize * Vector3.one);
            Gizmos.DrawCube(posmax, cubeSize * Vector3.one);
        }
    }
}
