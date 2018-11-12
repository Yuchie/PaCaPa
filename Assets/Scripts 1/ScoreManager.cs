using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public static int score;
    public Text text;
    public Text result;
    public static Text _text;
    public static Text _result;
	// Use this for initialization
	void Start () {
        _text = text;
        _result = result;
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}

    public static void UpdateScore(int delta)
    {
        score += delta;
        if (score < 0)
            score = 0;
        _text.text = "" + score;
        _result.text = _text.text;
    }
}
