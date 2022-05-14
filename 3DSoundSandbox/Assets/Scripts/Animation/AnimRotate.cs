using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRotate : MonoBehaviour
{

    public Vector3 rotateAxis;
    public float rotateSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAxis, rotateSpeed* Time.deltaTime);
    }
}
