//<summary>
//JointMove.cs
//DL-30A
//Created by 周伟 on 6/10/2015.
//Company: Sunnytech
//Function:
//机械臂关节运动的类
//
//<summary>
using UnityEngine;
using System.Collections;

public class JointMove {
    
    public JointMove(GameObject joint, Vector3 Direction)
    {
        RotateDirect = Direction;
        RobotJoint = joint;
    }
    public JointMove(string joint, Vector3 Direction)
    {
        RotateDirect = Direction;
        RobotJoint = GameObject.Find(joint);
    }

    public Vector3 RotateDirect;
    GameObject RobotJoint;
    float angle = 0;
    int softlimit = 135;//上限
    int _softlimit = -135;//下限
    public float Angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
        }
    }
    public int SoftLimit
    {
        get
        {
            return softlimit;
        }
        set
        {

            softlimit = value;
            
            
        }
    }

    public int _SoftLimit
    {
        get
        {
            return _softlimit;
        }
        set
        {

            _softlimit = value;
           
        }
    }

    //返回零点
    public void ReturnToZero()
    {
        RobotJoint.transform.localEulerAngles = new Vector3(0, 0, 0);
        //Debug.Log(RobotJoint.name);
        angle = 0;
    }


    /// <summary>
    /// 单纯以一定速度绕自身旋转轴旋转
    /// </summary>
    /// <param name="speed"></param>
    public bool Rotate(float speed)
    {
        if ((speed > 0 && angle >= softlimit) || (speed < 0 && angle <= _softlimit))
        {
            return false;
        }
        if (!GSKDATA.SystemWrong)
        {
            RobotJoint.transform.Rotate(RotateDirect, speed); //也可以不通过旋转，直接定位到位
            angle += speed;
        }
        return true;
        
    }
    /// <summary>
    /// 以一定速度旋转到固定角度
    /// (暂时只有test中用到)
    /// </summary>
    /// <param name="A">最终角度 </param>
    /// <param name="S">每帧转动的角度</param>
    public void RotateFixedAngle(float A, float S)
    {
        if (angle < A)
        {
            RobotJoint.transform.Rotate(RotateDirect, S);
            angle += S;
            if (angle > A)
            {
                RobotJoint.transform.Rotate(RotateDirect, angle - A);
                angle = A;
            }
        }
        else if (angle > A)
        {
            RobotJoint.transform.Rotate(RotateDirect, -S);
            angle -= S;
            if (angle < A)
            {
                RobotJoint.transform.Rotate(RotateDirect, A - angle);
                angle = A;
            }
        }
    }

    /// <summary>
    /// 在FixedTime的时间内转动一定角度
    /// </summary>
    /// <param name="theta"></param>
    public bool  RotateFixedAngle_FixedTime(float theta)
    {
        if (!(angle >= _softlimit && angle <= softlimit))
        {
            return false;
        }
        else
        {
            if (!GSKDATA.SystemWrong)
            {
                RobotJoint.transform.Rotate(RotateDirect, (theta - angle) / Time.deltaTime * Time.deltaTime);
                angle = theta;
            }
            return true;
        }
    }

    /// <summary>
    /// 三次多项式插值算法
    /// </summary>
    /// <param name="theta0">起始角度</param>
    /// <param name="thetaf">终点角度</param>
    /// <param name="Tf">总插补时长</param>
    /// <param name="t">已插补时长</param>
    /// <param name="S0">起始速度</param>
    /// <param name="Sf">终止速度</param>
    public void JointInterpolation_3(float theta0, float thetaf, float Tf, float t, float S0 = 0, float Sf = 0)
    {
        float _a0 = theta0;
        float _a1 = 0;
        float _a2 = (3 * (thetaf - theta0) - (2 * S0 + Sf) * Tf) / (Tf * Tf);
        float _a3 = (-2 * (thetaf - theta0) + (S0 + Sf) * Tf) / (Tf * Tf * Tf);
        if (t > Tf)
        {
            t = Tf;
        }
        //Debug.Log("已进行插补时长：" + t);
        float theta_T = _a0 + _a1 * t + _a2 * t * t + _a3 * t * t * t;
        RotateFixedAngle_FixedTime(theta_T);
        if (t == Tf)
        {
            GSKDATA.AxisRunning = false;//插补结束
            GSKDATA.MOVJ_t = 0f;
            
            //Debug.Log(angle);
            //float[] ff = CurrentAngle_All();
            //Debug.Log(ff[0] + "," + ff[1] + "," + ff[2] + "," + ff[3] + "," + ff[4] + "," + ff[5]);
        }
    }

}
