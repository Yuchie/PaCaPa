using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeAndDistanceRandomPosition : MonoBehaviour {

    public GameObject sitPosition;
    public GameObject godStick;
    public GameObject hand;
    public GameObject userInput;
    public SaveFile saveFile;
    public CreateUI createUI;

    private float next_size;
    private float next_distance;
    private float[] list = { 0.5f, 0.75f, 1.0f, 1.25f, 1.5f };
    private float[] rateList = { 1.0f };
    private List<float[]> patternList = new List<float[]>();

	// Use this for initialization
	void Start () {
        CreatePattern();
        next_size = 1.0f;
        next_distance = 1.0f;
        ChangePosSize(next_size, next_distance);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartTest () {

        GetComponent<Renderer>().enabled = false;
        godStick.GetComponent<Renderer>().enabled = false;
        hand.GetComponent<Renderer>().enabled = true;

        if (patternList.Count > 0) {
            int index = Random.Range(0, patternList.Count);
            next_size = patternList[index][0];
            next_distance = patternList[index][1];
            ChangePosSize(next_size, next_distance);
            createUI.InitValue(1.0f, 1.0f * next_size/next_distance);
            patternList.RemoveAt(index);
        } else {
            print("finish");
            Finish();
        }
    }

    public void Finish () {
        userInput.GetComponent<Renderer>().enabled = false;
        Application.Quit();
    }

    public float[] ReturnValue () {
        float[] values = { next_size, next_distance };
        return values;
    }

    private void ChangePosSize(float size, float distance) {
        transform.position = sitPosition.transform.position + new Vector3(0, size/2, -(distance+size/2));
        transform.localScale = new Vector3(size, size, size);
    }

    private void CreatePattern() {
        for (int i = 0; i < list.Length; i++){
            for (int j = 0; j < rateList.Length; j++)
            {
                float[] temp = { list[i], rateList[j] * list[i] };
                patternList.Add(temp);
            }
        }
        List<float[]> finishedList = saveFile.ReadFile("/Data/SizeAndDistance/saveData.csv");
        for (int i = 0; i < finishedList.Count; i++) {
            for (int j = 0; j < patternList.Count; j++) {
                if(patternList[j][0] == finishedList[i][0] && patternList[j][1] == finishedList[i][1]) {
                    patternList.RemoveAt(j);
                    break;
                }
            }
        }
    }
}
