using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class TriggerParameterFade : MonoBehaviour 
{
    public StudioEventEmitter eventEmitter;
    public string parameterName;

    public float targetValue = 0f;
    public float speed = 0.2f;

    public Collider probe;

    private bool currentlyFading;

    void OnTriggerEnter (Collider collider) 
    {
        if (probe == null || collider == probe)
        {
            EventInstance instance = eventEmitter.EventInstance;
            
            currentlyFading = true;
        }
	}

    void Update()
    {
        if (currentlyFading)
        {
            float currentValue;
            eventEmitter.EventInstance.getParameterByName(parameterName, out currentValue);

            float nextValue = Mathf.MoveTowards(currentValue, targetValue, speed * Time.deltaTime);
            eventEmitter.EventInstance.setParameterByName(parameterName, nextValue);

            if (nextValue == currentValue)
                currentlyFading = false;
        }
    }
}
