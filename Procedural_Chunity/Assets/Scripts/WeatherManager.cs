using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public Light Sun;
    public WindZone Wind;
    public ParticleSystem rainParticles;

    void Start()
    {
        //  default wind (almost nothing)
        setWind(0.55f);
        //  default weather (sunny)
        setWeather(1.0f);
        //  default skybox color (sunny)
        RenderSettings.skybox.SetFloat("_Exposure", 1);

    }

    //  set the wind from slider
    public void setWind(float windLevel)
    {
        Wind.windMain = windLevel;
        Wind.windTurbulence = windLevel;
        

    }

    public void setWeather(float weatherStatus)
    {
        //  weather [Rainy - Sunny]
        // remapping the slider value through interpolation
        //  sun light value
        Sun.intensity = Mathf.Lerp(0.1f, 1f, Mathf.InverseLerp(0f, 1f, weatherStatus));
        //  fog value
        RenderSettings.fogDensity = Mathf.Lerp(0.015f, 0.001f, Mathf.InverseLerp(0f, 1f, weatherStatus));
        //  skybox value
        RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(0.2f, 1f, Mathf.InverseLerp(0f, 1f, weatherStatus)));

        //  rain (accessing the emission component)
        ParticleSystem.EmissionModule rainEmission = rainParticles.emission;
        rainEmission.rateOverTime = Mathf.Lerp(2000, 0, Mathf.InverseLerp(0f, 1f, weatherStatus));     
    }

    void Update()
    {
    }
}
