using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;

public class Least2a : MonoBehaviour
{
    GenMesh gm;
    Vector3[] vv;
    private void Start()
    {
        gm = new GenMesh();
        vv = gm.Init(10, 10);
        gm.UpdateMesh_Public(vv);
    }

    // Update is called once per frame
    void Update()
    {
        // Calc2D_Least();
        Calc3D_Least(vv, 10, 10);
        gm.UpdateMesh_Public(vv);
    }

    // 最小二乗曲線の関数xに対応するyの値が得られる。wは係数ベクトル。
    float F(float x, Vector<float> w)
    {
        float num = 0;
        for (int i = 0; i < w.Count; i++)
        {
            num += w[i] * Mathf.Pow(x, i);
        }

        return num;
    }

    float FF(float x, float y, Vector<float> w1, Vector<float> w2)
    {
        float num = 0;
        for (int i = 0; i < w1.Count; i++)
        {
            num += w1[i] * Mathf.Pow(x, i);
        }

        for (int i = 0; i < w2.Count; i++)
        {
            num += w2[i] * Mathf.Pow(y, i);
        }

        return num;
    }

    // 引数:曲面の描画のための各頂点
    void Calc3D_Least(Vector3[] vvv, int tate, int yoko)
    {
        // w1とw2をそれぞれ偏微分して、連立方程式が出てくるので、それを解けばよい。
        Vector3[] vv = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            vv[i] = transform.GetChild(i).position;
        }

        const int M_ = 3; // M_ - 1次の関数になる。
        int N = vv.Length;

        if (M_ > N)
        {
            Debug.LogError("最小二乗解を見つけられません。");
            return;
        }

        Vector<float> t = Vector<float>.Build.Dense(N);
        for (int i = 0; i < t.Count; i++)
        {
            t[i] = vv[i].z;
        }

        Matrix<float> fai_1 = Matrix<float>.Build.Dense(N, M_);
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M_; j++)
            {
                fai_1[i, j] = Mathf.Pow(vv[i].x, j);
            }
        }

        Matrix<float> fai_2 = Matrix<float>.Build.Dense(N, M_);
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M_; j++)
            {
                fai_2[i, j] = Mathf.Pow(vv[i].y, j);
            }
        }

        var fai_1_t = fai_1.Transpose();
        var fai_2_t = fai_2.Transpose();

        var A = fai_1_t * t;
        var B = fai_2 * (fai_2_t * fai_2).Inverse() * fai_2_t * fai_1;
        var C = A - B.Transpose() * t;

        var D = fai_1_t * fai_1 - fai_1_t * fai_2 * (fai_2_t * fai_2).Inverse() * fai_2_t * fai_1;
        var w1 = (C * D.Inverse());

        var w2 = (fai_2_t * t - fai_1_t * fai_2 * w1) * (fai_2_t * fai_2).Inverse();

        Gizmos.color = Color.black;
        for (int i = 0; i < tate + 1; i++)
        {
            for (int j = 0; j < yoko + 1; j++)
            {
                int id = i * (yoko + 1) + j;
                vvv[id].z = FF(vvv[id].x, vvv[id].y, w1, w2);
            }
        }
    }

    void Calc2D_Least()
    {
        Vector3[] vv = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            vv[i] = transform.GetChild(i).position;
        }

        const int M_ = 5; // M_ - 1次の関数になる。
        int N = vv.Length;

        if (M_ > N)
        {
            Debug.LogError("最小二乗解を見つけられません。");
            return;
        }

        Vector<float> t = Vector<float>.Build.Dense(N);
        for (int i = 0; i < t.Count; i++)
        {
            t[i] = vv[i].y;
        }

        Matrix<float> fai = Matrix<float>.Build.Dense(N, M_);

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M_; j++)
            {
                fai[i, j] = Mathf.Pow(vv[i].x, j);
            }
        }

        var fai_t = fai.Transpose();
        var w = (fai_t * fai).Inverse() * fai_t * t;

        float min = -100;
        float max = 100;
        float delta = 0.1f;

        Vector3 pre = Vector3.zero;
        for (float tt = min; tt < max; tt += delta)
        {
            Vector3 v;
            v.x = tt;
            v.y = F(tt, w);
            v.z = 0;
            if (tt != min)
            {
                Debug.DrawLine(pre, v);
            }
            pre = v;
        }
    }
}

