using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateUI : MonoBehaviour {

    public Slider sizeSlider;
    public Slider distanceSlider;
    public Text sizeText;
    public Text distanceText;
    public GameObject userInput;
    public GameObject sitPosition;

    private float sizeSliderValue = 1.0F;
    private float distanceSliderValue = 1.0F;
    private float resolution = 0.01f;
    private float rate;
    private GUIStyle sizeStyle = new GUIStyle();
    private GUIStyle distanceStyle = new GUIStyle();

	// Use this for initialization
	void Start () {
        sizeSlider.maxValue = 2.0f;
        sizeSlider.minValue = 0.0f;
        distanceSlider.maxValue = 2.0f;
        distanceSlider.minValue = 0.0f;
        InitValue(1.0f, 1.0f);
	}

    public void UpdateUI()
    {
        sizeSlider.value = sizeSliderValue;
        distanceSlider.value = distanceSliderValue;
        sizeText.text = "size: " +sizeSliderValue.ToString("F2");
        distanceText.text = "distance: " + distanceSliderValue.ToString("F2");
        userInput.transform.position = sitPosition.transform.position + new Vector3(0, sizeSliderValue / 2, -(distanceSliderValue + sizeSliderValue / 2));
        userInput.transform.localScale = new Vector3(sizeSliderValue, sizeSliderValue, sizeSliderValue);
    }

    public void InitValue(float size, float distance) {
        sizeSliderValue = size;
        distanceSliderValue = distance;
        rate = size / distance;
        sizeText.color = Color.black;
        distanceText.color = Color.black;
    }

    public void SwitchMode(string mode)
    {
        switch (mode)
        {
            case "all":
                sizeText.color = Color.black;
                distanceText.color = Color.black;
                break;
            case "size":
                sizeText.color = Color.red;
                distanceText.color = Color.black;
                break;
            case "distance":
                sizeText.color = Color.black;
                distanceText.color = Color.red;
                break;
            default:
                print("mode Error");
                break;
        }
    }

    public float[] ReturnValue() {
        float[] valueSet = { sizeSliderValue, distanceSliderValue };
        return valueSet;
    }

    public void ChangeValue(string mode, int value){
        switch(mode)
        {
            case "all":
                sizeSliderValue += value * resolution;
                distanceSliderValue += rate * value * resolution;
                break;
            case "size":
                sizeSliderValue += value * resolution;
                break;
            case "distance":
                distanceSliderValue += value * resolution;
                break;
            default:
                print("mode Error");
                break;
        }
    }

}
