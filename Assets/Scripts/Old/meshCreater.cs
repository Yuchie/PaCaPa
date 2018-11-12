using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class meshCreater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// create Mesh
		var mesh = new Mesh();

		// set up vertices
		mesh.vertices = (
			from y in new[] {-1, 1}
			from x in new[] {-1, 1}
			select new Vector3(x, y, 0)).ToArray();

		// set up index of vertices
		mesh.triangles = new[] { 0, 1, 2, 2, 1, 3 };

		// calculate field and normal
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		// meshfilter
		GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
