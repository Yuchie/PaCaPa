using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodStickCollider : MonoBehaviour {

	private bool hit;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	/*void OnCollisionEnter (Collision collision) {
		int i = 0;
		Vector3 attachPoint = Vector3.zero;
		Vector3 normal = Vector3.zero;
		foreach (ContactPoint contactPoint in collision.contacts) {
			attachPoint += contactPoint.point;
			normal += contactPoint.normal;
			i++;
		}
		if (i > 0) {
			attachPoint = attachPoint / i;
			Vector3 direction;
			float distance;

			GameObject gameObject = collision.gameObject;
			if (Physics.ComputePenetration(GetComponent<Collider>(), transform.position, transform.rotation, gameObject.GetComponent<Collider>(), gameObject.transform.position, gameObject.transform.rotation, out direction, out distance)) {
				attachPoint += direction * distance;
				transform.position += direction * distance;
			}

			normal = normal / i;
			attachPoint = attachPoint + normal * 0.05f;

			calcGodStickMesh.InitAttachPoint (attachPoint, gameObject);
			collision.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		}

	}

	void OnCollisionStay (Collision collision) {
		
	}

	void OnCollisionExit (Collision collision) {
		
	}*/
		
}
