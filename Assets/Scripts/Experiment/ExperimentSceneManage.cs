using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentSceneManage : MonoBehaviour {
    public Serial serial;
    public RandomPosition randomPosition;
    public CreateGUI createGUI;
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
            float[] temp2 = createGUI.ReturnValue();
            temp[0] = temp1[0].ToString("F2");
            temp[1] = temp1[1].ToString("F2");
            temp[2] = Mathf.Sqrt(temp1[0] * temp1[0] + temp1[1] * temp1[1]).ToString() ;
            temp[3] = temp2[0].ToString("F2");
            temp[4] = temp2[1].ToString("F2");
            temp[5] = Mathf.Sqrt(temp2[0] * temp2[0] + temp2[1] * temp2[1]).ToString();
            wholeData.Add(temp);
            testNum++;
            if (testNum >= 14)
            {
                Debug.Log("take a rest");
                randomPosition.Finish();
            } else {
                randomPosition.StartTest();
                createGUI.InitValue();
            }
        }
    }

    void OnApplicationQuit() {
        string[] str = { "Actual_Size", "Actual_Distance", "Actual_Length", "Size", "Distance", "Length" };
        saveFile.WriteFile("/Data/saveData.csv", wholeData, str);
        serial.WriteToArduino("0");
        serial.CloseStream();
    }
}
