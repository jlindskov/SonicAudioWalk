using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseTrigger : MonoBehaviour 
{
    public Vector3 impulseForce;

    void OnTriggerEnter(Collider collider)
    {
        Rigidbody attachedRigidbody = collider.attachedRigidbody;

        if (attachedRigidbody)
        {
            attachedRigidbody.AddForce(impulseForce, ForceMode.Impulse);   
        }
    }
}
