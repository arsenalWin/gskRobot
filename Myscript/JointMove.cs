//<summary>
//JointMove.cs
//DL-30A
//Created by ��ΰ on 6/10/2015.
//Company: Sunnytech
//Function:
//��е�۹ؽ��˶�����
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
    int softlimit = 135;//����
    int _softlimit = -135;//����
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

    //�������
    public void ReturnToZero()
    {
        RobotJoint.transform.localEulerAngles = new Vector3(0, 0, 0);
        //Debug.Log(RobotJoint.name);
        angle = 0;
    }


    /// <summary>
    /// ������һ���ٶ���������ת����ת
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
            RobotJoint.transform.Rotate(RotateDirect, speed); //Ҳ���Բ�ͨ����ת��ֱ�Ӷ�λ��λ
            angle += speed;
        }
        return true;
        
    }
    /// <summary>
    /// ��һ���ٶ���ת���̶��Ƕ�
    /// (��ʱֻ��test���õ�)
    /// </summary>
    /// <param name="A">���սǶ� </param>
    /// <param name="S">ÿ֡ת���ĽǶ�</param>
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
    /// ��FixedTime��ʱ����ת��һ���Ƕ�
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
    /// ���ζ���ʽ��ֵ�㷨
    /// </summary>
    /// <param name="theta0">��ʼ�Ƕ�</param>
    /// <param name="thetaf">�յ�Ƕ�</param>
    /// <param name="Tf">�ܲ岹ʱ��</param>
    /// <param name="t">�Ѳ岹ʱ��</param>
    /// <param name="S0">��ʼ�ٶ�</param>
    /// <param name="Sf">��ֹ�ٶ�</param>
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
        //Debug.Log("�ѽ��в岹ʱ����" + t);
        float theta_T = _a0 + _a1 * t + _a2 * t * t + _a3 * t * t * t;
        RotateFixedAngle_FixedTime(theta_T);
        if (t == Tf)
        {
            GSKDATA.AxisRunning = false;//�岹����
            GSKDATA.MOVJ_t = 0f;
            
            //Debug.Log(angle);
            //float[] ff = CurrentAngle_All();
            //Debug.Log(ff[0] + "," + ff[1] + "," + ff[2] + "," + ff[3] + "," + ff[4] + "," + ff[5]);
        }
    }

}
