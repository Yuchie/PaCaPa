using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StiffnessRandom : MonoBehaviour {

    public SaveFile saveFile;
    public CalcGodStickMeshDeform calcGodStickMeshDeform;

    private float next_sight;
    private float next_pressure;
    private float[] sightList = { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };
    private float[] pressureList = { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };
    private List<float[]> patternList = new List<float[]>();

    // Use this for initialization
    void Start () {
        CreatePattern();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartTest()
    {
        if (patternList.Count > 0)
        {
            int index = Random.Range(0, patternList.Count);

            ChangeParameter(patternList[index][0], patternList[index][1]);
            patternList.RemoveAt(index);
        }
        else
        {
            print("finish");
            Finish();
        }
    }

    public float[] ReturnValue()
    {
        float[] values = { next_sight, next_pressure };
        return values;
    }

    private void ChangeParameter(float sight, float pressure)
    {

        calcGodStickMeshDeform.ChangeDeform(sight);
        calcGodStickMeshDeform.ChangePressure(pressure);

        next_sight = sight;
        next_pressure = pressure;
    }

    private void Finish()
    {
        GetComponent<Renderer>().enabled = false;
        Application.Quit();
    }

    private void CreatePattern()
    {
        for (int i = 0; i < sightList.Length; i++)
        {
            for (int j = 0; j < pressureList.Length ; j++)
            {
                float[] temp = { sightList[i], pressureList[j] };
                patternList.Add(temp);
            }
        }

        List<float[]> finishedList = saveFile.ReadFile("/Data/Stiffness/saveData.csv");
        for (int i = 0; i < finishedList.Count; i++)
        {
            for (int j = 0; j < patternList.Count; j++)
            {
                if (patternList[j][0] == finishedList[i][0] && patternList[j][1] == finishedList[i][1])
                {
                    patternList.RemoveAt(j);
                    break;
                }
            }
        }
        print(patternList.Count);
    }
}
