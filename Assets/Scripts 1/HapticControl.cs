using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//God-Objectの挙動を制御します。
public class HapticControl : MonoBehaviour {
    public GameObject realLocation;
    public GameObject virtualLocation;
    public bool isDelay;//藁切の時はtrue、モグラたたきの時はfalse
    public float delaySec;
    private Vector3 posB;
    private Vector3 posA;
    private Transform transformReal;
    private Transform transformVirtual;
    public int currentState;
    private Vector3 normalVectorOfEnterPlane;
	// Use this for initialization
	void Start () {
        transformReal = realLocation.GetComponent<Transform>();
        transformVirtual = virtualLocation.GetComponent<Transform>();
	}

    // Update is called once per frame
    private Vector3 m_velocity;
    private Vector3 m_omega;

    private void Update()
    {
        if (isDelay)
        {
            var selfPosition = transformVirtual.position;
            var targetPosition = transformReal.position;
            var deltaPosition = transformReal.position - transformVirtual.position;
            var destinationPosition = transformVirtual.position + deltaPosition;
            transformVirtual.rotation = transformReal.rotation;
            transformVirtual.position = Vector3.SmoothDamp(selfPosition, destinationPosition, ref m_velocity, delaySec);
            var selfRotation = transformVirtual.rotation;
            var targetRotation = transformReal.rotation;
            var deltaRotation = transformReal.rotation * Quaternion.Inverse(transformVirtual.rotation);
            var destinationRotation = transformVirtual.rotation * deltaRotation;

            
        }

    }

    public void delayMove(float delay, Vector3 addVector, float deltaTime,GameObject target)
    {
        var hash = new Hashtable();
        hash.Add("position", addVector);
        hash.Add("time", deltaTime);
        hash.Add("delay", delay);
        iTween.MoveAdd(target, hash);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDelay)
        {
            if(other.gameObject.tag == "target")
            {
                delaySec = 0.1f;
            }
        }
        else
        {
            if (other.gameObject.tag == "stiff")
            {
                currentState = 1;
            }
            else if (other.gameObject.tag == "soft")
            {
                currentState = 2;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (isDelay)
        {
            if (other.gameObject.tag == "target")
            {
                delaySec =0f;
            }
        }
        else
        {
            if (other.gameObject.tag == "stiff")
            {
                currentState = 0;
            }
            else if (other.gameObject.tag == "soft")
            {
                currentState = 0;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        

    }
}
