using System;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _IconAmount;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int amount;
    [SerializeField] private Transform parent;
    [SerializeField] private List<GameObject> _gameObjectsEnemy;
    
    private void Start()
    {
        _gameObjectsEnemy.Clear();
        for (int i = 0; i < amount; i++)
        {
          var Enemy =   Instantiate(objectToSpawn, parent);  
          _gameObjectsEnemy.Add(Enemy);
        }
    }

    private void OnDestroy()
    {
        foreach (var enemy in _gameObjectsEnemy)
        {
            Destroy(enemy);
        }
        _gameObjectsEnemy.Clear();
    }

    private void Update()
    {
        if (parent.childCount <= 0)
            Destroy(_IconAmount);
    }
}
