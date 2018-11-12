using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaState : MonoBehaviour {
    private Vector3 vecA;
    private Vector3 vecB;
    private Vector3 vel;
    private Vector3 vel_temp;
    public unity_cutter cutter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        vecA = transform.position;
        vel_temp = (vecA - vecB) / Time.deltaTime;
        vecB = transform.position;
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "target")
        {
            Vector3 dir = transform.up;
            float angle = Vector3.Angle(vel, dir);
            if (angle < 20)
            {
                cutter.isBack = false;
            }
            else
            {
                cutter.isBack = true;
            }
            cutter.isCutting = true;
        }
 
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "target")
        {
            cutter.isCutting = false;
        }
        
    }
}
