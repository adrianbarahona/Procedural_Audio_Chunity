using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// partially from https://www.cs.princeton.edu/~gewang/thesis.pdf (p. 85)
// a noise instance is passed through parallel band-pass filters. 
// the frequency and gain of the filters are modulated depending on the wind intensity.

public class Wind : MonoBehaviour
{
    void Start()
    {
        GetComponent<ChuckSubInstance>().RunCode(@"
            // white noise
            Noise n;
            // filters and gains(low freq wind (rumble) - high freq wind (tree branches, etc.) - speedWind (turbulence)
            ResonZ filterLow;
            Gain lowGain;
            ResonZ filterHigh;
            Gain highGain;
            ResonZ filterSpeedWind;
            Gain speedWindGain;

            // gain signal chain
            Gain buss => Gain master => dac;

            //  set the wind intensity (LOWER more intense as it changes values more frequently) [0-1]
            0.85 => global float windIntensity;

            // low frequencies signal chain
            n => filterLow => lowGain => buss;
            300.0 => float lowFreq;
            float target_lowFreq;

            0.5 => float lowGainValue;
            lowGainValue => lowGain.gain;
            float target_lowGain;

            //high frequencies signal chain
            n => filterHigh => highGain => buss;
            3500 => float highFreq;
            float target_highFreq;

            0.05 => float highGainValue;
            highGainValue => highGain.gain;
            float target_highGain;

            //speedWind frequencies signal chain
            n => filterSpeedWind => speedWindGain => buss;
            750 => float speedWindFreq;
            float target_speedWindFreq;

            1 => float speedWindGainValue;
            speedWindGainValue => speedWindGain.gain;

            //  set default values for the filters
            filterLow.set(lowFreq,2);
            filterLow.set(highFreq,2);
            filterSpeedWind.set(speedWindFreq,30);

            //  make some headroom
            0.5 => master.gain;

            //  new thread to ramp the filter frequencies and gains
            spork ~ rampNewValues();
            global Event windSpeedChanged;
            spork ~ bussGain();

            // infinite time-loop
            while( true )
            {
                // new frequency value every [windIntensity] seconds
                Std.rand2f(100,500) => target_lowFreq;
                Std.rand2f(1000,4000) => target_highFreq;
                Std.rand2f(500,1300) => target_speedWindFreq;
    
                Std.rand2f(0.3,1) => target_lowGain;
                Std.rand2f(0.03,0.15) => target_highGain;
                
                (windIntensity + 0.3) :: second => now;
            }

            fun float bussGain()
            {
                while(true)
                {
                    1 - windIntensity => buss.gain;
                    20 :: ms => now;
                }
            }

            fun float rampNewValues()
            {
                while (true)
                {
                   (target_lowFreq - lowFreq) * 0.005 + lowFreq => lowFreq => filterLow.freq;
                   (target_highFreq - highFreq) * (0.005 * 0.5) + highFreq => highFreq => filterHigh.freq;
                   (target_speedWindFreq - speedWindFreq) * 0.003 + speedWindFreq => speedWindFreq => filterSpeedWind.freq;
        
                   (target_lowGain - lowGainValue) * 0.005 + lowGainValue => lowGainValue => lowGain.gain;
                   (target_highGain - highGainValue) * 0.005 + highGainValue => highGainValue => highGain.gain;
        
                    if (windIntensity > 0.5)
                        {
                            0.0 => speedWindGain.gain;
                        }
                    else
                        {
                            1 - (windIntensity + 0.5) => speedWindGainValue => speedWindGain.gain;
                        }
        
                    10 :: ms => now;
                }
            }
		");
    }

    public void windIntensity(float newIntensity)
    {
        GetComponent<ChuckSubInstance>().SetFloat("windIntensity", Mathf.Lerp(1f, 0f, Mathf.InverseLerp(0f, 1.2f, newIntensity)));
    }
    void Update()
    {
    }
}
