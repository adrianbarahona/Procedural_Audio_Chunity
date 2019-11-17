using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightMouseOver : MonoBehaviour
{

    private Renderer tube_renderer;
    void Start()
    {
        tube_renderer = GetComponent<Renderer>();
    }

    public void OnMouseEnter()
    {
        tube_renderer.material.EnableKeyword("_EMISSION");
    }

    public void OnMouseExit()
    {
        tube_renderer.material.DisableKeyword("_EMISSION");
    }
}
