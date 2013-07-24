using UnityEngine;
using System.Collections;

public class HandTracker : MonoBehaviour
{
    static Leap.Controller controller;
    const float CoordinateScale = 1.0f / 200;
    public int handIndex;
    public GameObject fingerPrefab;
    GameObject[] fingers;
    [HideInInspector]
    public float
        openness;

    static Vector3 ConvertVector (Leap.Vector v)
    {
        return new Vector3 (v.x, v.y, -v.z);
    }

    static float FingerDistanceToOpenness (float dist)
    {
        return Mathf.Clamp01 ((dist - 0.45f) * 20.0f);
    }
    
    void Start ()
    {
        if (controller == null) {
            controller = new Leap.Controller ();
        }

        fingers = new GameObject[5];
        for (var i = 0; i < 5; i++) {
            fingers [i] = Instantiate (fingerPrefab) as GameObject;
        }
    }
    
    void Update ()
    {
        var frame = controller.Frame ();
        var maxDistance = 0.0f;

        transform.position = ConvertVector (frame.Hands [handIndex].PalmPosition) * CoordinateScale;

        for (var i = 0; i < 5; i++) {
            var finger = frame.Hands [handIndex].Fingers [i];

            fingers [i].renderer.enabled = finger.IsValid;
            if (!finger.IsValid) {
                continue;
            }

            fingers [i].transform.position = ConvertVector (finger.TipPosition) * CoordinateScale;

            var distance = (fingers [i].transform.position - transform.position).magnitude;
            maxDistance = Mathf.Max (maxDistance, distance);
        }

        openness = Mathf.Lerp (openness, FingerDistanceToOpenness (maxDistance), 0.1f);
        transform.localScale = Vector3.one * openness * 0.5f;

        renderer.enabled = frame.Hands [handIndex].IsValid;
    }
}
