using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingBall : MonoBehaviour {

	// ball prefab
	public GameObject ball;

	private GameObject balls;

	// velocity of ball
	public float speed = 1;

	// target position
	public Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// if z key is pressed
		if (Input.GetKeyDown (KeyCode.Z)) {
			// delete ball
			if (balls != null) {
				GameObject.Destroy (balls);
			}

			// replicate ball
			balls = Instantiate (ball) as GameObject;

			Vector3 force;

			force = this.gameObject.transform.forward * speed;

			balls.GetComponent<Rigidbody> ().AddForce (force);

			//the position of ball
			balls.transform.position = target.position - new Vector3 (0, -0.3f, 3) + target.transform.up * 0.5f;
		} else if (Input.GetKeyDown (KeyCode.A)) {
			// if a key is pressed
			// delete ball
			if (balls != null) {
				GameObject.Destroy (balls);
			}

			// replicate ball
			balls = Instantiate (ball) as GameObject;

			//the position of ball
			balls.transform.position = target.position + target.transform.forward * 0.3f + target.transform.up * 0.5f ;
			balls.GetComponent<Rigidbody>().useGravity = false;
		}
	}
}
