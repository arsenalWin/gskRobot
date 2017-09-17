/// <summary>
/// <Filename>: ServerCenter.cs
/// Author: Jiang Xiaolong
/// Created: 2016.04.13
/// Version: 1.0
/// Company: Sunnytech
/// Function: PC端当服务器连接Android；
///
/// Changed By: 
/// Modification Time: 
/// Discription: 
/// <summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;


public class ServerCenter : MonoBehaviour
{
    /// <summary>
    /// 当前服务器是否连接；
    /// </summary>
    public static bool IsConnected
    {
        get { return _isConnected; }
    }
    private static bool _isConnected = false;

    /// <summary>
    /// 服务器是否启动；
    /// </summary>
    public static bool IsServerOn
    {
        get { return _isServerOn; }
    }
    private static bool _isServerOn = false;

    /// <summary>
    /// 是否启用服务器模式；
    /// </summary>
    public static bool InServerMode
    {
        get { return _inServerMode; }
        set { _inServerMode = value; }
    }
    private static bool _inServerMode = false;

    private Dictionary<string, NetworkPlayer> _connectedMachine = new Dictionary<string, NetworkPlayer>();

    //GUI
    public GUIStyle sty_ModeWindow;
    public GUIStyle sty_StandaloneBtn;
    public GUIStyle sty_WirelessBtn;
    public GUIStyle sty_WirelessWindow;
    public GUIStyle sty_DownBtn;
    public GUIStyle sty_SwitchBtn;
    private Texture2D switchOff;
    private Texture2D switchOn;
    private Texture2D t2d_switch;
    public GUIStyle sty_Empty;
    public GUIStyle sty_Font;
    public GUIStyle sty_FontD;
    public GUIStyle sty_DownList;
    public GUIStyle sty_Line;
    public GUIStyle sty_DownListBtn;
    string panel_choose_btn = "开启数控面板";
    bool downList = false; //IP地址下拉列表
    Rect downRect = new Rect(0, 0, 100, 20);

    //public bool wireless_or_standalone = true; //无线或单击运行模式选择窗口控制
    //Rect chooseRect = new Rect(0, 0, 300, 200); //模式选择窗口Rect变量
    private Rect _wirelessRect = new Rect(0, 0, 323, 274); //模式选择窗口Rect变量
    private bool _connectWindow = true; //无线连接窗口


    private string _displayIP = "";
    private List<string> _IPList = new List<string>();
    private int _port = 11056;

    private Dictionary<string, Action> _Func_0Para = new Dictionary<string, Action>();
    private Dictionary<string, Action<int>> _Func_1Para_Int = new Dictionary<string, Action<int>>();
    private Dictionary<string, Action<string>> _Func_1Para_String = new Dictionary<string, Action<string>>();
    private Dictionary<string, Action<bool>> _Func_1Para_Bool = new Dictionary<string, Action<bool>>();
    private Dictionary<string, Action<float>> _Func_1Para_Float = new Dictionary<string, Action<float>>();
    private Dictionary<string, Action<float, float>> _Func_2Para_Float = new Dictionary<string, Action<float, float>>();

