using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickStatus : MonoBehaviour {

    public CalcGodStickMesh calcGodStickMesh;
	private bool hit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag != "GodStick" && collider.gameObject.tag != "Stage") {
			hit = true;
            calcGodStickMesh.ChangeHit();
		}
	}

	void OnTriggerStay(Collider collider)
	{
		if (collider.gameObject.tag != "GodStick" && collider.gameObject.tag != "Stage") {
			hit = true;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag != "GodStick" && collider.gameObject.tag != "Stage") {
			hit = false;
		}
	}

	public bool GetControllerHit() {
		return hit;
	}
}
