using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterTrigger : MonoBehaviour
{
    public Helicopter helicopter;
    public bool helicopterState;


    private void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponentInParent<PlayerMovement>();
        if (p != null)
        {
            helicopter.gameObject.SetActive(true);
            helicopter.SetHelicopterActive(helicopterState);
        }
    }
}
