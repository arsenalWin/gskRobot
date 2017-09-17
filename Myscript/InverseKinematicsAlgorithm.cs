//<summary>
//InverseKinematicsAlgorithm.cs
//ROBOT
//Created by 周伟 on 6/21/2015.
//Company: Sunnytech
//Function:
//逆运动学算法
//
//<summary>
using UnityEngine;
using System.Collections;
using System;

public class InverseKinematicsAlgorithm {
    //机械臂的连杆长度
    float a1;
    float a2;
    float a3;
    float d3;
    float d4;
    public InverseKinematicsAlgorithm(float a_1,float a_2,float a_3,float d_4)
    {
        a1 = a_1;
        a2 = a_2;
        a3 = a_3;
        d4 = d_4;
        d3 = 0f;
    }
    RobotMotion MoveScript;
    bool Special = false;
    GameObject ZiTai; //存放中间点的姿态
    //逆运动求解的角度，用于求解下一个解
    float angle1 = 0f;
    float angle2 = 0f;
    float angle3 = 0f;
    float angle4 = 0f;
    float angle5 = 0f;
    float angle6 = 0f;
    //逆运动求解的角度，用于运动
    float theta1 = 0f;
    float theta2 = 0f;
    float theta3 = 0f;
    float theta4 = 0f;
    float theta5 = 0f;
    float theta6 = 0f;
    //用于求坐标6和坐标1之间的变换矩阵
    float Px;
    float Py;
    float Pz;
    float Ax;
    float Ay;
    float Az;
    float Ox;
    float Oy;
    float Oz;
    float Nx;
    float Ny;
    float Nz;

   
    /// <summary>
    /// 接受插值点的位姿
    /// 并转换成变换矩阵的形式
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="Zitai"></param>
   public float[] AcceptInterPointPosture(Vector3 Pos, Quaternion Zitai_,float[] CurrentA)
   {
       Px = Pos.x;
       Py = Pos.y;
       Pz = Pos.z;
       //将四元数转换成旋转矩阵
       Matrix4x4 Matri = GetMatrixLH1(Zitai_);
       Nx = Matri[0, 0];
       Ny = Matri[1, 0];
       Nz = Matri[2, 0];
       Ox = Matri[0, 1];
       Oy = Matri[1, 1];
       Oz = Matri[2, 1];
       Ax = Matri[0, 2];
       Ay = Matri[1, 2];
       Az = Matri[2, 2];
    

       theta1 = CurrentA[0];
       theta2 = CurrentA[1];
       theta3 = CurrentA[2];
       theta4 = CurrentA[3];
       theta5 = CurrentA[4];
       theta6 = CurrentA[5];

       ///计算出运动学逆解并返回
       if (SolvingA3())
       {
           //Debug.Log("true");
           return new float[] { theta1, theta2, theta3, theta4, theta5, theta6 };
       }
       //Debug.Log("false");
       return new float[] { CurrentA[0], CurrentA[1], CurrentA[2], CurrentA[3], CurrentA[4], CurrentA[5] };
   }

