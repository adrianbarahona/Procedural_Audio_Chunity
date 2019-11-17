using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IndoorOutdoorManager : MonoBehaviour
{
    public GameObject FanPanel;
    public GameObject TubesPanel;
    public GameObject WindPanel;
    public GameObject WeatherPanel;
    public GameObject RainGameObject;
    public GameObject ProceduralRainGameobject;
    public GameObject ProceduralWindGameobject;
    Vector3 indoorPosition;
    Vector3 indoorRotation;
    Vector3 outdoorPosition;
    Vector3 outdoorRotation;

    // Start is called before the first frame update
    void Start()
    {
        // default indoor - panels
        WindPanel.SetActive(false);
        WeatherPanel.SetActive(false);

        //default indoor - no fog
        RenderSettings.fogDensity = 0.0f;
        RenderSettings.fog = false;
        //default indoor - no rain or wind
        RainGameObject.SetActive(false);
        ProceduralRainGameobject.SetActive(false);
        ProceduralWindGameobject.SetActive(false);

        // default camera position (indoor)
        indoorPosition = new Vector3(6.032256f, 2.488469f, -6.504231f);
        indoorRotation = new Vector3(6.188f, 317.273f, -0.006f);
        this.gameObject.transform.position = indoorPosition;
        this.gameObject.transform.eulerAngles = indoorRotation;

        // outdoor position value
        outdoorPosition = new Vector3(-12.2537f, 1.77f, 9.555555f);
        outdoorRotation = new Vector3(-12.892f, 219.987f, -0.006f);

    }

    //  place the camera indoor
    public void moveIndoor()
    {
        this.gameObject.transform.position = indoorPosition;
        this.gameObject.transform.eulerAngles = indoorRotation;
        FanPanel.SetActive(true);
        TubesPanel.SetActive(true);
        WindPanel.SetActive(false);
        WeatherPanel.SetActive(false);
        RenderSettings.fog = false;
        RainGameObject.SetActive(false);
        ProceduralRainGameobject.SetActive(false);
        ProceduralWindGameobject.SetActive(false);
    }

    //  place the camera outdoor
    public void moveOutdoor()
    {
        this.gameObject.transform.position = outdoorPosition;
        this.gameObject.transform.eulerAngles = outdoorRotation;
        FanPanel.SetActive(false);
        TubesPanel.SetActive(false);
        WindPanel.SetActive(true);
        WeatherPanel.SetActive(true);
        RenderSettings.fog = true;
        RainGameObject.SetActive(true);
        ProceduralRainGameobject.SetActive(true);
        ProceduralWindGameobject.SetActive(true);
    }
}
