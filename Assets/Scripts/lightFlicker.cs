using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class lightFlicker : MonoBehaviour
{
    private UnityEngine.Experimental.Rendering.Universal.Light2D light;
    float i = 0;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        light.pointLightOuterRadius = 3.5f + Mathf.Sin(Mathf.Deg2Rad * i) / 6;
        i = i + 0.5f;
        if (i > 360) i = 0;
        if (light.pointLightOuterRadius > 4f) light.pointLightOuterRadius = 4f;
        if (light.pointLightOuterRadius < 1f) light.pointLightOuterRadius = 1f;
        light.pointLightInnerRadius = light.pointLightOuterRadius * 0.5f;
    }
}
