using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDownTrigger : MonoBehaviour
{
    public Collider probe;

    [Space(10)]    
    public KeyCode key;

    [Space(10)]        
    public Transform target;
    public Transform teleport;

    private bool currentlyInside;

    void Update()
    {
        if (currentlyInside)
        {
            if (Input.GetKeyDown(key))
            {
                if (target && teleport)
                {
                    target.position = teleport.position;        
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (probe == null || collider == probe)
        {
            currentlyInside = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (probe == null || collider == probe)
        {
            currentlyInside = false;
        }
    }
}
