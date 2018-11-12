using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcGod : MonoBehaviour {

    public GameObject god;
    public GameObject controller;
    public GameObject debug;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {



	}

    public void FollowPos() {
        god.transform.position = controller.transform.position;
    }

    public void Calc() {
        Vector3 heading = controller.transform.position - god.transform.position;
        float distance = heading.magnitude;

        Ray ray = new Ray(god.transform.position, heading);
        RaycastHit hit;
        bool same = true;
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag != "Stage")
            {
                same = false;
                Vector3 intersect = ray.origin + hit.distance * ray.direction;
                debug.transform.position = intersect;

                Vector3 normal = hit.normal;
                float A = normal.x;
                float B = normal.y;
                float C = normal.z;
                float D = Vector3.Dot(intersect, normal);
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, A));
                matrix.SetRow(1, new Vector4(0.0f, 1.0f, 0.0f, B));
                matrix.SetRow(2, new Vector4(0.0f, 0.0f, 1.0f, C));
                matrix.SetRow(3, new Vector4(A, B, C, 0.0f));
                Matrix4x4 inverse = matrix.inverse;
                Vector4 vector4 = new Vector4(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z, D);
                Vector3 position = inverse * vector4;
                god.transform.position = position - heading * 0.01f;
            }
            else
            {
                same = true;
            }
            //Debug.Log("hit");
        }
        if (same)
        {
            god.transform.position = controller.transform.position;
        }
    }
}
