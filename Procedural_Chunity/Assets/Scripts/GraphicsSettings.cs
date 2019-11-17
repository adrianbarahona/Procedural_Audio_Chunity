using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    void Start()
    {
        // Force 30FPS
        Application.targetFrameRate = 30;
    }
}
