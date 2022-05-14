using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AudioGetSurfaceType : MonoBehaviour
{
    public LayerMask groundLayer;
    private Collider _currentCollider;
    private AudioSurfaceType _currentAudioSurfaceType;


    //TODO: Fix this to work with wwise. 
    
    void FixedUpdate()
    {
        Physics.Raycast(transform.position+Vector3.up, Vector3.down, out var hit, groundLayer);
        Debug.DrawLine(transform.position, transform.position + Vector3.down, Color.yellow);
        if (hit.transform != null && hit.collider != _currentCollider)
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.red);
            Debug.DrawLine(transform.position, hit.point, Color.magenta);

            _currentCollider = hit.collider;

            hit.transform.gameObject.TryGetComponent(out AudioSurfaceType newSurfaceType);
            if (newSurfaceType != null && newSurfaceType != _currentAudioSurfaceType)
            {
                SetSurfaceType(newSurfaceType);
            }
            
        }
    }

    void SetSurfaceType(AudioSurfaceType newSurfaceType)
    {
        SetWwiseSwitch(newSurfaceType);
        _currentAudioSurfaceType = newSurfaceType;
    }

    void SetWwiseSwitch(AudioSurfaceType newSurfaceType)
    {
        newSurfaceType.groundType.SetValue(this.gameObject);
    }
}