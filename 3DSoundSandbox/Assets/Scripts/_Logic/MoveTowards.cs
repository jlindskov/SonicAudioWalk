using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour 
{
    public Transform target;
    public float speed = 2f;
    public float completeDistance = 3f;
    public bool ignoreVertical = true;

	void Update () 
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = target.position;
        float currentDistance = Vector3.Distance(currentPosition, targetPosition);

        if (ignoreVertical == true)
        {
            targetPosition.y = currentPosition.y;
        }

        if (currentDistance > completeDistance)
        {
            Vector3 nextPosition = Vector3.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);

            transform.position = nextPosition;
        }
	}
}
