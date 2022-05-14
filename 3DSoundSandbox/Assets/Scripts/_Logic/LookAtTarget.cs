using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour 
{
    public Transform target;	
    public float speed = 3f;
    public bool ignoreVertical;

    private Vector3 currentLookAtPosition;

	void Update () 
    {
        Vector3 targetPosition = target.position;

        if (ignoreVertical == true)
        {
            targetPosition.y = transform.position.y;    
        }

        currentLookAtPosition = Vector3.Lerp(currentLookAtPosition, targetPosition, 3f * Time.deltaTime);

        transform.LookAt(currentLookAtPosition);	
	}
}
