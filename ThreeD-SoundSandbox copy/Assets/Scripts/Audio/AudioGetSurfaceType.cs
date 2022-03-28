using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AudioGetSurfaceType : MonoBehaviour
{
    public LayerMask groundLayer;
    private Collider _currentCollider;
    private AudioSurfaceType _currentAudioSurfaceType;


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
        if (_currentAudioSurfaceType != null)
        {
           TriggerFMODParameter(_currentAudioSurfaceType, 0);
        }
        
        TriggerFMODParameter(newSurfaceType, 1);

        _currentAudioSurfaceType = newSurfaceType;
    }

    void TriggerFMODParameter(AudioSurfaceType newSurfaceType, float value)
    {
        FMOD.RESULT result = RuntimeManager.StudioSystem.setParameterByID(newSurfaceType.ParameterDesctription.id, value);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError(string.Format(
                ("[FMOD] StudioGlobalParameterTrigger failed to set parameter {0} : result = {1}"),
                newSurfaceType.parameter, result));
        }
    }
}