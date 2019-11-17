using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    /*
    Fan model - subtractive synthesis
    A white noise instance is passed through a
    series of bandpass filters in parallel.
    The amplitude is modulated with a sinusoidal oscillator. 
    */

    // speed of the fan, controllable in real-time.
    public GameObject fanMotor;
    public float fanSpeed = 5.0f;

    void Start()
    {

        GetComponent<ChuckSubInstance>().RunCode(@"
            //  define variables
            5 => float fanspeed; // speed of the fan [CONTROLLABLE in real-time]
            3 => int NUM_FILTERS;   //  number of filters
            ResonZ bpfilter[NUM_FILTERS];   //  resonant band-pass filters
            [[400., 1.],[520., 1.],[750., 1.]] @=> float filterParams [][];  //  set the default filter parameters [frequency, Q]
            Noise n;    //  white noise instance
            Gain g[NUM_FILTERS];    //  individual filter gain control
            Gain buss, master;  //  noise-filter buss (to be modulated) and master gain control
            SinOsc modulator;   //  modulator
            modulator => blackhole;

            //  SIGNAL CHAIN
            //  set the default filter parameters (freq and Q) 
            for (int i; i < 3; i++)  {
                (filterParams[i][0], filterParams[i][1])  => bpfilter[i].set;
                1 => g[i].gain; //  individual frequency gain control
                n => bpfilter[i] => g[i]; // signal chain (noise->filter->gain)
            }

            g => buss;
            buss => master;
            master => dac;
            //some headroom
            0.5 => master.gain;


            //  set fan parameters (depending on the speed)
            fun void setFan(float speed)
            {

                speed => modulator.freq;
                for (int i; i < 3; i++)  {
                    (filterParams[i][0] + speed*20., filterParams[i][1] + speed/50)  => bpfilter[i].set;
                    1/(2/speed) => g[i].gain; //  individual frequency gain control
                }
            }

            global float fanSpeed;
            global Event valueChanged;

            //setFan(fanSpeed);
            spork ~ listener();

            
            //  infinite loop
            while (true)
            {
                //  modulate the amplitude
                Std.fabs(modulator.last()) => buss.gain;
                //  advance time
                1::samp => now;
            }

            
            //  Changes the fanspeed
            fun void listener()
            {
                while(true)
                {
                    valueChanged => now;
                    setFan(fanSpeed);
                }
            }
		");
    }
    private void Update()
    {
        // Monitor every frame for changes in the variable and pass it to ChucK
        GetComponent<ChuckSubInstance>().BroadcastEvent("valueChanged");
        GetComponent<ChuckSubInstance>().SetFloat("fanSpeed", fanSpeed);
    }

    public void changeSpeed(float newSpeed)
    {
        fanSpeed = newSpeed;
    }

    private void FixedUpdate()
    {
        // rotate the fan blades based on the speed
        fanMotor.transform.Rotate(Vector3.up * ((fanSpeed * 100) * Time.deltaTime));
    }
}

