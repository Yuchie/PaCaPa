using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour {

    public Serial serial;
    public GameObject sword;
    public GameObject stick;
    public GameObject target;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
        {
            serial.WriteToArduino("0");
            serial.CloseStream();
            SceneManager.LoadScene(0);
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            if (sword.activeSelf)
            {
                sword.SetActive(false);
                stick.SetActive(true);
                foreach (Transform child in target.transform)
                {
                    child.GetComponent<Rigidbody>().isKinematic = true;
                }
            } else
            {
                sword.SetActive(true);
                stick.SetActive(false);
                foreach (Transform child in target.transform)
                {
                    child.GetComponent<Rigidbody>().isKinematic = false;
                }
            }

        } else if (Input.GetKeyDown(KeyCode.A))
        {
            serial.WriteToArduino("a");
        } else if (Input.GetKeyDown(KeyCode.M))
        {
            serial.WriteToArduino("m");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            serial.WriteToArduino("t");
        }
    }

    void OnApplicationQuit()
    {
        serial.WriteToArduino("0");
        serial.CloseStream();
    }
}
