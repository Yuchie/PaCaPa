﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour {

    public Material capMaterial;
    public PhysicMaterial physicMaterial;
    // Use this for initialization
    void Start()
    {
        

    }

    void Update()
    {


            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {

                GameObject victim = hit.collider.gameObject;
                if(victim.tag == "target")
                {
                GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);
                pieces = CompareHeight(pieces[0], pieces[1]);
                Destroy(pieces[0].GetComponent<BoxCollider>());
                pieces[0].tag = "Untagged";
                pieces[0].layer = 2;
                pieces[1].tag = "target";
                Destroy(pieces[1].GetComponent<BoxCollider>());
                Destroy(pieces[0].GetComponent<MeshCollider>());
                Destroy(pieces[1].GetComponent<MeshCollider>());
                pieces[0].AddComponent<Rigidbody>();
                pieces[1].AddComponent<Rigidbody>();
                pieces[0].GetComponent<Rigidbody>().isKinematic = true;
                pieces[1].GetComponent<Rigidbody>().isKinematic = true;
                var m0 = pieces[0].AddComponent<MeshCollider>();
                m0.convex = true;
                m0.material = physicMaterial;
                var m1 = pieces[1].AddComponent<MeshCollider>();
                m1.convex = true;
                pieces[1].GetComponent<MeshCollider>().material = physicMaterial;
                pieces[0].GetComponent<Rigidbody>().isKinematic = false;
                pieces[1].GetComponent<Rigidbody>().isKinematic = false;
                pieces[1].AddComponent<Call_Next_Makiwara>();
                pieces[0].GetComponent<Transform>().position += new Vector3(0, 0.15f, 0);
                pieces[1].GetComponent<Transform>().position += new Vector3(0, 0.1f, 0);
                Destroy(pieces[1], 2.5f);
                Destroy(this.gameObject);
                }

            }
            
        
    }
    GameObject[] CompareHeight (GameObject g1,GameObject g2)
    {
        if (g1.GetComponent<Renderer>().bounds.center.y > g2.GetComponent<Renderer>().bounds.center.y)
        {
            return new GameObject[] { g1, g2 };
        }
        else
        {
            return new GameObject[] { g2, g1 };
        }
            
        
    }

    /*void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
        Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
        Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

        Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * 0.5f);

    }*/
}
