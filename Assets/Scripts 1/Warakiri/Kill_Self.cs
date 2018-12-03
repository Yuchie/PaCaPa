using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Self : MonoBehaviour {
    [SerializeField]
    private float timeTilDie;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, timeTilDie);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
