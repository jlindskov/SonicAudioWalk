using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour 
{
    public Transform pathHolder;
    public Rigidbody rigidbody;

    [Space(10)]
    public float arriveDistance = 2f;
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    private int targetIndex = 0; 
    private Transform targetPoint;

    void Start () 
    {
        targetPoint = pathHolder.GetChild(0);
    }

	void Update () 
    {
        //Proceed to next point when close enough
        float distanceToPoint = Vector3.Distance(transform.position, targetPoint.position);
        if (distanceToPoint < arriveDistance)
        {
            targetIndex++;
            if (targetIndex >= pathHolder.childCount)
                targetIndex = 0;

            targetPoint = pathHolder.GetChild(targetIndex);
        }

        //Rotate towards point
        Vector3 direction = targetPoint.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        transform.forward += (direction - transform.forward) * rotateSpeed * Time.deltaTime;

        //Move forwards
        if (rigidbody)
        {
            Vector3 moveVelocity = transform.forward * moveSpeed * Time.deltaTime;
            moveVelocity.y = rigidbody.velocity.y;
            rigidbody.velocity = moveVelocity;
        }
        else
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }       
	}

    //Draw line in editor
    void OnDrawGizmosSelected()
    {
        if (pathHolder.childCount < 2)
            return;
        
        Vector3 startPos = pathHolder.GetChild(0).position;
        Vector3 pos = startPos;

        for (int i = 1; i < pathHolder.childCount; i++)
        {
            Vector3 nextPos = pathHolder.GetChild(i).position;
            Debug.DrawLine(pos, nextPos);
            pos = nextPos;
        }

        Debug.DrawLine(pos, startPos);
    }
}