   #region 机器人机器人的逆运动学求解 方法三  10.23号之后用的方法
   public bool SolvingA3()
   {
       MoveScript = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
       Special = false;
       //theta1.....2
       float[] A_1 = new float[2];
       A_1[0] = Mathf.Atan2(Py, Px) - Mathf.Atan2(d3, Mathf.Sqrt(Px * Px + Py * Py - d3 * d3));
       A_1[1] = Mathf.Atan2(Py, Px) - Mathf.Atan2(d3, -Mathf.Sqrt(Px * Px + Py * Py - d3 * d3));
       if (Mathf.Abs(A_1[0] * 180 / Mathf.PI - theta1) < Mathf.Abs(A_1[1] * 180 / Mathf.PI - theta1))
       {
           angle1 = A_1[0];
       }
       else
           angle1 = A_1[1];
       theta1 = angle1 * 180 / Mathf.PI;
       if (float.IsNaN(theta1) || float.IsInfinity(theta1))
       {
           Debug.Log("theta1无穷大或者不是实数");
           return false;
       }
       
       //theta3......2
       float[] A_3 = new float[2];
       float k3 = (Px * Px + Py * Py + Pz * Pz + a1 * a1 - 2 * a1 * (Mathf.Cos(angle1) * Px + Mathf.Sin(angle1) * Py) - a2 * a2 - a3 * a3 - d3 * d3 - d4 * d4) / (2 * a2);
       A_3[0] = Mathf.Atan2(a3, d4) - Mathf.Atan2(k3, Mathf.Sqrt(a3 * a3 + d4 * d4 - k3 * k3));
       A_3[1] = Mathf.Atan2(a3, d4) - Mathf.Atan2(k3, -Mathf.Sqrt(a3 * a3 + d4 * d4 - k3 * k3));
       A_3[0] = A_3[0] * 180 / Mathf.PI;//转换成度数
       A_3[1] = A_3[1] * 180 / Mathf.PI;
       if ((float.IsNaN(A_3[0]) || float.IsInfinity(A_3[0])) && (float.IsNaN(A_3[1]) || float.IsInfinity(A_3[1])))
       {
           Debug.Log("theta3无穷大或者不是实数");
           angle3 = theta3 * Mathf.PI / 180;
           return false;
       }
       else
       {
           if (!MoveScript.AngleLimit(3, A_3[0]))
           {
               if (MoveScript.AngleLimit(3, A_3[0] + 360))
               {
                   A_3[0] = A_3[0] + 360;
                   Debug.Log("+360");
               }
               else if (MoveScript.AngleLimit(3, A_3[0] - 360))
               {
                   A_3[0] = A_3[0] - 360;
                   Debug.Log("-360");
               }
               else
               {
                   A_3[0] = 9999;
               }
           }
           if (!MoveScript.AngleLimit(3, A_3[1]))
           {
               if (MoveScript.AngleLimit(3, A_3[1] + 360))
               {
                   A_3[1] = A_3[1] + 360;
                   Debug.Log("+360");
               }
               else if (MoveScript.AngleLimit(3, A_3[1] - 360))
               {
                   A_3[1] = A_3[1] - 360;
                   Debug.Log("-360");
               }
               else
               {
                   A_3[1] = 9999;
               }

           }
           if (A_3[0] == 9999 && A_3[1] == 9999)
           {
               Debug.Log("theta3超过运动极限" + A_3);
               return false;
           }
           if (Mathf.Abs(A_3[0] - theta3) < Mathf.Abs(A_3[1] - theta3))
           {
               theta3 = A_3[0];
           }
           else
           {
               theta3 = A_3[1];
           }
           angle3 = theta3 / 180 * Mathf.PI;

       }
       
       
       

       //theta2.....2
       float[] A_2 = new float[2] ;
       float k21 = (-a3 - a2 * Mathf.Cos(angle3)) * Pz - (Mathf.Cos(angle1) * Px + Mathf.Sin(angle1) * Py - a1) * (d4 - a2 * Mathf.Sin(angle3));
       float k22 = (a2 * Mathf.Sin(angle3) - d4) * Pz + (a3 + a2 * Mathf.Cos(angle3)) * (Mathf.Cos(angle1) * Px + Mathf.Sin(angle1) * Py - a1);
       A_2[0] = Mathf.Atan2(k21, k22) - angle3 + Mathf.PI / 2;
       A_2[1] = Mathf.Atan2(k21, k22) - angle3 - Mathf.PI / 2;
       A_2[0] = A_2[0] * 180 / Mathf.PI;//转换成度数
       A_2[1] = A_2[1] * 180 / Mathf.PI;
       if ((float.IsNaN(A_2[0]) || float.IsInfinity(A_2[0])) && (float.IsNaN(A_2[1]) || float.IsInfinity(A_2[1])))
       {
           Debug.Log("theta2无穷大或者不是实数");
           return false;
       }
       else
       {
           if (!MoveScript.AngleLimit(2, A_2[0]))
           {
               if (MoveScript.AngleLimit(2, A_2[0] + 360))
               {
                   A_2[0] = A_2[0] + 360;
                   Debug.Log("+360");
               }
               else if (MoveScript.AngleLimit(2, A_2[0] - 360))
               {
                   A_2[0] = A_2[0] - 360;
                   Debug.Log("-360");
               }
               else
               {
                   A_2[0] = 9999;
               }
           }
           if (!MoveScript.AngleLimit(2, A_2[1]))
           {
               if (MoveScript.AngleLimit(2, A_3[1] + 360))
               {
                   A_2[1] = A_2[1] + 360;
                   Debug.Log("+360");
               }
               else if (MoveScript.AngleLimit(2, A_2[1] - 360))
               {
                   A_2[1] = A_2[1] - 360;
                   Debug.Log("-360");
               }
               else
               {
                   A_2[1] = 9999;
               }

           }
           if (A_2[0] == 9999 && A_2[1] == 9999)
           {
               Debug.Log("theta2超过运动极限");
               return false;
           }
           if (Mathf.Abs(A_2[0] - theta2) < Mathf.Abs(A_2[1] - theta2))
           {
               theta2 = A_2[0];
           }
           else
           {
               theta2 = A_2[1];
           }
           angle2 = (theta2 - 90) / 180 * Mathf.PI;//90 90 90

       }
       

       //theta4.....2
       float[] A_4=new float[2];
       float k41 = Mathf.Sin(angle1) * Ax - Mathf.Cos(angle1) * Ay;
       float k42 = Mathf.Cos(angle1) * Mathf.Cos(angle3 + angle2) * Ax + Mathf.Sin(angle1) * Mathf.Cos(angle3 + angle2) * Ay - Mathf.Sin(angle2 + angle3) * Az;
       A_4[0] = Mathf.Atan2(-k41, -k42);
       A_4[1] = Mathf.Atan2(-k41, -k42) + Mathf.PI;
       A_4[0] = A_4[0] * 180 / Mathf.PI;
       A_4[1] = A_4[1] * 180 / Mathf.PI;
       if (Mathf.Abs(-k41)< 0.00001f && Mathf.Abs(-k42) < 0.00001f)//奇异点
       {
           angle4 = (theta4) * Mathf.PI / 180;
       }
       else
       {
           if (float.IsNaN(theta4) || float.IsInfinity(theta4))
           {
               Debug.Log("theta4无穷大或者不是实数");
           }
           if (!MoveScript.AngleLimit(4, A_4[0]))
           {
               //Debug.Log("A_4[0]" + A_4[0]);
               A_4[0] = 9999;
           }
           if (!MoveScript.AngleLimit(4, A_4[1]))
           {
               if (MoveScript.AngleLimit(4, A_4[1] - 360))
               {
                   A_4[1] = A_4[1] - 360;
                   //Debug.Log("-360 :" + A_4[1]);
               }
               else
               {
                   A_4[1] = 9999;
               }
           }
           if (A_4[0] == 9999 && A_4[1] == 9999)
           {
               Debug.Log("theta4超过运动极限");
               return false;
           }
           else if (Mathf.Abs(A_4[0] - theta4) < Mathf.Abs(A_4[1] - theta4))
           {
               //if (Mathf.Abs(A_4[0] - theta4) < 90)
               //{
                   theta4 = A_4[0];
               //}
               //else
               //{

               //    Debug.Log("xxx" + A_4[0] + ",,," + A_4[1]);
               //}
           }
           else
           {
               //if (Mathf.Abs(A_4[1] - theta4) < 90)
               //{
                   theta4 = A_4[1];
               //}
               //else
               //{

               //    Debug.Log("xxx" + A_4[0] + ",,," + A_4[1]);
               //}
           }
           angle4 = theta4 / 180 * Mathf.PI;
       }


       //theta5.....1
       float A_5;
       float k51 = (Mathf.Sin(angle4) * Mathf.Sin(angle1) + Mathf.Cos(angle1) * Mathf.Cos(angle4) * Mathf.Cos(angle2 + angle3)) * Ax + (Mathf.Cos(angle4) * Mathf.Sin(angle1) * Mathf.Cos(angle2 + angle3) - Mathf.Cos(angle1) * Mathf.Sin(angle4)) * Ay - Mathf.Sin(angle2 + angle3) * Mathf.Cos(angle4) * Az;
       float k52 = -Mathf.Sin(angle2 + angle3) * Mathf.Cos(angle1) * Ax - Mathf.Sin(angle2 + angle3) * Mathf.Sin(angle1) * Ay - Mathf.Cos(angle2 + angle3) * Az;
       A_5 = Mathf.Atan2(-k51, k52);
       //float k51 = -Mathf.Cos(angle4) * k42 - Mathf.Sin(angle4) * k41;
       //float k52 = -Mathf.Sin(angle2 + angle3) * (Mathf.Cos(angle1) * Ax + Mathf.Sin(angle1) * Ay) + Mathf.Cos(angle2 + angle3) * Az;
       //A_5 = Mathf.Atan2(k51, k52);

       if (float.IsNaN(theta5) || float.IsInfinity(theta5))
       {
           Debug.Log("theta5无穷大或者不是实数");
           angle5 = (theta5) * Mathf.PI / 180;
           //return false;
       }
       else
       {
           A_5 = A_5 * 180 / Mathf.PI;
           if (!MoveScript.AngleLimit(5, A_5))
           {
               //Debug.Log("theta4 " + theta4);
               //Debug.Log("A_5: " + A_5);

               if (MoveScript.AngleLimit(5, A_5 + 360))
               {
                   A_5 = A_5 + 360;
                   //Debug.Log("A_5: " + A_5);
               }
               //else if (MoveScript.AngleLimit(5, A_5 - 180))
               //{
               //    A_5 = A_5 - 180;
               //    Debug.Log("A_5: " + A_5);
               //}
               //else
               //{
               //    Debug.Log("theta5超过运动极限" + A_5);
               //    return false;
               //}

               
           }
           //特殊处理部分,绕过突变点
           //if (Mathf.Abs(A_5 - theta5) > 20)
           //{
           //    Special = true;
           //    Debug.Log("tubian" + theta5);
           //}
           //else
           {
               theta5 = A_5;
               //Debug.Log("A_5 :"+A_5);
               angle5 = theta5 / 180 * Mathf.PI;
           }
           
       }
       

       //theta6.....1
       float A_6;
       float k61 = (Mathf.Cos(angle1) * Mathf.Sin(angle4) * Mathf.Cos(angle2 + angle3) - Mathf.Cos(angle4) * Mathf.Sin(angle1)) * Nx + (Mathf.Cos(angle1) * Mathf.Cos(angle4) + Mathf.Sin(angle1) * Mathf.Sin(angle4) * Mathf.Cos(angle2 + angle3)) * Ny - Mathf.Sin(angle2 + angle3) * Mathf.Sin(angle4) * Nz;
       float k62 = (Mathf.Sin(angle4) * Mathf.Sin(angle1) * Mathf.Cos(angle5) - Mathf.Cos(angle1) * Mathf.Sin(angle5) * Mathf.Sin(angle2 + angle3) + Mathf.Cos(angle1) * Mathf.Cos(angle4) * Mathf.Cos(angle2 + angle3) * Mathf.Cos(angle5)) * Nx - (Mathf.Cos(angle1) * Mathf.Sin(angle4) * Mathf.Cos(angle5) + Mathf.Sin(angle1) * Mathf.Sin(angle5) * Mathf.Sin(angle2 + angle3) - Mathf.Sin(angle1) * Mathf.Cos(angle4) * Mathf.Cos(angle5) * Mathf.Cos(angle2 + angle3)) * Ny + (-Mathf.Sin(angle5) * Mathf.Cos(angle2 + angle3) - Mathf.Cos(angle4) * Mathf.Cos(angle5) * Mathf.Sin(angle2 + angle3)) * Nz;
       A_6 = Mathf.Atan2(-k61, k62);
       //float V112 = Mathf.Cos(angle1) * Ox + Mathf.Sin(angle1) * Oy;
       //float V132 = Mathf.Sin(angle1) * Ox - Mathf.Cos(angle1) * Oy;
       //float V332 = -Mathf.Sin(angle2 + angle3) * V112 + Mathf.Cos(angle2 + angle3) * Oz;
       //float V312 = Mathf.Cos(angle2 + angle3) * V112 + Mathf.Sin(angle2 + angle3) * Oz;
       //float V432 = Mathf.Sin(angle4) * V312 + Mathf.Cos(angle4) * V132;
       //float V422 = V332;
       //float V412 = Mathf.Cos(angle4) * V312 - Mathf.Sin(angle4) * V132;
       //float k61 = -Mathf.Cos(angle5) * V412 - Mathf.Sin(angle5) * V422;
       //float k62 = -V432;
       //A_6 = Mathf.Atan2(k61, k62);

       if (float.IsNaN(theta6) || float.IsInfinity(theta6))
       {
           Debug.Log("theta6无穷大或者不是实数");
           angle6 = (theta6) * Mathf.PI / 180;
           //return false;
       }
       else
       {
           A_6 = A_6 * 180 / Mathf.PI;
           if (!MoveScript.AngleLimit(6, A_6))
           {
               if (MoveScript.AngleLimit(6, A_6 + 360))
               {
                   A_6 = A_6 + 360;
               }
               else if (MoveScript.AngleLimit(6, A_6 - 360))
               {
                   A_6 = A_6 - 360;
               }
               else
               {
                   Debug.Log("theta6超过运动极限" + A_6);
                   return false;
               }
           }
           theta6 = A_6;
           angle6 = theta6 / 180 * Mathf.PI;
       }

       //特殊情况处理
       //if (Special)
       //{
       //    theta4 = 5;
       //    theta5 = 0;
       //    theta6 = -theta4;
       //}
       
       return true;
   }

