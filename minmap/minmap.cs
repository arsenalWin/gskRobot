using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class minmap: MonoBehaviour
{
    public RenderTexture minmap_texture;
    public Material minmap_material;

	//画中画边框
	public Texture2D tex_circle;
	
	public float circle = 300f;
	public float fov;
	public Vector3 vec_pos;
	public Vector3 vec_elu;

    public string position = "left_up";
    List<Vector3> cameraPos_1 = new List<Vector3>();
    List<Vector3> cameraAng_1 = new List<Vector3>();

    int minCameraNo = 0;//地图相机对准哪个场景
    

    void Awake()
    {
		fov=20f;

		
    }
    void Start()
    {
        //case1
        cameraPos_1.Add(new Vector3(-24.896f, 0.408f, -2.783f));
        cameraPos_1.Add(new Vector3(-25.903f, -1.49f, -6.893f));
        cameraAng_1.Add(new Vector3(90, 0, 0));
        cameraAng_1.Add(new Vector3(0, 0, 0));
        //case2

        //case3


        CloseMinMap();
    }
    //开启小地图
    public void ShowMinMap()
    {
        if (!camera.enabled)
        {
            camera.enabled = true;
            minCameraNo = 0;
            SetMinCameraPos();
        }
        else
        {
            camera.enabled = false;
        }
        
    }
    //关闭小地图
    public void CloseMinMap()
    {
        camera.enabled = false;
    }

    //设置地图相机的 位置
    void SetMinCameraPos()
    {
        Vector3 temp_pos = Vector3.zero;
        Vector3 temp_ang = Vector3.zero;
        switch(GSKDATA.Scene_NO)
        {
            case 1:
                temp_pos = cameraPos_1[minCameraNo];
                temp_ang = cameraAng_1[minCameraNo];
                break;
            case 2:
                break;
            case 3:
                break;
        }
        camera.transform.localPosition = temp_pos;
        camera.transform.localEulerAngles = temp_ang;
    }

    //更改当前地图相机的位置
    public void ChangeCameraPos()
    {
        int totalNo = 0;
        switch (GSKDATA.Scene_NO)
        {
            case 1:
                totalNo = cameraPos_1.Count;
                break;
            case 2:
                break;
            case 3:
                break;
        }
        if (++minCameraNo == totalNo)
        {
            minCameraNo = 0;
        }
        SetMinCameraPos();
    }
	
    void OnGUI()
    {
		GUI.depth=1;
		if (Event.current.type == EventType.Repaint)
        {
			if(camera.enabled){
				minmap_texture = camera.targetTexture;
				minmap_material.mainTexture=minmap_texture;

				if (position == "right_up")
				{
					Graphics.DrawTexture(new Rect(Screen.width-circle,0,circle,circle),minmap_texture,minmap_material);

					GUI.DrawTexture(new Rect(Screen.width - circle, 0, circle, circle), tex_circle);
				}
				else if (position == "left_up")
				{
					Graphics.DrawTexture(new Rect(0, 60, circle, circle), minmap_texture, minmap_material);

					GUI.DrawTexture(new Rect(0, 60, circle, circle), tex_circle);
				}
				else if (position == "right_down")
				{
					Graphics.DrawTexture(new Rect(Screen.width - circle, Screen.height - circle, circle, circle), minmap_texture, minmap_material);

					GUI.DrawTexture(new Rect(Screen.width - circle, Screen.height - circle, circle, circle), tex_circle);
				}
				else if (position == "left_down")
				{
					Graphics.DrawTexture(new Rect(0, Screen.height - circle, circle, circle), minmap_texture, minmap_material);

					GUI.DrawTexture(new Rect(0, Screen.height - circle, circle, circle), tex_circle);
				}
				else if (position == "center")
				{
					Graphics.DrawTexture(new Rect(Screen.width / 2 - circle / 2, Screen.height / 2 - circle / 2, circle, circle), minmap_texture, minmap_material);

					GUI.DrawTexture(new Rect(Screen.width / 2 - circle / 2, Screen.height / 2 - circle / 2, circle, circle), tex_circle);
				}
			}   
       }
    }
	
    void Update()
	{
		
	}
}
