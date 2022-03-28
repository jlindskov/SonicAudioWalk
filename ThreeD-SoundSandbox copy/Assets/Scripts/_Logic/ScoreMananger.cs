using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMananger : MonoBehaviour 
{
    public static float score = 0f;
    public Rect position;
    public int fontSize = 16;

    private GUIStyle style = new GUIStyle();

    public static void AddPoints(float value)
    {
        score += value;
    }

    void OnGUI()
    {   
        style.fontSize = fontSize;
        GUI.Label(position, "Points : " + score.ToString(), style);
    }
}
