/*
Fan model - subtractive synthesis

A white noise instance is passed through a
series of bandpass filters in parallel.
The amplitude is modulated with a sinusoidal oscillator.
*/

//  MIDI setup
MidiIn min;
MidiMsg msg;

//  MIDI Port (NanoKontrol 2 in my setup, change it if you need)
0 => int port;
//  open the device
if (!min.open(port))
{
    me.exit();
}

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
0.2 => master.gain;


//  set fan parameters (depending on the speed)
fun void setFan(float speed)
{
    speed => modulator.freq;
    for (int i; i < 3; i++)  {
        (filterParams[i][0] + speed*20., filterParams[i][1] + speed/50)  => bpfilter[i].set;
        1/(2/speed) => g[i].gain; //  individual frequency gain control
    }
}


setFan(5);
spork ~ listener();

//  infinite loop
while (true)
{
    //  modulate the amplitude
    Std.fabs(modulator.last()) => buss.gain;
    //  advance time
    1::samp => now;
}


//  Checking when the slider/knob changes and triggering an event as a result
fun void listener()
{
    while( true )
    {
        min => now;
        while(min.recv(msg))
        {
            //<<<msg.data1, msg.data2, msg.data3>>>;
            if (msg.data1 == 176)   //  control message
            {
                if (msg.data2 == 0) //  slider/knob
                {
                    (msg.data3/127.) * 15. => fanspeed;
                    setFan(fanspeed);
                }
            }
        }
    }
}
