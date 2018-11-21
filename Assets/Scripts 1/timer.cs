using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {
    public static float currentTime;
    public  bool isBegin;
    public float maxTime;
    public Text text;
    public GameObject result;
    public GameObject start;
    public Text startText;
    public AudioSource audioWhistle;
    private bool isFinish;
    [Space]
    [Header("スタート時に入力するキーを選択")]
    public string keyName;
	// Use this for initialization
	void Start () {
        currentTime = maxTime;
        //audioWhistle.Play();
        
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isBegin)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(keyName))
            {
                StartCoroutine(StartTimer());
            }
        }

        text.text = currentTime.ToString("F0");
        if(currentTime < 0) {
            if (!isFinish)
            {
                audioWhistle.Play();
                isFinish = true;
            }

            //ゲーム終了処理
            currentTime = 0;
            result.SetActive(true);
        }
            
    }
    private IEnumerator StartTimer()
    {
        isBegin = false;
        start.SetActive(true);
        startText.text = "3";
        yield return new WaitForSeconds(1);
        startText.text = "2";
        yield return new WaitForSeconds(1);
        startText.text = "1";
        yield return new WaitForSeconds(1);
        startText.text = "Start!";
        yield return new WaitForSeconds(1);
        start.SetActive(false);
        isBegin = true;
    }
        
}
