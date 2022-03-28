using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour 
{
    public Transform headTransform;

    [Space(10)]
    public float acceleration = 0.7f;
    public float friction = 3f;

    [Space(10)]
    public float bobSize = 0.025f;
    public float randomDeviation = 0.1f;

    private Vector3 origin;
    private Vector3 currentVelocity;

	void Start ()   
    {
        origin = headTransform.localPosition;
	}
	
    void Update () 
    {
        Vector3 currentPosition = headTransform.localPosition;

        //Accelerate towards origin
        currentVelocity += (origin - currentPosition) * acceleration * Time.deltaTime;

        //Friction
        currentVelocity -= currentVelocity * friction * Time.deltaTime;

        //Apply velocity and update position
        Vector3 nextPosition = currentPosition + currentVelocity;
        headTransform.localPosition = nextPosition;
	}
        
    public void Bob()
    {
        currentVelocity.y -= bobSize * Random.Range(1f - randomDeviation, 1f + randomDeviation);
    }

    public void AddForce(float force)
    {
        currentVelocity.y += force;
    }
}
