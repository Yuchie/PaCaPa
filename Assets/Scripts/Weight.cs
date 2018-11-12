using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour {

    public Serial serial;
    [Range(0, 90)] public int target_weight = 10;

    private int weight = 0;
    private int next_weight;

    private bool hold;

    // Use this for initialization
    void Start () {
        hold = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (hold){
            next_weight = target_weight;
        } else {
            next_weight = 0;
        }

        if (serial.CheckFree() && (weight != next_weight))
        {
            serial.WriteToArduino(next_weight.ToString());
            weight = next_weight;
        }

    }

    void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "Stage") {
            hold = false;
        }

    }

    void OnCollisionStay(Collision collision) {
        
    }

    void OnCollisionExit(Collision collision) {

        if (collision.gameObject.tag == "Stage")
        {
            hold = true;
        }

    }
}
