using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour 
{
    public PlayerMovement player;
    public Collider probe;
    public Vector3 customGravity;

    private Vector3 previousGravity;
    private bool isPlayerInside;

    void OnTriggerEnter(Collider collider)
    {
        if (collider == probe)
        {
            previousGravity = player.gravity;

            player.gravity = customGravity;

            isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider == probe && isPlayerInside)
        {
            player.gravity = previousGravity;

            isPlayerInside = false;
        }
    }
}
