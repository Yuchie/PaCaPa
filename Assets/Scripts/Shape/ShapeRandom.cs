using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRandom : MonoBehaviour {

    public GameObject stageStand;
    public GameObject godStick;
    public GameObject cube;
    public GameObject circle;
    public GameObject triangle;
    public GameObject currentShape;
    public SaveFile saveFile;

    private float[] shapeList = { 1, 2, 3 };
    private float[,] heightWidthList = new float[3, 2] { { 1.0f, 1.0f }, { 1.25f, 0.75f }, { 0.75f, 1.25f } };
    private List<float[]> patternList = new List<float[]>();
    private string next_shape;
    private float next_shape_num;
    private float next_height;
    private float next_width;

    // Use this for initialization
    void Start () {
        CreatePattern();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void StartTest()
    {
        godStick.GetComponent<Renderer>().enabled = false;
        // currentShape.GetComponent<Renderer>().enabled = false;

        if (patternList.Count > 0)
        {
            int index = Random.Range(0, patternList.Count);

            ChangeShapeScale(patternList[index][0], patternList[index][1], patternList[index][2]);
            patternList.RemoveAt(index);
        }
        else
        {
            print("finish");
            Finish();
        }
    }

    public string[] ReturnValue()
    {
        string[] values = { next_shape_num.ToString(), next_height.ToString("F2"), next_width.ToString("F2"), next_shape };
        return values;
    }

    private void Finish()
    {
        Application.Quit();
    }

    private void ChangeShapeScale(float shape, float height, float width)
    {
        Destroy(currentShape);

        GameObject prefab;
        switch(Mathf.RoundToInt(shape))
        {
            case 1:
                currentShape = Instantiate(cube, stageStand.transform.position + new Vector3(0, 0.8f, 0), stageStand.transform.rotation) as GameObject;
                next_shape = "cube";
                break;
            case 2:
                currentShape = Instantiate(circle, stageStand.transform.position + new Vector3(0, 0.8f, 0), stageStand.transform.rotation) as GameObject;
                next_shape = "circle";
                break;
            case 3:
                currentShape = Instantiate(triangle, stageStand.transform.position + new Vector3(0, 0.8f, 0), stageStand.transform.rotation) as GameObject;
                next_shape = "triangle";
                break;
            default:
                print("error");
                break;
        }
        currentShape.transform.localScale = new Vector3(currentShape.transform.localScale.x * width, currentShape.transform.localScale.y * height, currentShape.transform.localScale.z) * 0.8f;
        next_shape_num = shape;
        next_height = height;
        next_width = width;

    }

    private void CreatePattern ()
    {

        for (int i = 0; i < shapeList.Length; i++)
        {
            for (int j = 0; j < heightWidthList.Length/2; j++)
            {
                float[] temp = { shapeList[i], heightWidthList[j, 0], heightWidthList[j, 1] };
                patternList.Add(temp);
            }
        }

        List<float[]> finishedList = saveFile.ReadFile("/Data/Shape/saveData.csv");
        for (int i = 0; i < finishedList.Count; i++)
        {
            for (int j = 0; j < patternList.Count; j++)
            {
                if (patternList[j][0] == finishedList[i][0] && patternList[j][1] == finishedList[i][1] && patternList[j][2] == finishedList[i][2])
                {
                    patternList.RemoveAt(j);
                    break;
                }
            }
        }
    }
}
