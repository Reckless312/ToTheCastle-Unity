using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    public void SetSliderValue(float value)
    {
        healthSlider.value = value;
    }

    public void SetMaxValue(float value)
    {
        healthSlider.maxValue = value;
        SetSliderValue(value);
    }
}
