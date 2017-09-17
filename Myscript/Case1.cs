using UnityEngine;
using System.Collections;
using System.Data;

public class Case1 : CaseClass
{
    public override IEnumerator Teach()
    {
        for (; stage_current < StageTotal; stage_current++)
        {
            PhaseStateSetting();
            string phase_name = "phase_" + stage_current;
            yield return StartCoroutine(phase_name);
        }

    }

    public override void DevicePosition()
    {
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyDevice.xls";
        Vector3 position = Vector3.zero;
        Vector3 posture = Vector3.zero;
        string fathername = "";
        string devicename = "";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable DeviceData = new DataTable("DeviceData");
        DeviceData = excelClass.ExcelReader(ExcelPath, caseName);
        if (stage_current > DeviceData.Rows.Count)
        {
            Debug.LogError("当前步骤设备没有设置参数");
        }
        else
        {
            devicename = DeviceData.Rows[0][0].ToString();
            fathername = DeviceData.Rows[stage_current][1].ToString();
            position = StrToVector(DeviceData.Rows[stage_current][2]);
            posture = StrToVector(DeviceData.Rows[stage_current][3]);
        }
        //Debug.Log(devicename);
        Transform device = GameObject.Find(devicename).GetComponent<Transform>();
        device.parent = GameObject.Find(fathername).GetComponent<Transform>();
        device.localPosition = position;
        device.localEulerAngles = posture;
        //device.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    public override void SetStageOutInfo()
    {
        int out_num = 1;
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyOut.xls";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable OutData = new DataTable("OutData");
        OutData = excelClass.ExcelReader(ExcelPath, caseName);
        if (stage_current > OutData.Rows.Count)
        {
            Debug.LogError("当前步骤OUT没有设置参数");
        }
        else
        {
            //Debug.Log(OutData.Rows[stage_current][out_num].ToString());
            if(OutData.Rows[stage_current][out_num].ToString()=="0")
            {
                GSKDATA.OutInfo[out_num] = false;
                //CaseClass.RBP_Set_OT_Data(out_num, GSKDATA.OutInfo[out_num]);
                ScreenScript.setOutData(out_num, 0);
            }
            else
            {
                GSKDATA.OutInfo[out_num] = true;
                //CaseClass.RBP_Set_OT_Data(out_num, GSKDATA.OutInfo[out_num]);
                ScreenScript.setOutData(out_num, 1);
            }
        }
    }
    public override void TransPosition()//旋转参考点位置的确定
    {
        ;
    }
    public override void ToolPosition()
    {
        ;
    } //末端加持装置的位置
    IEnumerator phase_0()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //delete the orginal program
            GSKFile.DeleteProgram("a1.prl");
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 12);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(12));//tab
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[1]);
            ReadTips(stage_name[1]); 
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(12));//tab
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose//a
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(12));//tab
            yield return StartCoroutine(button.ButtonClick(6));//chose
            FileName = "a1.prl";//程序名称
        }
        else
        {
            GSKFile.DeleteProgram(FileName);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_1()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            //12按钮同时高亮
            button.StartCoroutine(button.HighlightAxisBt());
            //运动
            MOVJCLASS.StartPos = MotionScript.CurrentAngle_All();
            MOVJCLASS.EndPos = new float[] { 90f, 0f, 0f, 0f, 0f, 0f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movj p1
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(36));//input
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_2()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            //12按钮同时高亮
            button.StartCoroutine(button.HighlightAxisBt());
            //运动
            MOVJCLASS.StartPos = MotionScript.CurrentAngle_All();
            MOVJCLASS.EndPos = new float[]{ 87.25f, 48.36f, -14.11f, 69.34f, 33.15f, -41.53f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movj p2
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(45));//2
            yield return StartCoroutine(button.ButtonClick(36));//input
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_3()//dout ot1 on
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(36, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMoveToMiddle();
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(WaitTime(1.0f));
            button.ShowRightMenu();
            yield return StartCoroutine(WaitTime(1.0f));
            button.ShowIOPanel();
            FuncPara.rightclick_menu_on = false;
            button.MouseMoveOut(1);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(WaitTime(1.0f));
            button.OnOutBtClick_Teach(1);//out 1
            //GSKDATA.DeviceSignal[1] = 1; //周边设备
            yield return StartCoroutine(WaitTime(1.0f));
            button.MouseMoveCloseIO();
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(WaitTime(1.0f));
            button.CloseIOPanel_Teach();
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }  
    IEnumerator phase_4()//delay
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(36));//input
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_5()//movj p2
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            //12按钮同时高亮
            button.StartCoroutine(button.HighlightAxisBt());
            //运动
            MOVJCLASS.StartPos = MotionScript.CurrentAngle_All();
            MOVJCLASS.EndPos = new float[] { 90f, 0f, 0f, 0f, 0f, 0f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movj p0
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[3]);
            ReadTips(stage_name[3]);
            waittime = GetVoiceTime(stage_name[3]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(6));//chose
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_6()//movj p3
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            SetStage(7);
            StartCoroutine("Teach");
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_7()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            SetStage(8);
            StartCoroutine("Teach");
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_8()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            SetStage(9);
            StartCoroutine("Teach");
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_9()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            SetStage(10);
            StartCoroutine("Teach");
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_10()//还原场景
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMoveToMiddle();
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(WaitTime(1.0f));
            button.ShowRightMenu();
            yield return StartCoroutine(WaitTime(1.0f));
            FuncPara.rightclick_menu_on = false;
            ResetScene();
            
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_11()//再现运行
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            showtips(stage_name[0]);
            ReadTips(stage_name[0]);
            waittime = GetVoiceTime(stage_name[0]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(11, 1);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(1));//F2
            //step:; first line
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(5));//up
            yield return StartCoroutine(button.ButtonClick(5));//up
            WinInitial("F2");
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(56));//reappear
            showtips(stage_name[3]);
            ReadTips(stage_name[3]);
            waittime = GetVoiceTime(stage_name[3]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(10));//ready
            showtips(stage_name[4]);
            ReadTips(stage_name[4]);
            waittime = GetVoiceTime(stage_name[4]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(59));//start
            showtips(stage_name[5]);
            ReadTips(stage_name[5]);
            waittime = GetVoiceTime(stage_name[5]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(7));//left
            yield return StartCoroutine(button.ButtonClick(6));//chose
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    

}
