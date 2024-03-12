using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int amount;
    [SerializeField] private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        while (parent.childCount > 0)
        {
            Transform t = parent.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }
       
        for (int i = 0; i < amount; i++)
            Instantiate(objectToSpawn, parent);        
    }

    private void Update()
    {
        if (parent.childCount <= 0)
            Destroy(gameObject);
    }
}
