using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallHPSlider : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        WallHPController.hpChanged += UpdateHPInfo;
    }

    private void OnDisable()
    {
        WallHPController.hpChanged -= UpdateHPInfo;
    }

    private void UpdateHPInfo(float hpRatio)
    {
        _slider.value = hpRatio;
    }
}
