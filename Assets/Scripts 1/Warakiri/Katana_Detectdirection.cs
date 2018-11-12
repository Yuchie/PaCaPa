using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana_Detectdirection : MonoBehaviour {
    public bool isBack;
    public bool isResting;
    public unity_cutter cutter;
    public Katana_Detectdirection otherSide;
    public CalcGodStickMeshDeform calcGodStickMeshDeform;
    public bool isFollowReal;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

    private void OnTriggerEnter(Collider other)
    {
        
        if ((!cutter.isCutting && other.gameObject.tag == "target"))
        {
            if (!isResting)
            {
                Debug.Log("hit");
                cutter.isBack = isBack;
                StartCoroutine("Rest");
            }
            cutter.isCutting = true;
            if (isFollowReal)
            {
                calcGodStickMeshDeform.ChangeHit(false);
                calcGodStickMeshDeform.ChangeCut(true);
            }

        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "target")
        {
            if (cutter.isCutting)
            {
                calcGodStickMeshDeform.ChangeHit(false);
                calcGodStickMeshDeform.StartFollow();
            }
            cutter.isCutting = false;
            calcGodStickMeshDeform.ChangeCut(false);
            calcGodStickMeshDeform.StartFollow();
        }
    }
    private IEnumerator Rest ()
    {
        isResting = true;
        otherSide.isResting = true;
        yield return new WaitForSeconds(2f);
        isResting = false;
        otherSide.isResting = false;
    }
}
