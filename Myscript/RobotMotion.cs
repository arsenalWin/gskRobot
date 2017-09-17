//<summary>
//Motion#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using System.Collections;
using System;

public static class MOVJCLASS
{
    public static bool run = false;
    public static float runtime = 0;
    public static float[] StartPos;
    public static float[] EndPos;
    public static void StartRun()
    {
        runtime = 0;
        run = true;
    }
    public static void EndRun()
    {
        runtime = 0;
        run = false;
    }
    
}

public class Axis7
{
    float rotSpeed = 0.1f;
    float rotR = 0.2f;

    public float Angle;
    public Vector3 trans;

    public Axis7()
    {
        Angle = 0;
        trans = Vector3.zero;
    }
    
    public void add()
    {
        if (Angle < 16)
        {
            Angle += rotSpeed * GSKDATA.ManualSpeed;
            trans = rotSpeed * rotR * new Vector3(1, 0, 0) * GSKDATA.ManualSpeed;
        }
        if(Angle > 16)
        {
            Angle = 16;
            trans = Vector3.zero;
        }
    }

    public void minus()
    {
        if (Angle > 0)
        {
            Angle -= rotSpeed * GSKDATA.ManualSpeed;
            trans = rotSpeed * rotR * new Vector3(-1, 0, 0) * GSKDATA.ManualSpeed;
        }
        if (Angle < 0)
        {
            Angle = 0;
            trans = Vector3.zero;
        }
    }

    public void JointInterpolation_3(float theta0, float thetaf, float Tf, float t, float S0 = 0, float Sf = 0)
    {
        
        if (t > Tf)
        {
            t = Tf;
        }
        //Debug.Log("已进行插补时长：" + t);
        float theta_T = (thetaf - theta0) / Tf * t;
        if (theta_T > 16)
        {
            theta_T = 16;
        }
        if (!GSKDATA.SystemWrong)
        {
            trans = (theta_T-Angle) * rotR * new Vector3(1, 0, 0);
            Angle = theta_T;
        }

        if (t == Tf)
        {
            Debug.Log(Angle);
            GSKDATA.AxisRunning = false;//插补结束
            GSKDATA.MOVJ_t = 0f;

        }
    }
}

public class RobotMotion : MonoBehaviour {
    ScreenBuild ScreenScript;
    ButtonRespond Button;

    public JointMove JMove_1;
    public JointMove JMove_2;
    public JointMove JMove_3;
    public JointMove JMove_4;
    public JointMove JMove_5;
    public JointMove JMove_6;
    public InverseKinematicsAlgorithm IKA;
    GameObject J1;
    GameObject J2;
    GameObject J3;
    GameObject J4;
    GameObject J5;
    GameObject J6;

    GameObject robot;
    Axis7 axis7;



    void Start()
    {
        ScreenScript = GameObject.Find("MyScreen").GetComponent<ScreenBuild>();
        Button = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
        DefineJMove("case1");

        axis7 = new Axis7();
        
    }

	// Update is called once per frame
	void Update ()
    {
        #region-------程序自动执行运动部分-----
        if (GSKDATA.GoPress || GSKDATA.BackPress||GSKDATA.ReappearRun)
        {
            if (GSKDATA.AxisRunning)
            {
                switch (GSKDATA.RBP_result)
                {
                    case "MOVJ":
                        GSKDATA.MOVJ_t += GSKDATA.D_time;//这里的先后顺序一定要注意
                        MOVJ(GSKDATA.MOVJ_t);
                        break;
                    case "MOVL":
                        GSKDATA.MOVL_t += GSKDATA.D_time;//这里的先后顺序一定要注意
                        MOVL(GSKDATA.MOVL_t);
                        break;
                    case "MOVC":
                        GSKDATA.MOVC_t += GSKDATA.D_time;//这里的先后顺序一定要注意
                        MOVC(GSKDATA.MOVC_t);
                        break;
                    default:
                        break;
                }
                GSKDATA.VList.Add(J5.transform.position);//记录点
            }
        }
        #endregion

        if (MOVJCLASS.run)
        {
            MOVJC(MOVJCLASS.runtime);
        }

    }

    void OnGUI()
    {
        if (GSKDATA.AxisRunning)
        {
            if (GSKDATA.AXIS_1)
            {
                JMove1(true);
            }
            else if (GSKDATA.AXIS_1_)
            {
                JMove1(false);
            }
            else if (GSKDATA.AXIS_2)
            {
                JMove2(true);
            }
            else if (GSKDATA.AXIS_2_)
            {
                JMove2(false);
            }
            else if (GSKDATA.AXIS_3)
            {
                JMove3(true);
            }
            else if (GSKDATA.AXIS_3_)
            {
                JMove3(false);
            }
            else if (GSKDATA.AXIS_4)
            {
                JMove4(true);
            }
            else if (GSKDATA.AXIS_4_)
            {
                JMove4(false);
            }
            else if (GSKDATA.AXIS_5)
            {
                JMove5(true);
            }
            else if (GSKDATA.AXIS_5_)
            {
                JMove5(false);
            }
            else if (GSKDATA.AXIS_6)
            {
                JMove6(true);
            }
            else if (GSKDATA.AXIS_6_)
            {
                JMove6(false);
            }
           
        }

        //if(GUILayout.Button("angle")){
        //    Debug.Log(axis7.Angle);
        //}
    }

