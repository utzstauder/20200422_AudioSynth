using UnityEngine;
using System.Collections;

[System.Serializable]
public class Oscillator
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

    // Random object for white noise
    System.Random random = new System.Random();
    float sampleBuffer;
    double phase;

    public void WriteAudioBuffer(float[] data, int channels, float gain, double increment)
    {
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
                    sampleBuffer = Mathf.Sign(Mathf.Sin((float)phase));
                    break;

                case WaveType.Triangle:
                    sampleBuffer = Mathf.PingPong((float)phase, 1.0f) * 2 - 1f;
                    break;

                case WaveType.Saw:
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
                data[i + c] += sampleBuffer * gain;
            }
        }
    }

}
