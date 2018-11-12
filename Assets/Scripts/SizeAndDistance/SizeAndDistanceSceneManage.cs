﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeAndDistanceSceneManage : MonoBehaviour {
    public Serial serial;
    public SizeAndDistanceRandomPosition randomPosition;
    public CreateUI createUI;
    public SaveFile saveFile;
    public SteamVR_TrackedObject trackedController;

    private List<string[]> wholeData = new List<string[]>();
    private int testNum = 0;

    // Update is called once per frame
    void Update () {
        var device = SteamVR_Controller.Input((int)trackedController.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            string[] temp = new string[6];
            float[] temp1 = randomPosition.ReturnValue();
            float[] temp2 = createUI.ReturnValue();
            temp[0] = temp1[0].ToString("F2");
            temp[1] = temp1[1].ToString("F2");
            temp[2] = Mathf.Sqrt(temp1[0] * temp1[0] + temp1[1] * temp1[1]).ToString();
            temp[3] = temp2[0].ToString("F2");
            temp[4] = temp2[1].ToString("F2");
            temp[5] = Mathf.Sqrt(temp2[0] * temp2[0] + temp2[1] * temp2[1]).ToString();
            wholeData.Add(temp);
            testNum++;
            randomPosition.StartTest();
        }
        else if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            var position = device.GetAxis();
            float x = position.x;
            float y = position.y;
            if (y >= 0.5f)
            {
                createUI.ChangeValue("all", 1);
            }
            else if (y <= -0.5f)
            {
                createUI.ChangeValue("all", -1);
            }
        }
        createUI.UpdateUI();
    }

    void OnApplicationQuit() {
        string[] str = { "Actual_Size", "Actual_Distance", "Actual_Length", "Size", "Distance", "Length" };
        saveFile.WriteFile("/Data/SizeAndDistance/saveData.csv", wholeData, str);
        serial.WriteToArduino("0");
        serial.CloseStream();
    }
}
