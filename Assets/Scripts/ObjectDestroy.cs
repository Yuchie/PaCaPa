using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Soft" || other.gameObject.tag == "Hard")
        {
            Destroy(other.gameObject);
        }		
	}
	
}
