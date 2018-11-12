using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSceneManage : MonoBehaviour {

    public Serial serial;
    public ShapeRandom shapeRandom;
    public SteamVR_TrackedObject trackedController;
    public GameObject[] userInput;
    public Material chosenMaterial;
    public SaveFile saveFile;
    public Text Num;
    public GameObject[] tutorialShapes;
    public GameObject stageStand;

    private List<string[]> wholeData = new List<string[]>();
    private float[,] heightWidthList = new float[3, 2] { { 1.0f, 1.0f }, { 1.25f, 0.75f }, { 0.75f, 1.25f } };
    private Material originalMaterial;
    private int userInputIndex;
    private int num = 0;

    // Use this for initialization
    void Start () {
        userInputIndex = 0;
        originalMaterial = userInput[userInputIndex].GetComponent<Renderer>().material;
        userInput[userInputIndex].GetComponent<Renderer>().material = chosenMaterial;

        for (int i=0; i<tutorialShapes.Length; i++)
        {
            tutorialShapes[i].transform.position = stageStand.transform.position + new Vector3(0, 0.8f, 0);
            tutorialShapes[i].transform.localScale = tutorialShapes[i].transform.localScale * 0.8f;

            if (i != 0)
            {
                tutorialShapes[i].SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        var device = SteamVR_Controller.Input((int)trackedController.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            string[] temp = new string[8];
            string[] temp1 = shapeRandom.ReturnValue();
            string[] temp2 = ReturnInput();
            temp[0] = temp1[0];
            temp[1] = temp1[1];
            temp[2] = temp1[2];
            temp[3] = temp1[3];
            temp[4] = temp2[0];
            temp[5] = temp2[1];
            temp[6] = temp2[2];
            temp[7] = temp2[3];
            wholeData.Add(temp);

            userInput[userInputIndex].GetComponent<Renderer>().material = originalMaterial;
            userInputIndex = 0;
            originalMaterial = userInput[userInputIndex].GetComponent<Renderer>().material;
            userInput[userInputIndex].GetComponent<Renderer>().material = chosenMaterial;
            shapeRandom.StartTest();

            if (num == 0)
            {
                ActivateShape(0);
            }
            num++;

            if (num > 9)
            {
                Num.text = "finish";
            } else
            {
                Num.text = num.ToString();
            }
            
        }
        else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            var position = device.GetAxis();
            float x = position.x;
            float y = position.y;
            if (y/x > 1 || y/x <-1)
            {
                if(y > 0)
                {
                    // UP
                    UpdateInput(-3);
                } else
                {
                    //DOWN
                    UpdateInput(3);
                }
            } else
            {
                if (x > 0)
                {
                    // RIGHT
                    UpdateInput(1);
                } else
                {
                    // LEFT
                    UpdateInput(-1);
                }
            }
        }
    }

    private void UpdateInput(int n)
    {
        if ( userInputIndex + n >= 0 && userInputIndex + n <userInput.Length)
        {
            // recover material
            userInput[userInputIndex].GetComponent<Renderer>().material = originalMaterial;

            userInputIndex += n;
            originalMaterial = userInput[userInputIndex].GetComponent<Renderer>().material;
            userInput[userInputIndex].GetComponent<Renderer>().material = chosenMaterial;
        }

        if (num == 0)
        {
            ActivateShape(userInputIndex / 3);
        }
    }

    private void ActivateShape(int n)
    {
        for (int i=0; i<tutorialShapes.Length; i++)
        {
            if (i==n)
            {
                tutorialShapes[i].SetActive(true);
            } else
            {
                tutorialShapes[i].SetActive(false);
            }
        }
    }

    private string[] ReturnInput()
    {
        int shape = userInputIndex / 3;
        string shapeName = "";
        switch(shape)
        {
            case 0:
                shapeName = "cube";
                break;
            case 1:
                shapeName = "circle";
                break;
            case 2:
                shapeName = "triangle";
                break;
        }

        int heightWidthIndex = userInputIndex % 3;
        string[] str = { shape.ToString(), heightWidthList[heightWidthIndex, 0].ToString("F2"), heightWidthList[heightWidthIndex, 1].ToString("F2"), shapeName };

        return str;
    }

    void OnApplicationQuit()
    {
        string[] str = { "Actual_Shape", "Actual_Height", "Actual_Width", "Actual_ShapeName", "Shape", "Height", "Width", "ShapeName" };
        saveFile.WriteFile("/Data/SHape/saveData.csv", wholeData, str);
        serial.WriteToArduino("0");
        serial.CloseStream();
    }
}
