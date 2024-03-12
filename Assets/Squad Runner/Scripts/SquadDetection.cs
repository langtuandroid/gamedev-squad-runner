using System;
using UnityEngine;
using JetSystems;

public class SquadDetection : MonoBehaviour
{
    [Header(" Managers ")]
    [SerializeField] private SquadFormation squadFormation;
    private Runner _runner;

    [Header(" Settings ")]
    [SerializeField] private LayerMask doorLayer;
    [SerializeField] private LayerMask finishLayer;
    [SerializeField] private LayerMask obstacleLayer;
    
    private void Update()
    {
        if (UIManager.IsGame())
        {
            DetectDoors();
            DetectFinishLine();
            DetectionObstacles();
        }
    }

    public void SetCharacter(GameObject runner)
    {
        _runner = runner.GetComponent<Runner>();
    }

    private void DetectDoors()
    {
        Collider[] detectedDoors = Physics.OverlapSphere(transform.position, squadFormation.GetSquadRadius(), doorLayer);

        if (detectedDoors.Length <= 0) return;

        Collider collidedDoorCollider = detectedDoors[0];
        Door collidedDoor = collidedDoorCollider.GetComponentInParent<Door>();

        int runnersAmountToAdd = collidedDoor.GetRunnersAmountToAdd(collidedDoorCollider, squadFormation.transform.childCount);
        squadFormation.AddRunners(runnersAmountToAdd);
        
    }

    private void DetectFinishLine()
    {
        if (Physics.OverlapSphere(transform.position, 1, finishLayer).Length > 0)
        {
            FindObjectOfType<FinishLine>().PlayConfettiParticles();
            SetLevelComplete();
        }
    }

    private void DetectionObstacles()
    {
        if (Physics.OverlapSphere(transform.position, 0.5f, obstacleLayer).Length > 0)
        {
            if (_runner != null)
            {
                Debug.Log("Explode");
                _runner.Explode();
            }
        }
    } 
     
    private void SetLevelComplete()
    {
        UIManager.setLevelCompleteDelegate?.Invoke(3);
        
    }
}
