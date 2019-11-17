//  rain model - pseudogranular + noise
//  rain intensity (inverted)
0.2 => float rainIntensity;

//  point to the path where the audio file is
me.dir() => string path;
//  signal chain
// drop panning
Pan2 panning;
Gain sampleGain;
Gain samplesBuss;
SndBuf audioFile  => sampleGain => samplesBuss  => LPF lowpass_s => HPF hipass_s  => panning => dac;
2000 => lowpass_s.freq;
950 => hipass_s.freq;
0 => sampleGain.gain;
//  load the sample - a fingersnap
path + "snap.wav" => audioFile.read;


//  noise signal chain
Gain noiseGain;
Noise noise => noiseGain => LPF lowpass_n => HPF hipass_n => dac;
4500 => lowpass_n.freq;
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
    //sample random spread
    Math.random2f( -0.5, 0.5 ) => panning.pan;
    rate => audioFile.rate;
    gain => sampleGain.gain;
}