    ROBOTFILE FileClass;
    string FILEPATH;
    ScreenBuild ScreenScript;
    void Awake ()
    {
        sty_ModeWindow.normal.background = (Texture2D)Resources.Load("Android/ModeWindow");
        sty_WirelessBtn.normal.background = (Texture2D)Resources.Load("Android/WirelessMode1");
        sty_WirelessBtn.hover.background = (Texture2D)Resources.Load("Android/WirelessMode2");
        sty_StandaloneBtn.normal.background = (Texture2D)Resources.Load("Android/Standalone1");
        sty_StandaloneBtn.hover.background = (Texture2D)Resources.Load("Android/Standalone2");
        sty_WirelessWindow.normal.background = (Texture2D)Resources.Load("Android/WirelessWindow");
        sty_DownBtn.normal.background = (Texture2D)Resources.Load("Android/DownArrow");
        sty_SwitchBtn.normal.background = (Texture2D)Resources.Load("Android/SwitchButton1");
        sty_SwitchBtn.active.background = (Texture2D)Resources.Load("Android/SwitchButton2");
        switchOff = (Texture2D)Resources.Load("Android/SwitchOff");
        switchOn = (Texture2D)Resources.Load("Android/SwitchOn");
        t2d_switch = switchOn;
        sty_Font.font = (Font)Resources.Load("font/msyh");
        sty_Font.fontSize = 20;
        sty_Font.normal.textColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
        sty_FontD.font = (Font)Resources.Load("font/msyh");
        sty_FontD.fontSize = 16;
        sty_FontD.normal.textColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
        sty_DownList.normal.background = (Texture2D)Resources.Load("Android/White");
        sty_Line.normal.background = (Texture2D)Resources.Load("Android/Line");
        sty_DownListBtn.hover.background = (Texture2D)Resources.Load("Android/DownListCursor");
        sty_DownListBtn.alignment = TextAnchor.MiddleCenter;
        sty_DownListBtn.font = (Font)Resources.Load("font/msyh");
        sty_DownListBtn.fontSize = 20;
        sty_DownListBtn.normal.textColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);

