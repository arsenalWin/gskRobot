using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeldLine : MonoBehaviour {
    public Material material;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv = new List<Vector2>();

    GameObject weldline;//
    MeshFilter weldline_mf;
    MeshRenderer weldline_mr;

    Vector3 lastmiddle_pos = Vector3.zero;
    Vector3 WeldLinePos = Vector3.zero;//焊缝的位置，这里设置为焊接起点的位置

    GameObject welding_torch;

	// Use this for initialization
	void Start () {
        weldline = new GameObject("weldline");
        weldline_mf = weldline.AddComponent<MeshFilter>();
        weldline_mr = weldline.AddComponent<MeshRenderer>();
        weldline.transform.parent = GameObject.Find("weldlinestart").GetComponent<Transform>();
        weldline.transform.localPosition = Vector3.zero;
        WeldLinePos = weldline.transform.position;
        lastmiddle_pos = WeldLinePos;
        //
        weldline_mr.material = material;
        welding_torch = GameObject.Find("welding_torch");
        
	}
	
	// Update is called once per frame
	void Update () {
        if (GSKDATA.Weld)
        {
            CreateMesh();
        }
	}

    void OnGUI()
    {
        
    }

    public void RemoveWeldLine()
    {
        vertices.Clear();
        uv.Clear();
        triangles.Clear();
        weldline_mf.mesh = null;
    }

    public void SePosition()
    {

        WeldLinePos = new Vector3(welding_torch.transform.position.x, -1.114038f, welding_torch.transform.position.z);
        weldline.transform.position = WeldLinePos;
        lastmiddle_pos = WeldLinePos;
    }

    public void CreateMesh()
    {

        Vector3[] new_pos = GetPoss(GetEndPos());
        int vertice_num = vertices.Count;
        if (vertice_num >= 2)
        {
            vertices.Add(vertices[vertice_num - 2]);
            vertices.Add(vertices[vertice_num - 1]);
            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
        }
        vertices.Add(new_pos[0]);
        vertices.Add(new_pos[1]);

        uv.Add(new Vector2(1, 0));
        uv.Add(new Vector2(1, 1));

        vertice_num = vertices.Count;
        if (vertice_num >= 4)
        {
            triangles.Add(vertice_num - 4);
            triangles.Add(vertice_num - 3);
            triangles.Add(vertice_num - 1);

            triangles.Add(vertice_num - 4);
            triangles.Add(vertice_num - 1);
            triangles.Add(vertice_num - 2);

            weldline_mf.mesh.vertices = vertices.ToArray();
            weldline_mf.mesh.triangles = triangles.ToArray();
            weldline_mf.mesh.uv = uv.ToArray();
            weldline_mf.mesh.RecalculateNormals();
            weldline_mf.mesh.Optimize();
        }
      
    }


    //获取每个新增面片的终点(中点)位置
    Vector3 GetEndPos()
    {
        Vector3 End_pos = Vector3.zero;
        End_pos = new Vector3(welding_torch.transform.position.x, -1.114038f, welding_torch.transform.position.z);
        End_pos = End_pos - WeldLinePos;
        return End_pos;
    }

    //获取新增的两个顶点
    Vector3[] GetPoss(Vector3 middlepos)
    {
        float weldline_wide = 0.006f;
        Vector3 weldline_direct = Vector3.zero;
        Vector3 weldline_normal = Vector3.up;
        weldline_direct = Vector3.Normalize(middlepos - lastmiddle_pos);
        Vector3 weldline_new_ = Vector3.Cross(weldline_direct, weldline_normal) * weldline_wide;
        Vector3[] end_poss = { Vector3.zero, Vector3.zero };
        end_poss[0] = middlepos - weldline_new_;
        end_poss[1] = middlepos + weldline_new_;
        lastmiddle_pos = middlepos;
        return end_poss;
    }
}
