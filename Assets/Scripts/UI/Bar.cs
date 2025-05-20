using UnityEngine;
using UnityEngine.UI;
public class Bar : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    void Update()
    {
        LookAtCamera();
    }

    public void IncreaseValue(float value)
    {
        _slider.value += value;
    }

    public void DeincreaseValue(float value)
    {
        _slider.value -= value;
    }
    public void SetValue(float newValue)
    {
        _slider.value = newValue;
    }

    public void SetMaxValue(float newValue)
    {
        _slider.maxValue = newValue;
    }
    
    public void SetMinValue(float newValue)
    {
        _slider.minValue = newValue;
    }
    
    private void LookAtCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
