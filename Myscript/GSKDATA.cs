//<summary>
//GSKDATA#FILEEXTENSION#
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

//系统速度等级分类
public class ManualSpeedRank
{
    float i, l, m, h, s;
    const float motorSpeed = 1f;//角度运动时则为10；
    float[] speed;
    int level;
    public ManualSpeedRank(float _i, float _l, float _m, float _h, float _s)
    {
        i = _i * motorSpeed;
        l = _l * motorSpeed;
        m = _m * motorSpeed;
        h = _h * motorSpeed;
        s = _s * motorSpeed;
        level = 0;
        speed = new float[5]{i, l, m, h, s};
    }
    public float SpeedAdd (){
        if (++level > 4)
        {
            level = 4;
        }
        return speed[level];
    }

    public float SpeedMinus()
    {
        if (level > 0)
        {
            level--;
        }
        return speed[level];
    }

    public float Level
    {
        get
        {
            return level;
        }
    }
    public float I
    {
        get
        {
            return i / motorSpeed;
        }
        set
        {
            i = value;
            speed = new float[5] { i, l, m, h, s };
        }
    }
    public float L
    {
        get
        {
            return l / motorSpeed;
        }
        set
        {
            l = value;
            speed = new float[5] { i, l, m, h, s };
        }
    }
    public float M
    {
        get
        {
            return m / motorSpeed;
        }
        set
        {
            m = value;
            speed = new float[5] { i, l, m, h, s };
        }
    }
    public float H
    {
        get
        {
            return h / motorSpeed;
        }
        set
        {
            h = value;
            speed = new float[5] { i, l, m, h, s };
        }
    }
    public float S
    {
        get
        {
            return s / motorSpeed;
        }
        set
        {
            s = value;
            speed = new float[5] { i, l, m, h, s };
        }
    }
}

public class GSKDATA :MonoBehaviour{
    //软件系统参数
    public static bool PathGuid = false;//开启引导模式
    //gsk 系统参数
    public static string CurrentProgramName = "777777.prl";//当前程序名
    public static int CurrentUser = 0;//当前用户坐标号
    public static int CurrentTool = 0;//当前工具坐标号
    public static string CurrentScreenPositon = "主页面>";//当前界面位置
    public static int CurrentSafeMode = 2;//当前的安全模式(编辑模式）
    public static int RobotMode = 2;//当前模式为示教模式
    public static int MotionCoordinateSystem = 1; //坐标系（关节坐标1、基座标2，工具3、用户4）
    public static ManualSpeedRank ManualSpeedR = new ManualSpeedRank(0.05f, 0.20f, 0.45f, 0.70f, 0.95f);//速度等级
    public static float ManualSpeed = ManualSpeedR.I; //初始速度
    public static string KaiJiSpeed = "微动[I]";
    public static int ChengXuCount = 7;
    public static int RuanJianPanNo = 0;//软键盘处于小写状态
    public static bool SystemWrong = false;//系统出错
    public static bool ActionCycle = false;//单段
    public static bool Scram = false;//急停
    public static bool IsReady = false;//伺服准备
    public static bool OpenCollideFunc = true;//是否开始碰撞检测
    public static bool IsCollide = false;//是否发生碰撞

    public static bool AxisRunning = false;//机械臂正在运转
    public static bool GoPress = false;//前进键按下
    public static bool BackPress = false;//后退键按下
    public static bool ReappearRun = false;//再现运行

    //机械臂的连杆参数
    public const float ROBOT1_a1 = 0.170124f;
    public const float ROBOT1_a2 = 0.560164f;
    public const float ROBOT1_a3 = 0.155122f;
    public const float ROBOT1_d4 = 0.6417539f;

	public const float ROBOT1_A1 = 0.1739001f;
	public const float ROBOT1_A2 = 1.0823f;
	public const float ROBOT1_A3 = 0.1078607f;
	public const float ROBOT1_D4 = 1.015824f;

    public const float ROBOT4_A1 = 0.175787f;
    public const float ROBOT4_A2 = 0.7377478f;
    public const float ROBOT4_A3 = 0.1473962f;
    public const float ROBOT4_D4 = 0.6397648f;

    public const float ROBOT5_A1 = 0.1700001f;
    public const float ROBOT5_A2 = 0.5598871f;
    public const float ROBOT5_A3 = 0.1591836f;
    public const float ROBOT5_D4 = 0.65521f;

    //控制各个轴的转动
    public static bool AXIS_1 = false;
    public static bool AXIS_2 = false;
    public static bool AXIS_3 = false;
    public static bool AXIS_4 = false;
    public static bool AXIS_5 = false;
    public static bool AXIS_6 = false;
    public static bool AXIS_1_ = false;
    public static bool AXIS_2_ = false;
    public static bool AXIS_3_ = false;
    public static bool AXIS_4_ = false;
    public static bool AXIS_5_ = false;
    public static bool AXIS_6_ = false;

    //运动数据记录
    public static List<Vector3> VList = new List<Vector3>();//记录数据点
    public static string RBP_result = ""; //程序运行结果
    public static float Speed = 0.5f;//运行时的速度
    public static int[] nextNUM = new int[2] { 0, 0 };