    [RPC]
    public void JMove1(bool ZorF)
    {
        switch (GSKDATA.MotionCoordinateSystem)
        {
            case 1:  //关节坐标系
                if (ZorF)
                {
                    JX();
                }
                else
                {
                    JX_();
                }
                break;
            case 2:  //基座标
                if (ZorF)
                {
                    BX();
                }
                else
                {
                    BX_();
                }
                break;
            case 3:  //工具座标
                if (ZorF)
                {
                    TX();
                }
                else
                {
                    TX_();
                }
                break;
            case 4:  //用户座标
                if (ZorF)
                {
                    UX();
                }
                else
                {
                    UX_();
                }
                break;
            case 5:  //7zhou
                if (ZorF)
                {
                    EX();
                }
                else
                {
                    EX_();
                }
                break;
        }

    }
    [RPC]
    public void JMove2(bool ZorF)
    {
        switch (GSKDATA.MotionCoordinateSystem)
        {
            case 1:  //关节坐标系
                if (ZorF)
                {
                    JY();
                }
                else
                {
                    JY_();
                }
                break;
            case 2:  //基座标
                if (ZorF)
                {
                    BY();
                }
                else
                {
                    BY_();
                }
                break;
            case 3:  //工具座标
                if (ZorF)
                {
                    TY();
                }
                else
                {
                    TY_();
                }
                break;
            case 4:  //用户座标
                if (ZorF)
                {
                    UY();
                }
                else
                {
                    UY_();
                }
                break;
        }

    }
    [RPC]
    public void JMove3(bool ZorF)
    {
        switch (GSKDATA.MotionCoordinateSystem)
        {
            case 1:  //关节坐标系
                if (ZorF)
                {
                    JZ();
                }
                else
                {
                    JZ_();
                }
                break;
            case 2:  //基座标
                if (ZorF)
                {
                    BZ();
                }
                else
                {
                    BZ_();
                }
                break;
            case 3:  //工具座标
                if (ZorF)
                {
                    TZ();
                }
                else
                {
                    TZ_();
                }
                break;
            case 4:  //用户座标
                if (ZorF)
                {
                    UZ();
                }
                else
                {
                    UZ_();
                }
                break;
        }

    }
    [RPC]
    public void JMove4(bool ZorF)
    {
        switch (GSKDATA.MotionCoordinateSystem)
        {
            case 1:  //关节坐标系
                if (ZorF)
                {
                    JA();
                }
                else
                {
                    JA_();
                }
                break;
            case 2:  //基座标
                if (ZorF)
                {
                    BA();
                }
                else
                {
                    BA_();
                }
                break;
            case 3:  //工具座标
                if (ZorF)
                {
                    TA();
                }
                else
                {
                    TA_();
                }
                break;
            case 4:  //用户座标
                if (ZorF)
                {
                    UA();
                }
                else
                {
                    UA_();
                }
                break;
        }

    }
    [RPC]
    public void JMove5(bool ZorF)
    {
        switch (GSKDATA.MotionCoordinateSystem)
        {
            case 1:  //关节坐标系
                if (ZorF)
                {
                    JB();
                }
                else
                {
                    JB_();
                }
                break;
            case 2:  //基座标
                if (ZorF)
                {
                    BB();
                }
                else
                {
                    BB_();
                }
                break;
            case 3:  //工具座标
                if (ZorF)
                {
                    TB();
                }
                else
                {
                    TB_();
                }
                break;
            case 4:  //用户座标
                if (ZorF)
                {
                    UB();
                }
                else
                {
                    UB_();
                }
                break;
        }

    }
    [RPC]
    public void JMove6(bool ZorF)
    {
        switch (GSKDATA.MotionCoordinateSystem)
        {
            case 1:  //关节坐标系
                if (ZorF)
                {
                    JC();
                }
                else
                {
                    JC_();
                }
                break;
            case 2:  //基座标
                if (ZorF)
                {
                    BC();
                }
                else
                {
                    BC_();
                }
                break;
            case 3:  //工具座标
                if (ZorF)
                {
                    TC();
                }
                else
                {
                    TC_();
                }
                break;
            case 4:  //用户座标
                if (ZorF)
                {
                    UC();
                }
                else
                {
                    UC_();
                }
                break;
        }

    }

    //计算出各个臂的转角
    public float[] CurrentAngle_All()
    {
        return new float[] { JMove_1.Angle, JMove_2.Angle, JMove_3.Angle, JMove_4.Angle, JMove_5.Angle, JMove_6.Angle, axis7.Angle, 0 };
    }

