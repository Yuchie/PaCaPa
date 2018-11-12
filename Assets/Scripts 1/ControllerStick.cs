using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStick : MonoBehaviour {

    public CalcStick calcStick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        calcStick.Follow();
	}

    void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Stage") {
            Debug.Log("hit");
        }
    }

    void OnTriggerExit(Collider other)
    {

    }
}