        //wireless_or_standalone = true;
        //chooseRect.x = Screen.width / 2 - chooseRect.width / 2;
        //chooseRect.y = Screen.height / 2 - chooseRect.height / 2;
        _connectWindow = false;
        _wirelessRect.x = Screen.width / 2 - _wirelessRect.width / 2;
        _wirelessRect.y = Screen.height / 2 - _wirelessRect.height / 2;

        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        if (ipHost.AddressList.Length > 1)
        {
            _displayIP = ipHost.AddressList[1].ToString();
            _IPList.Add(_displayIP);
            if (ipHost.AddressList.Length > 2)
            {
                for (int i = 2; i < ipHost.AddressList.Length; i++)
                {
                    _IPList.Add(ipHost.AddressList[i].ToString());
                }
            }
            _IPList.Add(ipHost.AddressList[0].ToString());
        }
        else
        {
            if (ipHost.AddressList.Length > 0)
            {
                _displayIP = ipHost.AddressList[0].ToString();
                _IPList.Add(_displayIP);
            }
        }
        foreach (string ip_string in _IPList)
        {
            Debug.Log(ip_string);        
        }
    }

    // Use this for initialization
    void Start ()
    {
        FILEPATH = Application.dataPath + "\\StreamingAssets\\Programs\\";
        FileClass = new ROBOTFILE(FILEPATH);
        ScreenScript = GameObject.Find("MyScreen").GetComponent<ScreenBuild>();

        //0参数函数绑定
        _Func_0Para.Add("OnJ1Press", ServerMediation.st_ButtonRespond.OnJ1Press);
        _Func_0Para.Add("OnJ1Release", ServerMediation.st_ButtonRespond.OnJ1Release);
        _Func_0Para.Add("OnJ1_Press", ServerMediation.st_ButtonRespond.OnJ1_Press);
        _Func_0Para.Add("OnJ1_Release", ServerMediation.st_ButtonRespond.OnJ1_Release);

        _Func_0Para.Add("OnJ2Press", ServerMediation.st_ButtonRespond.OnJ2Press);
        _Func_0Para.Add("OnJ2Release", ServerMediation.st_ButtonRespond.OnJ2Release);
        _Func_0Para.Add("OnJ2_Press", ServerMediation.st_ButtonRespond.OnJ2_Press);
        _Func_0Para.Add("OnJ2_Release", ServerMediation.st_ButtonRespond.OnJ2_Release);

        _Func_0Para.Add("OnJ3Press", ServerMediation.st_ButtonRespond.OnJ3Press);
        _Func_0Para.Add("OnJ3Release", ServerMediation.st_ButtonRespond.OnJ3Release);
        _Func_0Para.Add("OnJ3_Press", ServerMediation.st_ButtonRespond.OnJ3_Press);
        _Func_0Para.Add("OnJ3_Release", ServerMediation.st_ButtonRespond.OnJ3_Release);

        _Func_0Para.Add("OnJ4Press", ServerMediation.st_ButtonRespond.OnJ4Press);
        _Func_0Para.Add("OnJ4Release", ServerMediation.st_ButtonRespond.OnJ4Release);
        _Func_0Para.Add("OnJ4_Press", ServerMediation.st_ButtonRespond.OnJ4_Press);
        _Func_0Para.Add("OnJ4_Release", ServerMediation.st_ButtonRespond.OnJ4_Release);

        _Func_0Para.Add("OnJ5Press", ServerMediation.st_ButtonRespond.OnJ5Press);
        _Func_0Para.Add("OnJ5Release", ServerMediation.st_ButtonRespond.OnJ5Release);
        _Func_0Para.Add("OnJ5_Press", ServerMediation.st_ButtonRespond.OnJ5_Press);
        _Func_0Para.Add("OnJ5_Release", ServerMediation.st_ButtonRespond.OnJ5_Release);

        _Func_0Para.Add("OnJ6Press", ServerMediation.st_ButtonRespond.OnJ6Press);
        _Func_0Para.Add("OnJ6Release", ServerMediation.st_ButtonRespond.OnJ6Release);
        _Func_0Para.Add("OnJ6_Press", ServerMediation.st_ButtonRespond.OnJ6_Press);
        _Func_0Para.Add("OnJ6_Release", ServerMediation.st_ButtonRespond.OnJ6_Release);

        _Func_0Para.Add("OnStartClick", ServerMediation.st_ButtonRespond.OnStartClick);
        _Func_0Para.Add("OnStartRelease", ServerMediation.st_ButtonRespond.OnStartRelease);
        _Func_0Para.Add("OnStopClick", ServerMediation.st_ButtonRespond.OnStopClick);
        _Func_0Para.Add("OnStopRelease", ServerMediation.st_ButtonRespond.OnStopRelease);

        _Func_0Para.Add("OnScramClick", ServerMediation.st_ButtonRespond.OnScramClick);

        _Func_0Para.Add("OnReappearClick", ServerMediation.st_ButtonRespond.OnReappearClick);

        _Func_0Para.Add("OnTeachClick", ServerMediation.st_ButtonRespond.OnTeachClick);

        _Func_0Para.Add("OnRemoteClick", ServerMediation.st_ButtonRespond.OnTeachClick);

        _Func_0Para.Add("OnF1Click", ServerMediation.st_ButtonRespond.OnF1Click);

        _Func_0Para.Add("OnF2Click", ServerMediation.st_ButtonRespond.OnF2Click);

        _Func_0Para.Add("OnF3Click", ServerMediation.st_ButtonRespond.OnF3Click);

        _Func_0Para.Add("OnF4Click", ServerMediation.st_ButtonRespond.OnF4Click);

        _Func_0Para.Add("OnF5Click", ServerMediation.st_ButtonRespond.OnF5Click);

        _Func_0Para.Add("OnUpClick", ServerMediation.st_ButtonRespond.OnUpClick);

        _Func_0Para.Add("OnLeftClick", ServerMediation.st_ButtonRespond.OnLeftClick);

        _Func_0Para.Add("OnDownClick", ServerMediation.st_ButtonRespond.OnDownClick);

        _Func_0Para.Add("OnRightClick", ServerMediation.st_ButtonRespond.OnRightClick);

        _Func_0Para.Add("OnReadyClick", ServerMediation.st_ButtonRespond.OnReadyClick);

        _Func_0Para.Add("OnTabClick", ServerMediation.st_ButtonRespond.OnTabClick);

        _Func_0Para.Add("OnChooseClick", ServerMediation.st_ButtonRespond.OnChooseClick);

        _Func_0Para.Add("OnCancelClick", ServerMediation.st_ButtonRespond.OnCancelClick);

        _Func_0Para.Add("OnClearClick", ServerMediation.st_ButtonRespond.OnClearClick);

        _Func_0Para.Add("OnCutClick", ServerMediation.st_ButtonRespond.OnCutClick);

        _Func_0Para.Add("OnCopyClick", ServerMediation.st_ButtonRespond.OnCopyClick);

        _Func_0Para.Add("OnDeleteClick", ServerMediation.st_ButtonRespond.OnDeleteClick);

        _Func_0Para.Add("OnModifyClick", ServerMediation.st_ButtonRespond.OnModifyClick);

        _Func_0Para.Add("OnInputClick", ServerMediation.st_ButtonRespond.OnInputClick);

        _Func_0Para.Add("OnAddClick", ServerMediation.st_ButtonRespond.OnAddClick);

        _Func_0Para.Add("OnLeft_Click", ServerMediation.st_ButtonRespond.OnLeft_Click);

        _Func_0Para.Add("OnGetClick", ServerMediation.st_ButtonRespond.OnGetClick);

        _Func_0Para.Add("OnSpeedUpClick", ServerMediation.st_ButtonRespond.OnSpeedUpClick);

        _Func_0Para.Add("OnSpeedDownClick", ServerMediation.st_ButtonRespond.OnSpeedDownClick);

        _Func_0Para.Add("OnSetClick", ServerMediation.st_ButtonRespond.OnSetClick);

        _Func_0Para.Add("OnIsContinueClick", ServerMediation.st_ButtonRespond.OnIsContinueClick);

        _Func_0Para.Add("OnOutShaftClick", ServerMediation.st_ButtonRespond.OnOutShaftClick);

        _Func_0Para.Add("OnGoforwardClick", ServerMediation.st_ButtonRespond.OnGoforwardClick);

        //1个Int参数绑定
        _Func_1Para_Int.Add("OnInBtClick", ServerMediation.st_ButtonRespond.OnInBtClick);
        _Func_1Para_Int.Add("OnOutBtClick", ServerMediation.st_ButtonRespond.OnOutBtClick);


        //1个String参数绑定
        _Func_1Para_String.Add("OnNumber", ServerMediation.st_ScreenBuild.OnNumber);


        //2个Float参数绑定
        _Func_2Para_Float.Add("RotateMotion", ServerMediation.st_Camerascript.RotateMotion);

        _Func_2Para_Float.Add("MoveMotion", ServerMediation.st_Camerascript.MoveMotion);


        //1个Float参数绑定
        _Func_1Para_Float.Add("ZoomMotion", ServerMediation.st_Camerascript.ZoomMotion);


        //1个Bool参数绑定
        _Func_1Para_Bool.Add("MobileMotion", ServerMediation.st_Camerascript.MobileMotion);



    }

    void OnGUI ()
    {
        //if (wireless_or_standalone)
        //{
        //    chooseRect = GUI.Window(9, chooseRect, ModeChoose, "运行模式选择");
        //}

        if (_connectWindow && !_isConnected && _inServerMode)
        {
            _wirelessRect = GUI.Window(10, _wirelessRect, WirelessWindow, "", sty_WirelessWindow);
        }

        //IP地址下拉菜单显示窗口
        if (downList)
        {
            downRect = GUI.Window(13, downRect, DownListWindow, "", sty_DownList);
              GUI.BringWindowToFront(13);
        }

        //下拉菜关闭
        Event mouse_e = Event.current;
        if (downList && !downRect.Contains(mouse_e.mousePosition) && mouse_e.isMouse &&
            mouse_e.type == EventType.MouseDown && ( mouse_e.button == 0 || mouse_e.button == 1 || mouse_e.button == 2 ))
        {
            downList = false;
        }
    }

    void WirelessWindow (int WindowID)
    {
        //GUI.Label(new Rect(10, 15, 100, 20), "本机IP地址: ");
        //GUI.Label(new Rect(13, 30, 100, 20), "（优先连接）: ");
        //GUI.Box(new Rect(98, 28, 170, 102), "");

        Event mouse_e = Event.current;
        if (downList && !downRect.Contains(mouse_e.mousePosition) && mouse_e.isMouse &&
            mouse_e.type == EventType.MouseDown && ( mouse_e.button == 0 || mouse_e.button == 1 || mouse_e.button == 2 ))
        {
            downList = false;
        }

        GUI.Label(new Rect(70, 97, 160, 100), _displayIP, sty_Font);

        GUI.Label(new Rect(20, 150, 160, 100), "连接不上时请点击右侧箭头更换合适IP", sty_FontD);

        //if (_IPList.Count == 0)
        //{
        //    _displayIP = "无法获得本机IP, 请检查网络连接是否正常！";
        //}
        //else if (Network.peerType != NetworkPeerType.Server)
        //{
        //    _displayIP = "无法从本机创建服务器, 请检查网络连接是否正常！";
        //}
        //else
        //{
        //    _displayIP = "";
        //    for (int i = 0; i < _IPList.Count; i++)
        //    {
        //        _displayIP += _IPList[i] + "\n";
        //    }
        //}
        //GUI.Label(new Rect(100, 30, 160, 100), _displayIP);
        //if (Network.peerType == NetworkPeerType.Server)
        //{
        //    GUI.Label(new Rect(10, 140, 250, 50), "服务器创建成功，等待无线设备连接！");
        //}

        if (GUI.Button(new Rect(280, 103, 18, 14), "", sty_DownBtn))
        {
            if (_IPList.Count > 1)
            {
                downList = true;
                downRect.x = _wirelessRect.x + 53;
                downRect.y = _wirelessRect.y + 134;
                downRect.width = 264;
                downRect.height = _IPList.Count * 45;
            }
        }

        if (GUI.Button(new Rect(40, 206, 243, 48), "", sty_SwitchBtn))
        {
            _connectWindow = false;
            _inServerMode = false;
            //切换到单击模式

        }
        GUI.DragWindow();
    }

    //IP地址下拉List
    void DownListWindow (int WindowID)
    {
        for (int i = 0; i < _IPList.Count; i++)
        {
            if (GUI.Button(new Rect(0, i * 45, 264, 45), _IPList[i], sty_DownListBtn))
            {
                ButtonClick(i);
            }
        }
        for (int i = 0; i < _IPList.Count - 1; i++)
        {
            GUI.Label(new Rect(0, ( i + 1 ) * 45 - 5, 264, 10), "", sty_Line);
        }
    }

    //IP地址显示选择
    void ButtonClick (int index)
    {
        _displayIP = _IPList[index];
        downList = false;
    }

    

    //void ModeChoose (int WindowID)
    //{

    //    if (GUI.Button(new Rect(50, 50, 200, 40), "无线模式"))
    //    {
    //        wireless_window = true;
    //        Network.InitializeServer(100, _port, false);
    //        wireless_or_standalone = false;
    //    }

    //    if (GUI.Button(new Rect(50, 110, 200, 40), "单机模式"))
    //    {
    //        wireless_or_standalone = false;
    //    }

    //    GUI.DragWindow();
    //}

    void OnPlayerConnected (NetworkPlayer player)
    {
        _connectWindow = false;
        _isConnected = true;
        if (!_connectedMachine.ContainsKey(player.guid))
        {
            _connectedMachine.Add(player.guid, player);
        }

        //连接上
        //1、关闭当前PC端示教盒
        ServerMediation.st_ScreenBuild.WindowInitial();
        ServerMediation.st_ButtonRespond.CloseIOPanel();
        ServerMediation.st_ButtonRespond.ClosePanel();
        //2、本地恢复到初始状态
        ServerMediation.st_RobotMotion.ReturnToZero();
        ServerMediation.st_Camerascript.ResetScene();
        //
        DataClear_RPC();
        SendFileInfo_RPC();
        networkView.RPC("WinInitial_RPC", RPCMode.Others);
        SetProgram();
        networkView.RPC("InitialProgramInterface", RPCMode.Others);
    }

    void OnPlayerDisconnected (NetworkPlayer player)
    {
        _connectWindow = true;
        if (_connectedMachine.ContainsKey(player.guid))
        {
            _connectedMachine.Remove(player.guid);
        }
        if (_connectedMachine.Count == 0)
        {
            _isConnected = false;
        }
    }

    void OnApplicationQuit ()
    {
        if (_isServerOn)
        {
            if (_connectedMachine.Count > 0)
            {
                foreach (string id in _connectedMachine.Keys)
                {
                    Network.CloseConnection(_connectedMachine[id], true);
                }
            }
            _isConnected = false;
            _isServerOn = false;
        }
    }

    public void EnterServerMode ()
    {
        downList = false;
        if (!_isServerOn)
        {
            Network.InitializeServer(100, _port, false);
            _isServerOn = true;
        }
        _connectWindow = true;
        _inServerMode = true;
    }

    public void ExitServerMode ()
    {
        _inServerMode = false;
        _connectWindow = false;
        if (_isServerOn)
        {
            if (_connectedMachine.Count > 0)
            {
                foreach (string id in _connectedMachine.Keys)
                {
                    Network.CloseConnection(_connectedMachine[id], true);
                }
            }
            Network.Disconnect();
            _isServerOn = false;
            _isConnected = false;
        }
    }

    public static void Send_0Para ()
    { 
        
    }


    [RPC]
    public void Send_0ParaRPC (string func_name)
    {
        if (_Func_0Para.ContainsKey(func_name))
        {
            _Func_0Para[func_name]();
        }
    }


    public static void Send_1Para_Int (string func_name, int value)
    {

    }


    [RPC]
    public void Send_1Para_Int_RPC (string func_name, int value)
    {
        if (_Func_1Para_Int.ContainsKey(func_name))
        {
            _Func_1Para_Int[func_name](value);
        }
    }


    public void Send_1Para_String (string func_name, string value)
    {

    }

    [RPC]
    private void Send_1Para_String_RPC (string func_name, string value)
    {
        if (_Func_1Para_String.ContainsKey(func_name))
        {
            _Func_1Para_String[func_name](value);
        }
    }

    public void Send_1Para_Bool (string func_name, bool value)
    {

    }

    [RPC]
    private void Send_1Para_Bool_RPC (string func_name, bool value)
    {
        if (_Func_1Para_Bool.ContainsKey(func_name))
        {
            _Func_1Para_Bool[func_name](value);
        }
    }

    public void Send_1Para_Float (string func_name, float value)
    {

    }

    [RPC]
    private void Send_1Para_Float_RPC (string func_name, float value)
    {
        if (_Func_1Para_Float.ContainsKey(func_name))
        {
            _Func_1Para_Float[func_name](value);
        }
    }

    public void Send_2Para_Float (string func_name, float value1, float value2)
    {
        
    }

    [RPC]
    private void Send_2Para_Float_RPC (string func_name, float value1, float value2)
    {
        if (_Func_2Para_Float.ContainsKey(func_name))
        {
            _Func_2Para_Float[func_name](value1, value2);
        }
    }



    //PAD 菜单触发部分
    [RPC]
    public void SceneReturn_RPC()
    {
        GameObject.Find("CameraFree").GetComponent<Camerascript>().ResetScene();
    }
    [RPC]
    public void ReturnToZero_RPC()
    {
        GameObject.Find("MyMotion").GetComponent<RobotMotion>().ReturnToZero();
    }
    [RPC]
    public void CloseServer_RPC()
    {
        ExitServerMode();
    }
    [RPC]
    public void OpenORCloseView_RPC()
    {
        GameObject.Find("CameraMin").GetComponent<minmap>().ShowMinMap();
    }
    [RPC]
    public void SwitchView_RPC()
    {
        GameObject.Find("CameraMin").GetComponent<minmap>().ChangeCameraPos();
    }


    //
    [RPC]
    public void DataClear_RPC()
    {
        if (IsConnected)
        {
            networkView.RPC("DataClear_RPC", RPCMode.Others);
        }
    }
    [RPC]
    public void ClearFileListInfo_RPC()
    {
        
    }
    [RPC]
    public void GetFileListInfo_RPC()
    {
        if (IsConnected)
        {
            networkView.RPC("ClearFileListInfo_RPC", RPCMode.Server);
            for (int i = 0; i < GSKDATA.FileListInfo.Count; i++)
            {
                networkView.RPC("RecieveFileInfo_RPC", RPCMode.Others, GSKDATA.FileListInfo[i]);
            }
        }
    }

    // 光标运动到指定行
    public void CursorToLine(int line)
    {
        if (IsConnected)
        {
            networkView.RPC("CursorToLine_RPC", RPCMode.Others, line);
        }
    }
    [RPC]
    private void CursorToLine_RPC(int line)
    {

    }

    //从程序列表进入F2
    [RPC]
    public void EnterProgramFromList_RPC()
    {
        if (IsConnected)
        {
            SetProgram();
            networkView.RPC("InitialProgramInterface", RPCMode.Others);
            EnterF2_RPC();
        }
        

    }
    [RPC]
    public void EnterF2_RPC()
    {
        networkView.RPC("EnterF2_RPC", RPCMode.Others);
    }
    //译码
    [RPC]
    private void YIMA_RPC()
    {
        ScreenScript.GrammarTodata(); 
    }
    //覆盖示教点
    [RPC]
    public void PointRepeatWin_RPC()
    {
        if (IsConnected)
        {
            networkView.RPC("PointRepeatWin_RPC", RPCMode.Others);
        }
    }
   
    //创建程序
    public void CreateProgram(string filename)
    {
        if (IsConnected)
        {
            networkView.RPC("CreateProgram_RPC", RPCMode.Others, filename);
        }
    }
    [RPC]
    private void CreateProgram_RPC(string filename)
    {

    }
    //读取程序
    [RPC]
    private void ReadContent_RPC(string name)
    {

    }
    //写入程序
    [RPC]
    private void WriteContent_RPC(string line_content)
    {

    }

    //发送fileinfo
    public void SendFileInfo_RPC()
    {
        if (IsConnected)
        {
            for (int i = 0; i < GSKDATA.FileListInfo.Count; i++)
            {
                networkView.RPC("RecieveFileInfo_RPC", RPCMode.Others, GSKDATA.FileListInfo[i]);
            }
        }
    }
    //接受程序信息列表
    [RPC]
    public void RecieveFileInfo_RPC(string fileinfo)
    {

    }
    // 示教器的初始化
    [RPC]
    public void WinInitial_RPC()
    {

    }
    //设置程序名
    [RPC]
    public void SetProgramName(string name)
    {
        
    }
    [RPC]
    public void StorageContent(string line_content)
    {

    }
    // 设置当前程序
    public void SetProgram()
    {
        networkView.RPC("SetProgramName", RPCMode.Others, GSKDATA.CurrentProgramName);
        GSKDATA.ProgramContents = FileClass.GetFileContents(GSKDATA.CurrentProgramName);
        networkView.RPC("ClearProgramContent", RPCMode.Others);
        for (int i = 0; i < GSKDATA.ProgramContents.Count; i++)
        {
            networkView.RPC("StorageContent", RPCMode.Others, GSKDATA.ProgramContents[i]);
        }



    }
    [RPC]
    public void ClearProgramContent()
    {
        
    }
    [RPC]
    public void InitialProgramInterface()
    {

    }
    //警告信息
    public void WarningMes(string message)
    {
        if (IsConnected)
        {
            networkView.RPC("WarningMes_RPC", RPCMode.Others, message);
        }
    }
    [RPC]
    public void WarningMes_RPC(string message)
    {

    }
}
