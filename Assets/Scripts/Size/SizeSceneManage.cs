using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSceneManage : MonoBehaviour {
    public Serial serial;
    public SizeRandomPosition randomPosition;
    public CreateUI createUI;
    public SaveFile saveFile;
    public SteamVR_TrackedObject trackedController;

    private List<string[]> wholeData = new List<string[]>();
    private int testNum = 0;

    void Start () {
        createUI.SwitchMode("size");
    }
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
            temp[2] = temp1[2].ToString("F2");
            temp[3] = temp2[0].ToString("F2");
            temp[4] = temp2[1].ToString("F2");
            temp[5] = Mathf.Sqrt(temp2[0] * temp2[0] + temp2[1] * temp2[1]).ToString();
            wholeData.Add(temp);
            testNum++;
            randomPosition.StartTest();
        } else if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            var position = device.GetAxis();
            float x = position.x;
            float y = position.y;
            if (y >= 0.5f)
            {
               createUI.ChangeValue("size", 1);
            }
            else if (y <= -0.5f)
            {
                createUI.ChangeValue("size", -1);
            }
        }
        createUI.UpdateUI();
    }

    void OnApplicationQuit() {
        string[] str = { "Actual_Size", "Actual_Distance", "hand_pos", "Size", "Distance", "Length" };
        saveFile.WriteFile("/Data/Size/saveData.csv", wholeData, str);
        serial.WriteToArduino("0");
        serial.CloseStream();
    }


}
