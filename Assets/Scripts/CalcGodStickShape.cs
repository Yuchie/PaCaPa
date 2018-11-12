using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcGodStickShape : MonoBehaviour {

    public Serial serial;
    public GameObject controller;
    public GameObject god;
    public GameObject pivot;
    public GameObject debugSphere;
    public GameObject hitTargetStart;
    public GameObject hitTargetEnd;
    public GameObject hitTargetMiddle;
    public CreateLine createLine;

    private bool hit;
    private bool controllerHit;
    private int sidesStatus;
    private Ray axisRay;
    private float axisLength;
    private Plane surface;
    private Vector3 attachPoint;
    private float lengthToPivot;
    private float lengthToAttachPoint;

    private int degree = 0;
    private int next_degree;

    // Use this for initialization
    void Start () {

        hit = false;
        lengthToPivot = (pivot.transform.position - controller.transform.position).magnitude;
        surface.SetNormalAndPosition(-hitTargetMiddle.transform.forward, hitTargetMiddle.transform.position);

    }

    public void Follow()
    {
        god.transform.position = controller.transform.position;
        god.transform.rotation = controller.transform.rotation;
    }

    // Update is called once per frame
    void Update () {

        next_degree = CalcGodStatus();

        if (!hit)
        {
            Follow();
            serial.WriteToArduino("0");
            serial.ChangeFree();
            degree = 0;

        } else
        {
            if (serial.CheckFree() && (degree != next_degree))
            {
                serial.WriteToArduino(next_degree.ToString());
                degree = next_degree;
            }
        }

	}

    private int CalcGodStatus()
    {
        hit = false;
        int degree = 0;

        Ray controllerStickRay = new Ray(pivot.transform.position, controller.transform.forward);
        float controllerStickDistance;
        if (surface.Raycast(controllerStickRay, out controllerStickDistance))
        {
            Vector3 Intersect = controllerStickRay.origin + controllerStickRay.direction * controllerStickDistance;

            debugSphere.transform.position = Intersect;

            if (controllerStickDistance <= lengthToPivot + controller.transform.lossyScale.z / 2.0f)
            {

                if (Intersect.x >= hitTargetEnd.transform.position.x && Intersect.x <= hitTargetStart.transform.position.x)
                {
                    attachPoint = createLine.ReturnPoint(Intersect.x);

                    if(Intersect.y <= attachPoint.y)
                    {
                        hit = true;
                        god.transform.forward = attachPoint - pivot.transform.position;
                        god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;

                        Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                        float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                        degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

                    }
                }
            }
        }

        return degree;

    }
}