   #endregion
   
    
    
    /// <summary>
    /// 运动学正解并返回位置(对)
    /// </summary>
    /// <param name="angel_8"></param>
    /// <returns></returns>
    public Vector3 SolutionOfKinematics(float[] angel_8_)
    {
        //float[] angel_8 = new float[8];
        //for (int i = 0; i < 8; i++)
        //{
        //    angel_8[i] = angel_8_[i] / 180 * Mathf.PI;
        //}
        //angel_8[1] -= Mathf.PI / 2;
        //float px = (float)(Math.Cos(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1));
        //float py = (float)(Math.Sin(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1) + d3 * Math.Cos(angel_8[0]));
        //float pz = (float)(-a3 * Math.Sin(angel_8[1] + angel_8[2]) - a2 * Math.Sin(angel_8[1]) - d4 * Math.Cos(angel_8[1] + angel_8[2]));
        //return new Vector3(px, py, pz);

        double[] angel_8 = new double[8];
        for (int i = 0; i < 8; i++)
        {
            angel_8[i] = angel_8_[i] / 180 * Mathf.PI;
        }
        angel_8[1] -= Mathf.PI / 2;
        float px = (float)(Math.Cos(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1));
        float py = (float)(Math.Sin(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1) + d3 * Math.Cos(angel_8[0]));
        float pz = (float)(-a3 * Math.Sin(angel_8[1] + angel_8[2]) - a2 * Math.Sin(angel_8[1]) - d4 * Math.Cos(angel_8[1] + angel_8[2]));
        return new Vector3(px, py, pz);
    }
    public Vector3 SolutionOfKinematics_DOUBLE(double[] angel_8_)
    {
        double[] angel_8 = new double[8];
        for (int i = 0; i < 8; i++)
        {
            angel_8[i] = angel_8_[i] / 180 * Mathf.PI;
        }
        angel_8[1] -= Mathf.PI / 2;
        float px = (float)(Math.Cos(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1));
        float py = (float)(Math.Sin(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1) + d3 * Math.Cos(angel_8[0]));
        float pz = (float)(-a3 * Math.Sin(angel_8[1] + angel_8[2]) - a2 * Math.Sin(angel_8[1]) - d4 * Math.Cos(angel_8[1] + angel_8[2]));
        return new Vector3(px, py, pz);
    }

