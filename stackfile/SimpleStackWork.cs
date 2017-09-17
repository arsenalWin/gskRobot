using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStackWork : StackWork {
    private SimpleStack stack;
    private Quaternion pointQua;

	
    public SimpleStackWork(SimpleStack stack)
    {
        this.stack = stack;
        pointQua = getQua(stack.p1);
    }

    
    public override List<StackPoints> getStackPoints()
    {
        List<Vector3> pointsVec = getPointsPos();
        List<StackPoints> result = new List<StackPoints>();
        bool success = true;

        for (int z = 0; z < stack.floors; z++)
        {
            for (int j = 0; j < stack.lines; j++)
            {
                for (int i = 0; i < stack.columns; i++)
                {
                    int index = i + j * stack.columns + z * stack.lines * stack.columns;
                    float[] curAngles = new float[8];
                    if (i == 0 && j == 0 && z == 0)
                    {
                        curAngles = stack.p1;
                    }
                    else
                    {
                        if (i != 0)
                        {
                            curAngles = result[index - 1].p2;
                        }
                        else if (j != 0)
                        {
                            curAngles = result[index - stack.columns].p2;
                        }
                        else
                        {
                            curAngles = result[index - stack.columns*stack.lines].p2;
                        }
                        
                    }
                    
                    result.Add(getEachStackPoints(pointsVec[index], pointQua, curAngles, ref success));

                    if (!success)
                    {
                        Debug.LogError(stack.lines+"行，"+stack.columns+"列，"+stack.floors+"层的码垛点不能到达");
                    }
                }
            } 
        }

        return result;
    }

    public override List<Vector3> getPointsPos()
    {
        List<Vector3> result = new List<Vector3>();
        Vector3 p1Pos = getPos(stack.p1);
        Vector3 p2Pos = getPos(stack.p2);
        Vector3 p3Pos = getPos(stack.p3);
        Vector3 p4Pos = getPos(stack.p4);

        Vector3 rowPosDev = getPosDev(p1Pos, p2Pos, stack.columns);
        Vector3 columnPosDev = getPosDev(p1Pos, p3Pos, stack.lines);
        Vector3 heightPosDev = Vector3.Normalize(getPosDev(p1Pos, p4Pos, 2)) * stack.height ;

        for (int z=0;z<stack.floors;z++)
        {
            for(int j=0;j<stack.lines;j++)
            {
                for(int i = 0; i < stack.columns; i++)
                {
                    Vector3 tmp = p1Pos + i * columnPosDev + j * rowPosDev + z * heightPosDev;
                    result.Add(tmp);
                }
            }
        }

        return result;
    }

    private StackPoints getEachStackPoints(Vector3 position, Quaternion posture, float[] currentAngles, ref bool success)
    {
        Vector3 _position = position + new Vector3(stack.xDev, stack.yDev, stack.zDev);
        //float[] p1 = IKA.AcceptInterPointPosture(_position, posture, currentAngles, ref success);
        if(!success)
        {
            Debug.LogError("减速点不能到达");
        }
        //float[] p2 = IKA.AcceptInterPointPosture(position, posture, currentAngles, ref success);
        if (!success)
        {
            Debug.LogError("放置点不能到达");
        }
        //return new StackPoints { p1 = p1, p2 = p2, p3 = p1 };
        return null;
    }

}
