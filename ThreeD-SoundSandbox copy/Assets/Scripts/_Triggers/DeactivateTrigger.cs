using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateTrigger : MonoBehaviour 
{
    public GameObject target;

    void OnTriggerEnter(Collider collider)
    {
        target.SetActive(false);
    }
}