// 正方形状に三角形メッシュを生成する
public class GenMesh
{
    int tate; // 分割数 頂点の数は、+1になるので注意！
    int yoko; // tateの分割数で長さをlengthになるように合わせているので、yokoをtateと違う値にすると長方形になる。

    MeshFilter my_meshFilter;

    bool isInit;

    public GenMesh()
    {
        isInit = false;
    }

    // 初期に生成されたメッシュの各頂点を返す。
    public Vector3[] Init(int divNum = 10, float _length = 1)
    {
        if (!isInit)
        {
            tate = divNum;
            yoko = tate;

            float div_size = _length / tate; // 一つの三角形の大きさ
            float cube_size = div_size * 0.1f; // 調整用cubeの大きさ

            Vector3[] vv = new Vector3[(tate + 1) * (yoko + 1)];

            for (int i = 0; i < tate + 1; i++)
            {
                for (int j = 0; j < yoko + 1; j++)
                {
                    int index = j + (yoko + 1) * i;
                    Vector3 v = vv[index] = new Vector3(j * div_size, i * div_size, 0);
                }
            }

            // meshの初期設定をする。
            GameObject g = new GameObject();
            g.name = yoko.ToString() + "_" + tate.ToString();

            my_meshFilter = g.AddComponent<MeshFilter>();
            Material mat = new Material(Shader.Find("Diffuse"));
            mat.color = Color.red;
            g.AddComponent<MeshRenderer>().material = mat;

            isInit = true;

            return vv;
        }
        else
        {
            Debug.LogError("既に初期化されています.");
            return null;
        }
    }

    public void UpdateMesh_Public(Vector3[] _vv)
    {
        my_meshFilter.mesh = UpdateMesh(_vv);
    }

    // 引数に与えられた、ベクトル
    Mesh UpdateMesh(Vector3[] _vvv)
    {
        if (!isInit)
        {
            Debug.LogError("初期化されていません.");
        }

        if (_vvv.Length != (tate + 1) * (yoko + 1))
        {
            Debug.LogError("引数の配列のサイズが不正です");
            return null;
        }

        Mesh mesh = new Mesh();

        List<Vector3> vtx = new List<Vector3>();

        bool isGenBack = true; // 裏側のメッシュも作るかどうか。

        for (int i = 0; i < tate; i++)
        {
            for (int j = 0; j < yoko; j++)
            {
                int id = i * (yoko + 1) + j;
                Vector3 hidari_sita = _vvv[id];
                Vector3 migi_sita = _vvv[id + 1];
                Vector3 hidari_ue = _vvv[id + yoko + 1];
                Vector3 migi_ue = _vvv[id + 1 + yoko + 1];

                // 四角形の下側の三角形を生成する。
                vtx.Add(hidari_sita);
                vtx.Add(migi_sita);
                vtx.Add(hidari_ue);

                if (isGenBack)
                {
                    vtx.Add(hidari_ue);
                    vtx.Add(migi_sita);
                    vtx.Add(hidari_sita);
                }

                // 四角形の上側の三角形を生成する。
                vtx.Add(migi_sita);
                vtx.Add(migi_ue);
                vtx.Add(hidari_ue);

                if (isGenBack)
                {
                    vtx.Add(hidari_ue);
                    vtx.Add(migi_ue);
                    vtx.Add(migi_sita);
                }
            }
        }
        int[] tri = Enumerable.Range(0, vtx.Count).ToArray();
        // mesh.name = "my_mesh_" + mesh_num_str; // 毎フレームごとに名前を付けなおすのは、やめた。
        mesh.vertices = vtx.ToArray();
        mesh.triangles = tri;
        mesh.RecalculateNormals();

        return mesh;
    }
}
