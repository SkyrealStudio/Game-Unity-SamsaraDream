using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class LightDimer : MonoBehaviour
{
    public Light2D light1;
    public Light2D light2;
    public Vector2 positionOrigin;

    public float stageW;
    public float stageR;

    public float redLightHighest = 0.2f;
    public float whiLightHighest = 1f;

    void Start()
    {
        if (light1.intensity != 1f)
            whiLightHighest = light1.intensity;
    }

    void Update()
    {
        light1.intensity = (Light_treat1( whiLightHighest - stageW * Vector2.Distance(light1.gameObject.transform.position, positionOrigin)));
        light2.intensity = (Light_treat2(stageR * Vector2.Distance(light2.gameObject.transform.position, positionOrigin)));
    }

    private float Light_treat1(float input)
    {
        if(input<=1f)
        {
            return input;
        }
        else
        {
            return 1f;
        }
    }

    private float Light_treat2(float input)
    {
        if (input <= redLightHighest)
        {
            return input;
        }
        else
        {
            return 0.2f;
        }
    }
}
