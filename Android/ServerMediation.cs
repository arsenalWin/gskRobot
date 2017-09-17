/// <summary>
/// <Filename>: ServerMediation.cs
/// Author: Jiang Xiaolong
/// Created: 2016.04.13
/// Version: 1.0
/// Company: Sunnytech
/// Function: PC端与Android端交互的中介；
///
/// Changed By: 
/// Modification Time: 
/// Discription: 
/// <summary>

using UnityEngine;

public sealed class ServerMediation
{
    public static ServerCenter st_ServerCenter = null;
    public static ScreenBuild st_ScreenBuild = null;
    public static ButtonRespond st_ButtonRespond = null;
    public static RobotMotion st_RobotMotion = null;
    public static Camerascript st_Camerascript = null;
    static ServerMediation()
    {
        st_ServerCenter = GameObject.Find("Network").GetComponent<ServerCenter>();
        st_ScreenBuild = GameObject.Find("MyScreen").GetComponent<ScreenBuild>();
        st_ButtonRespond = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
        st_RobotMotion = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        st_Camerascript = GameObject.Find("CameraFree").GetComponent<Camerascript>();

    }

    
    public ServerMediation ()
    { 
        
    }


    public static void EnterServerMode ()
    { 
        //判断当前Server是否已启动
        st_ServerCenter.EnterServerMode();
    }


    public static void ExitServerMode ()
    {
        st_ServerCenter.ExitServerMode();
        //1、开启当前PC端示教盒
        st_ScreenBuild.WindowInitial();
        st_ButtonRespond.ShowPanel();
        st_ButtonRespond.ShowIOPanel();
        //2、本地恢复到初始状态
        st_RobotMotion.ReturnToZero();
        st_Camerascript.ResetScene();
    }

}
