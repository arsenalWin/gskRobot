using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStack
{
    public int ID {get; set; }

    public int lines {get; set; }

    public int columns {get; set; }

    public int floors {get; set; }

    public float height {get; set; }

    public float[] enterPoint {get; set; }

    public float xDev {get; set;}

    public float yDev {get; set; }
    
    public float zDev {get; set; }

    public float putHeight {get; set; }

    public int type {get; set ;}

    public float[] p1 {get; set;}

    public float[] p2 {get; set;}

    public float[] p3 {get; set;}

    public float[] p4 {get; set;}

    public List<StackPoints> getStackPoints()
    {
        return null;
    }

    
    public List<Vector3> getPointsPos()
    {
        List<Vector3> result = new List<Vector3>();
        Vector3 p1Pos = getPos(p1);
        Vector3 p2Pos = getPos(p2);
        Vector3 p3Pos = getPos(p3);
        Vector3 p4Pos = getPos(p4);

        result.Add(p1Pos);


        return null;
    }

    private Vector3 getPos(float[] point)
    {
        
        return Vector3.zero;
    }


    private Quaternion getQua(float[] point)
    {
        return Quaternion.identity;
    } 

    private StackPoints getEachStackPoints(Vector3 position, Quaternion posture)
    {
        return null;
    }

    private Vector3 getPosDev(Vector3 start, Vector3 end, int num)
    {

        return Vector3.zero;
    }







}