using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        SetMaxEnergy(100);
    }

    public void SetMaxEnergy(float energy)
    {
        slider.maxValue = energy;
        slider.value = energy;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            slider.value -= 0.05f;
        }
        else
        {
            slider.value += 0.05f;
        }
    }
}
