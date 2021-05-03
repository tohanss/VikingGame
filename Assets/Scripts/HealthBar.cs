using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject hpBarCanvas;

    public bool showHPBar = false;

    public void SetMaxHealth(float health) 
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
        hpBarCanvas.SetActive(showHPBar);
    }

    public void SetHealth(float health) 
    {
        slider.value = health;

        if (slider.value < slider.maxValue) 
        {
            showHPBar = true;
        }
        fill.color = gradient.Evaluate(slider.normalizedValue);
        hpBarCanvas.SetActive(showHPBar);
    }
}
