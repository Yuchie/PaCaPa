using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlane : MonoBehaviour {

    private Plane plane;

    private Vector3 normal;
    private Vector3 position;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Create(GameObject victim) {
        plane = new Plane();

        normal = victim.transform.InverseTransformDirection(transform.up);
        position = victim.transform.InverseTransformPoint(transform.position);
        plane.SetNormalAndPosition(normal, position);
    }

    public Plane GetPlane() {
        return plane;
    }

}
