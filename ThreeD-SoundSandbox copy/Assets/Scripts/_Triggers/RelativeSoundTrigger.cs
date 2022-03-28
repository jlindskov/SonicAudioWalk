using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using FMODUnity;

public class RelativeSoundTrigger : MonoBehaviour 
{
    // [EventRef]
    // public string eventName;

    public Transform target;
    public Vector3 relativeOffset;

    void OnTriggerEnter(Collider collider)
    {
        Vector3 position;

        if (target)
        {
            position = target.TransformPoint(relativeOffset);
        }
        else
        {
            position = transform.TransformPoint(relativeOffset);
        }

        // Utils.PlayAudioEvent(eventName, position);            
	}
}
