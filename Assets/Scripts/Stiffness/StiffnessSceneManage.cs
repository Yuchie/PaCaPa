using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StiffnessSceneManage : MonoBehaviour {

    public Serial serial;
    public StiffnessRandom stiffnessRandom;
    public SteamVR_TrackedObject trackedController;
    public SaveFile saveFile;
    public Text[] userInput;
    public Text Num;

    private List<string[]> wholeData = new List<string[]>();
    private int userInputIndex;
    private int num = 0;

    // Use this for initialization
    void Start () {
        userInputIndex = 1;
        UpdateInput(0);
	}
	
	// Update is called once per frame
	void Update () {
        var device = SteamVR_Controller.Input((int)trackedController.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            string[] temp = new string[4];
            float[] temp1 = stiffnessRandom.ReturnValue();
            temp[0] = temp1[0].ToString("F2");
            temp[1] = temp1[1].ToString("F2");
            temp[2] = userInputIndex.ToString();
            switch (temp[2])
            {
                case "0":
                    temp[3] = "sight";
                    break;
                case "1":
                    return;
                    break;
                case "2":
                    temp[3] = "tactile";
                    break;
            }
            wholeData.Add(temp);

            stiffnessRandom.StartTest();
            userInputIndex = 1;
            UpdateInput(0);
            num++;
            if (num > 25)
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
            if (y / x > 1 || y / x < -1)
            {
                if (y > 0)
                {
                    // UP
                    UpdateInput(-1);
                }
                else
                {
                    //DOWN
                    UpdateInput(1);
                }
            }
            else
            {
                if (x > 0)
                {
                    // RIGHT
                }
                else
                {
                    // LEFT
                }
            }
        }
    }

    private void UpdateInput(int num)
    {
        if (userInputIndex + num >= 0 && userInputIndex + num < userInput.Length)
        {
            userInputIndex += num;
            for (int i = 0; i < userInput.Length; i++)
            {
                if (i == userInputIndex)
                {
                    userInput[i].color = Color.red;
                }
                else
                {
                    userInput[i].color = Color.black;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        string[] str = { "Sight", "Pressure", "Softer" };
        saveFile.WriteFile("/Data/Stiffness/saveData.csv", wholeData, str);
        serial.WriteToArduino("0");
        serial.CloseStream();
    }
}
