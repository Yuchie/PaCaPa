using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.y >= 0)
        {
            // Move the object forward along its z axis 1 unit/second.
            transform.Translate(Vector3.left * Time.deltaTime);
        }
	}
}
