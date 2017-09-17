//<summary>
//ButtonRespond#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class ButtonRespond : MonoBehaviour {
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_IN_Data@@YAHH_N@Z")]
    //private static extern int RBP_Set_IN_Data(int num, bool value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Get_IN_Data@@YAHHAA_N@Z")]
    //private static extern int RBP_Get_IN_Data(int num, ref bool value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Get_OT_Data@@YAHHAA_N@Z")]
    //private static extern int RBP_Get_OT_Data(int num, ref bool value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_OT_Data@@YAHH_N@Z")]
    //private static extern int RBP_Set_OT_Data(int num, bool value);

    ScreenBuild ScreenScript;
    GameObject IO_Panel;//io面板
    UIButton[] PanelBt = new UIButton[62];//62 buttons on the panel 
    Vector3[] PanelBt_pos = new Vector3[62];//62 buttons on the panel 
    string[] PanelBt_name = new string[62];//panel name
    CursorMove_zw Mouse;
    Vector2 s = Vector2.zero;//mouse start postion
    Vector2 e = Vector2.zero;//mouse end position

    const float clickTime = 1f;

    void Awake()
    {
        PanelBt_name[0] = "F1Bt"; PanelBt_name[13] = "Get"; PanelBt_name[26] = "J5+"; PanelBt_name[39] = "6numb"; PanelBt_name[52] = "0numb";
        PanelBt_name[1] = "F2Bt"; PanelBt_name[14] = "J1+"; PanelBt_name[27] = "J5-"; PanelBt_name[40] = "Delete"; PanelBt_name[53] = ".numb";
        PanelBt_name[2] = "F3Bt"; PanelBt_name[15] = "J1-"; PanelBt_name[28] = "J3+"; PanelBt_name[41] = "Add"; PanelBt_name[54] = "Left_";
        PanelBt_name[3] = "F4Bt"; PanelBt_name[16] = "Flip"; PanelBt_name[29] = "J3-"; PanelBt_name[42] = "Out"; PanelBt_name[55] = "Apply";
        PanelBt_name[4] = "F5Bt"; PanelBt_name[17] = "SpeedUpBT"; PanelBt_name[30] = "7numb"; PanelBt_name[43] = "Cut"; PanelBt_name[56] = "Reappear";
        PanelBt_name[5] = "Up"; PanelBt_name[18] = "Switch"; PanelBt_name[31] = "8numb"; PanelBt_name[44] = "1numb"; PanelBt_name[57] = "Remote";
        PanelBt_name[6] = "Chose"; PanelBt_name[19] = "J4+"; PanelBt_name[32] = "9numb"; PanelBt_name[45] = "2numb"; PanelBt_name[58] = "Teach";
        PanelBt_name[7] = "Left"; PanelBt_name[20] = "J4-"; PanelBt_name[33] = "J6+"; PanelBt_name[46] = "3numb"; PanelBt_name[59] = "Start";
        PanelBt_name[8] = "Right"; PanelBt_name[21] = "J2+"; PanelBt_name[34] = "J6-"; PanelBt_name[47] = "Copy"; PanelBt_name[60] = "Stop";
        PanelBt_name[9] = "Down"; PanelBt_name[22] = "J2-"; PanelBt_name[35] = "Clean"; PanelBt_name[48] = "Modify"; PanelBt_name[61] = "Scram";
        PanelBt_name[10] = "Ready"; PanelBt_name[23] = "1Or2"; PanelBt_name[36] = "Input"; PanelBt_name[49] = "Goforward"; 
        PanelBt_name[11] = "Cancel"; PanelBt_name[24] = "SpeedDownBT"; PanelBt_name[37] = "4numb"; PanelBt_name[50] = "Goback";
        PanelBt_name[12] = "TAB"; PanelBt_name[25] = "Set"; PanelBt_name[38] = "5numb"; PanelBt_name[51] = "-numb";
        for (int i = 0; i < 62; i++)
        {
            PanelBt[i] = GameObject.Find(PanelBt_name[i]).GetComponent<UIButton>();
        }
        //PanelBt[0] = GameObject.Find("F1Bt").GetComponent<UIButton>(); PanelBt[19] = GameObject.Find("J4+").GetComponent<UIButton>(); PanelBt[38] = GameObject.Find("5numb").GetComponent<UIButton>();
        //PanelBt[1] = GameObject.Find("F2Bt").GetComponent<UIButton>(); PanelBt[20] = GameObject.Find("J4-").GetComponent<UIButton>(); PanelBt[39] = GameObject.Find("6numb").GetComponent<UIButton>();
        //PanelBt[2] = GameObject.Find("F3Bt").GetComponent<UIButton>(); PanelBt[21] = GameObject.Find("J2+").GetComponent<UIButton>(); PanelBt[40] = GameObject.Find("Delete").GetComponent<UIButton>();
        //PanelBt[3] = GameObject.Find("F4Bt").GetComponent<UIButton>(); PanelBt[22] = GameObject.Find("J2-").GetComponent<UIButton>(); PanelBt[41] = GameObject.Find("Add").GetComponent<UIButton>();
        //PanelBt[4] = GameObject.Find("F5Bt").GetComponent<UIButton>(); PanelBt[23] = GameObject.Find("1Or2").GetComponent<UIButton>(); PanelBt[42] = GameObject.Find("Out").GetComponent<UIButton>();
        //PanelBt[5] = GameObject.Find("Up").GetComponent<UIButton>(); PanelBt[24] = GameObject.Find("SpeedDownBT").GetComponent<UIButton>(); PanelBt[43] = GameObject.Find("Cut").GetComponent<UIButton>();
        //PanelBt[6] = GameObject.Find("Chose").GetComponent<UIButton>(); PanelBt[25] = GameObject.Find("Set").GetComponent<UIButton>(); PanelBt[44] = GameObject.Find("1numb").GetComponent<UIButton>();
        //PanelBt[7] = GameObject.Find("Left").GetComponent<UIButton>(); PanelBt[26] = GameObject.Find("J5+").GetComponent<UIButton>(); PanelBt[45] = GameObject.Find("2numb").GetComponent<UIButton>();
        //PanelBt[8] = GameObject.Find("Right").GetComponent<UIButton>(); PanelBt[27] = GameObject.Find("J5-").GetComponent<UIButton>(); PanelBt[46] = GameObject.Find("3numb").GetComponent<UIButton>();
        //PanelBt[9] = GameObject.Find("Down").GetComponent<UIButton>(); PanelBt[28] = GameObject.Find("J3+").GetComponent<UIButton>(); PanelBt[47] = GameObject.Find("Copy").GetComponent<UIButton>();
        //PanelBt[10] = GameObject.Find("Ready").GetComponent<UIButton>(); PanelBt[29] = GameObject.Find("J3-").GetComponent<UIButton>(); PanelBt[48] = GameObject.Find("Modify").GetComponent<UIButton>();
        //PanelBt[11] = GameObject.Find("Cancel").GetComponent<UIButton>(); PanelBt[30] = GameObject.Find("7numb").GetComponent<UIButton>(); PanelBt[49] = GameObject.Find("Goforward").GetComponent<UIButton>();
        //PanelBt[12] = GameObject.Find("TAB").GetComponent<UIButton>(); PanelBt[31] = GameObject.Find("8numb").GetComponent<UIButton>(); PanelBt[50] = GameObject.Find("Goback").GetComponent<UIButton>();
        //PanelBt[13] = GameObject.Find("Get").GetComponent<UIButton>(); PanelBt[32] = GameObject.Find("9numb").GetComponent<UIButton>(); PanelBt[51] = GameObject.Find("-numb").GetComponent<UIButton>();
        //PanelBt[14] = GameObject.Find("J1+").GetComponent<UIButton>(); PanelBt[33] = GameObject.Find("J6+").GetComponent<UIButton>(); PanelBt[52] = GameObject.Find("0numb").GetComponent<UIButton>();
        //PanelBt[15] = GameObject.Find("J1-").GetComponent<UIButton>(); PanelBt[34] = GameObject.Find("J6-").GetComponent<UIButton>(); PanelBt[53] = GameObject.Find(".numb").GetComponent<UIButton>();
        //PanelBt[16] = GameObject.Find("Flip").GetComponent<UIButton>(); PanelBt[35] = GameObject.Find("Clean").GetComponent<UIButton>(); PanelBt[54] = GameObject.Find("Left_").GetComponent<UIButton>();
        //PanelBt[17] = GameObject.Find("SpeedUpBT").GetComponent<UIButton>(); PanelBt[36] = GameObject.Find("Input").GetComponent<UIButton>(); PanelBt[55] = GameObject.Find("Apply").GetComponent<UIButton>();
        //PanelBt[18] = GameObject.Find("Switch").GetComponent<UIButton>(); PanelBt[37] = GameObject.Find("4numb").GetComponent<UIButton>(); PanelBt[56] = GameObject.Find("Reappear").GetComponent<UIButton>();
        //PanelBt[57] = GameObject.Find("Remote").GetComponent<UIButton>(); PanelBt[58] = GameObject.Find("Teach").GetComponent<UIButton>(); PanelBt[59] = GameObject.Find("Start").GetComponent<UIButton>();
        //PanelBt[60] = GameObject.Find("Stop").GetComponent<UIButton>(); PanelBt[61] = GameObject.Find("Scram").GetComponent<UIButton>();
    }

    void Start()
    {

        ScreenScript = GameObject.Find("MyScreen").GetComponent<ScreenBuild>();
        Mouse = GameObject.Find("MainScript").GetComponent<CursorMove_zw>();
        IO_Panel = GameObject.Find("panellight");
        CloseIOPanel();//一开始不显示IO面板
        ClosePanel();
    }

    void Update()
    {
        //for (int i = 0; i < 32; i++)
        //{
        //    //int rr = RBP_Get_OT_Data(i, ref GSKDATA.OutInfo[i]);
        //    ////Debug.Log(GSKDATA.OutInfo[i]);
        //    //int ww = RBP_Get_IN_Data(i, ref GSKDATA.InInfo[i]);
            

        //}
    }
    void FixedUpdate()
    {
        for (int i = 0; i < 32; i++)
        {
            InLight(i);
            OutLight(i);
        }
    }

    #region ------------------------IO面板触发区-----------------------
    public void OnInBt0()
    {
        OnInBtClick(0);
    }
    public void OnInBt1()
    {
        OnInBtClick(1);
    }
    public void OnInBt2()
    {
        OnInBtClick(2);
    }
    public void OnInBt3()
    {
        OnInBtClick(3);
    }
    public void OnInBt4()
    {
        OnInBtClick(4);
    }
    public void OnInBt5()
    {
        OnInBtClick(5);
    }
    public void OnInBt6()
    {
        OnInBtClick(6);
    }
    public void OnInBt7()
    {
        OnInBtClick(7);
    }
    public void OnInBt8()
    {
        OnInBtClick(8);
    }
    public void OnInBt9()
    {
        OnInBtClick(9);
    }
    public void OnInBt10()
    {
        OnInBtClick(10);
    }
    public void OnInBt11()
    {
        OnInBtClick(11);
    }
    public void OnInBt12()
    {
        OnInBtClick(12);
    }
    public void OnInBt13()
    {
        OnInBtClick(13);
    }
    public void OnInBt14()
    {
        OnInBtClick(14);
    }
    public void OnInBt15()
    {
        OnInBtClick(15);
    }
    public void OnInBt16()
    {
        OnInBtClick(16);
    }
    public void OnInBt17()
    {
        OnInBtClick(17);
    }
    public void OnInBt18()
    {
        OnInBtClick(18);
    }
    public void OnInBt19()
    {
        OnInBtClick(19);
    }
    public void OnInBt20()
    {
        OnInBtClick(20);
    }
    public void OnInBt21()
    {
        OnInBtClick(21);
    }
    public void OnInBt22()
    {
        OnInBtClick(22);
    }
    public void OnInBt23()
    {
        OnInBtClick(23);
    }
    public void OnInBt24()
    {
        OnInBtClick(24);
    }
    public void OnInBt25()
    {
        OnInBtClick(25);
    }
    public void OnInBt26()
    {
        OnInBtClick(26);
    }
    public void OnInBt27()
    {
        OnInBtClick(27);
    }
    public void OnInBt28()
    {
        OnInBtClick(28);
    }
    public void OnInBt29()
    {
        OnInBtClick(29);
    }
    public void OnInBt30()
    {
        OnInBtClick(20);
    }
    public void OnInBt31()
    {
        OnInBtClick(31);
    }
    public void OnInBtClick(int num)
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            return;
        }
        GSKDATA.InInfo[num] = !GSKDATA.InInfo[num];
        ScreenScript.setInData(num, GSKDATA.InInfo[num] ? 1 : 0);
        //int ww = RBP_Set_IN_Data(num, GSKDATA.InInfo[num]);
        //InLight(num);
    }
    public void SetInfo(int num, bool value)
    {
        GSKDATA.InInfo[num] = value;
        //int ww = RBP_Set_IN_Data(num, GSKDATA.InInfo[num]);
        ScreenScript.setInData(num, GSKDATA.InInfo[num] ? 1 : 0);
    }
    public void InLight(int num)
    {
        
        if (GSKDATA.InInfo[num])
        {
            GameObject.Find("IN" + (num)).GetComponent<UISprite>().spriteName = "light1";
        }
        else
        {
            GameObject.Find("IN" + (num)).GetComponent<UISprite>().spriteName = "light2";
        }
    }
    public void OnOutBt0()
    {
        OnOutBtClick(0);
    }
    public void OnOutBt1()
    {
        OnOutBtClick(1);
    }
    public void OnOutBt2()
    {
        OnOutBtClick(2);
    }
    public void OnOutBt3()
    {
        OnOutBtClick(3);
    }
    public void OnOutBt4()
    {
        OnOutBtClick(4);
    }
    public void OnOutBt5()
    {
        OnOutBtClick(5);
    }
    public void OnOutBt6()
    {
        OnOutBtClick(6);
    }
    public void OnOutBt7()
    {
        OnOutBtClick(7);
    }
    public void OnOutBt8()
    {
        OnOutBtClick(8);
    }
    public void OnOutBt9()
    {
        OnOutBtClick(9);
    }
    public void OnOutBt10()
    {
        OnOutBtClick(10);
    }
    public void OnOutBt11()
    {
        OnOutBtClick(11);
    }
    public void OnOutBt12()
    {
        OnOutBtClick(12);
    }
    public void OnOutBt13()
    {
        OnOutBtClick(13);
    }
    public void OnOutBt14()
    {
        OnOutBtClick(14);
    }
    public void OnOutBt15()
    {
        OnOutBtClick(15);
    }
    public void OnOutBt16()
    {
        OnOutBtClick(16);
    }
    public void OnOutBt17()
    {
        OnOutBtClick(17);
    }
    public void OnOutBt18()
    {
        OnOutBtClick(18);
    }
    public void OnOutBt19()
    {
        OnOutBtClick(19);
    }
    public void OnOutBt20()
    {
        OnOutBtClick(20);
    }
    public void OnOutBt21()
    {
        OnOutBtClick(21);
    }
    public void OnOutBt22()
    {
        OnOutBtClick(22);
    }
    public void OnOutBt23()
    {
        OnOutBtClick(23);
    }
    public void OnOutBt24()
    {
        OnOutBtClick(24);
    }
    public void OnOutBt25()
    {
        OnOutBtClick(25);
    }
    public void OnOutBt26()
    {
        OnOutBtClick(26);
    }
    public void OnOutBt27()
    {
        OnOutBtClick(27);
    }
    public void OnOutBt28()
    {
        OnOutBtClick(28);
    }
    public void OnOutBt29()
    {
        OnOutBtClick(29);
    }
    public void OnOutBt30()
    {
        OnOutBtClick(30);
    }
    public void OnOutBt31()
    {
        OnOutBtClick(31);
    }
    public void OnOutBtClick(int num)
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            return;
        }
        GSKDATA.OutInfo[num] = !GSKDATA.OutInfo[num];
        //int ww = RBP_Set_OT_Data(num, GSKDATA.OutInfo[num]);
        ScreenScript.setOutData(num, GSKDATA.OutInfo[num] ? 1 : 0);
        //OutLight(num);
    }
    public void SetOutfo(int num, bool value)
    {
        GSKDATA.OutInfo[num] = value;
        //int ww = RBP_Set_OT_Data(num, GSKDATA.OutInfo[num]);
        ScreenScript.setOutData(num, GSKDATA.OutInfo[num] ? 1 : 0);
    }
    public void OnOutBtClick_Teach(int num)
    {
        GSKDATA.OutInfo[num] = !GSKDATA.OutInfo[num];
        //int ww = RBP_Set_OT_Data(num, GSKDATA.OutInfo[num]);
        ScreenScript.setOutData(num, GSKDATA.OutInfo[num] ? 1 : 0);
        //OutLight(num);
    }
    public void OutLight(int num)
    {
        //int ww = RBP_Set_OT_Data(num, GSKDATA.OutInfo[num]);
        if (GSKDATA.OutInfo[num])
        {
            GameObject.Find("OUT" + (num)).GetComponent<UISprite>().spriteName = "light1";
        }
        else
        {
            GameObject.Find("OUT" + (num)).GetComponent<UISprite>().spriteName = "light2";
        }
    }
    #endregion
    public void OnStartClick()
    {
        ScreenScript.OnStart();
    }
    public void OnStartRelease()
    {
        ScreenScript.OnStartUp();
    }
    public void OnStopClick()
    {
        ScreenScript.OnStop();
    }
    public void OnStopRelease()
    {
        ScreenScript.OnStopUp();
    }
    public void OnScramClick()
    {
        ScreenScript.OnScram();
    }
    public void OnReappearClick()
    {
        ScreenScript.OnReappear();
    }
    public void OnTeachClick()
    {
        ScreenScript.OnTeach();
    }
    public void OnRemoteClick()
    {
        ScreenScript.OnRemote();
    }
    public void OnF1Click()
    {
        ScreenScript.OnF1();
    }

    public void OnF2Click()
    {
        ScreenScript.OnF2();
    }

    public void OnF3Click()
    {
        ScreenScript.OnF3();
    }

    public void OnF4Click()
    {
        ScreenScript.OnF4();
    }

    public void OnF5Click()
    {
        ScreenScript.OnF5();
    }

    public void OnUpClick()
    {
        ScreenScript.OnUp();
    }

    public void OnLeftClick()
    {
        ScreenScript.OnLeft();
    }

    public void OnDownClick()
    {
        ScreenScript.OnDown();
    }

    public void OnRightClick()
    {
        ScreenScript.OnRight();
    }
    public void OnReadyClick()
    {
        ScreenScript.OnReady();
    }
    public void OnSwitchClick(){
        ScreenScript.OnSwitch(); ;
    }
    public void OnTabClick()
    {
        ScreenScript.OnTab();
    }
    public void OnFlipClick(){
        ;
    }
    public void OnChooseClick()
    {
        ScreenScript.OnChoose();
    }

    public void OnCancelClick()
    {
        ScreenScript.OnCancel();
    }
    public void OnClearClick()
    {
        ScreenScript.OnClear();
    }

    public void OnCutClick()
    {
        ScreenScript.OnCut();
    }

    public void OnCopyClick()
    {
        ScreenScript.OnCopy();
    }

    public void OnDeleteClick()
    {
        ScreenScript.OnDelete();
    }

    public void OnModifyClick()
    {
        ScreenScript.OnModify();
    }

    public void OnInputClick()
    {
        ScreenScript.OnInput();
    }
    public void OnOutClick(){
        ;
    }
    public void OnAddClick()
    {
        ScreenScript.OnAdd();
    }
    public void OnLeft_Click()
    {
        ScreenScript.OnLeft_();
    }
    public void OnGetClick()
    {
        ScreenScript.OnGet();
    }
    public void OnApplyClick(){
        ;
    }
    public void OnSpeedUpClick()
    {
        ScreenScript.OnSpeedUp();
    }

    public void OnSpeedDownClick()
    {
        ScreenScript.OnSpeedDown();
    }

    public void OnSetClick()
    {
        ScreenScript.OnSet();
    }

    public void OnIsContinueClick()
    {
        ScreenScript.OnIsContinue();
    }
    public void OnOutShaftClick()
    {
        ScreenScript.OnOutShaft();
    }



    public void OnNum_0()
    {
        ScreenScript.OnNumber("0");
    }
    public void OnNum_1()
    {
        ScreenScript.OnNumber("1");
    }
    public void OnNum_2()
    {
        ScreenScript.OnNumber("2");
    }
    public void OnNum_3()
    {
        ScreenScript.OnNumber("3");
    }
    public void OnNum_4()
    {
        ScreenScript.OnNumber("4");
    }
    public void OnNum_5()
    {
        ScreenScript.OnNumber("5");
    }
    public void OnNum_6()
    {
        ScreenScript.OnNumber("6");
    }
    public void OnNum_7()
    {
        ScreenScript.OnNumber("7");
    }
    public void OnNum_8()
    {
        ScreenScript.OnNumber("8");
    }
    public void OnNum_9()
    {
        ScreenScript.OnNumber("9");
    }
    public void OnNum_dot()
    {
        ScreenScript.OnNumber(".");
    }
    public void OnNum_minus()
    {
        ScreenScript.OnNumber("-");
    }
    public void OnGoforwardClick()
    {
        ScreenScript.OnGoforwardClick();
    }

    //放大示教器
    public void LargePanel()
    {
        Camera panelcamera = GameObject.Find("CameraUI").GetComponent<Camera>();
        if(panelcamera.orthographicSize>0.5f)
        {
            panelcamera.orthographicSize -= 0.5f;
        }
    }

    //缩小示教器
    public void MinifyPanel()
    {
        Camera panelcamera = GameObject.Find("CameraUI").GetComponent<Camera>();
        if (panelcamera.orthographicSize < 3.5f)
        {
            panelcamera.orthographicSize += 0.5f;
        }
    }

    //关闭示教器
    public void ClosePanel()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            return;
        }
        Camera panelcamera = GameObject.Find("CameraUI").GetComponent<Camera>();
        panelcamera.enabled = false;
    }

    //显示示教器
    public void ShowPanel()
    {
        Camera panelcamera = GameObject.Find("CameraUI").GetComponent<Camera>();
        panelcamera.orthographicSize = 2;
        PanelPosition();
        panelcamera.enabled = true;
    }
    //面板初始化
    public void PanelInitial()
    {
        ShowPanel();
        ScreenScript.WindowInitial();
        BtAllow();       
    }
    //退出面板
    public void ExitPanel()
    {
        ClosePanel();
        CloseIOPanel();
        GameObject.Find("MyMotion").GetComponent<RobotMotion>().ReturnToZero();
        //清除警报
        ScreenScript.ClearAllWarning();
    }
    //显示IO面板
    public void ShowIOPanel()
    {
        //IO_Panel.SetActive(true);
        IO_PanelPosition(false);
    }
    //关闭IO面板
    public void CloseIOPanel()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            return;
        }
        IO_PanelPosition(true);
    }
    public void CloseIOPanel_Teach()
    {
        IO_PanelPosition(true);
    }
    //IO面板的位置控制
    private void IO_PanelPosition(bool right)
    {
        Transform panel_trans = GameObject.Find("panellight").GetComponent<Transform>();
        float panel_width = GameObject.Find("panellight").GetComponent<UISprite>().width;
        Camera ui_camera = GameObject.Find("CameraUI2").GetComponent<Camera>();
        if (right)
        {
            panel_trans.position = ui_camera.ScreenToWorldPoint(new Vector3(-panel_width, 0, 7));
        }
        else
        {
            panel_trans.position = ui_camera.ScreenToWorldPoint(new Vector3(0, 0, 7));
        }
    }
    //示教器位置的控制
    private void PanelPosition()
    {
        Transform panel_trans = GameObject.Find("Panel").GetComponent<Transform>();
        Camera ui_camera = GameObject.Find("CameraUI").GetComponent<Camera>();
        panel_trans.position = ui_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 7));
    }

    #region -----------------连续响应区---------------
    public void OnGoforwardPress()
    {
        ScreenScript.OnGoforwardPress();
    }
    public void OnGoforwardRelease()
    {
        ScreenScript.OnGoforwardRelease();
    }
    public void OnJ1Press()
    {
        onPress(ref GSKDATA.AXIS_1);
    }
    public void OnJ1Release()
    {
        onRelease(ref GSKDATA.AXIS_1);
    }
    public void OnJ1_Press()
    {
        onPress(ref GSKDATA.AXIS_1_);
    }
    public void OnJ1_Release()
    {
        onRelease(ref GSKDATA.AXIS_1_);
    }

    public void OnJ2Press()
    {
        onPress(ref GSKDATA.AXIS_2);
    }
    public void OnJ2Release()
    {
        onRelease(ref GSKDATA.AXIS_2);
    }
    public void OnJ2_Press()
    {
        onPress(ref GSKDATA.AXIS_2_);
    }
    public void OnJ2_Release()
    {
        onRelease(ref GSKDATA.AXIS_2_);
    }

    public void OnJ3Press()
    {
        onPress(ref GSKDATA.AXIS_3);
    }
    public void OnJ3Release()
    {
        onRelease(ref GSKDATA.AXIS_3);
    }
    public void OnJ3_Press()
    {
        onPress(ref GSKDATA.AXIS_3_);
    }
    public void OnJ3_Release()
    {
        onRelease(ref GSKDATA.AXIS_3_);
    }

    public void OnJ4Press()
    {
        onPress(ref GSKDATA.AXIS_4);
    }
    public void OnJ4Release()
    {
        onRelease(ref GSKDATA.AXIS_4);
    }
    public void OnJ4_Press()
    {
        onPress(ref GSKDATA.AXIS_4_);
    }
    public void OnJ4_Release()
    {
        onRelease(ref GSKDATA.AXIS_4_);
    }

    public void OnJ5Press()
    {
        onPress(ref GSKDATA.AXIS_5);
    }
    public void OnJ5Release()
    {
        onRelease(ref GSKDATA.AXIS_5);
    }
    public void OnJ5_Press()
    {
        onPress(ref GSKDATA.AXIS_5_);
    }
    public void OnJ5_Release()
    {
        onRelease(ref GSKDATA.AXIS_5_);
    }

    public void OnJ6Press()
    {
        onPress(ref GSKDATA.AXIS_6);
    }
    public void OnJ6Release()
    {
        onRelease(ref GSKDATA.AXIS_6);
    }
    public void OnJ6_Press()
    {
        onPress(ref GSKDATA.AXIS_6_);
    }
    public void OnJ6_Release()
    {
        onRelease(ref GSKDATA.AXIS_6_);
    }

    private void onPress(ref bool axis)
    {
        ScreenScript.AxisOnPress(ref axis);
    }

    private void onRelease(ref bool axis)
    {
        ScreenScript.AxisOnRelease(ref axis);
    }

    #endregion

    #region ------------- --教学鼠标控制区-------------
    //mouse move from button to button
    public void MouseMove(int start, int end)
    {
        CalculatePanelBt_pos(start);
        CalculatePanelBt_pos(end);
        s = new Vector2(UICamera.currentCamera.WorldToScreenPoint(PanelBt_pos[start]).x, Screen.height - UICamera.currentCamera.WorldToScreenPoint(PanelBt_pos[start]).y);
        e = new Vector2(UICamera.currentCamera.WorldToScreenPoint(PanelBt_pos[end]).x, Screen.height - UICamera.currentCamera.WorldToScreenPoint(PanelBt_pos[end]).y);
        Mouse.MovingStart(s, e);
    }
    public void MouseMove(int end)
    {
        CalculatePanelBt_pos(end);
        s = e;
        e = new Vector2(UICamera.currentCamera.WorldToScreenPoint(PanelBt_pos[end]).x, Screen.height - UICamera.currentCamera.WorldToScreenPoint(PanelBt_pos[end]).y);
        Mouse.MovingStart(s, e);
    }
    public void MouseMoveToMiddle()//将鼠标移动到屏幕中间
    {
        s = e;
        e = new Vector2(Screen.width / 2, Screen.height / 2);
        Mouse.MovingStart(s, e);
    }
    public void ShowRightMenu()//在屏幕中间显示右键菜单
    {
        e = new Vector2(Screen.width / 2, Screen.height / 2);
        GameObject.Find("CameraFree").GetComponent<RightClickMenu>().RightClickControl(true, e);
    }
    public void MouseMoveOut(int end)
    {
        s = e;
        Vector3 tempV = GameObject.Find("OUT"+end).GetComponent<Transform>().position;
        e = new Vector2(UICamera.currentCamera.WorldToScreenPoint(tempV).x, Screen.height - UICamera.currentCamera.WorldToScreenPoint(tempV).y);
        Mouse.MovingStart(s, e);
    }
    public void MouseMoveIn(int end)
    {
        s = e;
        Vector3 tempV = GameObject.Find("IN" + end).GetComponent<Transform>().position;
        e = new Vector2(UICamera.currentCamera.WorldToScreenPoint(tempV).x, Screen.height - UICamera.currentCamera.WorldToScreenPoint(tempV).y);
        Mouse.MovingStart(s, e);
    }
    public void MouseMoveCloseIO()
    {
        s = e;
        Vector3 tempV = GameObject.Find("ColsePanel2").GetComponent<Transform>().position;
        e = new Vector2(UICamera.currentCamera.WorldToScreenPoint(tempV).x, Screen.height - UICamera.currentCamera.WorldToScreenPoint(tempV).y);
        Mouse.MovingStart(s, e);
    }
    public void MouseStop()
    {
        Mouse.MovingStop();
    }
    //改变鼠标运动的速度
    public void MouseSpeed(float speed,float speedRate)
    {
        Mouse.ChangeRate(speed, speedRate);
    }

    //计算当前按钮位置坐标
    void CalculatePanelBt_pos(int num)
    {
        PanelBt_pos[num] = GameObject.Find(PanelBt_name[num]).GetComponent<Transform>().position; 
    }

    //鼠标按钮触发
    public IEnumerator ButtonClick(int num)
    {
        MouseMove(num);
        FuncPara.loopControl = 0;
        yield return StartCoroutine(WaitLoop());
        PanelBt[num].isEnabled = true;
        PanelBt[num].disabledColor = Color.white;
        PanelBt[num].isEnabled = false;
        yield return StartCoroutine(Timer(clickTime));
        switch (num)
        {
            case 0: OnF1Click(); break;
            case 1: OnF2Click(); break;
            case 2: OnF3Click(); break;
            case 3: OnF4Click(); break;
            case 4: OnF5Click(); break;
            case 5: OnUpClick(); break;
            case 6: OnChooseClick(); break;
            case 7: OnLeftClick(); break;
            case 8: OnRightClick(); break;
            case 9: OnDownClick(); break;
            case 10: OnReadyClick(); break;
            case 11: OnCancelClick(); break;
            case 12: OnTabClick(); break;
            case 13: OnGetClick(); break;
            case 16: OnFlipClick(); break;
            case 17: OnSpeedUpClick(); break;
            case 18: OnSwitchClick(); break;
            case 23: OnIsContinueClick(); break;
            case 24: OnSpeedDownClick(); break;
            case 25: OnSetClick(); break;
            case 30: OnNum_7(); break;
            case 31: OnNum_8(); break;
            case 32: OnNum_9(); break;
            case 35: OnClearClick(); break;
            case 36: OnInputClick(); break;
            case 37: OnNum_4(); break;
            case 38: OnNum_5(); break;
            case 39: OnNum_6(); break;
            case 40: OnDeleteClick(); break;
            case 41: OnAddClick(); break;
            case 42: OnOutClick(); break;
            case 43: OnCutClick(); break;
            case 44: OnNum_1(); break;
            case 45: OnNum_2(); break;
            case 46: OnNum_3(); break;
            case 47: OnCopyClick(); break;
            case 48: OnModifyClick(); break;
            case 51: OnNum_minus(); break;
            case 52: OnNum_0(); break;
            case 53: OnNum_dot(); break;
            case 54: OnLeft_Click(); break;
            case 55: OnApplyClick(); break;
            case 56: OnReappearClick(); break;
            case 59: OnStartClick(); break;
            case 60: OnStopClick(); break;
        }
        yield return StartCoroutine(Timer(clickTime));
        PanelBt[num].isEnabled = true;
        PanelBt[num].disabledColor = Color.grey;
        PanelBt[num].isEnabled = false;
    }
    public void OriginalDisableColor()
    {
        for (int i = 0; i <= 55; i++)
        {
            PanelBt[i].isEnabled = true;
            PanelBt[i].disabledColor = Color.grey;
            PanelBt[i].isEnabled = false;
        }
    }
    public IEnumerator HighlightAxisBt()
    {
        int[] axisbt = new int[] { 14, 15, 19, 20, 21, 22, 26, 27, 28, 29, 33, 34 };
        for (int i = 0; i < 12; i++)
        {
            PanelBt[axisbt[i]].isEnabled = true;
            PanelBt[axisbt[i]].disabledColor = Color.white;
            PanelBt[axisbt[i]].isEnabled = false;
        }
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < 12; i++)
        {
            PanelBt[axisbt[i]].isEnabled = true;
            PanelBt[axisbt[i]].disabledColor = Color.grey;
            PanelBt[axisbt[i]].isEnabled = false;
        }
    }
    //禁用按钮
    public void BtForbid()
    {
        for (int i = 0; i < 62; i++)
        {
            PanelBt[i].isEnabled = false;
        }
    }
    //启用按钮
    public void BtAllow()
    {
        for (int i = 0; i < 62; i++)
        {
            PanelBt[i].isEnabled = true;
        }
    }
    IEnumerator WaitLoop()
    {
        while (FuncPara.loopControl <= 4 || !FuncPara.isPlaying)
        {
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator Timer(float time_value)
    {

        if (FuncPara.speedRate <= 1.01f) //正常时间
            yield return new WaitForSeconds(time_value);
        else
        {  //时间加快
            float ratePara = 0.1f;
            if (FuncPara.speedRate > 1.59f && FuncPara.speedRate < 1.81f)
                ratePara = 0.15f;
            else if (FuncPara.speedRate > 1.79f)
                ratePara = 0.25f;
            float newTime = time_value / (FuncPara.speedRate - ratePara);
            yield return new WaitForSeconds(newTime);
        }
        while (!FuncPara.isPlaying)
        {
            yield return new WaitForSeconds(0.01f);
        }

    }

    #endregion
}
