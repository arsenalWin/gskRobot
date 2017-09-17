//<summary>
//PenAndPalette#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//判断画笔是否和画板接触的类
//
//<summary>
using UnityEngine;
using System.Collections;

public class PenAndPalette{
    private Transform left1;
    private Transform left2;
    private Transform right1;
    private Transform right2;
    private Transform pen;
    private float palette_width;
    private float palette_height;
    private float deviation;
    public PenAndPalette(string z1, string z2, string y1, string y2, string pen_, float width, float height, float devi)
    {
        left1 = GameObject.Find(z1).GetComponent<Transform>();
        left2 = GameObject.Find(z2).GetComponent<Transform>();
        right1 = GameObject.Find(y1).GetComponent<Transform>();
        right2 = GameObject.Find(y2).GetComponent<Transform>();
        pen = GameObject.Find(pen_).GetComponent<Transform>();
        palette_width = width;
        palette_height = height;
        deviation = devi;
    }

    //判断笔是否和画板接触
    public bool IsContact()
    {
        float zPen = pen.transform.position.z;
        float xPen = pen.transform.position.x;
        float yPen = pen.transform.position.y;
        float xPalette = left1.transform.position.x;
        float zMax = right1.transform.position.z;
        float zMin = left1.transform.position.z;
        float yMin = right1.transform.position.y;
        float yMax = right2.transform.position.y;
        if (xPen <= xPalette + deviation && xPen >= xPalette - deviation)
        {
            if (zPen < zMax && zPen > zMin && yPen < yMax && yPen > yMin)
            {
                
                return true;
            }
        }
        return false;
    }

    //获取接触点的像素点
    public Vector2 GetContactPoint()
    {
        float zPen = pen.transform.position.z;
        float yPen = pen.transform.position.y;
        float zMax = right1.transform.position.z;
        float zMin = left1.transform.position.z;
        float yMin = right1.transform.position.y;
        float yMax = right2.transform.position.y;

        Vector2 point = Vector2.zero;
        point.x = (zPen - zMin) / (zMax - zMin) * palette_width;//attention
        point.y = (yPen - yMin) / (yMax - yMin) * palette_height;
        return point;
    }




}
