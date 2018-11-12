using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {
    public static float currentTime;
    public float maxTime;
    public Text text;
    public GameObject result;
	// Use this for initialization
	void Start () {
        currentTime = maxTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        currentTime -= Time.deltaTime;
        text.text = currentTime.ToString("F0");
        if(currentTime < 0)
        {
            //ゲーム終了処理
            currentTime = 0;
            result.SetActive(true);
        }
        
	}
}
