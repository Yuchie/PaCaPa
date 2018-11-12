using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class collisionScript2 : MonoBehaviour {

	SerialPort stream = new SerialPort("\\\\.\\COM13", 9600);
	public GameObject racket;
	private float time;
	private bool back = false;

	// Use this for initialization
	void Start () {
		stream.Open();
		stream.ReadTimeout = 1;
		racket = GameObject.FindWithTag("GameController");
		time = 0f;
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

		if (time > 0f)
		{
			float xVal = racket.GetComponent<Rigidbody>().angularVelocity.x;
			if (xVal < 0f)
			{
				xVal = xVal + 0.002f;
				racket.GetComponent<Rigidbody>().angularVelocity = new Vector3(xVal, 0, 0);
			}
			else
			{
				racket.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
				time = 0f;
				back = true;
			}
		}
		//else if (back == false) {
		//    racket.rigidbody.angularVelocity = new Vector3(0.3f, 0, 0);
		//}

	}

	//If your GameObject starts to collide with another GameObject with a Collider
	void OnCollisionEnter(Collision collision)
	{
		//Output the Collider's GameObject's name
		if (back != true)
		{
			Debug.Log(collision.collider.name);
			WriteToArduino("1");
			racket.GetComponent<Rigidbody>().angularVelocity = new Vector3(-0.3f, 0, 0);
			time = Time.time;
			back = true;
		}
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
