using UnityEngine;
using System.Collections;

public class Synthesizer : MonoBehaviour
{
    public HandTracker hand;
    public HandTracker anotherHand;
    float freq = 440.0f;
    int semitone = 0;
    float modulation = 0.7f;
    int octave = 0;
    float time01 = 0.0f;
    float volume = 0.0f;
    float clean = 1.0f;
    float dirty = 0.0f;
    int[] scale = {0, 2, 4, 5, 7, 9, 11};

    void UpdateSynth ()
    {
//      freq = 440.0f * Mathf.Pow(2.0f, (semitone - 9 + octave * 12) / 12.0f);
    }

    float TickSynth ()
    {
        float modulator = Mathf.Sin (time01 * Mathf.PI * 2 * 15) * modulation;
        return Mathf.Sin (time01 * Mathf.PI * 2 + modulator) * volume;
    }

    void Update ()
    {
        var prevVolume = volume;

        var pos = hand.transform.position;
        var pos2 = anotherHand.transform.position;

        volume = Mathf.Clamp01 (hand.openness / 0.8f - 0.2f);

        if (anotherHand.openness > 0.0f) {
            modulation = Mathf.Clamp01 ((pos2 - pos).magnitude - 0.8f) * 3.0f;
        } else {
            modulation = 0.0f;
        }

        if (prevVolume == 0.0f && volume > 0.0f) {
            int index = (int)(Mathf.Clamp01 (pos.y - 0.2f) * 24);
            semitone = scale [index % scale.Length];
            octave = index / scale.Length;
            freq = 440.0f * Mathf.Pow (2.0f, (semitone - 9 + (octave - 2) * 12) / 12.0f);
        }
    }

    void OnAudioFilterRead (float[] data, int channels)
    {
        UpdateSynth ();
        float delta = freq / Stk.Config.SampleRate;
        for (var i = 0; i < data.Length; i += 2) {
            data [i] = data [i + 1] = TickSynth ();
            time01 += delta;
            time01 -= (int)time01;
        }
    }
}
