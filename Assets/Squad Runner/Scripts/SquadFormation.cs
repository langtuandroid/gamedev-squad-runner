using System.Collections.Generic;
using JetSystems;
using UnityEngine;
using TMPro;

public class SquadFormation : MonoBehaviour
{
    [Header(" Components ")]
    [SerializeField] private TextMeshPro squadAmountText;

    [Header(" Formation Settings ")]
    [Range(0f, 1f)]
    [SerializeField] private float radiusFactor;
    [Range(0f, 1f)]
    [SerializeField] private float angleFactor;
    [SerializeField]
    private Runner _runnerPrefab;

    private void Update()
    {
        FermatSpiralPlacement();
        squadAmountText.text = transform.childCount.ToString();
    }

   
    public void SetCharacter(GameObject runner)
    {
        _runnerPrefab = runner.GetComponent<Runner>();
    }

    private void FermatSpiralPlacement()
    {
        float goldenAngle = 137.5f * angleFactor;  

        for (int i = 0; i < transform.childCount; i++)
        {
            float x = radiusFactor * Mathf.Sqrt(i+1) * Mathf.Cos(Mathf.Deg2Rad * goldenAngle * (i+1));
            float z = radiusFactor * Mathf.Sqrt(i+1) * Mathf.Sin(Mathf.Deg2Rad * goldenAngle * (i+1));

            Vector3 runnerLocalPosition = new Vector3(x, 0, z);
            transform.GetChild(i).localPosition = Vector3.Lerp(transform.GetChild(i).localPosition, runnerLocalPosition, 0.1f);
        }
    }

    public float GetSquadRadius()
    {
        return radiusFactor * Mathf.Sqrt(transform.childCount);
    }

    public void AddRunners(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Runner runnerInstance = Instantiate(_runnerPrefab, transform);
            runnerInstance.StartRunning();
            runnerInstance.name = "Runner_" + runnerInstance.transform.GetSiblingIndex();
        }
    }
}
