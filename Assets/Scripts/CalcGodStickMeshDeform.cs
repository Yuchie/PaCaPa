using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcGodStickMeshDeform : MonoBehaviour
{

    public GameObject controller;
    public GameObject hammerHead;
    public GameObject god;
    public GameObject god2;
    public GameObject godHammerHead;
    public GameObject pivot;
    public GameObject debugSphere;
    public StickStatusStiffness stickStatus;
    public Serial serial;
    public MeshDeformer meshDeformer;
    public bool delay;
    public bool hammer;

    [Range(0, 1)] public float stiffness;
    [Range(0, 1)] public float deform;

    private GameObject target;
    private Vector3 attachPoint;
    private float lengthToPivot;
    private float lengthToAttachPoint;
    public bool hit;
    public bool cut;
    private Vector3[] record;
    private int dataNum;
    private int start;
    private bool startPlay;
    private float delayTime = 0.5f;

    public int degree = 0;

    // Use this for initialization
    void Start()
    {
        lengthToPivot = (pivot.transform.position - controller.transform.position).magnitude;
        hit = false;
        cut = false;
        stiffness = 1;
        deform = 0;

        dataNum = (int)(delayTime * 30 / 0.5f);
        record = new Vector3[dataNum];
        startPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGodStick();
    }

    public void UpdateGodStick()
    {
        if (hit && !cut)
        {
            //int next_degree = (int)(CalcBoxCast() * stiffness);
            int next_degree = CalcBoxCast();

            // if degree is larger than a value, the stiffness becomes 1
            if (stiffness < 1)
            {
                if (next_degree * stiffness >= 5f)
                {
                    next_degree = 5 + (next_degree - (int)(5f / stiffness));
                } else
                {
                    next_degree = (int)(next_degree * stiffness);
                }
            }

            if (next_degree != degree && serial.CheckFree())
            {
                serial.WriteToArduino(next_degree.ToString());
                degree = next_degree;
            }
        }
        else if(cut)
        {
            if (delay)
            {
                int next_degree = (int)(DelayFollow() * stiffness);
                if (next_degree != degree && serial.CheckFree())
                {
                    serial.WriteToArduino(next_degree.ToString());
                    degree = next_degree;
                }
            } else
            {
                Follow();
            }
        } else
        {
            Follow();
        }

    }

    public void ChangePressure(float next_pressure)
    {
        stiffness = next_pressure;
    }

    public void ChangeDeform(float next_deform)
    {
        deform = next_deform;
    }

    private int DelayFollow()
    {
        int degree = 0;

        if (start == -1)
        {
            start++;
            startPlay = false;
        }
        record[start] = controller.transform.position;
        start++;
        if (start == dataNum)
        {
            startPlay = true;
            start = 0;
        }
        if (startPlay)
        {
            attachPoint = record[start];

        }

        lengthToAttachPoint = (god.transform.position - attachPoint).magnitude;
        Vector3 _attachPoint = controller.transform.position + controller.transform.forward * lengthToAttachPoint;
        Vector3 direction = _attachPoint - attachPoint;
        meshDeformer.AddDeformingForce(attachPoint - direction * 0.2f, direction.magnitude * deform);

        Vector3 crossUp = Vector3.Cross(new Vector3(0, 1, 0), controller.transform.up);
        float cos_sitaUp = Vector3.Dot(crossUp, controller.transform.forward);
        float rotationDegree = Vector3.Angle(new Vector3(0, 1, 0), controller.transform.up) * (cos_sitaUp < 0 ? -1 : 1);

        god.transform.forward = attachPoint - pivot.transform.position;
        god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
        god.transform.Rotate(0, 0, rotationDegree);

        debugSphere.transform.position = attachPoint;
        _attachPoint = attachPoint + 0.1f * deform * direction;

        god2.transform.forward = _attachPoint - pivot.transform.position;
        god2.transform.position = pivot.transform.position + god2.transform.forward * lengthToPivot;
        god2.transform.Rotate(0, 0, rotationDegree);

        Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
        float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
        degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

        // need to correct so that there's no degree when cut is just started
        /*if (!startPlay)
        {
            degree = 0;
        }*/

        return degree;
    }

    private void Follow()
    {
        god.transform.position = controller.transform.position;
        god.transform.rotation = controller.transform.rotation;
        god2.transform.position = controller.transform.position;
        god2.transform.rotation = controller.transform.rotation;
    }

    public void StartFollow()
    {
        hit = false;
        cut = false;
        Follow();
        serial.WriteToArduino("0");
        degree = 0;
        serial.ChangeFree();
    }

    private int CalcBoxCast()
    {
        int degree = 0;

        if (hammer)
        {
            RaycastHit hitRayTarget;
            Vector3 heading = hammerHead.transform.position - godHammerHead.transform.position;
            float distance = heading.magnitude;

            if (Physics.SphereCast(godHammerHead.transform.position - 2 * (heading / distance) * godHammerHead.transform.lossyScale.x, godHammerHead.transform.lossyScale.x * 1.5f, heading, out hitRayTarget))
            {
                if (hitRayTarget.distance < distance - godHammerHead.transform.lossyScale.y)
                {
                    attachPoint = godHammerHead.transform.position;
                    target = hitRayTarget.collider.gameObject;

                    lengthToAttachPoint = (god.transform.position - attachPoint).magnitude;
                    Vector3 _attachPoint = controller.transform.position + controller.transform.forward * lengthToAttachPoint;
                    Vector3 direction = _attachPoint - attachPoint;

                    Vector3 compare = controller.transform.up;
                    compare = Vector3.ProjectOnPlane(compare, controller.transform.forward);
                    Vector3 crossUp = Vector3.Cross(new Vector3(0, 1, 0), compare);
                    float cos_sitaUp = Vector3.Dot(crossUp, controller.transform.forward);
                    float rotationDegree = Vector3.Angle(new Vector3(0, 1, 0), compare) * (cos_sitaUp < 0 ? -1 : 1);

                    god.transform.forward = attachPoint - pivot.transform.position;
                    god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
                    god.transform.Rotate(0, 0, rotationDegree);

                    debugSphere.transform.position = attachPoint;
                    _attachPoint = attachPoint + 0.1f * deform * direction;

                    god2.transform.forward = attachPoint - pivot.transform.position;
                    god2.transform.position = pivot.transform.position + god2.transform.forward * lengthToPivot;
                    god2.transform.Rotate(0, 0, rotationDegree);

                    attachPoint = hitRayTarget.point;
                    meshDeformer.AddDeformingForce(attachPoint - direction * 0.2f, direction.magnitude * deform);

                    Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                    float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                    degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

                }
                else
                {
                    if (stickStatus.GetControllerHit())
                    {
                        // StartFollow();
                        attachPoint = godHammerHead.transform.position;

                        lengthToAttachPoint = (god.transform.position - attachPoint).magnitude;
                        Vector3 _attachPoint = controller.transform.position + controller.transform.forward * lengthToAttachPoint;
                        Vector3 direction = _attachPoint - attachPoint;

                        Vector3 compare = controller.transform.up;
                        compare = Vector3.ProjectOnPlane(compare, controller.transform.forward);
                        Vector3 crossUp = Vector3.Cross(new Vector3(0, 1, 0), compare);
                        float cos_sitaUp = Vector3.Dot(crossUp, controller.transform.forward);
                        float rotationDegree = Vector3.Angle(new Vector3(0, 1, 0), compare) * (cos_sitaUp < 0 ? -1 : 1);

                        god.transform.forward = attachPoint - pivot.transform.position;
                        god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
                        god.transform.Rotate(0, 0, rotationDegree);

                        debugSphere.transform.position = attachPoint;
                        _attachPoint = attachPoint + 0.1f * deform * direction;

                        god2.transform.forward = attachPoint - pivot.transform.position;
                        god2.transform.position = pivot.transform.position + god2.transform.forward * lengthToPivot;
                        god2.transform.Rotate(0, 0, rotationDegree);

                        meshDeformer.AddDeformingForce(attachPoint + godHammerHead.transform.up * godHammerHead.transform.lossyScale.y / 2, direction.magnitude * deform);

                        Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                        float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                        degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);
                    }
                    else
                    {
                        StartFollow();
                    }
                }
            }
            else
            {
                if (stickStatus.GetControllerHit())
                {

                }
                else
                {
                    StartFollow();
                }
            }
        } else
        {
            RaycastHit hitRayTarget;
            Vector3 heading = controller.transform.position - god.transform.position;
            float distance = heading.magnitude;

            if (Physics.BoxCast(god.transform.position - 4 * (heading / distance) * god.transform.lossyScale.x, new Vector3(0, 0, 1), heading, out hitRayTarget, god.transform.rotation))
            {
                if (hitRayTarget.distance < distance * (1 + 4 * god.transform.lossyScale.x))
                {
                    attachPoint = hitRayTarget.point + hitRayTarget.normal * god.transform.lossyScale.x / 2;
                    target = hitRayTarget.collider.gameObject;

                    lengthToAttachPoint = (god.transform.position - attachPoint).magnitude;
                    Vector3 _attachPoint = controller.transform.position + controller.transform.forward * lengthToAttachPoint;
                    Vector3 direction = _attachPoint - attachPoint;
                    meshDeformer.AddDeformingForce(attachPoint - direction * 0.2f, direction.magnitude * deform);

                    Vector3 crossUp = Vector3.Cross(new Vector3(0, 1, 0), controller.transform.up);
                    float cos_sitaUp = Vector3.Dot(crossUp, controller.transform.forward);
                    float rotationDegree = Vector3.Angle(new Vector3(0, 1, 0), controller.transform.up) * (cos_sitaUp < 0 ? -1 : 1);

                    god.transform.forward = attachPoint - pivot.transform.position;
                    god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
                    god.transform.Rotate(0, 0, rotationDegree);

                    debugSphere.transform.position = attachPoint;
                    _attachPoint = attachPoint + 0.1f * deform * direction;

                    god2.transform.forward = _attachPoint - pivot.transform.position;
                    god2.transform.position = pivot.transform.position + god2.transform.forward * lengthToPivot;
                    god2.transform.Rotate(0, 0, rotationDegree);

                    Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                    float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                    degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

                }
                else
                {
                    if (stickStatus.GetControllerHit())
                    {

                    }
                    else
                    {
                        StartFollow();
                    }
                }
            }
            else
            {
                if (stickStatus.GetControllerHit())
                {

                }
                else
                {
                    StartFollow();
                }
            }
        }

        return degree;
    }

    public void ChangeHit(bool isHit)
    {
        hit = isHit;
    }

    public void ChangeCut(bool isCut)
    {
        if (!cut && cut == isCut)
        {
            start = -1;
            hit = false;
        }
        cut = isCut;
    }

}
