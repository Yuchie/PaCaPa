using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Hekomi : MonoBehaviour {
    //オブジェクトにアタッチされたシェーダーに値を渡してオブジェクトをへこませるやつです。
    //変形作業の大部分はシェーダーに担当してもらうのでそっちに書きます。
    public Transform[] pointForNormal;//ハンマーの叩く部分の平面の法線ベクトルを求めるための3点です。 
    public Transform transformHammer;
    public GameObject target;
    private Renderer rendererTarget;
    private Transform transformTarget;
	// Use this for initialization
	void Start () {
        //変形対象のコンポーネントを取得
        rendererTarget = target.GetComponent<Renderer>();
        transformTarget = target.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cross = CalculateNormalVector(pointForNormal);
        SetToShader(transformTarget.position, transformHammer.position, cross);

	}

    public Vector3 CalculateNormalVector (Transform[] transfroms)
    {
        //配列の最初の3つのベクトルを取得して外積を計算後正規化して返します。
        Vector3 vec1 = transfroms[1].position - transfroms[0].position;
        Vector3 vec2 = transfroms[2].position - transfroms[0].position;
        return Vector3.Cross(vec1, vec2).normalized;

    }
    public void SetToShader(Vector3 posTarget, Vector3 posHammer, Vector3 normalVector)
    {
        //シェーダーに値を渡します。オブジェクト自身の位置、ハンマーの位置、ハンマーの平面の法線ベクトル。
        rendererTarget.material.SetVector("_CurrentCenter", posTarget);
        rendererTarget.material.SetVector("_ForceCenter", posHammer);
        rendererTarget.material.SetVector("_ForceNormal", normalVector);

    }

    
}
