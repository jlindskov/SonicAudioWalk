using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParameterDistanceFade : MonoBehaviour 
{
    // public StudioEventEmitter eventEmitter;
    public string parameterName;

    [Space(10)]
    public float minDistance = 5f;
    public float maxDistance = 40f;

    [Space(10)]
    public float minValue = 0f;
    public float maxValue = 1f;

    [Space(10)]
    public Transform fromTarget;
    public Transform toTarget;

    // EventInstance instance;

    private void Start()
    {
        // instance = eventEmitter.EventInstance;

    }

    void Update () 
    {

        // if (instance.isValid())
        // {
        //     float currentDistance = Vector3.Distance(fromTarget.position, toTarget.position);
        //
        //     if (currentDistance < maxDistance)
        //     {
        //         float currentFraction = Mathf.InverseLerp(minDistance, maxDistance, currentDistance);
        //
        //         float nextValue = Mathf.Lerp(minValue, maxValue, currentFraction);
        //
        //         instance.setParameterByName(parameterName, nextValue);
        //     }
        // }
	}
}
