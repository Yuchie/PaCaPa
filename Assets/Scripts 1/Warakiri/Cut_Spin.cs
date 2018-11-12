using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut_Spin : MonoBehaviour {
    int i = 0;
	// Use this for initialization
	void Start () {
        Roop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Roop ()
    {
        if(i == 3)
        {
            Destroy(this.gameObject);
        }
        var hash = new Hashtable();
        hash.Add("x", 120);
        hash.Add("time",0.05f);
        hash.Add("islocal", true);
        hash.Add("oncomplete","Roop");
        hash.Add("oncompletetarget", this.gameObject);
        iTween.RotateAdd(this.gameObject, hash);
        i++;
        
    }
}
