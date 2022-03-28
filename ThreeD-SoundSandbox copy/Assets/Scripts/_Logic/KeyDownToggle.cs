using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDownToggle : MonoBehaviour 
{
    public bool currentlyActive;
    public GameObject target;
    public KeyCode key;
	
	
	void Update () 
    {
        if (Input.GetKeyDown(key))
        {
            currentlyActive = !currentlyActive;
        }

        target.SetActive(currentlyActive);		
	}
}
