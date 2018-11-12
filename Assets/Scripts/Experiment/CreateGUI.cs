using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGUI : MonoBehaviour {

    public Slider sizeSlider;
    public Slider distanceSlider;
    public Text sizeText;
    public Text distanceText;
    public SteamVR_TrackedObject trackedController;
    public GameObject userInput;
    public GameObject sitPosition;

    private float sizeSliderValue = 1.0F;
    private float distanceSliderValue = 1.0F;
    private string focused = "size";
    private float resolution = 0.01f;
    private GUIStyle sizeStyle = new GUIStyle();
    private GUIStyle distanceStyle = new GUIStyle();

	// Use this for initialization
	void Start () {
        sizeSlider.maxValue = 2.0f;
        sizeSlider.minValue = 0.0f;
        distanceSlider.maxValue = 2.0f;
        distanceSlider.minValue = 0.0f;
        InitValue();
	}
	
	// Update is called once per frame
	void Update () {
        var device = SteamVR_Controller.Input((int)trackedController.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            var position = device.GetAxis();
            float x = position.x;
            float y = position.y;
            if (y >= 0.5f && x < 0.5f && x > -0.5f)
            {
                focused = "size";
                sizeText.color = Color.red;
                distanceText.color = Color.black;
            }
            else if (y <= -0.5f && x < 0.5f && x > -0.5f)
            {
                focused = "distance";
                sizeText.color = Color.black;
                distanceText.color = Color.red;
            }

            if (x >= 0.5f && y < 0.5f && y > -0.5f)
            {
                ChangeValue(1);
            }
            else if (x <= -0.5f && y < 0.5f && y > -0.5f)
            {
                ChangeValue(-1);
            }
        }
        UpdateUI();
	}

    void UpdateUI()
    {
        sizeSlider.value = sizeSliderValue;
        distanceSlider.value = distanceSliderValue;
        sizeText.text = "size: " +sizeSliderValue.ToString("F2");
        distanceText.text = "distance: " + distanceSliderValue.ToString("F2");
        userInput.transform.position = sitPosition.transform.position + new Vector3(0, sizeSliderValue / 2, -(distanceSliderValue + sizeSliderValue / 2));
        userInput.transform.localScale = new Vector3(sizeSliderValue, sizeSliderValue, sizeSliderValue);
    }

    public void InitValue() {
        sizeSliderValue = 1.0f;
        distanceSliderValue = 1.0f;
        focused = "size";
        sizeText.color = Color.red;
        distanceText.color = Color.black;
    }

    public float[] ReturnValue() {
        float[] valueSet = { sizeSliderValue, distanceSliderValue };
        return valueSet;
    }

    private void ChangeValue(int value){
        switch (focused)
        {
            case "size":
                sizeSliderValue += value * resolution;
                break;
            case "distance":
                distanceSliderValue += value * resolution;
                break;
            default:
                print("error");
                break;
        }
    }

}
