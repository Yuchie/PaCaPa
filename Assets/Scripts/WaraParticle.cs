using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaraParticle : MonoBehaviour {
    public ParticleSystem particle;
    public float velocity;
    public float particlecount;
    public bool isGenerating;
    private Vector3 posB;
    private Vector3 posA;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        posA = transform.position;
        if (isGenerating)
        {
            velocity = (posA - posB).magnitude / Time.deltaTime;
            particle.Emit((int)(velocity * particlecount));
        }
        posB = transform.position;
	}
}
