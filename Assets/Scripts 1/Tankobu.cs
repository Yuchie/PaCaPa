using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tankobu : MonoBehaviour {
    public float maxSize;
	// Use this for initialization
	void Start () {
        var hash = new Hashtable();
        hash.Add("x", maxSize);
        hash.Add("y", maxSize);
        hash.Add("z", maxSize);
        hash.Add("time", 3f);
        hash.Add("easetype", "linear");
        iTween.ScaleTo(gameObject, hash);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
