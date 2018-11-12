using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mogura : MonoBehaviour {
    public int addedScore;//モグラに与えられるスコアです
    private Collider capsuleCollider;
    public GameObject GameController;
    public float immuneTime;
    public float delayUntilDisappear;
    public bool isBoss;
    public bool istouched;
    public bool isSoft;
    public CalcGodStickMeshDeform calcGodStickMeshDeform;
    public GameObject[] normalEyes;
    public GameObject[] yarareEyes;
    [SerializeField]
    private AudioClip soundOnHit;
	// Use this for initialization
	void Start () {
        capsuleCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "hammer" && !istouched)
        {
            ScoreManager.UpdateScore(addedScore);
                istouched = true;
            StartCoroutine("Immune", immuneTime);
            Disappear();
            
            if (isSoft)
            {
                calcGodStickMeshDeform.stiffness = 0.2f;
                calcGodStickMeshDeform.deform = 6;
                GetComponent<AudioSource>().PlayOneShot(soundOnHit);
            } else
            {
                calcGodStickMeshDeform.stiffness = 1;
                calcGodStickMeshDeform.deform = 0;
                GetComponent<AudioSource>().PlayOneShot(soundOnHit);
            }

            SetEyes(true);
        }

        
    }
    private IEnumerator Immune (float immueTime)
    {
        yield return new WaitForSeconds(delayUntilDisappear);
        
        yield return new WaitForSeconds(immueTime);
        capsuleCollider.enabled = false;
        SetEyes(false);
        //capsuleCollider.enabled = true;

    }
    private void Disappear()
    {
        var disappear = new Hashtable();
        disappear.Add("y", -4);
        disappear.Add("time", 1f);
        disappear.Add("delay", delayUntilDisappear);
        disappear.Add("islocal", true);
        iTween.MoveTo(this.gameObject, disappear);

    }
    public void SetUnableCollider(GameObject g)
    {
        try
        {
            g.GetComponent<CapsuleCollider>().enabled = false;
        }
        catch
        {

        }
        try
        {
            g.GetComponent<MeshCollider>().enabled = false;
        }
        catch
        {

        }
        
    }

    private void SetEyes(bool isYarare)
    {
        if (isYarare)
        {
            SetActiveGroup(normalEyes,false);
            SetActiveGroup(yarareEyes, true);
        }
        else
        {
            SetActiveGroup(normalEyes, true);
            SetActiveGroup(yarareEyes, false);
        }
    }

    private void SetActiveGroup(GameObject[] group, bool isActive)
    {
        foreach (GameObject g in group)
        {
            g.SetActive(isActive);
        }
    }
}
