using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcGodStick : MonoBehaviour {

    public GameObject controller;
    public GameObject god;
    public GameObject pivot;
    public GameObject debugSphere;
    public GameObject hitTarget;

    private bool hit;
    private bool controllerHit;
    private int sidesStatus;
    private Ray axisRay;
    private float axisLength;
    private Plane surface;
    private Vector3 attachPoint;
    private Plane plane;
    private float lengthToPivot;
    private float lengthToAttachPoint;

    // Use this for initialization
    void Start () {
        sidesStatus = 0;
        hit = false;
        lengthToPivot = (pivot.transform.position - controller.transform.position).magnitude;
	}
	
	// Update is called once per frame
	public int UpdateGodStick () {
        int degree = 0;
        if (sidesStatus == 0) {
            Follow();
        } else {
            degree = CalcGodStatus();
        }

        return degree;
	}

    public void Follow () {
        god.transform.position = controller.transform.position;
        god.transform.rotation = controller.transform.rotation;
    }

    public void ChangeStatus (int sides)
    {
        if (sidesStatus == 0)
        {
            sidesStatus = sides;
            Vector3 origin = Vector3.zero;
            Vector3 direction = Vector3.zero;

            switch (sidesStatus)
            {
                case 1:
                    origin = hitTarget.transform.position + (-0.5f) * hitTarget.transform.up * hitTarget.transform.localScale.y + (-0.5f) * hitTarget.transform.forward * hitTarget.transform.localScale.z + 0.5f * hitTarget.transform.right * hitTarget.transform.localScale.x;
                    direction = hitTarget.transform.up;
                    surface.SetNormalAndPosition(-hitTarget.transform.forward, hitTarget.transform.position + (-0.5f) * hitTarget.transform.forward * hitTarget.transform.localScale.z);
                    axisLength = 1.0f * hitTarget.transform.localScale.y;
                    break;
                case 2:
                    origin = hitTarget.transform.position + 0.5f * hitTarget.transform.up * hitTarget.transform.localScale.y + (-0.5f) * hitTarget.transform.forward * hitTarget.transform.localScale.z + 0.5f * hitTarget.transform.right * hitTarget.transform.localScale.x;
                    direction = hitTarget.transform.forward;
                    surface.SetNormalAndPosition(hitTarget.transform.up, hitTarget.transform.position + (0.5f) * hitTarget.transform.up * hitTarget.transform.localScale.y);
                    axisLength = 1.0f * hitTarget.transform.localScale.z;
                    break;
                case 3:
                    origin = hitTarget.transform.position + (-0.5f) * hitTarget.transform.up * hitTarget.transform.localScale.y + 0.5f * hitTarget.transform.forward * hitTarget.transform.localScale.z + 0.5f * hitTarget.transform.right * hitTarget.transform.localScale.x;
                    direction = hitTarget.transform.up;
                    surface.SetNormalAndPosition(hitTarget.transform.forward, hitTarget.transform.position + (0.5f) * hitTarget.transform.forward * hitTarget.transform.localScale.z);
                    axisLength = 1.0f * hitTarget.transform.localScale.y;
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            axisRay = new Ray(origin, direction);
        }

    }

    public void ChangeTarget (GameObject target) {
        hitTarget = target;
    }

    public void InitAttachPoint() {
        Ray ray = new Ray(pivot.transform.position, controller.transform.forward);
        float distance;
        if (surface.Raycast(ray, out distance)) {
            attachPoint = ray.origin + ray.direction * distance;
            attachPoint += 0.015f * surface.normal;
            lengthToAttachPoint = (god.transform.position - attachPoint).magnitude;
            debugSphere.transform.position = attachPoint;
        }
    }

    public bool GetHit() {
        return hit;
    }

    public void SetHit(bool value) {
        hit = value;
    }

    public void SetControllerHit(bool value) {
        controllerHit = value;
    }

    public int CalcGodStatus()
    {
        int degree = 0;
        bool calculated = false;

        Vector3 _attachPoint = controller.transform.position + controller.transform.forward * lengthToAttachPoint;
        _attachPoint -= 0.015f * surface.normal;
        Ray attachPointRay = new Ray(attachPoint, _attachPoint - attachPoint);
        float attachDistance;
        Vector3 attachIntersect = attachPoint;

        if(surface.Raycast(attachPointRay, out attachDistance)) {
            attachIntersect = attachPointRay.origin + attachPointRay.direction * attachDistance;
            Ray godStickRay = new Ray(pivot.transform.position, attachIntersect - pivot.transform.position);
            float godStickDistance;
            if (surface.Raycast(godStickRay, out godStickDistance)) {
                if (godStickDistance >= lengthToPivot + god.transform.lossyScale.z / 2.0f + 0.015f) {
                    god.transform.forward = godStickRay.direction;
                    god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
                    Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                    float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                    degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

                    // calculated = true;
                }
            }
        } else {
            if (!controllerHit) {
                hit = false;
                sidesStatus = 0;
                Follow();
                calculated = true;
            }
        }

        _attachPoint += 0.0f * surface.normal;
        if (!calculated) {
            plane = new Plane();
            Vector3 point1 = attachIntersect;
            Vector3 point2 = pivot.transform.position;
            Vector3 point3 = controller.transform.position;

            plane.Set3Points(point1, point2, point3);

            float distance;

            if (plane.Raycast(axisRay, out distance)) {
                if (distance <= axisLength) {
                    attachPoint = axisRay.origin + axisRay.direction * distance;
                    debugSphere.transform.position = attachPoint;

                    attachPoint += surface.normal * 0.015f;
                    god.transform.forward = attachPoint - pivot.transform.position;
                    god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;

                    Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                    float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                    degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

                } else {
                    hit = false;
                    sidesStatus = 0;
                    Follow();
                }
            }
        }

        return degree;

        /*plane = new Plane();
        plane.SetNormalAndPosition(controller.transform.right, controller.transform.position);
        // debugPlane.transform.up = controller.transform.right;
        // debugPlane.transform.position = controller.transform.position;

        float distance;
        if (plane.Raycast(axisRay, out distance))
        {
            if (distance <= 1)
            {
                attachPoint = axisRay.origin + axisRay.direction * distance;
                debugSphere.transform.position = attachPoint;

                attachPoint -= controller.transform.up * 0.01f;
                god.transform.forward = attachPoint - pivot.transform.position;
                god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;

                Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);
                Vector3 dif = cross / cross.magnitude - controller.transform.right;

                if (dif.magnitude <= 0.01f)
                {
                    hit = false;
                    if (degree >= 10)
                    {
                        sidesStatus = 0;
                    }
                }
                else
                {
                    hit = true;
                }

            }
        }
        else
        {
            hit = false;
            sidesStatus = 0;
        }

        if (!hit || degree > 180 || degree < 0)
        {
            Follow();
            degree = 0;
        }*/
    }

}
