using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformChange : MonoBehaviour {
    public float deform;
    public CalcGodStickMeshDeform calcGodStickMeshDeform;
	// Use this for initialization
	void Start () {
        StartCoroutine("ChangeDeform", deform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator ChangeDeform (float deform)
    {
        yield return new WaitForSeconds(0.1f);
        calcGodStickMeshDeform.deform = deform;
    }
}
