using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshtest001 : MonoBehaviour {
    public Shader shader;
	// Use this for initialization
	void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        vertices[0] += Vector3.up*0.1f;
        vertices[1] += Vector3.up* 0.1f;
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        
        
    }
}
