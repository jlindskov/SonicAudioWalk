using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTarget : MonoBehaviour 
{
    public Vector3 amount;
    public Transform target; 

	void Update () 
    {
        if (target)
        {
            target.Rotate(amount * Time.deltaTime);
        }
        else
        {
            transform.Rotate(amount * Time.deltaTime);
        }
	}
}
