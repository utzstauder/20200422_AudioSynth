using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice
{
    float frequency = 220;
    float gain = 0.1f;
    float sampleRate = 41000;

    double phase;
    double increment;

    float sampleBuffer;

    Oscillator[] oscillators;

    public bool IsActive => oscillators != null;

    public void NoteOn(int noteNumber, float velocity, Oscillator[] oscillators)
    {
        frequency = NoteInput.NoteToFrequency(noteNumber);
        gain = velocity;
        this.oscillators = oscillators;

        sampleRate = AudioSettings.outputSampleRate;
    }

    public void WriteAudioBuffer(float[] data, int channels)
    {
        increment = frequency * 2 * Mathf.PI / sampleRate;

        // iterate through audio buffer
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            if (phase > (Mathf.PI * 2))
            {
                phase -= (Mathf.PI * 2);
            }

            sampleBuffer = 0;

            for (int o = 0; o < oscillators.Length; o++)
            {
                sampleBuffer += oscillators[o].GetWaveSample(phase);
            }


            // write same data in all channels
            for (int c = 0; c < channels; c++)
            {
                data[i + c] += sampleBuffer * gain;
            }
        }
    }
}
