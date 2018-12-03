using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMogura : MonoBehaviour {
    public int addedScore;
    public int maxHP;
    [SerializeField]
    private int HP;
    public float immuneTime;
    private bool isImmune;
    public GameObject tankobu;
    public GameObject[] bansoukou;
    public GameObject hammerHead;
    public GameObject[] normalEyes;
    public GameObject[] yarareEyes;
    public GameObject result;
    public CalcGodStickMeshDeform calcGodStickMeshDeform;
    private int tankobuNum;
    public AudioSource soundOnHit;
	// Use this for initialization
	void Start () {
        HP = maxHP;
        SetBansoukou(0);
        tankobuNum = 0;
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        calcGodStickMeshDeform.deform = 0;
        calcGodStickMeshDeform.stiffness = 1;
        if(other.gameObject.tag == "hammer")
        {
            HP--;
            soundOnHit.Play();
            if(HP > 0)
            {
                //一瞬無敵になる・絆創膏を貼る
                SetBansoukou(3-(HP/2));
                if (isImmune)
                {
                    
                    Immune(immuneTime);
                }
                SetEyes(false);
            }
            else if(HP == 0 && !result.activeSelf)
            {
                SetEyes(true);
                //やられ処理　たんこぶ生成
                if (HP == 0)
                {
                    ScoreManager.UpdateScore(addedScore);
                }
                //レイをハンマーから飛ばして接触した表面にたんこぶ
                Vector3 vec = this.transform.position - hammerHead.transform.position;
                Ray ray = new Ray(hammerHead.transform.position, vec);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit,10f))
                {
                    if(hit.transform.gameObject.tag == "boss")
                    {
                        Instantiate(tankobu, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }
        }
    }

    private IEnumerator Immune (float _ImmuneTime)
    {
        isImmune = true;
        yield return new WaitForSeconds(_ImmuneTime);
        isImmune = false;
    }

    private void SetBansoukou (int howMany)
    {
        Debug.Log("BansoukouSet" + howMany);
        foreach(GameObject g in bansoukou)
        {
            g.SetActive(false);
        }
        for(int i = 0; i < howMany; i++)
        {
            bansoukou[i].SetActive(true);
        }
        if (tankobuNum < howMany)
        {
            //レイをハンマーから飛ばして接触した表面にたんこぶ
            Vector3 vec = this.transform.position - hammerHead.transform.position;
            Ray ray = new Ray(hammerHead.transform.position, vec);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.transform.gameObject.tag == "boss")
                {
                    Instantiate(tankobu, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            tankobuNum++;
            Debug.Log("SetTankobu" + tankobuNum); 
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
        foreach(GameObject g in group)
        {
            g.SetActive(isActive);
        }        
    }
    
}
