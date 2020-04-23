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

    public float GetWaveSample(double phase)
    {
        switch (waveType)
        {
            case WaveType.Sine:
                return Mathf.Sin((float)phase);

            case WaveType.Square:
                return Mathf.Sign(Mathf.Sin((float)phase));

            case WaveType.Triangle:
                return Mathf.PingPong((float)phase, 1.0f) * 2 - 1f;

            case WaveType.Saw:
                return Mathf.InverseLerp(0, Mathf.PI * 2, (float)phase) * 2 - 1f;

            case WaveType.Noise:
                return (float)(random.NextDouble() * 2 - 1);

            default:
                return 0;
        }
    }

}