    //转动角度限制
    public bool AngleLimit(int xuhao, float angle)
    {
        switch (xuhao)
        {
            case 1:
                if (angle >= JMove_1._SoftLimit && angle <= JMove_1.SoftLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 2:
                if (angle >= JMove_2._SoftLimit && angle <= JMove_2.SoftLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 3:
                if (angle >= JMove_3._SoftLimit && angle <= JMove_3.SoftLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 4:
                if (angle >= JMove_4._SoftLimit && angle <= JMove_4.SoftLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 5:
                if (angle >= JMove_5._SoftLimit && angle <= JMove_5.SoftLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 6:
                if (angle >= JMove_6._SoftLimit && angle <= JMove_6.SoftLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            default:
                Debug.LogError("没有找到相应的轴");
                return false;

        }
    }
    //定义机器人
    public void DefineJMove(string name)
    {
        switch (name)
        {
            case "case1": case "case2":
                IKA = new InverseKinematicsAlgorithm(GSKDATA.ROBOT1_a1,GSKDATA.ROBOT1_a2,GSKDATA.ROBOT1_a3,GSKDATA.ROBOT1_d4);
                J1 = GameObject.Find("MechanicalArm_11");
                J2 = GameObject.Find("MechanicalArm_21");
                J3 = GameObject.Find("MechanicalArm_31");
                J4 = GameObject.Find("MechanicalArm_41");
                J5 = GameObject.Find("MechanicalArm_51");
                J6 = GameObject.Find("MechanicalArm_61");
                JMove_1 = new JointMove(J1, new Vector3(0, 1, 0));
                JMove_2 = new JointMove(J2, new Vector3(0, 0, 1));
                JMove_3 = new JointMove(J3, new Vector3(0, 0, 1));
                JMove_4 = new JointMove(J4, new Vector3(-1, 0, 0));
                JMove_5 = new JointMove(J5, new Vector3(0, 0, 1));
                JMove_6 = new JointMove(J6, new Vector3(-1, 0, 0));
                //设置软极限
                JMove_1.SoftLimit = 180;
                JMove_1._SoftLimit = -180;
                JMove_2.SoftLimit = 120;
                JMove_2._SoftLimit = -80;
                JMove_3.SoftLimit = 80;
                JMove_3._SoftLimit = -120;
                JMove_4.SoftLimit = 170;
                JMove_4._SoftLimit = -170;
                JMove_5.SoftLimit = 130;
                JMove_5._SoftLimit = -130;
                JMove_6.SoftLimit = 360;
                JMove_6._SoftLimit = -360;
                AxisPositionSet(0f, 0f, 0, 0, 0, 0);
                break;
			case "case3":
				IKA = new InverseKinematicsAlgorithm(GSKDATA.ROBOT1_A1,GSKDATA.ROBOT1_A2,GSKDATA.ROBOT1_A3,GSKDATA.ROBOT1_D4);
				J1 = GameObject.Find("MechanicalArm_13");
				J2 = GameObject.Find("MechanicalArm_23");
				J3 = GameObject.Find("MechanicalArm_33");
				J4 = GameObject.Find("MechanicalArm_43");
				J5 = GameObject.Find("MechanicalArm_53");
				J6 = GameObject.Find("MechanicalArm_63");
				JMove_1 = new JointMove(J1, new Vector3(0, 1, 0));
				JMove_2 = new JointMove(J2, new Vector3(0, 0, 1));
				JMove_3 = new JointMove(J3, new Vector3(0, 0, 1));
				JMove_4 = new JointMove(J4, new Vector3(-1, 0, 0));
				JMove_5 = new JointMove(J5, new Vector3(0, 0, 1));
				JMove_6 = new JointMove(J6, new Vector3(-1, 0, 0));
				//设置软极限
				JMove_1.SoftLimit = 180;
				JMove_1._SoftLimit = -180;
				JMove_2.SoftLimit = 120;
				JMove_2._SoftLimit = -80;
				JMove_3.SoftLimit = 80;
				JMove_3._SoftLimit = -120;
				JMove_4.SoftLimit = 170;
				JMove_4._SoftLimit = -170;
				JMove_5.SoftLimit = 130;
				JMove_5._SoftLimit = -130;
				JMove_6.SoftLimit = 360;
				JMove_6._SoftLimit = -360;
				AxisPositionSet(0f,-17f,17,0,65,0);
				break;
            case "case4":
                IKA = new InverseKinematicsAlgorithm(GSKDATA.ROBOT4_A1, GSKDATA.ROBOT4_A2, GSKDATA.ROBOT4_A3, GSKDATA.ROBOT4_D4);
                J1 = GameObject.Find("MechanicalArm_14");
                J2 = GameObject.Find("MechanicalArm_24");
                J3 = GameObject.Find("MechanicalArm_34");
                J4 = GameObject.Find("MechanicalArm_44");
                J5 = GameObject.Find("MechanicalArm_54");
                J6 = GameObject.Find("MechanicalArm_64");
                JMove_1 = new JointMove(J1, new Vector3(0, 1, 0));
                JMove_2 = new JointMove(J2, new Vector3(0, 0, 1));
                JMove_3 = new JointMove(J3, new Vector3(0, 0, 1));
                JMove_4 = new JointMove(J4, new Vector3(-1, 0, 0));
                JMove_5 = new JointMove(J5, new Vector3(0, 0, 1));
                JMove_6 = new JointMove(J6, new Vector3(-1, 0, 0));
                //设置软极限
                JMove_1.SoftLimit = 180;
                JMove_1._SoftLimit = -180;
                JMove_2.SoftLimit = 120;
                JMove_2._SoftLimit = -80;
                JMove_3.SoftLimit = 80;
                JMove_3._SoftLimit = -120;
                JMove_4.SoftLimit = 170;
                JMove_4._SoftLimit = -170;
                JMove_5.SoftLimit = 130;
                JMove_5._SoftLimit = -130;
                JMove_6.SoftLimit = 360;
                JMove_6._SoftLimit = -360;
                AxisPositionSet(0f, -35f, 37, 0, 35, 0);
                break;
            case "case6":
                IKA = new InverseKinematicsAlgorithm(GSKDATA.ROBOT1_A1, GSKDATA.ROBOT1_A2, GSKDATA.ROBOT1_A3, GSKDATA.ROBOT1_D4);
                J1 = GameObject.Find("MechanicalArm_13");
                J2 = GameObject.Find("MechanicalArm_23");
                J3 = GameObject.Find("MechanicalArm_33");
                J4 = GameObject.Find("MechanicalArm_43");
                J5 = GameObject.Find("MechanicalArm_53");
                J6 = GameObject.Find("MechanicalArm_63_2");
                JMove_1 = new JointMove(J1, new Vector3(0, 1, 0));
                JMove_2 = new JointMove(J2, new Vector3(0, 0, 1));
                JMove_3 = new JointMove(J3, new Vector3(0, 0, 1));
                JMove_4 = new JointMove(J4, new Vector3(-1, 0, 0));
                JMove_5 = new JointMove(J5, new Vector3(0, 0, 1));
                JMove_6 = new JointMove(J6, new Vector3(-1, 0, 0));
                //设置软极限
                JMove_1.SoftLimit = 180;
                JMove_1._SoftLimit = -180;
                JMove_2.SoftLimit = 120;
                JMove_2._SoftLimit = -80;
                JMove_3.SoftLimit = 80;
                JMove_3._SoftLimit = -120;
                JMove_4.SoftLimit = 170;
                JMove_4._SoftLimit = -170;
                JMove_5.SoftLimit = 130;
                JMove_5._SoftLimit = -130;
                JMove_6.SoftLimit = 360;
                JMove_6._SoftLimit = -360;
                AxisPositionSet(160f, 0f, 0, 0, 90, 0);
                break;
            case "case5":
                IKA = new InverseKinematicsAlgorithm(GSKDATA.ROBOT1_A1, GSKDATA.ROBOT1_A2, GSKDATA.ROBOT1_A3, GSKDATA.ROBOT1_D4);
                J1 = GameObject.Find("MechanicalArm_16");
                J2 = GameObject.Find("MechanicalArm_26");
                J3 = GameObject.Find("MechanicalArm_36");
                J4 = GameObject.Find("MechanicalArm_46");
                J5 = GameObject.Find("MechanicalArm_56");
                J6 = GameObject.Find("MechanicalArm_66");
                robot = GameObject.Find("robot6");
                JMove_1 = new JointMove(J1, new Vector3(0, 1, 0));
                JMove_2 = new JointMove(J2, new Vector3(1, 0, 0));
                JMove_3 = new JointMove(J3, new Vector3(1, 0, 0));
                JMove_4 = new JointMove(J4, new Vector3(0, 0, -1));
                JMove_5 = new JointMove(J5, new Vector3(1, 0, 0));
                JMove_6 = new JointMove(J6, new Vector3(0, 0, -1));
                //设置软极限
                JMove_1.SoftLimit = 180;
                JMove_1._SoftLimit = -180;
                JMove_2.SoftLimit = 120;
                JMove_2._SoftLimit = -80;
                JMove_3.SoftLimit = 80;
                JMove_3._SoftLimit = -120;
                JMove_4.SoftLimit = 170;
                JMove_4._SoftLimit = -170;
                JMove_5.SoftLimit = 130;
                JMove_5._SoftLimit = -130;
                JMove_6.SoftLimit = 360;
                JMove_6._SoftLimit = -360;
                AxisPositionSet(0f, -46f, 45f, 0f, 30f, 0f);
                break;
        }

    }
    //机器人返回到零点
    public void ReturnToZero()
    {
        JMove_1.ReturnToZero();
        JMove_2.ReturnToZero();
        JMove_3.ReturnToZero();
        JMove_4.ReturnToZero();
        JMove_5.ReturnToZero();
        JMove_6.ReturnToZero();
        axis7.Angle = 0; 
        if(GSKDATA.Scene_NO == 5)
        {
            robot.transform.position = new Vector3(-14.190791f, robot.transform.position.y, robot.transform.position.z);
        }

        GSKDATA.IsCollide = false;
        if (GSKDATA.AxisRunning)
        {
            ScreenScript.OnScram();
            GSKDATA.AxisRunning = !GSKDATA.AxisRunning;
        }
    }
    //位置和姿态
    public float[] PostureAndPosition()
    {
        float[] PP = new float[6];
        PP[0] = (J1.transform.position.z - J6.transform.position.z) * 1000;
        PP[1] = (J6.transform.position.x - J1.transform.position.x) * 1000;
        PP[2] = (J6.transform.position.y - J1.transform.position.y) * 1000;
        PP[3] = Vector3.Angle(-J1.transform.forward, -J6.transform.forward);
        PP[4] = Vector3.Angle(J1.transform.right, J6.transform.right);
        PP[5] = Vector3.Angle(-J1.transform.up, -J6.transform.up);
        return PP;
    }
    //各个轴的位置定位
    public void AxisPositionSet(float j1, float j2, float j3, float j4, float j5, float j6)
    {
        J1.transform.localEulerAngles = JMove_1.RotateDirect * j1;
        J2.transform.localEulerAngles = JMove_2.RotateDirect * j2;
        J3.transform.localEulerAngles = JMove_3.RotateDirect * j3;
        J4.transform.localEulerAngles = JMove_4.RotateDirect * j4;
        J5.transform.localEulerAngles = JMove_5.RotateDirect * j5;
        J6.transform.localEulerAngles = JMove_6.RotateDirect * j6;
        JMove_1.Angle = j1; JMove_3.Angle = j3; JMove_5.Angle = j5;
        JMove_2.Angle = j2; JMove_4.Angle = j4; JMove_6.Angle = j6;
        //J1.transform.localEulerAngles = new Vector3(0, j1, 0);
        //J2.transform.localEulerAngles = new Vector3(0, 0, j2);
        //J3.transform.localEulerAngles = new Vector3(0, 0, j3);
        //J4.transform.localEulerAngles = new Vector3(-j4, 0, 0);
        //J5.transform.localEulerAngles = new Vector3(0, 0, j5);
        //J6.transform.localEulerAngles = new Vector3(-j6, 0, 0);
        //JMove_1.Angle = j1; JMove_3.Angle = j3; JMove_5.Angle = j5;
        //JMove_2.Angle = j2; JMove_4.Angle = j4; JMove_6.Angle = j6;
    }
    public void AxisPositionSet(float[] AxisAngle)
    {
        J1.transform.localEulerAngles = new Vector3(0, AxisAngle[0], 0);
        J2.transform.localEulerAngles = new Vector3(0, 0, AxisAngle[1]);
        J3.transform.localEulerAngles = new Vector3(0, 0, AxisAngle[2]);
        J4.transform.localEulerAngles = new Vector3(-AxisAngle[3], 0, 0);
        J5.transform.localEulerAngles = new Vector3(0, 0, AxisAngle[4]);
        J6.transform.localEulerAngles = new Vector3(-AxisAngle[5], 0, 0);
        JMove_1.Angle = AxisAngle[0]; JMove_3.Angle = AxisAngle[2]; JMove_5.Angle = AxisAngle[4];
        JMove_2.Angle = AxisAngle[1]; JMove_4.Angle = AxisAngle[3]; JMove_6.Angle = AxisAngle[5];
    }

    /// <summary>
    /// 关节插补算法部分
    /// </summary>
    /// <param name="t"> 插补时长 </param>
    public void MOVJ(float t)
    {
        float tf = CalculateTime(GSKDATA.MOVJ_P1, GSKDATA.MOVJ_P2, GSKDATA.MOVJ_A1, GSKDATA.MOVJ_A2);  //完成插补所需的时间
        if (tf != 0)
        {
            JMove_1.JointInterpolation_3(GSKDATA.MOVJ_A1[0], GSKDATA.MOVJ_A2[0], tf, t);
            JMove_2.JointInterpolation_3(GSKDATA.MOVJ_A1[1], GSKDATA.MOVJ_A2[1], tf, t);
            JMove_3.JointInterpolation_3(GSKDATA.MOVJ_A1[2], GSKDATA.MOVJ_A2[2], tf, t);
            JMove_4.JointInterpolation_3(GSKDATA.MOVJ_A1[3], GSKDATA.MOVJ_A2[3], tf, t);
            JMove_5.JointInterpolation_3(GSKDATA.MOVJ_A1[4], GSKDATA.MOVJ_A2[4], tf, t);
            JMove_6.JointInterpolation_3(GSKDATA.MOVJ_A1[5], GSKDATA.MOVJ_A2[5], tf, t);
            axis7.JointInterpolation_3(GSKDATA.MOVJ_A1[6], GSKDATA.MOVJ_A2[6], tf, t);
            if (robot)
            {
                robot.transform.position += axis7.trans;
            }
            
        }
        else
        {
            GSKDATA.AxisRunning=false;//插补结束
            GSKDATA.MOVJ_t = 0f;
        }
    }

    public void MOVJC(float t)
    {
        float tf = 2f;  //完成插补所需的时间
        if (tf != 0)
        {
            JMove_1.JointInterpolation_3(MOVJCLASS.StartPos[0], MOVJCLASS.EndPos[0], tf, t);
            JMove_2.JointInterpolation_3(MOVJCLASS.StartPos[1], MOVJCLASS.EndPos[1], tf, t);
            JMove_3.JointInterpolation_3(MOVJCLASS.StartPos[2], MOVJCLASS.EndPos[2], tf, t);
            JMove_4.JointInterpolation_3(MOVJCLASS.StartPos[3], MOVJCLASS.EndPos[3], tf, t);
            JMove_5.JointInterpolation_3(MOVJCLASS.StartPos[4], MOVJCLASS.EndPos[4], tf, t);
            JMove_6.JointInterpolation_3(MOVJCLASS.StartPos[5], MOVJCLASS.EndPos[5], tf, t);
            MOVJCLASS.runtime += GSKDATA.D_time;
        }
        else
        {
            MOVJCLASS.EndRun();
        }
        if(tf<=t)
        {
            MOVJCLASS.EndRun();
        }
    }

    /// <summary>
    /// 直线插补算法部分
    /// </summary>
    /// <param name="t">已插补时长</param>
    public void MOVL(float t)
    {
        float Tb = GSKDATA.Speed / GSKDATA.MOVL_a; //加速段的时间
        float Lb = 0.5f * GSKDATA.MOVL_a * Tb * Tb;
        float L = Vector3.Distance(GSKDATA.MOVL_P1, GSKDATA.MOVL_P2);//直线运动的总位移
        float tf = 2 * Tb + (L - 2 * Lb) / GSKDATA.Speed;//直线运动的总时间
        //归一化
        if (tf != 0)
        {
            float L_b = Lb / L;
            float T_b = Tb / tf;
            float a_ = 2 * L_b / (T_b * T_b);
            t = t / tf;
            //
            float namta = 0f;
            if (t >= 0 && t <= T_b)
            {
                namta = 0.5f * a_ * t * t;
            }
            else if (t > T_b && t <= (1 - T_b))
            {
                namta = 0.5f * a_ * T_b * T_b + a_ * T_b * (t - T_b);
            }
            else if (t > (1 - T_b) && t <= 1)
            {
                namta = 0.5f * a_ * T_b * T_b + a_ * T_b * (t - T_b) - 0.5f * a_ * (t + T_b - 1) * (t + T_b - 1);
            }
            else if (t > 1)
            {
                namta = 1;
            }
            ///计算出插值点
            //位置
            Vector3 Inter_Point_P = Vector3.Lerp(GSKDATA.MOVL_P1, GSKDATA.MOVL_P2, namta);
            //姿态
            Quaternion Inter_Point_Z = Quaternion.Slerp(GSKDATA.MOVL_Z1, GSKDATA.MOVL_Z2, namta);

            float[] movl_A_inter = IKA.AcceptInterPointPosture(Inter_Point_P, Inter_Point_Z, CurrentAngle_All());
            //Debug.Log("movl_A_inter[1]:" + movl_A_inter[1]);
            JMove_1.RotateFixedAngle_FixedTime(movl_A_inter[0]);
            JMove_2.RotateFixedAngle_FixedTime(movl_A_inter[1]);
            JMove_3.RotateFixedAngle_FixedTime(movl_A_inter[2]);
            JMove_4.RotateFixedAngle_FixedTime(movl_A_inter[3]);
            JMove_5.RotateFixedAngle_FixedTime(movl_A_inter[4]);
            JMove_6.RotateFixedAngle_FixedTime(movl_A_inter[5]);

            if (namta == 1)
            {
                GSKDATA.AxisRunning = false;//插补结束
                GSKDATA.MOVL_t = 0f;
            }
        }
        else
        {
            tf = CalculateTime(GSKDATA.MOVL_P1, GSKDATA.MOVL_P2);
            if (tf != 0)
            {
                JMove_1.JointInterpolation_3(GSKDATA.MOVJ_A1[0], GSKDATA.MOVJ_A2[0], tf, t);
                JMove_2.JointInterpolation_3(GSKDATA.MOVJ_A1[1], GSKDATA.MOVJ_A2[1], tf, t);
                JMove_3.JointInterpolation_3(GSKDATA.MOVJ_A1[2], GSKDATA.MOVJ_A2[2], tf, t);
                JMove_4.JointInterpolation_3(GSKDATA.MOVJ_A1[3], GSKDATA.MOVJ_A2[3], tf, t);
                JMove_5.JointInterpolation_3(GSKDATA.MOVJ_A1[4], GSKDATA.MOVJ_A2[4], tf, t);
                JMove_6.JointInterpolation_3(GSKDATA.MOVJ_A1[5], GSKDATA.MOVJ_A2[5], tf, t);

            }
            else
            {
                GSKDATA.AxisRunning = false;//插补结束
                GSKDATA.MOVL_t = 0f;
            }
        }


    }

    /// <summary>
    /// 圆弧插补算法部分
    /// </summary>
    /// <param name="t">已插补时长</param>
    /// <returns></returns>
    public bool MOVC(float t)
    {
        float namta = t;
        ///求出圆心与半径
        //判断是否在一条直线上
        float det = GSKDATA.MOVC_P1.x * GSKDATA.MOVC_P2.y * GSKDATA.MOVC_P3.z + GSKDATA.MOVC_P1.y * GSKDATA.MOVC_P2.z * GSKDATA.MOVC_P3.x + GSKDATA.MOVC_P2.x * GSKDATA.MOVC_P3.y * GSKDATA.MOVC_P1.z - GSKDATA.MOVC_P1.z * GSKDATA.MOVC_P2.y * GSKDATA.MOVC_P3.x - GSKDATA.MOVC_P1.y * GSKDATA.MOVC_P2.x * GSKDATA.MOVC_P3.z - GSKDATA.MOVC_P1.x * GSKDATA.MOVC_P2.z * GSKDATA.MOVC_P2.y;
        if (det == 0)
        {
            GSKDATA.InterpolationMethod = "";
            return false; //报错
        }
        //圆心
        float k11 = (GSKDATA.MOVC_P1.y - GSKDATA.MOVC_P3.y) * (GSKDATA.MOVC_P2.z - GSKDATA.MOVC_P3.z) - (GSKDATA.MOVC_P2.y - GSKDATA.MOVC_P3.y) * (GSKDATA.MOVC_P1.z - GSKDATA.MOVC_P3.z);
        float k12 = (GSKDATA.MOVC_P2.x - GSKDATA.MOVC_P3.x) * (GSKDATA.MOVC_P1.z - GSKDATA.MOVC_P3.z) - (GSKDATA.MOVC_P1.x - GSKDATA.MOVC_P3.x) * (GSKDATA.MOVC_P2.z - GSKDATA.MOVC_P3.z);
        float k13 = (GSKDATA.MOVC_P1.x - GSKDATA.MOVC_P3.x) * (GSKDATA.MOVC_P2.y - GSKDATA.MOVC_P3.y) - (GSKDATA.MOVC_P2.x - GSKDATA.MOVC_P3.x) * (GSKDATA.MOVC_P1.y - GSKDATA.MOVC_P3.y);
        float k14 = -(GSKDATA.MOVC_P3.x * k11 + k12 * GSKDATA.MOVC_P3.y + k13 * GSKDATA.MOVC_P3.z);
        float k21 = (GSKDATA.MOVC_P2.x - GSKDATA.MOVC_P1.x);
        float k22 = (GSKDATA.MOVC_P2.y - GSKDATA.MOVC_P1.y);
        float k23 = (GSKDATA.MOVC_P2.z - GSKDATA.MOVC_P1.z);
        float k24 = -((GSKDATA.MOVC_P2.x * GSKDATA.MOVC_P2.x - GSKDATA.MOVC_P1.x * GSKDATA.MOVC_P1.x) + (GSKDATA.MOVC_P2.y * GSKDATA.MOVC_P2.y - GSKDATA.MOVC_P1.y * GSKDATA.MOVC_P1.y) + (GSKDATA.MOVC_P2.z * GSKDATA.MOVC_P2.z - GSKDATA.MOVC_P1.z * GSKDATA.MOVC_P1.z)) / 2;
        float k31 = (GSKDATA.MOVC_P3.x - GSKDATA.MOVC_P2.x);
        float k32 = (GSKDATA.MOVC_P3.y - GSKDATA.MOVC_P2.y);
        float k33 = (GSKDATA.MOVC_P3.z - GSKDATA.MOVC_P2.z);
        float k34 = -((GSKDATA.MOVC_P3.x * GSKDATA.MOVC_P3.x - GSKDATA.MOVC_P2.x * GSKDATA.MOVC_P2.x) + (GSKDATA.MOVC_P3.y * GSKDATA.MOVC_P3.y - GSKDATA.MOVC_P2.y * GSKDATA.MOVC_P2.y) + (GSKDATA.MOVC_P3.z * GSKDATA.MOVC_P3.z - GSKDATA.MOVC_P2.z * GSKDATA.MOVC_P2.z)) / 2;
        Matrix4x4 M0 = Matrix4x4.zero;
        Matrix4x4 M1 = Matrix4x4.zero;
        Matrix4x4 M2 = Matrix4x4.zero;
        M1.SetRow(0, new Vector4(k11, k12, k13, 0));
        M1.SetRow(1, new Vector4(k21, k22, k23, 0));
        M1.SetRow(2, new Vector4(k31, k32, k33, 0));
        M1.SetRow(3, new Vector4(0, 0, 0, 1));
        M2.SetColumn(0, new Vector4(-k14, -k24, -k34, 0));
        M2.SetColumn(1, new Vector4(0, 0, 0, 0));
        M2.SetColumn(2, new Vector4(0, 0, 0, 0));
        M2.SetColumn(3, new Vector4(0, 0, 0, 1));
        M0 = M1.inverse * M2;
        float X0 = M0[0, 0];
        float Y0 = M0[1, 0];
        float Z0 = M0[2, 0];
        //半径
        float R = Mathf.Sqrt((GSKDATA.MOVC_P1.x - X0) * (GSKDATA.MOVC_P1.x - X0) + (GSKDATA.MOVC_P1.y - Y0) * (GSKDATA.MOVC_P1.y - Y0) + (GSKDATA.MOVC_P1.z - Z0) * (GSKDATA.MOVC_P1.z - Z0));
        /////测试部分
        //新坐标系和基座标之间的变换矩阵
        Matrix4x4 T = Matrix4x4.zero;
        Vector3 a = Vector3.Normalize(new Vector3(k11, k12, k13));
        Vector3 n = Vector3.Normalize(new Vector3((GSKDATA.MOVC_P1.x - X0), (GSKDATA.MOVC_P1.y - Y0), (GSKDATA.MOVC_P1.z - Z0)));
        Vector3 o = Vector3.Cross(a, n);
        T.SetColumn(0, new Vector4(n.x, n.y, n.z));
        T.SetColumn(1, new Vector4(o.x, o.y, o.z));
        T.SetColumn(2, new Vector4(a.x, a.y, a.z));
        T.SetColumn(3, new Vector4(X0, Y0, Z0, 1));
        //圆弧上的点在新坐标系中的位置
        //三个点在新坐标系中的位置

        Vector3 Npoint0 = TransFromOld(new Vector3(X0, Y0, Z0), T);
        Vector3 Npoint1 = TransFromOld(GSKDATA.MOVC_P1, T);
        Vector3 Npoint2 = TransFromOld(GSKDATA.MOVC_P2, T);
        Vector3 Npoint3 = TransFromOld(GSKDATA.MOVC_P3, T);

        //进行插补
        float theta2 = 0f;
        float theta3 = 0f;
        if (Npoint2.y >= 0)
        {
            theta2 = Mathf.Atan2(Npoint2.y, Npoint2.x);
        }
        else
            theta2 = Mathf.Atan2(Npoint2.y, Npoint2.x) + 360;

        if (Npoint3.y >= 0)
        {
            theta3 = Mathf.Atan2(Npoint3.y, Npoint3.x);
        }
        else
            theta3 = Mathf.Atan2(Npoint3.y, Npoint3.x) + 2 * Mathf.PI;

        float tf = theta3 * R / GSKDATA.Speed;
        float tf_2 = theta2 * 2 * Mathf.PI * R / 360 / GSKDATA.Speed;
        t = t / tf; //归一化
        float theta = 0f;
        if (t > 1)
        {
            t = 1;
            GSKDATA.AxisRunning = false;//插补结束
            GSKDATA.MOVC_t = 0f;
        }
        theta = t * theta3;
        float u = R * Mathf.Cos(theta);
        float v = R * Mathf.Sin(theta);
        //插补点在久坐标系中的位置
        //位置
        Vector3 Inter_Point_P = TransFromNew(new Vector3(u, v, 0), T);
        //姿态
        Quaternion Inter_Point_Z;
        if (namta < tf_2)
        {
            Inter_Point_Z = Quaternion.Slerp(GSKDATA.MOVC_Z1, GSKDATA.MOVC_Z2, namta / tf_2);
        }
        else
        {
            Inter_Point_Z = Quaternion.Slerp(GSKDATA.MOVC_Z2, GSKDATA.MOVC_Z3, (namta - tf_2) / (tf - tf_2));
        }
        float[] movl_A_inter = IKA.AcceptInterPointPosture(Inter_Point_P, Inter_Point_Z, CurrentAngle_All());

        JMove_1.RotateFixedAngle_FixedTime(movl_A_inter[0]);
        JMove_2.RotateFixedAngle_FixedTime(movl_A_inter[1]);
        JMove_3.RotateFixedAngle_FixedTime(movl_A_inter[2]);
        JMove_4.RotateFixedAngle_FixedTime(movl_A_inter[3]);
        JMove_5.RotateFixedAngle_FixedTime(movl_A_inter[4]);
        JMove_6.RotateFixedAngle_FixedTime(movl_A_inter[5]);

        return true;
    }
    /// <summary>
    /// 计算完成运动所需的时间
    /// </summary>
    /// <param name="P1">起始点</param>
    /// <param name="P2">终点</param>
    /// <returns></returns>
    float CalculateTime(Vector3 P1, Vector3 P2)
    {
        float L = Vector3.Distance(P1, P2); //空间轨迹点之间的距离
        float tt = L / GSKDATA.Speed;
        //Debug.Log("插补时间：" + tt);
        if (tt == 0)
        {
            float D_value = 0;
            for (int i = 0; i < 8; i++)
            {
                if (Math.Abs((GSKDATA.MOVJ_A1[i] - GSKDATA.MOVJ_A2[i])) > D_value)
                {
                    D_value = Math.Abs((GSKDATA.MOVJ_A1[i] - GSKDATA.MOVJ_A2[i]));
                }
            }
            if (D_value != 0)
            {
                tt = D_value / (GSKDATA.Speed * 0.5f);
            }

        }
        return tt;
    }

    float CalculateTime(Vector3 P1, Vector3 P2, float[] A1, float[] A2)
    {
        float L = Vector3.Distance(P1, P2); //空间轨迹点之间的距离
        float tt = L / GSKDATA.Speed;
        float D_value = 0;
        for (int i = 0; i < 6; i++)
        {
            if (Math.Abs((A1[i] - A2[i])) > D_value)
            {
                D_value = Math.Abs((A1[i] - A2[i]));
            }
        }
        if (robot)
        {
            for (int i = 6; i < 8; i++)
            {
                if (Math.Abs((A1[i] - A2[i])) > D_value)
                {
                    D_value = Math.Abs((A1[i] - A2[i]));
                }
            }
        }
        float tt2 = D_value / (GSKDATA.Speed * 20);
        if (tt < tt2)
        {
            return tt2;
        }
        else
            return tt;
    }
    /// <summary>
    /// 返回圆弧点在新坐标系的坐标
    /// </summary>
    /// <param name="Point"></param>
    /// <param name="TT"></param>
    /// <returns></returns>
    public Vector3 TransFromOld(Vector3 Point, Matrix4x4 TT)
    {
        Vector3 newPoint = Vector3.zero;
        Matrix4x4 T_N = Matrix4x4.zero;
        Matrix4x4 T_O = Matrix4x4.zero;
        T_O.SetColumn(0, new Vector4(Point.x, Point.y, Point.z, 1));
        T_O.SetColumn(3, new Vector4(0, 0, 0, 1));
        T_N = TT.inverse * T_O;
        newPoint.x = T_N[0, 0];
        newPoint.y = T_N[1, 0];
        newPoint.z = T_N[2, 0];
        //Debug.Log("圆弧点在新坐标系下的坐标：" + newPoint.ToString("0.000"));
        return newPoint;
    }

    /// <summary>
    /// 将圆弧上的点变换到原本的坐标系中去
    /// </summary>
    /// <param name="Point"></param>
    /// <param name="TT"></param>
    /// <returns></returns>
    public Vector3 TransFromNew(Vector3 Point, Matrix4x4 TT)
    {
        Vector3 oldPoint = Vector3.zero;
        Matrix4x4 T_N = Matrix4x4.zero;
        Matrix4x4 T_O = Matrix4x4.zero;
        T_O.SetColumn(0, new Vector4(Point.x, Point.y, Point.z, 1));
        T_O.SetColumn(3, new Vector4(0, 0, 0, 1));
        T_N = TT * T_O;
        oldPoint.x = T_N[0, 0];
        oldPoint.y = T_N[1, 0];
        oldPoint.z = T_N[2, 0];
        //Debug.Log("圆弧差补点在旧坐标系下的坐标：" + oldPoint.ToString("0.000"));
        return oldPoint;
    }
    //机器人的引导运动
    public void GuidMove(Vector3 Direction)
    {
        const float rate1 = 0.01f;
        Vector3 StartPoint = IKA.SolutionOfKinematics(CurrentAngle_All());
        Quaternion Posture = IKA.SolutionOfKinematics_posture(CurrentAngle_All());
        Vector3 EndPoint = StartPoint + Direction * rate1;
        float[] movl_A_inter = IKA.AcceptInterPointPosture(EndPoint, Posture, CurrentAngle_All());

        if (!JMove_1.RotateFixedAngle_FixedTime(movl_A_inter[0]) ||
        !JMove_2.RotateFixedAngle_FixedTime(movl_A_inter[1]) ||
        !JMove_3.RotateFixedAngle_FixedTime(movl_A_inter[2]) ||
        !JMove_4.RotateFixedAngle_FixedTime(movl_A_inter[3]) ||
        !JMove_5.RotateFixedAngle_FixedTime(movl_A_inter[4]) ||
        !JMove_6.RotateFixedAngle_FixedTime(movl_A_inter[5]))
        {
            StartCoroutine(ScreenScript.WarningYellow("运动极限"));
        }
        GSKDATA.VList.Add(J5.transform.position);//记录点
    }
    
    #region ----------------------|J|B|T|U坐标下的各个机械臂的转动-------------
    private void JX(){
        if (!JMove_1.Rotate(GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J1达到软极限+"));
        }
    }
    private void JX_(){
        if (!JMove_1.Rotate(-GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J1达到软极限-"));
        }
    }
    private void JY(){
        if (!JMove_2.Rotate(GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J2达到软极限+"));
        }
    }
    private void JY_(){
        if (!JMove_2.Rotate(-GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J2达到软极限-"));
        }
    }
    private void JZ(){
        if (!JMove_3.Rotate(GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J3达到软极限+"));
        }
    }
    private void JZ_(){
        if (!JMove_3.Rotate(-GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J3达到软极限-"));
        }
    }
    private void JA(){
        if (!JMove_4.Rotate(GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J4达到软极限+"));
        }
    }
    private void JA_(){
        if (!JMove_4.Rotate(-GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J4达到软极限-"));
        }
    }
    private void JB(){
        if (!JMove_5.Rotate(GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J5达到软极限+"));
        }
    }
    private void JB_(){
        if (!JMove_5.Rotate(-GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J5达到软极限-"));
        }
    }
    private void JC(){
        if (!JMove_6.Rotate(GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J6达到软极限+"));
        }
    }
    private void JC_(){
        if (!JMove_6.Rotate(-GSKDATA.ManualSpeed))
        {
            StartCoroutine(ScreenScript.WarningYellow("J6达到软极限-"));
        }
    }

    /// <summary>
    /// 基座标的X正向运动
    /// </summary>
    private void BX()
    {
        XYZ_Move(new Vector3(1, 0, 0));
    }
    /// <summary>
    /// 基座标的X负向运动
    /// </summary>
    private void BX_()
    {
        XYZ_Move(new Vector3(-1, 0, 0));
    }

    /// <summary>
    /// 基座标的Y正向运动
    /// </summary>
    private void BY()
    {
        XYZ_Move(new Vector3(0, 1, 0));
    }

    /// <summary>
    /// 基座标的Y负向运动
    /// </summary>
    private void BY_()
    {
        XYZ_Move(new Vector3(0, -1, 0));
    }
    /// <summary>
    /// 基座标的Z正向运动
    /// </summary>
    private void BZ()
    {
        XYZ_Move(new Vector3(0, 0, 1));
    }

    /// <summary>
    /// 基座标的Z负向运动
    /// </summary>
    private void BZ_()
    {
        XYZ_Move(new Vector3(0, 0, -1));
    }
    /// <summary>
    /// 基座标的X轴正向旋转
    /// </summary>
    private void BA()
    {
        XYZ_Rotate(new Vector3(1, 0, 0));
    }

    /// <summary>
    /// 基座标的X轴反向旋转
    /// </summary>
    private void BA_()
    {
        XYZ_Rotate(new Vector3(-1, 0, 0));
    }
    /// <summary>
    /// 基座标的Y轴正向旋转
    /// </summary>
    private void BB()
    {
        XYZ_Rotate(new Vector3(0, 1, 0));
    }
    /// <summary>
    /// 基座标的Y轴反向旋转
    /// </summary>
    private void BB_()
    {
        XYZ_Rotate(new Vector3(0, -1, 0));
    }
    /// <summary>
    /// 基座标的Z轴正向旋转
    /// </summary>
    private void BC()
    {
        XYZ_Rotate(new Vector3(0, 0, 1));
    }
    /// <summary>
    /// 基座标的Z轴反向旋转
    /// </summary>
    private void BC_()
    {
        XYZ_Rotate(new Vector3(0, 0, -1));
    }
    /// <summary>
    /// 工具座标的X正向运动
    /// </summary>
    private void TX()
    {
        XYZ_Move(new Vector3(1, 0, 0));
    }

    /// <summary>
    /// 工具座标的X负向运动
    /// </summary>
    private void TX_()
    {
        XYZ_Move(new Vector3(-1, 0, 0));
    }

    /// <summary>
    /// 工具座标的Y正向运动
    /// </summary>
    private void TY()
    {
        XYZ_Move(new Vector3(0, 1, 0));
    }

    /// <summary>
    /// 工具座标的Y负向运动
    /// </summary>
    private void TY_()
    {
        XYZ_Move(new Vector3(0, -1, 0));
    }

    /// <summary>
    /// 工具座标的Z正向运动
    /// </summary>
    private void TZ()
    {
        XYZ_Move(new Vector3(0, 0, 1));
    }
    /// <summary>
    /// 工具座标的Z负向运动
    /// </summary>
    private void TZ_()
    {
        XYZ_Move(new Vector3(0, 0, -1));
    }
    /// <summary>
    /// 工具座标的X轴正向旋转
    /// </summary>
    private void TA()
    {
        XYZ_Rotate(new Vector3(1, 0, 0));
    }
    /// <summary>
    /// 工具座标的X轴负向旋转
    /// </summary>
    private void TA_()
    {
        XYZ_Rotate(new Vector3(-1, 0, 0));
    }
    /// <summary>
    /// 工具座标的Y轴正向旋转
    /// </summary>
    private void TB()
    {
        XYZ_Rotate(new Vector3(0, 1, 0));
    }
    /// <summary>
    /// 工具座标的Y轴负向旋转
    /// </summary>
    private void TB_()
    {
        XYZ_Rotate(new Vector3(0, -1, 0));
    }
    /// <summary>
    /// 工具座标的Z轴正向旋转
    /// </summary>
    private void TC()
    {
        XYZ_Rotate(new Vector3(0, 0, 1));
    }
    /// <summary>
    /// 工具座标的Z轴负向旋转
    /// </summary>
    private void TC_()
    {
        XYZ_Rotate(new Vector3(0, 0, -1));
    }

    /// <summary>
    /// 用户座标的X正向运动
    /// </summary>
    private void UX()
    {
        XYZ_Move(new Vector3(1, 0, 0));
    }

    private void EX()
    {
        axis7.add();
        robot.transform.position += axis7.trans;
    }

    private void EX_()
    {
        axis7.minus();
        robot.transform.position += axis7.trans;
    }

    /// <summary>
    /// 用户座标的X负向运动
    /// </summary>
    private void UX_()
    {
        XYZ_Move(new Vector3(-1, 0, 0));
    }

    /// <summary>
    /// 用户座标的Y正向运动
    /// </summary>
    private void UY()
    {
        XYZ_Move(new Vector3(0, 1, 0));
    }

    /// <summary>
    /// 用户座标的Y负向运动
    /// </summary>
    private void UY_()
    {
        XYZ_Move(new Vector3(0, -1, 0));
    }

    /// <summary>
    /// 用户座标的Z正向运动
    /// </summary>
    private void UZ()
    {
        XYZ_Move(new Vector3(0, 0, -1));
    }

    /// <summary>
    /// 用户座标的Z负向运动
    /// </summary>
    private void UZ_()
    {
        XYZ_Move(new Vector3(0, 0, -1));
    }

    /// <summary>
    /// 用户座标的X轴正向旋转
    /// </summary>
    private void UA()
    {
        XYZ_Rotate(new Vector3(1, 0, 0));
    }

    /// <summary>
    /// 用户座标的X轴负向旋转
    /// </summary>
    private void UA_()
    {
        XYZ_Rotate(new Vector3(-1, 0, 0));
    }

    /// <summary>
    /// 用户座标的Y轴正向旋转
    /// </summary>
    private void UB()
    {
        XYZ_Rotate(new Vector3(0, 1, 0));
    }

    /// <summary>
    /// 用户座标的Y轴负向旋转
    /// </summary>
    private void UB_()
    {
        XYZ_Rotate(new Vector3(0, -1, 0));
    }

    /// <summary>
    /// 用户座标的Z轴正向旋转
    /// </summary>
    private void UC()
    {
        XYZ_Rotate(new Vector3(0, 0, 1));
    }

    /// <summary>
    /// 用户座标的Z轴负向旋转
    /// </summary>
    private void UC_()
    {
        XYZ_Rotate(new Vector3(0, 0, -1));
    }

    private void XYZ_Move(Vector3 Direction)
    {
        const float rate11 = 0.001f;
        //Debug.Log("Direction:" + Direction.ToString("0.0000"));
        Vector3 StartPoint = IKA.SolutionOfKinematics(CurrentAngle_All());
        Quaternion Posture = IKA.SolutionOfKinematics_posture(CurrentAngle_All());
        Vector3 EndPoint = StartPoint + GSKDATA.ManualSpeed * Direction * rate11;
        float[] movl_A_inter = IKA.AcceptInterPointPosture(EndPoint, Posture, CurrentAngle_All());

        if (!JMove_1.RotateFixedAngle_FixedTime(movl_A_inter[0]) ||
        !JMove_2.RotateFixedAngle_FixedTime(movl_A_inter[1]) ||
        !JMove_3.RotateFixedAngle_FixedTime(movl_A_inter[2]) ||
        !JMove_4.RotateFixedAngle_FixedTime(movl_A_inter[3]) ||
        !JMove_5.RotateFixedAngle_FixedTime(movl_A_inter[4]) ||
        !JMove_6.RotateFixedAngle_FixedTime(movl_A_inter[5]))
        {
            StartCoroutine(ScreenScript.WarningYellow("运动极限"));
        }
        GSKDATA.VList.Add(J5.transform.position);//记录点
    }
    private void XYZ_Rotate(Vector3 Direction)
    {
       
    }
#endregion

    
}
