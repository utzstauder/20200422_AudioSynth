using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    [Range(0, 1)]
    public float gain = 0.1f; // volume control

    public float frequency = 220f;
    float sampleRate;
    
    double increment;

    public Oscillator[] oscillators;

    private void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
    }

    private void OnEnable()
    {
        NoteInput.NoteOn += NoteInput_NoteOn;
    }

    private void NoteInput_NoteOn(int noteNumber, float velocity)
    {
        frequency = NoteInput.NoteToFrequency(noteNumber);
        Debug.Log($"{noteNumber}: NoteOn");
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2 * Mathf.PI / sampleRate;

        for (int i = 0; i < oscillators.Length; i++)
        {
            oscillators[i].WriteAudioBuffer(data, channels, gain, increment);
        }


        for (int i = 0; i < data.Length; i += channels)
        {
            // normalize volume
            data[i] /= oscillators.Length;
            data[i] = Mathf.Clamp(data[i], -1, 1);

            // force mono
            for (int c = 1; c < channels; c++)
            {
                data[i + c] = data[i];
            }
        }
    }
}
