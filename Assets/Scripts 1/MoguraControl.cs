using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoguraControl : MonoBehaviour {
    public int wave;
    //public GameObject[] softMogura;
    public GameObject softMogura;
    public GameObject[] stiffMogura;
    public GameObject[] maruta;
    public GameObject[] moguraHole;
    public GameObject bossMogura;
    public GameObject bossMoguraHole;
    public float appearSoftY;
    public float appearStiffY;
    public float disappearY;
    public float spawnSpan;
    private float currentTime;
    private int[] moguraStatus;
    private bool didSoftMoguraAppear;
	// Use this for initialization
	void Start () {
        currentTime = spawnSpan;
        /*foreach(GameObject mogura in softMogura)
        {
            var disappear = new Hashtable();
            disappear.Add("y", disappearY);
            disappear.Add("time", 1f);
            disappear.Add("islocal", true);
            iTween.MoveTo(mogura, disappear);
        }*/
        foreach (GameObject mogura in stiffMogura)
        {
            var disappear = new Hashtable();
            disappear.Add("y", disappearY);
            disappear.Add("time", 1f);
            disappear.Add("islocal", true);
            iTween.MoveTo(mogura, disappear);
            mogura.GetComponent<CapsuleCollider>().enabled = false;
        }
        foreach (GameObject g in maruta)
        {
            var disappear = new Hashtable();
            disappear.Add("y", disappearY);
            disappear.Add("time", 1f);
            disappear.Add("islocal", true);
            iTween.MoveTo(g, disappear);
            g.GetComponent<MeshCollider>().enabled = false;
        }
        softMogura.GetComponent<CapsuleCollider>().enabled = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        currentTime -= Time.deltaTime;
        if(currentTime < 0)
        {
            if(timer.currentTime > 15)
            {
                //通常のモグラ
                moguraStatus = GenerateWave();
                MoguraAppear(moguraStatus);
                currentTime = spawnSpan;
            }
            else
            {
                //ボスモグラ
                foreach(GameObject hole in moguraHole)
                {
                    Destroy(hole);
                }
                bossMoguraHole.SetActive(true);
                softMogura.transform.position = new Vector3(0, -100, 0);
            }
            
        }
	}
    public int[] GenerateWave()
    {
        //モグラが出てくる穴とモグラの種類を決定します。
        int howManySpawn = Random.Range(1, 4);
        int[] spawnMogura = new int[5];//0=出てこない 1=出てくる
        for(int i = 0; i < howManySpawn; i++)
        {
            while (true)
            {
                int rand = Random.Range(0, 5);
                if(spawnMogura[rand] == 0)
                {
                    spawnMogura[rand] = Random.Range(1,3);
                    break;
                }
                
            }
        }
        if (Random.Range(0,2) == 0 && !didSoftMoguraAppear)
        {
            didSoftMoguraAppear = true;
            //やわらかいモグラも出現
            while  (true) {
                int rand = Random.Range(0, 5);
                if(spawnMogura[rand] == 1 || spawnMogura[rand] == 2)
                {
                    spawnMogura[rand] = 3;
                    break;
                }
            }
        }
        else
        {
            didSoftMoguraAppear = false;
        }
        return spawnMogura;
    }
    public void MoguraAppear(int[] moguraData)
    {

        
        for (int i = 0; i<moguraData.Length; i++) {
            switch (moguraData[i])
            {
                case 0:
                    //iTween.MoveTo(softMogura[i], disappear);
                    var disappear = MakeHashTableForItem(2,stiffMogura[i]);
                    iTween.MoveTo(stiffMogura[i], disappear);
                    disappear = MakeHashTableForItem(2, maruta[i]);
                    iTween.MoveTo(maruta[i], disappear);
                    break;
                case 3:
                    var appearSoft = MakeHashTableForItem(0, softMogura);
                    iTween.MoveTo(softMogura, appearSoft);
                    var disappear2 = MakeHashTableForItem(2, stiffMogura[i]);
                    iTween.MoveTo(stiffMogura[i], disappear2);
                    disappear2 = MakeHashTableForItem(2, maruta[i]);
                    iTween.MoveTo(maruta[i], disappear2);
                    SoftMoguraMove(i);
                    break;
                case 1:
                    //iTween.MoveTo(softMogura[i], disappear);
                    var appearStiff = MakeHashTableForItem(1,stiffMogura[i]);
                    iTween.MoveTo(stiffMogura[i], appearStiff);
                    disappear2 = MakeHashTableForItem(2, maruta[i]);
                    iTween.MoveTo(maruta[i], disappear2);
                    break;
                case 2:
                    disappear = MakeHashTableForItem(2, stiffMogura[i]);
                    iTween.MoveTo(stiffMogura[i], disappear);
                    appearStiff = MakeHashTableForItem(1, maruta[i]);
                    iTween.MoveTo(maruta[i], appearStiff);
                    break;
            }
        }
        if (!didSoftMoguraAppear)
        {
            var disappear = MakeHashTableForItem(2, softMogura);
            iTween.MoveTo(softMogura, disappear);
            softMogura.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
    public void SoftMoguraMove (int holeId)
    {
        Vector2[] holePosition = new Vector2[5];
        for(int i = 0; i < 5; i++)
        {
            holePosition[i] = new Vector2(moguraHole[i].transform.position.x, moguraHole[i].transform.position.z - 0.1f);
            //holePosition[i] = new Vector2(moguraHole[i].transform.position.x + 0.1f, moguraHole[i].transform.position.z);

        }
        softMogura.transform.position = new Vector3(holePosition[holeId].x, softMogura.transform.position.y, holePosition[holeId].y);
    }
    public void SetEnableCollider (GameObject g)
    {
        try
        {
            g.GetComponent<CapsuleCollider>().enabled = true;
            g.GetComponent<Mogura>().istouched = false;
        }
        catch
        {

        }
        try
        {
            g.GetComponent<MeshCollider>().enabled = true;
            g.GetComponent<Mogura>().istouched = false;
        }
        catch
        {

        }
    }
    public void SetUnableCollider (GameObject g)
    {
        try
        {
            g.GetComponent<CapsuleCollider>().enabled = false;
            //g.GetComponent<Mogura>().istouched = true;
        }
        catch
        {

        }
        try
        {
            g.GetComponent<MeshCollider>().enabled = false;
            //g.GetComponent<Mogura>().istouched = false;
        }
        catch
        {

        }
    }


    public Hashtable MakeHashTableForItem(int mode,GameObject g)
    {
        switch (mode) {
            case 0:
                var appearSoft = new Hashtable();
                appearSoft.Add("y", appearSoftY);
                appearSoft.Add("time", 1f);
                appearSoft.Add("islocal", true);
                appearSoft.Add("oncompletetarget", this.gameObject);
                appearSoft.Add("oncomplete", "SetEnableCollider");
                appearSoft.Add("oncompleteparams", g);
                return appearSoft;
            case 1:
                var appearStiff = new Hashtable();
                appearStiff.Add("y", appearStiffY);
                appearStiff.Add("time", 1f);
                appearStiff.Add("islocal", true);
                appearStiff.Add("oncompletetarget", this.gameObject);
                appearStiff.Add("oncomplete", "SetEnableCollider");
                appearStiff.Add("oncompleteparams", g);
                return appearStiff;
            case 2:
                var disappear = new Hashtable();
                disappear.Add("y", disappearY);
                disappear.Add("time", 1f);
                disappear.Add("islocal", true);
                disappear.Add("oncompletetarget", this.gameObject);
                disappear.Add("oncomplete", "SetUnableCollider");
                disappear.Add("oncompleteparams", g);
                return disappear;
            default:
                return null;
        }
    }
}
