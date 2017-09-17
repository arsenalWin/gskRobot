using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class Robot
{
    private string suffix;//后缀
    private string path;//路径
    private string version;//型号

    //机械臂参数
    private JointMove[] joints;
    private Vector3[] joint_directs;//机械臂的转动方向
    private int[] soft_limits;//各个臂运动的极限
    private float a1, a2, a3, d4;//结构参数

    private InverseKinematicsAlgorithm IKA;//机器人逆运动学求解类

    //插补所需位置坐标
    private float[] movj_s = new float[8];
    private float[] movj_e = new float[8];
    private float[] movl_s = new float[8];
    private float[] movl_e = new float[8];
    private float[] movc_s = new float[8];
    private float[] movc_m = new float[8];
    private float[] movc_e = new float[8];

    //运行相关
    private int result = 0;//每行程序的运行结果
    private const float delt_time = 0.03f;//每帧插补时长
    private float movj_t = 0;
    private float movl_t = 0;
    private float movl_a = 5f;  //直线段的加速度，可以增加
    private float movc_t = 0;
    private float run_speed = 0.5f;//运行速度
    private bool axis_running = false;//当前是否在运行中


    //设置运行速度
    public float SetSpeed
    {
        set
        {
            run_speed = value;
        }
    }

    public bool AxisRunning
    {
        set { axis_running = value; }
        get { return axis_running; }
    }

    public Robot(string version,string streampath)
    {
        switch (version)
        {
            case "GSK":
                suffix = ".prl";
                break;
        }
        this.version = version;
        path = streampath + @"/Programs/"+version;
        joint_directs = new Vector3[6] { new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(-1, 0, 0) };
        soft_limits = new int[12] {180,-180,120,-80,80,-120,170,-170,130,-130,360,-360 };
        joints = new JointMove[6];
    }

    //定义机器人
    //*********注意：：各个臂的位置因是其旋转中心点的位置，否则计算会出错！***********
    public void DefineRobot(string[] names)
    {
        if (names.Length < 6)
        {
            Debug.LogError("请输入机器人六个臂的名称");
            return;
        }
        for (int i = 0; i < 6; i++)
        {
            joints[i] = new JointMove(names[i], joint_directs[i]);
            joints[i].SoftLimit = soft_limits[i * 2];
            joints[i]._SoftLimit = soft_limits[i * 2+1];
        }
        auto_calculate(names);
        IKA = new InverseKinematicsAlgorithm(a1, a2, a3, d4);
    }

    //设置各个臂的转角极限
    public void SetSoftLimit(int[] limit)
    {
        if (limit.Length < 12)
        {
            Debug.LogError("请输入机器人六个臂的12个运动极限");
            return;
        }
        for (int i = 0; i < 12; i++)
        {
            soft_limits[i] = limit[i];
        }
    } 



    
    //编译
    public void Compile(string name)
    {

    }


    //运行
    //next_num 返回一行行号
    public string Run(int direct,int[] next_num)
    {

        motion_execute();
        return "";
    }


    //新建程序
    //0---正确
    //1---失败
    //2---已存在
    //3---程序名为空
    public int NewBuilt(string name)
    {

        return 0;
    }


    //插入程序行
    public void InsertProgram(string name,List<string> contents,int line)
    {

    }


    //输入键
    public string InputKey()
    {
        return "";
    }


    //获取目录表
    //NO,Name,size,day;//依次为序号、名称、大小、时间
    public List<string> Catalog()
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        List<string> listFiles = new List<string>();//保存所有的文件信息
        FileInfo[] fileInfoArray = directory.GetFiles();

        //对文件进行排序
        SortAsFileCreationTime(ref fileInfoArray);

        if (fileInfoArray.Length > 0)
        {
            for (int i = 0; i < fileInfoArray.Length; i++)
            {
                if (fileInfoArray[i].Name.IndexOf(".meta") == -1 && fileInfoArray[i].Name.IndexOf(suffix) != -1)
                {
                    listFiles.Add(i+1+fileInfoArray[i].Name + "," + fileInfoArray[i].Length.ToString() + "," + fileInfoArray[i].CreationTime.ToString("yyyy-MM-dd"));
                }
            }
        }
        return listFiles;
    }

    //获取目录中所有程序的总共占用的空间
    public long GetMemotySize()
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] fileInfoArray = directory.GetFiles();
        long size = 0;
        for (int i = 0; i < fileInfoArray.Length; i++)
        {
            size += fileInfoArray[i].Length;
        }
        return size;
    }


    //获取程序内容
    public List<string> Contents(string name)
    {
        name += suffix;
        List<string> tempList = new List<string>();
        FileStream file = new FileStream(path +"\\"+ name, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(file);
        string temp1 = null;
        while ((temp1 = sr.ReadLine()) != null)
        {            
            if (temp1 != "")
            {
                tempList.Add(temp1);
            }            
        }
        sr.Close();
        file.Close();
        return tempList;
    }


    //当前各个臂的转角 
    public float[] CurrentAngle_All()
    {
        return new float[] { joints[0].Angle, joints[1].Angle, joints[2].Angle, joints[3].Angle, joints[4].Angle, joints[5].Angle, 0, 0 };
    }


#region ======================私有函数部分====================
    //计算a1,a2,a3,d4
    private void auto_calculate(string[] names)
    {
        a1 = Mathf.Abs(GameObject.Find(names[0]).transform.position.x - GameObject.Find(names[1]).transform.position.x);
        a2 = Mathf.Abs(GameObject.Find(names[1]).transform.position.y - GameObject.Find(names[2]).transform.position.y);
        a3 = Mathf.Abs(GameObject.Find(names[2]).transform.position.y - GameObject.Find(names[4]).transform.position.y);
        d4 = Mathf.Abs(GameObject.Find(names[2]).transform.position.x - GameObject.Find(names[4]).transform.position.x);
    }

    //程序自动执行运动部分
    //需要在update中调用
    private void motion_execute()
    {
        if (axis_running)
        {
            switch (result)
            {
                case 1:
                    movj_t += delt_time;movj();break;
                case 2:
                    movl_t += delt_time;movl();break;
                case 3:
                    movc_t += delt_time; movc(); break;
            }
        }
    }

    //movj
    private void movj()
    {
        float tf = CalculateTime(movj_s,movj_e);  //完成插补所需的时间
        if (tf != 0)
        {
            for (int i = 0; i < 6; i++)
            {
                joints[i].JointInterpolation_3(movj_s[i], movj_e[i], tf, movj_t);
            }
        }
        else
        {
            axis_running = false;//插补结束
            movj_t = 0f;
        }
    }

    //movl
    private void movl()
    {
        Vector3 p1 = IKA.SolutionOfKinematics(movl_s);
        Vector3 p2 = IKA.SolutionOfKinematics(movl_e);
        Quaternion z1 = IKA.SolutionOfKinematics_posture(movl_s);
        Quaternion z2 = IKA.SolutionOfKinematics_posture(movl_e);

        float Tb = run_speed / GSKDATA.MOVL_a; //加速段的时间
        float Lb = 0.5f * movl_a * Tb * Tb;
        float L = Vector3.Distance(p1, p2);//直线运动的总位移
        float tf = 2 * Tb + (L - 2 * Lb) / run_speed;//直线运动的总时间
        //归一化
        if (tf != 0)
        {
            float L_b = Lb / L;
            float T_b = Tb / tf;
            float a_ = 2 * L_b / (T_b * T_b);
            movl_t = movl_t / tf;
            //
            float namta = 0f;
            if (movl_t >= 0 && movl_t <= T_b)
            {
                namta = 0.5f * a_ * movl_t * movl_t;
            }
            else if (movl_t > T_b && movl_t <= (1 - T_b))
            {
                namta = 0.5f * a_ * T_b * T_b + a_ * T_b * (movl_t - T_b);
            }
            else if (movl_t > (1 - T_b) && movl_t <= 1)
            {
                namta = 0.5f * a_ * T_b * T_b + a_ * T_b * (movl_t - T_b) - 0.5f * a_ * (movl_t + T_b - 1) * (movl_t + T_b - 1);
            }
            else if (movl_t > 1)
            {
                namta = 1;
            }
            ///计算出插值点
            //位置
            Vector3 Inter_Point_P = Vector3.Lerp(p1, p2, namta);
            //姿态
            Quaternion Inter_Point_Z = Quaternion.Slerp(z1, z2, namta);

            float[] movl_A_inter = IKA.AcceptInterPointPosture(Inter_Point_P, Inter_Point_Z, CurrentAngle_All());
            for (int i = 0; i < 6; i++)
            {
                joints[i].RotateFixedAngle_FixedTime(movl_A_inter[i]);
            }

            if (namta == 1)
            {
                axis_running = false;//插补结束
                movl_t = 0f;
            }
        }
        else
        {
            tf = CalculateTime(movl_s, movl_e);
            if (tf != 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    joints[i].JointInterpolation_3(movl_s[i], movl_e[i], tf, movl_t);
                }
            }
            else
            {
                axis_running = false;//插补结束
                movl_t = 0f;
            }
        }
    }

    //movc的插补算法
    private bool movc()
    {
        float namta = movc_t;
        Vector3 p1 = IKA.SolutionOfKinematics(movc_s);
        Vector3 p2 = IKA.SolutionOfKinematics(movc_m);
        Vector3 p3 = IKA.SolutionOfKinematics(movc_e);
        Quaternion z1 = IKA.SolutionOfKinematics_posture(movc_s);
        Quaternion z2 = IKA.SolutionOfKinematics_posture(movc_m);
        Quaternion z3 = IKA.SolutionOfKinematics_posture(movc_e);
        ///求出圆心与半径
        //判断是否在一条直线上
        float det = p1.x * p2.y * p3.z + p1.y * p2.z * p3.x + p2.x * p3.y * p1.z - p1.z * p2.y * p3.x - p1.y * p2.x * p3.z - p1.x * p2.z * p2.y;
        if (det == 0)
        {
            return false; //报错
        }
        //圆心
        float k11 = (p1.y - p3.y) * (p2.z - p3.z) - (p2.y - p3.y) * (p1.z - p3.z);
        float k12 = (p2.x - p3.x) * (p1.z - p3.z) - (p1.x - p3.x) * (p2.z - p3.z);
        float k13 = (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        float k14 = -(p3.x * k11 + k12 * p3.y + k13 * p3.z);
        float k21 = (p2.x - p1.x);
        float k22 = (p2.y - p1.y);
        float k23 = (p2.z - p1.z);
        float k24 = -((p2.x * p2.x - p1.x * p1.x) + (p2.y * p2.y - p1.y * p1.y) + (p2.z * p2.z - p1.z * p1.z)) / 2;
        float k31 = (p3.x - p2.x);
        float k32 = (p3.y - p2.y);
        float k33 = (p3.z - p2.z);
        float k34 = -((p3.x * p3.x - p2.x * p2.x) + (p3.y * p3.y - p2.y * p2.y) + (p3.z * p3.z - p2.z * p2.z)) / 2;
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
        float R = Mathf.Sqrt((p1.x - X0) * (p1.x - X0) + (p1.y - Y0) * (p1.y - Y0) + (p1.z - Z0) * (p1.z - Z0));
        /////测试部分
        //新坐标系和基座标之间的变换矩阵
        Matrix4x4 T = Matrix4x4.zero;
        Vector3 a = Vector3.Normalize(new Vector3(k11, k12, k13));
        Vector3 n = Vector3.Normalize(new Vector3((p1.x - X0), (p1.y - Y0), (p1.z - Z0)));
        Vector3 o = Vector3.Cross(a, n);
        T.SetColumn(0, new Vector4(n.x, n.y, n.z));
        T.SetColumn(1, new Vector4(o.x, o.y, o.z));
        T.SetColumn(2, new Vector4(a.x, a.y, a.z));
        T.SetColumn(3, new Vector4(X0, Y0, Z0, 1));
        //圆弧上的点在新坐标系中的位置
        //三个点在新坐标系中的位置

        Vector3 Npoint0 = TransFromOld(new Vector3(X0, Y0, Z0), T);
        Vector3 Npoint1 = TransFromOld(p1, T);
        Vector3 Npoint2 = TransFromOld(p2, T);
        Vector3 Npoint3 = TransFromOld(p3, T);

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

        float tf = theta3 * R / run_speed;
        float tf_2 = theta2 * 2 * Mathf.PI * R / 360 / run_speed;
        movc_t = movc_t / tf; //归一化
        float theta = 0f;
        if (movc_t > 1)
        {
            movc_t = 1;
            axis_running = false;//插补结束
            movc_t = 0f;
        }
        theta = movc_t * theta3;
        float u = R * Mathf.Cos(theta);
        float v = R * Mathf.Sin(theta);
        //插补点在久坐标系中的位置
        //位置
        Vector3 Inter_Point_P = TransFromNew(new Vector3(u, v, 0), T);
        //姿态
        Quaternion Inter_Point_Z;
        if (namta < tf_2)
        {
            Inter_Point_Z = Quaternion.Slerp(z1, z2, namta / tf_2);
        }
        else
        {
            Inter_Point_Z = Quaternion.Slerp(z2, z3, (namta - tf_2) / (tf - tf_2));
        }
        float[] movl_A_inter = IKA.AcceptInterPointPosture(Inter_Point_P, Inter_Point_Z, CurrentAngle_All());
        for (int i = 0; i < 6; i++)
        {
            joints[i].RotateFixedAngle_FixedTime(movl_A_inter[i]);
        }

        return true;
    }

    //计算插补一共所需的时间
    private float CalculateTime(float[] A1, float[] A2)
    {
        Vector3 p1 = IKA.SolutionOfKinematics(A1);
        Vector3 p2 = IKA.SolutionOfKinematics(A2);

        float L = Vector3.Distance(p1, p2); //空间轨迹点之间的距离
        float tt = L / run_speed;
        float D_value = 0;
        for (int i = 0; i < 8; i++)
        {
            if (Mathf.Abs((A1[i] - A2[i])) > D_value)
            {
                D_value = Mathf.Abs((A1[i] - A2[i]));
            }
        }
        float tt2 = D_value / (run_speed * 40);
        if (tt < tt2)
        {
            return tt2;
        }
        else
            return tt;
    }

    private Vector3 TransFromOld(Vector3 Point, Matrix4x4 TT)
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
        return newPoint;
    }

    private Vector3 TransFromNew(Vector3 Point, Matrix4x4 TT)
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
        return oldPoint;
    }

    /// <summary>
    /// C#按创建时间排序（顺序）
    /// </summary>
    /// <param name="arrFi">待排序数组</param>
    private void SortAsFileCreationTime(ref FileInfo[] arrFi)
    {
        Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
    }
#endregion

}
