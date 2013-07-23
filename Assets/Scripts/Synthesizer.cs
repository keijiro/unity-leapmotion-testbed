using UnityEngine;
using System.Collections;

public class Synthesizer : MonoBehaviour {
	public HandTracker[] hands;

	float freq = 440.0f;
	int semitone = 0;
	float modulation = 0.2f;
	int octave = 0;
	float time01 = 0.0f;
	float volume = 0.9f;
	float clean = 1.0f;
	float dirty = 0.0f;

	int[] scale = {0, 2, 4, 7, 9};

	void UpdateSynth()
	{
//		freq = 440.0f * Mathf.Pow(2.0f, (semitone - 9 + octave * 12) / 12.0f);
	}

	float TickSynth()
	{
		float modulator = Mathf.Sin(time01 * Mathf.PI * 2 * 3) * modulation;
		float hb = Mathf.Sin(time01 * Mathf.PI * 2 + modulator) * volume;
		float lb = hb * 4;
		lb = lb - (int)lb;
		return hb * clean + lb * dirty;
	}

	void Update() {
		var pos1 = hands[0].transform.position;
		var pos2 = hands[1].transform.position;

		modulation = (hands[0].openness + hands[1].openness) * 4.0f;

		freq = Mathf.Clamp01 ((pos2 - pos1).magnitude - 0.5f) * 1000.0f + 110.0f;
		dirty = Mathf.Clamp01 ((pos1.y + pos2.y) * 0.5f - 0.4f);
		clean = 1.0f - dirty;

//		volume = Mathf.Clamp01 ((pos2 - pos1).magnitude - 0.5f);

//		int index = (int)(Mathf.Clamp01 (Mathf.Min (pos1.y, pos2.y) - 0.4f) * 12);
//		semitone = scale[index % scale.Length];
//		octave = index / scale.Length;
//		freq = 440.0f * Mathf.Pow(2.0f, (semitone - 9 + (octave - 1) * 12) / 12.0f);
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		UpdateSynth();
		float delta = freq / Stk.Config.SampleRate;
		for (var i = 0; i < data.Length; i += 2) {
			data[i] = data[i + 1] = TickSynth();
			time01 += delta;
			time01 -= (int)time01;
		}
	}
}
