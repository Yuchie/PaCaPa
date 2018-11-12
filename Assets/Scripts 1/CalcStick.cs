using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcStick : MonoBehaviour
{

    public GameObject controller;
    public GameObject god;
    public GameObject pivot;
    public GameObject debugSphere;
    public GameObject hitTarget;

    private bool hit;
    private bool controllerHit;
    private Vector3 attachPoint;
    private int sidesStatus;
    private Ray axisRay;
    private Plane surface;
    private Plane plane;
    private float lengthToPivot;
    private float lengthToAttachPoint;

    // Use this for initialization
    void Start()
    {
        sidesStatus = 0;
        lengthToPivot = (pivot.transform.position - controller.transform.position).magnitude;
        hit = false;
        controllerHit = false;
    }

    // Update is called once per frame
    public int UpdateGodStick()
    {
        int degree = 0;
        if (sidesStatus == 0)
        {
            Follow();
        }
        else
        {
            degree = CalcGodStatus();
        }

        return degree;
    }

    public void Follow()
    {
        god.transform.position = controller.transform.position;
        god.transform.rotation = controller.transform.rotation;
    }

    public void ChangeStatus(int sides)
    {
        sidesStatus = sides;
        Vector3 origin = Vector3.zero;
        Vector3 direction = Vector3.zero;

        switch (sidesStatus)
        {
            case 1:
                origin = hitTarget.transform.position + (-0.5f) * hitTarget.transform.up + (-0.5f) * hitTarget.transform.forward + 0.5f * hitTarget.transform.right;
                direction = hitTarget.transform.up;
                surface.SetNormalAndPosition(-hitTarget.transform.forward, hitTarget.transform.position + (-0.5f) * hitTarget.transform.forward);
                break;
            case 2:
                origin = hitTarget.transform.position + 0.5f * hitTarget.transform.up + (-0.5f) * hitTarget.transform.forward + 0.5f * hitTarget.transform.right;
                direction = hitTarget.transform.forward;
                surface.SetNormalAndPosition(hitTarget.transform.up, hitTarget.transform.position + (0.5f) * hitTarget.transform.up);
                break;
            case 3:
                origin = hitTarget.transform.position + (-0.5f) * hitTarget.transform.up + 0.5f * hitTarget.transform.forward + 0.5f * hitTarget.transform.right;
                direction = hitTarget.transform.up;
                surface.SetNormalAndPosition(hitTarget.transform.forward, hitTarget.transform.position + (0.5f) * hitTarget.transform.forward);
                break;
            default:
                Debug.Log("Error");
                break;
        }
        axisRay = new Ray(origin, direction);

    }

    public void ChangeTarget(GameObject target)
    {
        hitTarget = target;
    }

    public void InitAttachPoint() {
        Ray ray = new Ray(pivot.transform.position, controller.transform.forward);
        float distance;
        if(surface.Raycast(ray, out distance)) {
            attachPoint = ray.origin + ray.direction * distance;
            attachPoint += 0.05f * surface.normal;
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

    public int CalcGodPoint(Vector3 _attachPoint) {
        int degree = 0;

        attachPoint = _attachPoint;
        god.transform.forward = attachPoint - pivot.transform.position;
        god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;

        Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
        float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
        degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

        return degree;
    }

    public int CalcGodStatus()
    {
        int degree = 0;
        bool calculated = false;

        Vector3 _attachPoint = controller.transform.position + controller.transform.forward * lengthToAttachPoint;
        _attachPoint -= 0.05f * surface.normal;
        Ray attachPointRay = new Ray(attachPoint, _attachPoint - attachPoint);
        float attachDistance;
        Vector3 attachIntersect = attachPoint;
        if (surface.Raycast(attachPointRay, out attachDistance)) {
            attachIntersect = attachPointRay.origin + attachPointRay.direction * attachDistance;
            Ray godStickRay = new Ray(pivot.transform.position, attachIntersect - pivot.transform.position);
            float godStickDistance;
            if (surface.Raycast(godStickRay, out godStickDistance)) {
                if (godStickDistance >= lengthToPivot + god.transform.lossyScale.z / 2.0f + 0.05f) {
                    god.transform.forward = godStickRay.direction;
                    god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
                    Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                    // calculate degree
                    float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                    degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);
                    calculated = true;
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

        if (!calculated) {
            
            plane = new Plane();
            Vector3 point1 = attachIntersect;
            Vector3 point2 = pivot.transform.position;
            Vector3 point3 = controller.transform.position;

            plane.Set3Points(point1, point2, point3);

            float distance;
            if (plane.Raycast(axisRay, out distance))
            {
                if (distance <= 1)
                {
                    attachPoint = axisRay.origin + axisRay.direction * distance;
                    debugSphere.transform.position = attachPoint;

                    attachPoint -= Vector3.down * 0.05f;
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
    }

}
