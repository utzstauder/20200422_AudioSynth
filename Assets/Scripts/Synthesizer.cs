using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    [Range(0, 1)]
    public float gain = 0.1f; // volume control
    
    public Oscillator[] oscillators;

    List<Voice> voices;

    private void Awake()
    {
        voices = new List<Voice>();
        voices.Add(new Voice());
        voices.Add(new Voice());
        voices.Add(new Voice());
    }

    private void Start()
    {
        // StartCoroutine(Arpeggiator(20, 60, 0.1f));
    }

    IEnumerator Arpeggiator(int start, int end, float t)
    {
        for (int i = start; i <= end; i++)
        {
            NoteInput_NoteOn(i, 1f);
            yield return new WaitForSeconds(t);
        }
    }

    private void OnEnable()
    {
        NoteInput.NoteOn += NoteInput_NoteOn;
    }


    private void NoteInput_NoteOn(int noteNumber, float velocity)
    {
        voices[0].NoteOn(noteNumber, velocity, oscillators);
        voices[1].NoteOn(noteNumber + 4, velocity, oscillators);
        voices[2].NoteOn(noteNumber + 7, velocity, oscillators);
        Debug.Log($"{noteNumber}: NoteOn");
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        foreach (var voice in voices)
        {
            if (!voice.IsActive) continue;
            voice.WriteAudioBuffer(data, channels);
        }

        for (int i = 0; i < data.Length; i += channels)
        {
            // normalize volume
            data[i] /= voices.Count;
            data[i] *= gain;
            data[i] = Mathf.Clamp(data[i], -1, 1);

            // force mono
            for (int c = 1; c < channels; c++)
            {
                data[i + c] = data[i];
            }
        }
    }
}
