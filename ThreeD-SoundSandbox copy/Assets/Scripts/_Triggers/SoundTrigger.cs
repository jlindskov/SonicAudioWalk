using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundTrigger : MonoBehaviour 
{
    [EventRef]
    public string eventName;
    public Transform target;
    public Collider probe;

    void OnTriggerEnter(Collider collider)
    {
        if (probe == null || collider == probe)
        {
            Vector3 position;

            if (target)
            {
                position = target.position;


            }
            else
            {
                position = transform.position;

            }

            Utils.PlayAudioEvent(eventName, position);            
        }
	}
}
