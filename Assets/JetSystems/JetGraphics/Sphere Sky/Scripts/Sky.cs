using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [Header(" Settings ")]
    public Transform target;
    
    private void Update()
    {
        transform.position = target.position;
    }
}
