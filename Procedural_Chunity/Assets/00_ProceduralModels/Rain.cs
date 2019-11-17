using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{    //  rain model - pseudogranular + noise

    void Start()
    {
        GetComponent<ChuckSubInstance>().RunCode(@"
        //  rain intensity (inverted)
        1 => global float rainIntensity;

        //  point to the path where the audio file is
        me.dir() => string path;
        //  signal chain
        Gain sampleGain;
        Gain samplesBuss;
        SndBuf audioFile  => sampleGain => samplesBuss  => LPF lowpass_s => HPF hipass_s => dac;
        2000 => lowpass_s.freq;
        950 => hipass_s.freq;
        0 => sampleGain.gain;
        //  load the sample - a fingersnap
        path + ""snap.wav"" => audioFile.read;


        //  noise signal chain
        Gain noiseGain;
        Noise noise => noiseGain => LPF lowpass_n => HPF hipass_n => dac;
        3500 => lowpass_n.freq;
        450 => hipass_n.freq;
        0 => noiseGain.gain;

        spork ~ setFreqandGain();

        while (true)
        {
            ((Math.sin(rainIntensity) + 0.01) * 5.5) * Math.random2f(0.05, 0.2) ::second => now;
            spork ~ triggerSample(Math.random2f(2, 6), Math.random2f(0.05, 1));
        }
        fun void setFreqandGain()
        {        
            while (true)
            {
                // other curves will make a better model!
                (1 - rainIntensity) * 0.2 => noiseGain.gain;    
                (1 - rainIntensity) * 3500 => lowpass_n.freq;
                (1 - rainIntensity) * 4 => samplesBuss.gain;
                10 :: ms => now;
            }
        }
        fun void triggerSample(float rate, float gain)
        {
            0 => audioFile.pos;
            rate => audioFile.rate;
            gain => sampleGain.gain;
        }
        ");
    }
    public void rainIntensity(float newIntensity)
    {
        GetComponent<ChuckSubInstance>().SetFloat("rainIntensity", Mathf.Lerp(0.2f, 1f, Mathf.InverseLerp(0f, 1.0f, newIntensity)));
    }
}