    ///插补相关
    public static string InterpolationMethod = ""; //插补方式
    public static float[] CurrentAngle = new float[6];//当前角度
    public static bool IsIng = false; //是否正在差补的进行中；
    public static float D_time = 0.03f;//每帧增加的插补时长
    public static bool SHIFT = false;//平移指令
    public static int SHIFT_int = 0;//测试用
    public static double[] SHIFT_data = new double[6];//平移参数
    public static PXClass SHIFT_PX;
    //MOVJ
    public static float[] MOVJ_A1 = new float[8];
    public static float[] MOVJ_A2 = new float[8];
    public static Vector3 MOVJ_P1 = Vector3.zero;
    public static Vector3 MOVJ_P2 = Vector3.zero;
    public static float MOVJ_t = 0f; //已进行插补时间
    //MOVL
    public static float[] MOVL_A1 = new float[6];//用于逆解的优化
    public static float[] MOVL_A2 = new float[6];
    public static Vector3 MOVL_P1 = Vector3.zero;//位置
    public static Vector3 MOVL_P2 = Vector3.zero;
    public static Quaternion MOVL_Z1;
    public static Quaternion MOVL_Z2;
    public static float MOVL_t = 0f;  //已进行插补时间
    public static float MOVL_a = 5f;  //直线段的加速度，可以增加
    //MOVC
    //public static double[] MOVC_A1 = new double[8];//用于逆解的优化
    //public static double[] MOVC_A2 = new double[8];
    //public static double[] MOVC_A3 = new double[8];
    public static Vector3 MOVC_P1 = Vector3.zero;//位置
    public static Vector3 MOVC_P2 = Vector3.zero;
    public static Vector3 MOVC_P3 = Vector3.zero;
    public static Quaternion MOVC_Z1;
    public static Quaternion MOVC_Z2;
    public static Quaternion MOVC_Z3;
    public static float MOVC_t = 0f;  //已进行插补时间
    public static float MOVC_a = 5f;  //直线段的加速度，，可以增加
    //I/O数据存取区
    public static bool[] InInfo = new bool[IO_MAX_NUM] ;
    public static bool[] OutInfo = new bool[IO_MAX_NUM] ;
    public static int WaitInput = -1;
    public static int WaitInputState = -1;
    //变量数据存储区
    //整数型
    public static int[] R_data = new int[100];//值
    public static int[] R_state = new int[100];//状态
    public static string[] R_notation = new string[100];//注释
    //笛卡尔坐姿
    public static double[,] PX_data = new double[100, 6];//值  改为8是为了计算方便
    public static int[] PX_state = new int[100];//状态
    public static string[] PX_notation = new string[100];//注释

    // 场景选择
    public static int Scene_NO = 1;//当前场景，0代表当前没有选择场景
    public static string CaseName = "";//当前案例名称
    //教练考模式
    public static bool FreeOperation = true; //是否自由操作
    public static string SoftCurrentMode = ""; //软件的当前的模式
    //案例数据部分
    public static int CaseStage = 0;//当前案例步骤
    public static bool Painting = false;//是否开启绘画功能


    //周边设备信号参数
    public static int[] DeviceSignal = new int[50];
    //传感器信号
    public static bool[] SensorSignal = new bool[11];

    //程序存放list
    public static List<string> ProgramContents = new List<string>();
    public static List<string> FileListInfo = new List<string>();

    //焊接参数
    public static bool Weld = false;

    //轨迹显示
    public static bool PathShow = false;

    //射线显示
    public static bool RayShow = false;

    public static int AXIS7_SPEED = 0;
    public const int IO_MAX_NUM = 100;




    //广数参数的初始化
    public static void GSKDATA_INITIAL()
    {
        CurrentProgramName = "777777.prl";
        GameObject.Find("MyScreen").GetComponent<ScreenBuild>().EditInitial();
        CurrentUser = 0;
        CurrentTool = 0;
        CurrentScreenPositon = "主页面>";
        GameObject.Find("CurrentPosition").GetComponent<UILabel>().text = CurrentScreenPositon;
        CurrentSafeMode = 2;
        GameObject.Find("SafeMode").GetComponent<UISprite>().spriteName = "Editing";
        RobotMode = 2;
        GameObject.Find("Modeshow").GetComponent<UILabel>().text = "示教模式";
        GameObject.Find("KAIGUAN").GetComponent<UISprite>().spriteName = "U4";
        ManualSpeedRank ManualSpeedR = new ManualSpeedRank(0.05f, 0.20f, 0.45f, 0.70f, 0.95f);//速度等级
        ManualSpeed = ManualSpeedR.I; //初始速度
        GameObject.Find("Speed").GetComponent<UISprite>().spriteName = "I";
        KaiJiSpeed = "微动[I]";
        ActionCycle = false;
        GameObject.Find("OneTwo").GetComponent<UISprite>().spriteName = "one";
        Scram = false;
        SystemWrong = false;
        IsCollide = false; 
        GameObject.Find("Scram").GetComponent<UIButton>().normalSprite = "U1";
        GameObject.Find("State").GetComponent<UISprite>().spriteName = "Stoping";
        ReappearRun = false;
        IsReady = false;//伺服准备
        GameObject.Find("Ready").GetComponent<UIButton>().normalSprite = "11";
        AxisRunning = false;//机械臂正在运转
        GoPress = false;//前进键按下
        BackPress = false;//后退键按下
        RBP_result = "";
        Speed = 0.5f;//运行时的速度

        Weld = false;

        for (int i = 0; i < GSKDATA.IO_MAX_NUM; i++)
        {
            GSKDATA.InInfo[i] = false;
            GSKDATA.OutInfo[i] = false;
        }

        //InInfo = new bool[32] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        //OutInfo = new bool[32] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        GameObject.Find("MyScreen").GetComponent<ScreenBuild>().ClearOutAndIn();

        for (int i = 0; i < 50; i++)
        {
            DeviceSignal[i] = 0; //周边设备信号初始化
        }

        for (int i = 0; i < SensorSignal.Length; i++)
        {
            SensorSignal[i] = false;
        }
        SensorSignal[10] = true;
    }

}
