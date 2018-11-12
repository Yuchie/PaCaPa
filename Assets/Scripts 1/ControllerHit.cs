using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHit : MonoBehaviour {

    public CalcGod calcGod;

    private bool trigger = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        calcGod.Calc();
	}

    void OnTriggerEnter(Collider other)
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {

    }
}
