using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    public enum WaveType
    {
        Sine,
        Square,
        Triangle,
        Saw,
        Noise
    }

    public WaveType waveType;

    [Range(0, 1)]
    public float gain = 0.1f; // volume control

    // Random object for white noise
    System.Random random = new System.Random();
    float sampleBuffer;

    public float frequency = 440.0f;
    float sampleRate;
    double phase;
    double increment;

    private void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
    }

    private void OnAudioFilterRead(float[] data, int channels)
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

            switch (waveType)
            {
                case WaveType.Sine:
                    sampleBuffer = Mathf.Sin((float)phase);
                    break;

                case WaveType.Square:
                    sampleBuffer = Mathf.Sign( Mathf.Sin((float)phase) );
                    break;

                case WaveType.Triangle:
                    //sampleBuffer = 2 * Mathf.Abs(2 * ((float)phase - Mathf.Floor(0.5f + (float)phase))) - 1;
                    sampleBuffer = Mathf.PingPong((float)phase, 1.0f) * 2 - 1f;
                    break;

                case WaveType.Saw:
                    //sampleBuffer = 2 * ((float)phase - Mathf.Floor(0.5f + (float)phase));
                    sampleBuffer = Mathf.InverseLerp(0, Mathf.PI * 2, (float)phase) * 2 - 1f;
                    break;

                case WaveType.Noise:
                    sampleBuffer = (float)(random.NextDouble() * 2 - 1);
                    break;

                default:
                    break;
            }


            // write same data in all channels
            for (int c = 0; c < channels; c++)
            {
                data[i + c] = sampleBuffer * gain;
            }
        }
    }
}
