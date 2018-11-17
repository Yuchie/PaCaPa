using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHitSoundGenerate : MonoBehaviour {
    public unity_cutter unity_Cutter;
    private bool diditSound;
    public GameObject hitSound;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "target" && !diditSound && unity_Cutter.isBack)
        {
            Instantiate(hitSound);
            diditSound = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "target")
        {
            diditSound = false;
        }
    }
}
