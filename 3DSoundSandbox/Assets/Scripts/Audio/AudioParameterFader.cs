using System;
using AK.Wwise;
using UnityEngine;


public class AudioParameterFader : MonoBehaviour
{
    
    //TODO: Might be worth doing for wwise
    
    [Serializable]
    public class FaderSettings
    {
        public RTPC rtpc;
        public float fadeFrom;
        public float fadeTo;
        [HideInInspector]
        public float currentValue;

        public void Init()
        {
            
        }
    }

    public FaderSettings[] fades;
    private bool _fadeActive;
    private Transform _trackTransform;
    void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        _fadeActive = true;
        _trackTransform = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        _fadeActive = false;
        _trackTransform = null;
    }


    void Update()
    {
        if(!_fadeActive) return;
        foreach (FaderSettings fade in fades)
        {
            float t = transform.InverseTransformPoint(_trackTransform.position).x + 0.5f;
            fade.currentValue = Mathf.Lerp(fade.fadeFrom, fade.fadeTo, t);
            fade.rtpc.SetGlobalValue(fade.currentValue);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.zero, new Vector3(1,0.1f,0.1f));
        Gizmos.DrawSphere(Vector3.right/2, 0.1f);
    }
}