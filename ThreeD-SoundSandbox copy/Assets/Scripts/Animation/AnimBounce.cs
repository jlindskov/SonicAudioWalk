using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBounce : MonoBehaviour
{
    public Vector3 bounceAxis = Vector3.up;
    public float bounceAmount = 1;
    public float bounceSpeed = 1;

    private void Update()
    {
        transform.position += Time.deltaTime * (Mathf.Sin(Time.time * bounceSpeed) * bounceAmount) * bounceAxis;
    }
}
