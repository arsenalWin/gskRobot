using System.Collections.Generic;
using UnityEngine;

public class StackWork {
    protected InverseKinematicsAlgorithm IKA;
    
    public StackWork()
    {
        //IKA = AxisEncry.getIKA();
    }

    public virtual List<StackPoints> getStackPoints()
    {
        return null;
    }

    public virtual List<Vector3> getPointsPos()
    {
        return null;
    }

    public Vector3 getPos(float[] point)
    {
        return IKA.SolutionOfKinematics(point);
    }

    public Quaternion getQua(float[] point)
    {
        return IKA.SolutionOfKinematics_posture(point);
    }

    public Vector3 getPosDev(Vector3 start, Vector3 end, int num)
    {
        if(num == 1)
        {
            return Vector3.zero;
        }
        else
        {
            return (end - start) / (num - 1);
        }
    }
}
