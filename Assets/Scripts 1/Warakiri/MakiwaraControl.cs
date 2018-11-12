using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakiwaraControl : MonoBehaviour {
    public GameObject[] _Makiwaras;
    public static GameObject[] makiwaras;
    public GameObject _Makiwara;
    public static GameObject makiwara;
    public float _DistanceBetweenMakiwaras;
    public static float distanceBetweenMakiwaras;
    public static bool isMoving = false;
    // Use this for initialization
    void Start()
    {
        makiwaras = _Makiwaras;
        makiwara = _Makiwara;
        distanceBetweenMakiwaras = _DistanceBetweenMakiwaras;
        makiwaras[0].GetComponent<Transform>().GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void UpdateMakiwara()
    {
        if (!isMoving)
        {
            isMoving = true;
            Destroy(makiwaras[0]);
            for (int i = 1; i < makiwaras.Length; i++)
            {
                makiwaras[i - 1] = makiwaras[i];
                var hash = new Hashtable();
                hash.Add("x", -distanceBetweenMakiwaras);
                hash.Add("time", 1f);
                hash.Add("islocal", true);
                if (i == 1)
                {
                    hash.Add("oncompletetarget", makiwaras[i - 1].GetComponent<Transform>().GetChild(0).gameObject);
                    hash.Add("oncomplete", "AddTag");
                    makiwaras[i - 1].GetComponent<Transform>().GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                iTween.MoveAdd(makiwaras[i - 1], hash);
                print(makiwaras[i - 1]);
                //makiwaras[i - 1].GetComponent<Transform>().position -= new Vector3(distanceBetweenMakiwaras,0,0);
            }
            makiwaras[makiwaras.Length - 1] = Instantiate(makiwara, new Vector3(distanceBetweenMakiwaras * (makiwaras.Length - 1), 2.5f, 0.9860001f), Quaternion.Euler(90, 0, 0));
        }

    }
}
