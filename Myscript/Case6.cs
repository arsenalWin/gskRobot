using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;

public class Case6 : CaseClass
{
    GameObject origianlWorkpiece;//加工之前的工件
    public override void ObjectInitial()
    {
        ;
    }
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
        Vector3 scale = Vector3.zero;
        string fathername = "";
        string devicename = "";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable DeviceData = new DataTable("DeviceData");
        DeviceData = excelClass.ExcelReader(ExcelPath, caseName);

        devicename = DeviceData.Rows[0][0].ToString();
        fathername = DeviceData.Rows[stage_current][1].ToString();
        position = StrToVector(DeviceData.Rows[stage_current][2]);
        posture = StrToVector(DeviceData.Rows[stage_current][3]);
        scale = StrToVector(DeviceData.Rows[stage_current][4]);

        Transform device = GameObject.Find(devicename).GetComponent<Transform>();
        device.parent = GameObject.Find(fathername).GetComponent<Transform>();
        device.localPosition = position;
        device.localEulerAngles = posture;
        device.localScale = scale;

        if (DeviceData.Rows[stage_current][5].ToString() == "0")
        {
            GameObject.Find("MyMotion").GetComponent<InstantiateObjects>().DestoryBox();
        }
    }

    public override void SetStageOutInfo()
    {
        int[] out_num = { 1, 2, 3, 31 };
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
            for (int i = 0; i < 4; i++)
            {
                if (OutData.Rows[stage_current][out_num[i]].ToString() == "0")
                {
                    GSKDATA.OutInfo[out_num[i]] = false;
                    //CaseClass.RBP_Set_OT_Data(out_num[i], GSKDATA.OutInfo[out_num[i]]);
                    ScreenScript.setOutData(out_num[i], 0);
                }
                else
                {
                    GSKDATA.OutInfo[out_num[i]] = true;
                    //CaseClass.RBP_Set_OT_Data(out_num[i], GSKDATA.OutInfo[out_num[i]]);
                    ScreenScript.setOutData(out_num[i], 1);
                }
            }

        }
    }

    public override void ReturnToOriAng()
    {
        MotionScript.AxisPositionSet(160f, 0f, 0, 0, 90, 0);
    }

    IEnumerator phase_0()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            GSKFile.DeleteProgram("a6.prl");
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
            yield return StartCoroutine(button.ButtonClick(39));//6
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(12));//tab
            yield return StartCoroutine(button.ButtonClick(6));//chose
            FileName = "a6.prl";//程序名称
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
            //CameraPosition();
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
            MOVJCLASS.EndPos = new float[] { 163.24f, 38.63f, -52.74f, 3.66f, 99.61f, -14.01f };
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
            //CameraPosition();
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
            MOVJCLASS.EndPos = new float[] { 163.98f, 36.74f, -18.21f, 3.99f, 66.85f, -16.09f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movl p2
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(9));//down
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
            //CameraPosition();
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


    IEnumerator phase_4()//wait
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
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

    IEnumerator phase_5()//movl 3
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
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
            MOVJCLASS.EndPos = new float[] { 163.98f, 38.32f, -49.86f, 3.70f, 96.84f, -14.07f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movl p3
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(46));//3
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


    IEnumerator phase_6()//movj p4
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            //12按钮同时高亮
            button.StartCoroutine(button.HighlightAxisBt());
            //运动
            MOVJCLASS.StartPos = MotionScript.CurrentAngle_All();
            MOVJCLASS.EndPos = new float[] { 111.00f, 5.93f, -4.44f, 3.28f, 83.54f, 21.27f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(6.0f);
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(37));//4
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

    IEnumerator phase_7()//movl p5
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            //12按钮同时高亮
            button.StartCoroutine(button.HighlightAxisBt());
            //运动
            MOVJCLASS.StartPos = MotionScript.CurrentAngle_All();
            MOVJCLASS.EndPos = new float[] { 108.14f, 11.22f, 9.66f, 0.39f, 63.46f, 18.28f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(38));//5
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

    IEnumerator phase_8()//dout ot1 off
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();
            button.MouseMove(36, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(36));//input

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(36));//input

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
            button.OnOutBtClick_Teach(1);//out 2
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

    IEnumerator phase_9()//dealy 1 s
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();

            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose

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

    IEnumerator phase_10()//movl p4
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
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
            MOVJCLASS.EndPos = new float[] { 111.00f, 5.93f, -4.44f, 3.28f, 83.54f, 21.27f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movl p4
            showtips(stage_name[1]);
            ReadTips(stage_name[1]);
            waittime = GetVoiceTime(stage_name[1]);
            yield return StartCoroutine(Timer(waittime));
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(37));//4
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


    IEnumerator phase_11()//reapeat the prograss
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            LstageRead();

            List<string> stage_code = new List<string>();
            string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyCode.xls";
            ExcelOperator excelClass = new ExcelOperator();
            DataTable CodeData = new DataTable("CodeData");
            CodeData = excelClass.ExcelReader(ExcelPath, caseName);
            int i = 0;
            while (i < CodeData.Rows.Count)
            {
                string tempcode = CodeData.Rows[i][stage_current+1].ToString();
                if (tempcode != "")
                    stage_code.Add(CodeData.Rows[i][stage_current+1].ToString());
                i++;
            }
            if (stage_code.Count > 0)
            {
                //Debug.Log("程序行：" + stage_code.Count +" " + FileName);
                GSKFile.CreateFile(FileName);
                GSKFile.writeContents(stage_code, FileName);
                //GSKDATA.CurrentProgramName = FileName;
            }
            else
            {
                GSKFile.DeleteProgram(FileName);
                //FileName = "777777.prl";
            }

            ScreenScript.SetCurrentWin_Teach("F3");
        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }

    IEnumerator phase_12()//restore the scene
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
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

    IEnumerator phase_13()//再现运行
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //CameraPosition();
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
