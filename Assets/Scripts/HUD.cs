using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    public HandTracker[] tracker;

    void OnGUI ()
    {
        GUILayout.BeginArea (new Rect (16, 16, Screen.width - 32, Screen.height - 32));
        GUILayout.Label ("Hand[0] openness: " + tracker [0].openness);
        GUILayout.Label ("Hand[1] openness: " + tracker [1].openness);
        GUILayout.EndArea ();
    }
}
