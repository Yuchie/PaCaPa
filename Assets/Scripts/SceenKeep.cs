using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceenKeep : MonoBehaviour {
    public string currentSceneName;
    public string nextSceneName;
    public Serial serial;
    public string reloadKey;
    public string moveKey;
    public string quitKey;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(reloadKey))
        {
            SceneManager.LoadScene (currentSceneName);
        }else if (Input.GetKeyDown(moveKey))
        {
            SceneManager.LoadScene(nextSceneName);
        }else if (Input.GetKeyDown(quitKey))
        {
            Application.Quit();
        }
	}

    void OnApplicationQuit()
    {
        serial.WriteToArduino("0");
        serial.CloseStream();
    }
}
