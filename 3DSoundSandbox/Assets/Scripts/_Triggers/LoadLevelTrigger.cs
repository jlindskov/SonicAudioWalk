using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LoadLevelTrigger : MonoBehaviour 
{
    public SceneAsset scene;
    public Collider probe;
	
    void OnTriggerEnter (Collider collider) 
    {
        if (probe == null || collider == probe)
        {
            SceneManager.LoadScene(scene.name);
        }
	}
}
