//<summary>
//PaintScript#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintScript : MonoBehaviour {
    PenAndPalette PenPalette;
    List<Vector2> paintline = new List<Vector2>();//绘画的边
    Texture2D palette;
    GameObject Huaban;
    
	// Use this for initialization
	void Start () {
        palette = (Texture2D)GameObject.Find("xqhh0311").GetComponent<MeshRenderer>().materials[1].mainTexture;
        PenPalette = new PenAndPalette("zuo1", "zuo2", "you1", "you2", "pen_", 340f, 256f, 0.02f);
        Huaban = GameObject.Find("xqhh0311");
        ClearPanel();
	}

    //清空画板
    public void ClearPanel()
    {
        GUILine.TextureInitial(palette);//清空画板
    }

	// Update is called once per frame
	void Update () {
        //
        if (GSKDATA.Painting && GSKDATA.AxisRunning)
        {
            if (PenPalette.IsContact())
            {
                //高亮
                if (Huaban.GetComponent<FlashingController>() == null)
                {
                    Huaban.AddComponent<FlashingController>();
                }
                paintline.Add(PenPalette.GetContactPoint());
                if (paintline.Count > 2)
                {
                    paintline.RemoveAt(0);
                }
                if (paintline.Count == 2)
                {
                    //画线
                    PaintLine();
                }
            }
            else
            {
                paintline.Clear();
                if (Huaban.GetComponent<FlashingController>() != null)
                {
                    Destroy(Huaban.GetComponent<FlashingController>());
                    Destroy(Huaban.GetComponent<HighlightableObject>());
                }
            }
        }
	}

    private void PaintLine()
    {
        Color color = Color.black;//黑色的线
        int start_x = (int)paintline[0].x;
        int start_y = (int)paintline[0].y;
        int end_x = (int)paintline[1].x;
        int end_y = (int)paintline[1].y;
        GUILine.DrawSegment(palette, start_x, start_y, end_x, end_y, color, 3);
    }
}
