using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour {

    public GameObject startVertice;
    public GameObject endVertice;
    public GameObject middleVertice;
    LineRenderer line;
    private float interval;
    private Vector3 center;
    private float r;

    // Use this for initialization
    void Start () {

        line = GetComponent<LineRenderer>();
        line.positionCount = 21;

    }
	
	// Update is called once per frame
	void Update () {

        float a = middleVertice.transform.position.y - startVertice.transform.position.y;
        float b = startVertice.transform.position.x - middleVertice.transform.position.x;

        if (a == 0)
        {
            r = 0.0f;
        }
        else
        {
            r = (a * a + b * b) / (2 * a);
        }

        interval = (startVertice.transform.position.x - endVertice.transform.position.x) / (line.positionCount - 1);

        center = middleVertice.transform.position - new Vector3(0, r, 0);

        for (int i=0; i < line.positionCount; i++) {

            Vector3 linePosition = ReturnPoint(startVertice.transform.position.x - i * interval);

            line.SetPosition(i, linePosition);
            
        }

	}

    public Vector3 ReturnPoint(float x)
    {

        Vector3 linePosition = center;
        linePosition.x = x;

        if (r > 0)
        {
            float addY = Mathf.Sqrt(r * r - (middleVertice.transform.position.x - linePosition.x) * (middleVertice.transform.position.x - linePosition.x));
            linePosition += new Vector3(0, addY, 0);
        }
        else if (r < 0)
        {
            float addY = Mathf.Sqrt(r * r - (middleVertice.transform.position.x - linePosition.x) * (middleVertice.transform.position.x - linePosition.x));
            linePosition -= new Vector3(0, addY, 0);
        }

        return linePosition;
    }

}
