using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unity_cutter : MonoBehaviour {
    public bool isBack;
    public bool isCutting;
    public GameObject Cut;

    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Vector3 endPosition;
    private Quaternion startQuaternion;
    private Quaternion endQuaternion;
    public Katana_Detectdirection katana_Detectdirection;
    public KatanaLocate katanaLocate;
    public WaraParticle waraParticle;
    public Transform pivot;
    public GameObject CuttingSound;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(GenerateCuttingSound());
    }

    // Update is called once per frame
    void Update()
    {
        katana_Detectdirection.isFollowReal = KatanaCheck();
        katanaLocate.isFollowReal = KatanaCheck();
        SetParticle();
        
    }

    IEnumerator GenerateCuttingSound()
    {
        if(!isBack && isCutting)
        {
            Instantiate(CuttingSound);
            yield return new WaitForSeconds(0.12f);
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(GenerateCuttingSound());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "target")
        {
            startPosition = other.ClosestPointOnBounds(this.transform.position);
            startQuaternion = this.transform.rotation;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "target")
        {
            endPosition = other.ClosestPointOnBounds(this.transform.position);
            endQuaternion = this.transform.rotation;
            if (!isBack)
            {
                Vector3 vec1 = startPosition - endPosition;
                Vector3 vec2 = startPosition - pivot.position;
                Vector3 normal = Vector3.Cross(vec1, vec2);
                Instantiate(Cut, endPosition, Quaternion.LookRotation(normal));
                /*float theta = Mathf.Atan2(startPosition.y - endPosition.y, startPosition.x - endPosition.x);
                Instantiate(Cut, endPosition, endQuaternion * Quaternion.Euler(0,90,90));*/
            }
        }

    }

    private Vector3 AverageWithoutZ(Vector3 pos1, Vector3 pos2)
    {
        return new Vector3(pos1.x + pos2.x, pos1.y + pos2.y) / 2;
    }

    private Vector3 CalculateNormalVectorFromQuaternion(Quaternion q1, Quaternion q2)
    {
        return Vector3.Cross(q1 * Vector3.up, q2 * Vector3.up);
    }

    private bool KatanaCheck()
    {
        if (isBack && isCutting)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void SetParticle()
    {
        waraParticle.isGenerating = isCutting && !isBack;
    }

}
