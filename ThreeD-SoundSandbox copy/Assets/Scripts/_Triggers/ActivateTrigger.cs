using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour 
{
    public GameObject target;

    void OnTriggerEnter(Collider collider)
    {
        target.SetActive(true);
    }
}