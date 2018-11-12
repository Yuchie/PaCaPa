using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaLocate : MonoBehaviour {
    public GameObject realStick;
    public GameObject virtualStick;
    private Transform transformReal;
    private Transform transformVirtual;
    public GameObject otherSide;
    public GameObject isActive;
    public bool isFollowReal;

    // Use this for initialization
    void Start () {
        transformReal = realStick.GetComponent<Transform>();
        transformVirtual = virtualStick.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        // transform.position = transformVirtual.position;
        //transform.eulerAngles = new Vector3(transformVirtual.eulerAngles.x, transformVirtual.eulerAngles.y, transformReal.eulerAngles.z - 90);
        //刃の入り方によってgod-object方式で描写するか刃が物体に刺さっていくか決めます。
        if (isFollowReal)
        {
            transform.position = transformReal.position;
            transform.rotation = transformReal.rotation * Quaternion.Euler(0, 0, -90);
        }
        else
        {
            transform.position = transformVirtual.position;
            transform.eulerAngles = new Vector3(transformVirtual.eulerAngles.x, transformVirtual.eulerAngles.y, transformReal.eulerAngles.z - 90);
        }
    }
}
