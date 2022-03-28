using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour 
{
    public Rigidbody characterRigidbody;
    public Transform camera;

    [Space(10)]
    public float mouseSensitivityX = 250f;
    public float mouseSensitivityY = 200f;

    [Space(10)]
    public float smoothSpeedX = 15f;
    public float smoothSpeedY = 10f;

    [Space(10)]
    public float verticalAngleMax = 40f;
    public float verticalAngleMin = -50f;

    [Space(10)]
    public bool lockCursor = true;

    private bool cursorIsLocked;
    private float currentVerticalAngle;
    private Quaternion characterTargetRotation;

	void Start () 
    {
        characterTargetRotation = characterRigidbody.transform.localRotation;
	}

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        //Rotate character
        characterTargetRotation *= Quaternion.Euler(0f, mouseX, 0f);
        Quaternion nextCharacterRotation = Quaternion.Slerp(characterRigidbody.rotation, characterTargetRotation, smoothSpeedX * Time.deltaTime);
        characterRigidbody.MoveRotation(nextCharacterRotation);

        //Look up and down
        float nextVerticalAngle = currentVerticalAngle - mouseY;
        currentVerticalAngle = Mathf.Clamp(nextVerticalAngle, verticalAngleMin, verticalAngleMax);

        Quaternion cameraRotationTarget = Quaternion.Euler(currentVerticalAngle, 0f, 0f);
        camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraRotationTarget, smoothSpeedY * Time.deltaTime);

        LockCursor();
    }

    void LockCursor()
    {
        if (lockCursor == false)
        {
            return;
        }

        //Unlock with escape and lock again by clicking in game view
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorIsLocked = true;
        }

        //Lock and unlock cursor
        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
