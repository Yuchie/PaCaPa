using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcGodStickMesh : MonoBehaviour {

	public GameObject controller;
	public GameObject god;
	public GameObject pivot;
	public GameObject debugSphere;
	public StickStatus stickStatus;
    public Serial serial;

    [Range(0, 1)] public float stiffness;

    private GameObject target;
	private Vector3 attachPoint;
	private float lengthToPivot;
	private float lengthToAttachPoint;
	private bool hit;

    private int degree = 0;

	// Use this for initialization
	void Start () {
		lengthToPivot = (pivot.transform.position - controller.transform.position).magnitude;
		hit = false;
        stiffness = 1;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateGodStick();
	}

	public void UpdateGodStick () {
        if (hit)
        {
            int next_degree = (int)(CalcBoxCast() * stiffness);
            if (next_degree != degree && serial.CheckFree())
            {
                serial.WriteToArduino(next_degree.ToString());
                degree = next_degree;
            }
        } else
        {
            Follow();
        }
       
	}

    public void ChangeStiffness(float next_stiffness)
    {
        stiffness = next_stiffness;
    }

	private void Follow()
	{
		god.transform.position = controller.transform.position;
		god.transform.rotation = controller.transform.rotation;
	}

	private void StartFollow() {
		hit = false;
        serial.WriteToArduino("0");
        degree = 0;
        serial.ChangeFree();
    }

    private int CalcBoxCast()
    {
        int degree = 0;

        RaycastHit hitRayTarget;
        Vector3 heading = controller.transform.position - god.transform.position;
        float distance = heading.magnitude;

        if(Physics.BoxCast(god.transform.position - (heading/distance) * god.transform.lossyScale.x, new Vector3(0, 0, 1), heading, out hitRayTarget, god.transform.rotation)) {
            if (hitRayTarget.distance < distance * (1 + god.transform.lossyScale.x))
            {
                attachPoint = hitRayTarget.point + hitRayTarget.normal * god.transform.lossyScale.x / 2;
                target = hitRayTarget.collider.gameObject;

                debugSphere.transform.position = attachPoint;
                god.transform.forward = attachPoint - pivot.transform.position;
                god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;

                Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
                float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
                degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);
            } else
            {
                if (stickStatus.GetControllerHit())
                {

                } else
                {
                    StartFollow();
                }
            }
        } else {
           if (stickStatus.GetControllerHit())
            {

            } else
            {
                StartFollow();
            }
        }

        return degree;
    }

    public void ChangeHit()
    {
        hit = true;
    }

	private int CalcGodStick() {
		int degree = 0;

		Vector3 _attachPoint = controller.transform.position + controller.transform.forward * lengthToAttachPoint;
		Vector3 heading = _attachPoint - attachPoint;
		float distance = heading.magnitude;

		Ray ray = new Ray(attachPoint, heading);
		RaycastHit hitRayTarget;
		bool same = true;

		if (Physics.Raycast (ray, out hitRayTarget, distance)) {
			if (hitRayTarget.collider.gameObject == target) {
				attachPoint = hitRayTarget.point - heading * 0.05f;
			} else {
				if (stickStatus.GetControllerHit ()) {

				} else {
					StartFollow ();
				}
			}
		} else {
			if (stickStatus.GetControllerHit ()) {
				
			} else {
				StartFollow ();
			}
		}

		Vector3 direction;

		god.transform.forward = attachPoint - pivot.transform.position;
		god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
		if (Physics.ComputePenetration(god.GetComponent<Collider>(), god.transform.position, god.transform.rotation, target.GetComponent<Collider>(), target.transform.position, target.transform.rotation, out direction, out distance)) {
			attachPoint += direction * distance;
		}
        
		god.transform.forward = attachPoint - pivot.transform.position;
		god.transform.position = pivot.transform.position + god.transform.forward * lengthToPivot;
        debugSphere.transform.position = attachPoint;

        Vector3 cross = Vector3.Cross(controller.transform.forward, god.transform.forward);
		float cos_sita = Vector3.Dot(controller.transform.forward, god.transform.forward);
		degree = (int)(Mathf.Acos(cos_sita) * 180 / Mathf.PI);

		return degree;
	}

}
