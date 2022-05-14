using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    public Action<BirdTrigger, GameObject> onTriggerEnter;
    public bool hasBird;
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(this, other.gameObject);
    }


}
