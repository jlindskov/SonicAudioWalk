using FMODUnity;
using UnityEngine;

public class AudioSurfaceType : MonoBehaviour
{
    
    [ParamRef]
    public string parameter;
    private FMOD.Studio.PARAMETER_DESCRIPTION parameterDescription;
    public FMOD.Studio.PARAMETER_DESCRIPTION ParameterDesctription { get { return parameterDescription; } }
    public float value;
    
    
    FMOD.RESULT Lookup()
    {
        FMOD.RESULT result = RuntimeManager.StudioSystem.getParameterDescriptionByName(parameter, out parameterDescription);
        return result;
    }
    
    void Awake()
    {
        if (string.IsNullOrEmpty(parameterDescription.name))
        {
            Lookup();
        }
    }
}