    public Vector3 SolutionOfTCP(float[] angel_8_, Vector3 localV){
        float[] angel_8 = new float[8];
        for (int i = 0; i < 8; i++)
        {
            angel_8[i] = angel_8_[i] / 180 * Mathf.PI;
        }
        angel_8[1] -= Mathf.PI / 2;
        float px = (float)(Math.Cos(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1));
        float py = (float)(Math.Sin(angel_8[0]) * (a2 * Math.Cos(angel_8[1]) + a3 * Math.Cos(angel_8[1] + angel_8[2]) - d4 * Math.Sin(angel_8[1] + angel_8[2]) + a1) + d3 * Math.Cos(angel_8[0]));
        float pz = (float)(-a3 * Math.Sin(angel_8[1] + angel_8[2]) - a2 * Math.Sin(angel_8[1]) - d4 * Math.Cos(angel_8[1] + angel_8[2]));
        float r11 = Mathf.Cos(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[5])) - Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Cos(angel_8[5])) + Mathf.Sin(angel_8[0]) * (Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[5]));
        float r21 = Mathf.Sin(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[5])) - Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Cos(angel_8[5])) - Mathf.Cos(angel_8[0]) * (Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[5]));
        float r31 = -Mathf.Sin(angel_8[1] + angel_8[2]) * (Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[5])) - Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Cos(angel_8[5]);
        float r12 = Mathf.Cos(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (-Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[5])) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Sin(angel_8[5])) + Mathf.Sin(angel_8[0]) * (-Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[5]));
        float r22 = Mathf.Sin(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (-Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[5])) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Sin(angel_8[5])) - Mathf.Cos(angel_8[0]) * (-Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[5]));
        float r32 = -Mathf.Sin(angel_8[1] + angel_8[2]) * (-Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[5])) + Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Sin(angel_8[5]);
        float r13 = -Mathf.Cos(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[4]) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[4])) - Mathf.Sin(angel_8[0]) * Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[4]);
        float r23 = -Mathf.Sin(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[4]) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[4])) + Mathf.Cos(angel_8[0]) * Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[4]);
        float r33 = Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[4]) - Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[4]);
        Matrix4x4 MM = new Matrix4x4();
        MM[0, 0] = r11; MM[0, 1] = r12; MM[0, 2] = r13; MM[0, 3] = px;
        MM[1, 0] = r21; MM[1, 1] = r22; MM[1, 2] = r23; MM[1, 3] = py;
        MM[2, 0] = r31; MM[2, 1] = r32; MM[2, 2] = r33; MM[2, 3] = pz;
        MM[3, 0] = 0; MM[3, 1] = 0; MM[3, 2] = 0; MM[3, 3] = 1;
        return MM.MultiplyPoint3x4(localV);
    }
    /// <summary>
    /// 获取姿态（还有问题）
    /// </summary>
    /// <param name="angel_8"></param>
    /// <returns></returns>
    public Quaternion SolutionOfKinematics_posture(float[] angel_8_)
    {
        //float[] angel_8 = new float[8];
        //for (int i = 0; i < 8; i++)
        //{
        //    angel_8[i] = angel_8_[i] / 180 * Mathf.PI;
        //}
        //angel_8[1] -= Mathf.PI / 2;
        //float r11 = Mathf.Cos(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[5])) - Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Cos(angel_8[5])) + Mathf.Sin(angel_8[0]) * (Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[5]));
        //float r21 = Mathf.Sin(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[5])) - Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Cos(angel_8[5])) - Mathf.Cos(angel_8[0]) * (Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[5]));
        //float r31 = -Mathf.Sin(angel_8[1] + angel_8[2]) * (Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Cos(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[5])) - Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Cos(angel_8[5]);
        //float r12 = Mathf.Cos(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (-Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[5])) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Sin(angel_8[5])) + Mathf.Sin(angel_8[0]) * (-Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[5]));
        //float r22 = Mathf.Sin(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * (-Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[5])) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Sin(angel_8[5])) - Mathf.Cos(angel_8[0]) * (-Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) + Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[5]));
        //float r32 = -Mathf.Sin(angel_8[1] + angel_8[2]) * (-Mathf.Cos(angel_8[3]) * Mathf.Cos(angel_8[4]) * Mathf.Sin(angel_8[5]) - Mathf.Sin(angel_8[3]) * Mathf.Cos(angel_8[5])) + Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Sin(angel_8[4]) * Mathf.Sin(angel_8[5]);
        //float r13 = -Mathf.Cos(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[4]) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[4])) - Mathf.Sin(angel_8[0]) * Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[4]);
        //float r23 = -Mathf.Sin(angel_8[0]) * (Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[4]) + Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[4])) + Mathf.Cos(angel_8[0]) * Mathf.Sin(angel_8[3]) * Mathf.Sin(angel_8[4]);
        //float r33 = Mathf.Sin(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[3]) * Mathf.Sin(angel_8[4]) - Mathf.Cos(angel_8[1] + angel_8[2]) * Mathf.Cos(angel_8[4]);

        ////四元数和旋转矩阵之间的转换(网上的源码)
        //float[,] mm={{r11,r12,r13,0},{r21,r22,r23,0},{r31,r32,r33,0},{0,0,0,1}};
        //Quaternion Qposture;
        //Qposture = GetQuaternion2(mm);
        //return Qposture;

        double[] angel_8 = new double[8];
        for (int i = 0; i < 8; i++)
        {
            angel_8[i] = angel_8_[i] / 180 * Mathf.PI;
        }
        angel_8[1] -= Mathf.PI / 2;
        float r11 = (float)(Math.Cos(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Sin(angel_8[5])) - Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Cos(angel_8[5])) + Math.Sin(angel_8[0]) * (Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Sin(angel_8[5])));
        float r21 = (float)(Math.Sin(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Sin(angel_8[5])) - Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Cos(angel_8[5])) - Math.Cos(angel_8[0]) * (Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Sin(angel_8[5])));
        float r31 = (float)(-Math.Sin(angel_8[1] + angel_8[2]) * (Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Sin(angel_8[5])) - Math.Cos(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Cos(angel_8[5]));
        float r12 = (float)(Math.Cos(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (-Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Cos(angel_8[5])) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Sin(angel_8[5])) + Math.Sin(angel_8[0]) * (-Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Cos(angel_8[5])));
        float r22 = (float)(Math.Sin(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (-Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Cos(angel_8[5])) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Sin(angel_8[5])) - Math.Cos(angel_8[0]) * (-Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Cos(angel_8[5])));
        float r32 = (float)(-Math.Sin(angel_8[1] + angel_8[2]) * (-Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Cos(angel_8[5])) + Math.Cos(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Sin(angel_8[5]));
        float r13 = (float)(-Math.Cos(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[3]) * Math.Sin(angel_8[4]) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[4])) - Math.Sin(angel_8[0]) * Math.Sin(angel_8[3]) * Math.Sin(angel_8[4]));
        float r23 = (float)(-Math.Sin(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[3]) * Math.Sin(angel_8[4]) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[4])) + Math.Cos(angel_8[0]) * Math.Sin(angel_8[3]) * Math.Sin(angel_8[4]));
        float r33 = (float)(Math.Sin(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[3]) * Math.Sin(angel_8[4]) - Math.Cos(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[4]));

        //四元数和旋转矩阵之间的转换(网上的源码)
        float[,] mm = { { r11, r12, r13, 0 }, { r21, r22, r23, 0 }, { r31, r32, r33, 0 }, { 0, 0, 0, 1 } };
        Quaternion Qposture;
        Qposture = GetQuaternion2(mm);
        return Qposture;
    }

    public Quaternion SolutionOfKinematics_posture_DOUBLE(double[] angel_8_)
    {
        double[] angel_8 = new double[8];
        for (int i = 0; i < 8; i++)
        {
            angel_8[i] = angel_8_[i] / 180 * Mathf.PI;
        }
        angel_8[1] -= Mathf.PI / 2;
        float r11 = (float)(Math.Cos(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Sin(angel_8[5])) - Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Cos(angel_8[5])) + Math.Sin(angel_8[0]) * (Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Sin(angel_8[5])));
        float r21 = (float)(Math.Sin(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Sin(angel_8[5])) - Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Cos(angel_8[5])) - Math.Cos(angel_8[0]) * (Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Sin(angel_8[5])));
        float r31 = (float)(-Math.Sin(angel_8[1] + angel_8[2]) * (Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Cos(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Sin(angel_8[5])) - Math.Cos(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Cos(angel_8[5]));
        float r12 = (float)(Math.Cos(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (-Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Cos(angel_8[5])) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Sin(angel_8[5])) + Math.Sin(angel_8[0]) * (-Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Cos(angel_8[5])));
        float r22 = (float)(Math.Sin(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * (-Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Cos(angel_8[5])) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Sin(angel_8[5])) - Math.Cos(angel_8[0]) * (-Math.Sin(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) + Math.Cos(angel_8[3]) * Math.Cos(angel_8[5])));
        float r32 = (float)(-Math.Sin(angel_8[1] + angel_8[2]) * (-Math.Cos(angel_8[3]) * Math.Cos(angel_8[4]) * Math.Sin(angel_8[5]) - Math.Sin(angel_8[3]) * Math.Cos(angel_8[5])) + Math.Cos(angel_8[1] + angel_8[2]) * Math.Sin(angel_8[4]) * Math.Sin(angel_8[5]));
        float r13 = (float)(-Math.Cos(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[3]) * Math.Sin(angel_8[4]) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[4])) - Math.Sin(angel_8[0]) * Math.Sin(angel_8[3]) * Math.Sin(angel_8[4]));
        float r23 = (float)(-Math.Sin(angel_8[0]) * (Math.Cos(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[3]) * Math.Sin(angel_8[4]) + Math.Sin(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[4])) + Math.Cos(angel_8[0]) * Math.Sin(angel_8[3]) * Math.Sin(angel_8[4]));
        float r33 = (float)(Math.Sin(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[3]) * Math.Sin(angel_8[4]) - Math.Cos(angel_8[1] + angel_8[2]) * Math.Cos(angel_8[4]));

        //四元数和旋转矩阵之间的转换(网上的源码)
        float[,] mm = { { r11, r12, r13, 0 }, { r21, r22, r23, 0 }, { r31, r32, r33, 0 }, { 0, 0, 0, 1 } };
        Quaternion Qposture;
        Qposture = GetQuaternion2(mm);
        return Qposture;
    }

    public Matrix4x4 GetMatrixLH1(Quaternion q)
    {
        Matrix4x4 ret = new Matrix4x4();
        float xx = q.x * q.x;
        float yy = q.y * q.y;
        float zz = q.z * q.z;
        float xy = q.x * q.y;
        float wz = q.w * q.z;
        float wy = q.w * q.y;
        float xz = q.x * q.z;
        float yz = q.y * q.z;
        float wx = q.w * q.x;
        ret[0, 0] = 1.0f - 2 * (yy + zz);
        ret[0, 1] = 2 * (xy - wz);
        ret[0, 2] = 2 * (wy + xz);
        ret[0, 3] = 0.0f;

        ret[1, 0] = 2 * (xy + wz);
        ret[1, 1] = 1.0f - 2 * (xx + zz);
        ret[1, 2] = 2 * (yz - wx);
        ret[1, 3] = 0.0f;

        ret[2, 0] = 2 * (xz - wy);///ggggggg
        ret[2, 1] = 2 * (yz + wx);
        ret[2, 2] = 1.0f - 2 * (xx + yy);
        ret[2, 3] = 0.0f;

        ret[3, 0] = 0.0f;
        ret[3, 1] = 0.0f;
        ret[3, 2] = 0.0f;
        ret[3, 3] = 1.0f;
        return ret;
    }



    Quaternion GetQuaternion2(float[,] mat)
    {
        Quaternion q=new Quaternion();
        float trace;
        float s;
        float t;
        int i;
        int j;
        int k;

        int[] next = { 1, 2, 0 };

        trace = mat[0, 0] + mat[1, 1] + mat[2, 2];

        if (trace > 0.0f)
        {
            t = trace + 1.0f;
            s = 0.5f / Mathf.Sqrt(t);
            q[3] = s * t;
            q[0] = (mat[2, 1] - mat[1, 2]) * s;
            q[1] = (mat[0, 2] - mat[2, 0]) * s;
            q[2] = (mat[1, 0] - mat[0, 1]) * s;

        }
        else
        {
            i = 0;
            if (mat[1, 1] > mat[0, 0])
            {
                i = 1;
            }
            if (mat[2, 2] > mat[i, i])
            {
                i = 2;
            }
            j = next[i];
            k = next[j];
            t = (mat[i, i] - (mat[j, j] + mat[k, k])) + 1.0f;
            s = 0.5f / Mathf.Sqrt(t);
            q[i] = s * t;
            q[3] = (mat[k, j] - mat[j, k]) * s;
            q[j] = (mat[j, i] + mat[i, j]) * s;
            q[k] = (mat[k, i] + mat[i, k]) * s;
        }
        return q;
    }
}
