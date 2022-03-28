using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour 
{
    public Collider probe;

    [Space(10)]
    public float score = 10f;
    public GameObject deactivateTarget;

    // [EventRef]
    // public string eventName;

    void OnTriggerEnter(Collider collider)
    {
        if (probe == null || collider == probe)
        {
            ScoreMananger.AddPoints(score);

            if (deactivateTarget)
            {
                deactivateTarget.SetActive(false);
            }

            // if (eventName != "")
            // {
            //     Utils.PlayAudioEvent(eventName, transform.position);            
            // }
        }
	}
}
