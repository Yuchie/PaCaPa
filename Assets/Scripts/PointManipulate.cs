using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManipulate : MonoBehaviour {

    public SteamVR_TrackedObject trackedController;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var device = SteamVR_Controller.Input((int)trackedController.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            var position = device.GetAxis();
            float x = position.x;
            float y = position.y;

            if (y >= 0.5f && x < 0.5f && x > -0.5f)
            {
                Vector3 newPosition = transform.position + new Vector3(0, 0.01f, 0);
                transform.position = newPosition;
            }
            else if (y <= -0.5f && x < 0.5f && x > -0.5f)
            {
                Vector3 newPosition = transform.position - new Vector3(0, 0.01f, 0);
                transform.position = newPosition;
            }
        }
    }
}
