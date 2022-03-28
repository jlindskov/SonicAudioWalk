using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public static class Utils
{
    public static EventInstance PlayAudioEvent (string eventName, Vector3 position) 
    {

        EventInstance instance = RuntimeManager.CreateInstance(eventName);

        if (string.IsNullOrEmpty(eventName))
            return instance;
        
        instance.start();

        FMOD.ATTRIBUTES_3D positionAttribute = RuntimeUtils.To3DAttributes(position);
        instance.set3DAttributes(positionAttribute);

        instance.release();

        return instance;
    }	
}
