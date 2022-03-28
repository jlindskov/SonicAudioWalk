using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour 
{
    [Header("---- Audio ----")]

    // [EventRef]
    // public string landEvent;
    //
    // [EventRef]
    // public string jumpEvent;
    //
    // [EventRef]
    // public string footstepEvent;

    [Header("---- Headbob ----")]
    public Headbob headbob;
    public float stepDistance = 2f;
    public float stepDeviation = 0.1f;
    public float headbobLandForce = 1f;

    [Header("---- Movement ----")]
    public float acceleration = 1800f;
    public float maxSpeed = 6f;

    [Space(10)]
    public float accelerationSprinting = 3500f;
    public float maxSpeedSprinting = 12f;

    [Space(10)]
    public float accelerationAir = 2000f;
    public float maxSpeedAir = 10f;

    [Space(10)]
    public float friction = 300f;
    public float breakFriction = 400f;
    public float slopeFriction = 1200f;
    public float airFriction = 150f;
    public float maxGroundAngle = 50f;

    [Header("---- Jump ----")]
    public Vector3 jumpStrength = new Vector3(0f, 12f, 0f);
    public float jumpVeclocityBoost = 0.7f;

    [Space(10)]
    public Vector3 gravity = new Vector3(0f, 1000f, 0f);

    [Header("---- Carry ----")]
    public float pickupDistance = 2.5f;
    public Vector3 carryPosition;
    public float carryForce = 20f;
    public float carryAngularFriction = 10f;
    public float maxThrowVelocity = 1f;

    [Header("---- Setup ----")]
    public Rigidbody characterRigidbody;
    public CapsuleCollider characterCollider;
    public Transform mainCamera;
    public LayerMask groundLayerMask;
    public LayerMask pickupLayerMask;
    public Texture crosshair;

    [Header("---- State ----")]
    public float currentSpeed;
    public float groundAngle;
    public Rigidbody currentlyCarrying;
    public bool currentlyGrounded;
    public bool previouslyGrounded;

    private Vector3 groundNormal;
    private Vector3 groundSurfaceNormal;
    private float distanceToNextStep;

	void FixedUpdate () 
    {
        //----// Gather information about current state //----//

        //Input
		Application.runInBackground = true;
        Vector3 input;
        input.x = Input.GetAxis("Horizontal");
        input.y = 0;
        input.z = Input.GetAxis("Vertical");

        bool currentlySprinting = Input.GetKey(KeyCode.LeftShift);

        //Environment
        GroundCheck();

        CarryObjects();

        Vector3 velocity = characterRigidbody.velocity;

        Vector3 horizontalVelocity = velocity;
        horizontalVelocity.y = 0;

        currentSpeed = velocity.magnitude;

        if (Mathf.Abs(currentSpeed) < 0.001f)
            currentSpeed = 0;

        //----// Act on current state //----//

        if (input.magnitude > 0.2f)
        {
            //Movement
            Vector3 moveDirection = characterRigidbody.transform.TransformDirection(input);
            float currentAcceleration = 0;

            if (currentlyGrounded && groundAngle < maxGroundAngle)
            {
                if (currentlySprinting)
                {
                    //Ground movement, sprinting
                    if (currentSpeed < maxSpeedSprinting)
                    {
                        currentAcceleration = accelerationSprinting;
                    }
                }
                else
                {
                    //Ground movement, normal
                    if (currentSpeed < maxSpeed)
                    {
                        currentAcceleration = acceleration;
                    }
                }
            }
            else if(currentlyGrounded == false)
            {
                //Air movement
                if (currentSpeed < maxSpeedAir)
                {
                    currentAcceleration = accelerationAir;
                }
            }

            //Add movement force
            characterRigidbody.AddForce(moveDirection * currentAcceleration * Time.deltaTime); 
        }
        else
        {
            //Break friction
            if (currentlyGrounded && groundAngle < maxGroundAngle)
            {
                Vector3 breakFrictionForce = -horizontalVelocity * breakFriction * Time.deltaTime;
                characterRigidbody.AddForce(breakFrictionForce); 

                //Slope friction
                Vector3 slopeFrictionForce = groundSurfaceNormal * slopeFriction * Time.deltaTime;
                characterRigidbody.AddForce(slopeFrictionForce); 
            }
        }

        //Constant friction
        if (currentlyGrounded)
        {
            //Ground friction
            Vector3 frictionForce = -horizontalVelocity * friction * Time.deltaTime;
            characterRigidbody.AddForce(frictionForce); 
        }
        else
        {
            //Air friction
            Vector3 frictionForce = -characterRigidbody.velocity * friction * Time.deltaTime;
            frictionForce.y = 0;
            characterRigidbody.AddForce(frictionForce); 
        }

        //Gravity
        Vector3 gravityForce = gravity * Time.deltaTime;
        characterRigidbody.AddForce(gravityForce); 

        //Jump
        if (currentlyGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //Footstep Sound
        if (currentlyGrounded)
        {
            distanceToNextStep -= velocity.magnitude * Time.deltaTime;
        
            if (distanceToNextStep <= 0)
            {
                //TODO: FS sound 

                if (headbob)
                {
                    headbob.Bob();
                }

                distanceToNextStep = stepDistance * Random.Range(1f - stepDeviation, 1f + stepDeviation);
            }
        }
	}

    void Jump()
    {
        Vector3 jumpImpulse = characterRigidbody.transform.TransformDirection(jumpStrength);

        Vector3 jumpBoost = characterRigidbody.velocity * jumpVeclocityBoost;
        jumpBoost.y = 0;
        jumpImpulse += jumpBoost;

        characterRigidbody.AddForce(jumpImpulse * -Mathf.Sign(gravity.y), ForceMode.Impulse);     

        //TODO: Jump
    }

    void GroundCheck()
    {
        Vector3 origin = characterCollider.transform.position;
        RaycastHit hit;

        currentlyGrounded = false;
        groundNormal = Vector3.up;
        groundAngle = 0f;

        if (Physics.SphereCast(origin, characterCollider.radius * 0.9f, gravity.normalized, out hit, 100f, groundLayerMask, QueryTriggerInteraction.Ignore))
        {
            float groundDistance = Mathf.Abs(origin.y - hit.point.y) - characterCollider.height * 0.5f;

            if (groundDistance < 0.02f)
            {
                currentlyGrounded = true;
                groundNormal = hit.normal;
                groundAngle = Vector3.Angle(Vector3.up * -Mathf.Sign(gravity.y), hit.normal);

                Vector3 groundRight = Vector3.Cross(hit.normal, Vector3.up);
                groundSurfaceNormal = Vector3.Cross(groundRight, hit.normal);
            }
        }

        if (currentlyGrounded && previouslyGrounded == false)
        {
            if (headbob)
            {
                headbob.AddForce(characterRigidbody.velocity.y * headbobLandForce * 0.01f);
            }

            //TODO: LAND
           
        }

        previouslyGrounded = currentlyGrounded;
    }

    void CarryObjects()
    {
        bool mouseLeftDown = Input.GetMouseButton(0);

        if (currentlyCarrying)
        {
            if (mouseLeftDown)
            {
                //Carry object
                Vector3 targetPosition = mainCamera.TransformPoint(carryPosition);

                Vector3 followVelocity = (targetPosition - currentlyCarrying.position) * carryForce;
                currentlyCarrying.velocity = followVelocity;

                Vector3 angularFrictionForce = -currentlyCarrying.angularVelocity * carryAngularFriction * Time.deltaTime;
                currentlyCarrying.AddTorque(angularFrictionForce);
            }
            else
            {
                //Drop Object
                currentlyCarrying.useGravity = true;
                currentlyCarrying.velocity = Vector3.ClampMagnitude(currentlyCarrying.velocity, maxThrowVelocity);
                currentlyCarrying = null;
            }
        }
        else 
        {
            if (mouseLeftDown)
            {
                //Pickup Object
                Vector3 origin = mainCamera.transform.position;
                Vector3 direction = mainCamera.transform.forward;

                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(origin, direction, out hit, pickupDistance, pickupLayerMask, QueryTriggerInteraction.Ignore))
                {
                    Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;

                    if (attachedRigidbody)
                    {
                        currentlyCarrying = attachedRigidbody;
                        currentlyCarrying.useGravity = false;
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        if (crosshair)
        {
            float size = 20f;

            float x = (Screen.width - size) * 0.5f;
            float y = (Screen.height - size) * 0.5f;

            Rect position = new Rect(x, y, size, size);

            GUI.DrawTexture(position, crosshair);
        }
    }
}
