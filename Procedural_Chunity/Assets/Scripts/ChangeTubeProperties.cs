using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTubeProperties : MonoBehaviour
{
    string tubeMaterial;
    string interactionMode;
    int attackTime;

    // set the material from the UI dropdown menu
    public void setMaterial(int material)
    {
        // get the value from the UI dropwodn menu
        switch(material)
        {
            case 0:
                tubeMaterial = "metal_1";
                break;
            case 1:
                tubeMaterial = "metal_2";
                break;
            case 2:
                tubeMaterial = "glass";
                break;
        }

        foreach (GameObject tube in GameObject.FindGameObjectsWithTag("tube"))
        {
            modalSynthesiser synth = tube.GetComponent(typeof(modalSynthesiser)) as modalSynthesiser;
            synth.impactMaterial = tubeMaterial;
        }
    }

    // set the interaction mode from the UI dropdown menu
    public void setInteraction(int impactModality)
    {
        // get the value from the UI dropwodn menu
        switch (impactModality)
        {
            case 0:
                interactionMode = "hit";
                break;
            case 1:
                interactionMode = "slide";
                break;
        }

        foreach (GameObject tube in GameObject.FindGameObjectsWithTag("tube"))
        {
            modalSynthesiser synth = tube.GetComponent(typeof(modalSynthesiser)) as modalSynthesiser;
            synth.impactModality = interactionMode;
        }
    }

    // set attack from the UI slider
    public void setAttack(float attack)
    {
        attackTime = Mathf.RoundToInt(attack);
        foreach (GameObject tube in GameObject.FindGameObjectsWithTag("tube"))
        {
            modalSynthesiser synth = tube.GetComponent(typeof(modalSynthesiser)) as modalSynthesiser;
            synth.impactAttack = attackTime;
        }
    }
}
