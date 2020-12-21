using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioSampler : MonoBehaviour
{
    public AudioClip track;
    public AudioMixerGroup amgMaster;
    private AudioSource audioSrc;
    public static float[] spectrum;
    public static float[] wave;
    public static float[] bands;
    public static float amplitude = 0;
    public static float smoothedAmplitude = 0;
    public static int frameSize = 1024;
    public float binWidth;
    public float sampleRate;
    
    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        spectrum = new float[frameSize];
        wave = new float[frameSize];
        bands = new float[(int) Mathf.Log(frameSize, 2)];
                
        audioSrc.clip = track;
        audioSrc.outputAudioMixerGroup = amgMaster;
        
        audioSrc.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;
        binWidth = AudioSettings.outputSampleRate / 2 / frameSize;
    }

    void GetFrequencyBands()
    {        
        for (int i = 0; i < bands.Length; i++)
        {
            int start = (int)Mathf.Pow(2, i) - 1;
            int width = (int)Mathf.Pow(2, i);
            int end = start + width;
            float average = 0;
            for (int j = start; j < end; j++)
            {
                average += spectrum[j] * (j + 1);
            }
            average /= (float) width;
            bands[i] = average;
        }

    }

    public void GetAmplitude()
    {
        float total = 0;
        for(int i = 0 ; i < wave.Length ; i ++)
            {
        total += Mathf.Abs(wave[i]);
        }
        amplitude = total / wave.Length;
        smoothedAmplitude = Mathf.Lerp(smoothedAmplitude, amplitude, Time.deltaTime * 3);
  }

    // Update is called once per frame
    void Update()
    {
        audioSrc.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        audioSrc.GetOutputData(wave, 0);
        GetAmplitude();
        GetFrequencyBands();
    }
}
