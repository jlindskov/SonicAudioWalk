using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AudioRigidbody : MonoBehaviour
{
    // [EventRef]
    // public string impactEvent;
    //
    // [EventRef]
    // public string moveEvent;
    //
    // [EventRef]
    // public string moveStopEvent;

    [Space(10)]
    public string speedParameterName;

    [Space(10)]
    public float groundDistance = 1f;
    public LayerMask environmentLayerMask;

    [Space(10)]
    public float minimumSpeed = 0.01f;
    public float maximumSpeed = 2f;

    [Space(10)]
    public float minimumImpactVelocity = 0.1f;
    public float settleSpeed = 0.1f;

    [Header("---- State ----")]
    public float currentSpeed;
    public float currentSpeedFraction;

    private bool previouslyMoving;

    private Rigidbody objectRigidbody;
    // private EventInstance instance;

    private float speedParameter;

    void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        // if (string.IsNullOrEmpty(moveEvent) == false)
        // {
        //     // instance = RuntimeManager.CreateInstance(moveEvent);
        //     //
        //     // FMOD.ATTRIBUTES_3D positionAttribute = RuntimeUtils.To3DAttributes(transform, objectRigidbody);
        //     // instance.set3DAttributes(positionAttribute);
        //     //
        //     // instance.getParameterByName(speedParameterName, out speedParameter);
        // }
    }

    void FixedUpdate()
    {
        bool currentlyGrounded = GroundCheck();

        currentSpeed = objectRigidbody.velocity.magnitude;
        bool currentlyMoving = currentSpeed > settleSpeed && currentlyGrounded;

        if (currentlyMoving)
        {
            if(previouslyMoving == false)
            {
                //Start moving    
             //   instance.start();
            }

            currentSpeedFraction = Mathf.InverseLerp(minimumSpeed, maximumSpeed, currentSpeed);
           // instance.setParameterByName(speedParameterName, currentSpeedFraction);
            //Debug.Log(speedParameter);
        }
        else
        {
            if(previouslyMoving)
            {
                //Stop moving
              //  instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                //Stop event
                // if (currentlyGrounded)
                // {
                //     Utils.PlayAudioEvent(moveStopEvent, objectRigidbody.position);           
                // }
            }
        }
            
        previouslyMoving = currentlyMoving;
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > minimumImpactVelocity)
        {
            Vector3 position = collision.contacts[0].point;
           // Utils.PlayAudioEvent(impactEvent, position);           
        }
    }

    bool GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(objectRigidbody.position, Vector3.down, out hit, groundDistance, environmentLayerMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDistance); 
    }
}
