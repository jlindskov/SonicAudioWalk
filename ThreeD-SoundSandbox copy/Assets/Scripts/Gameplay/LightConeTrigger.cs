using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightConeTrigger : MonoBehaviour
{

    public Action<PlayerMovement> onPlayerTriggerEnter;
    public Action<PlayerMovement> onPlayerTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        var p = other.gameObject.GetComponentInParent<PlayerMovement>();
        if (p)
        {
            onPlayerTriggerEnter.Invoke(p);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var p = other.gameObject.GetComponentInParent<PlayerMovement>();
        if (p)
        {
            onPlayerTriggerExit.Invoke(p);
        }    }
}
