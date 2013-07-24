using UnityEngine;
using System.Collections;

public class HandsBar : MonoBehaviour
{
    public HandTracker[] hands;

    void Update ()
    {
        if (hands [0].openness > 0.1f && hands [1].openness > 0.1f) {
            var pos1 = hands [0].transform.position;
            var pos2 = hands [1].transform.position;

            transform.position = (pos1 + pos2) * 0.5f;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, pos2 - pos1);
            transform.localScale = new Vector3(0.02f, (pos2 - pos1).magnitude * 0.5f, 0.02f);

            var power = Mathf.Clamp01 ((pos2 - pos1).magnitude - 0.8f);
            renderer.material.color = Color.Lerp (Color.white, Color.red, power);
            renderer.enabled = true;
        } else {
            renderer.enabled = false;
        }
    }
}
