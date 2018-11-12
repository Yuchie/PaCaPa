using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class collisionScript : MonoBehaviour {

    SerialPort stream = new SerialPort("\\\\.\\COM13", 9600);

	public GameObject racketShell;
	public GameObject racket;
	// velocity 
	public float speed = 1;
	// collision animation
	private Animation collisionAnim;

	// Use this for initialization
    void Start () {
        stream.Open();
        stream.ReadTimeout = 1;
		collisionAnim = racketShell.gameObject.GetComponent<Animation> ();
    }
	
	// Update is called once per frame
    void Update () {
        string reply = null;
        try {
            reply = stream.ReadLine();
        } catch (System.Exception) {
            
        }

        if (reply != null) {
            Debug.Log(reply);
        }

	}

    //If your GameObject starts to collide with another GameObject with a Collider
    void OnCollisionEnter(Collision collision)
    {
        //Output the Collider's GameObject's name
        // Debug.Log(collision.collider.name);
	    WriteToArduino("1");

		if (collision.collider.GetComponent<Rigidbody> ().useGravity == false) {
			collision.collider.GetComponent<Rigidbody> ().useGravity = true;

			Vector3 force;

			force = racket.transform.forward * 1000;
			collision.collider.GetComponent<Rigidbody> ().AddForce (force);
		}

		//add force to racket
		// Vector3 force;
		// force = this.gameObject.transform.forward * speed;
		// racket.GetComponent<Rigidbody> ().AddForce (force);

		//collisionAnim.Play ();

    }

    //If your GameObject keeps colliding with another GameObject with a Collider, do something
    void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision) {
        
    }


    public void WriteToArduino(string message) {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

}
