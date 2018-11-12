using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Random : MonoBehaviour {
    public float minScale;
    public float maxScale;
    // Use this for initialization
    void Start()
    {
        float rnd = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(rnd, rnd, 1);
        transform.localPosition = new Vector3(0, (0.9f - rnd) / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTag()
    {
        this.gameObject.tag = "target";
        MakiwaraControl.isMoving = false;
    }
}
