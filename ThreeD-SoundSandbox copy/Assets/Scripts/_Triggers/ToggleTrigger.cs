using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTrigger : MonoBehaviour 
{
    public GameObject targetOn;
    public GameObject targetOff;
    public Collider probe;

    void OnTriggerEnter(Collider collider)
    {
        if (probe == null || collider == probe)
        {
            if (targetOn)
            {
                targetOn.SetActive(true);
            }

            if (targetOff)
            {    
                targetOff.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (probe == null || collider == probe)
        {
            if (targetOn)
            {
                targetOn.SetActive(false);
            }

            if (targetOff)
            {
                targetOff.SetActive(true);
            }
        }
    }
}