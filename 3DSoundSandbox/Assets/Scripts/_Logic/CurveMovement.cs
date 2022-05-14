using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveMovement : MonoBehaviour 
{
    public float speed;
    public Vector3 offset;
    public AnimationCurve curve;

    private float timer;
    private Vector3 origin;

    void Start()
    {
        origin = transform.localPosition;      
    }

	void Update () 
    {
        timer += speed * Time.deltaTime;
        float fraction = timer % 1f;
        transform.localPosition = origin + offset * curve.Evaluate(fraction);
	}
}
