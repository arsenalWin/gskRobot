using UnityEngine;
using System.Collections;
using System.Data;

public class Case3 : CaseClass
{
    GameObject origianlWorkpiece;//加工之前的工件
    public override void ObjectInitial()
    {
        origianlWorkpiece = origianlWorkpiece = GameObject.Find("scx0261");
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
        if (DeviceData.Rows[stage_current][5].ToString() == "1")
        {
            origianlWorkpiece.SetActive(true);
        }
        else
        {
            origianlWorkpiece.SetActive(false);
        }

        Transform device = GameObject.Find(devicename).GetComponent<Transform>();
        device.parent = GameObject.Find(fathername).GetComponent<Transform>();
        device.localPosition = position;
        device.localEulerAngles = posture;
        device.localScale = scale;
        origianlWorkpiece.SetActive(true);
    }

    public override void SetStageOutInfo()
    {
        int[] out_num = {1,2,3,31};
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
            for (int i = 0; i < 4;i++ )
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
                    ScreenScript.setOutData(out_num[i], 0);
                }
            }
                
        }
    }

    public override void ReturnToOriAng()
    {
       MotionScript.AxisPositionSet(0f, -17f, 15, 0, 65, 0);
    }

    IEnumerator phase_0()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //delete the orginal program
            GSKFile.DeleteProgram("a3.prl");
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
            yield return StartCoroutine(button.ButtonClick(46));//3
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(12));//tab
            yield return StartCoroutine(button.ButtonClick(6));//chose
            FileName = "a3.prl";//程序名称
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
            MOVJCLASS.EndPos = new float[] { 166.7227f, 35.273f, -24.26576f, -0.02574971f, 72.69278f, 47.9298f };
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
            MOVJCLASS.EndPos = new float[] { 166.7227f, 36.75487f, -17.62833f, -0.0273132f, 64.57349f, 47.93386f };
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


    IEnumerator phase_3()//dout ot2 on
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
            yield return StartCoroutine(button.ButtonClick(45));//2
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
            button.OnOutBtClick_Teach(2);//out 2
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

    IEnumerator phase_5()//movl 1
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
            MOVJCLASS.EndPos = new float[] { 166.7227f, 35.273f, -24.26576f, -0.02574971f, 72.69278f, 47.9298f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movl p1
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
            MOVJCLASS.EndPos = new float[] { -77.82337f, -41.00603f, 32.88304f, 2.875046f, 88.52769f, -18.79467f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(6.0f);
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(46));//3
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

    IEnumerator phase_7()//movl p4
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
            MOVJCLASS.EndPos = new float[] { -87.39079f, 32.88435f, -35.83857f, 4.447632f, 83.97859f, -28.64893f };
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

    IEnumerator phase_8()//movl p5
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
            MOVJCLASS.EndPos = new float[] { -91.18701f, 31.97004f, -34.39893f, 5.036433f, 83.76875f, -32.49337f };
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

    IEnumerator phase_9()//dout ot3 on
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
            button.MouseMove(36, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(46));//3
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
            button.OnOutBtClick_Teach(3);//out 3

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

    IEnumerator phase_10()//delay 2
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


    IEnumerator phase_11()//dout ot2 off
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

            button.MouseMove(36, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(45));//2
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
            button.OnOutBtClick_Teach(2);//out 2
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

    IEnumerator phase_12()//delay 1
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

    IEnumerator phase_13()//movl p6
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
            MOVJCLASS.EndPos = new float[] { -91.18701f, 34.45065f, -46.46085f, 5.015003f, 93.31339f, -31.65466f };
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
            yield return StartCoroutine(button.ButtonClick(39));//6
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


    IEnumerator phase_14()//movl p7
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
            MOVJCLASS.EndPos = new float[] { -94.85555f, -39.80563f, 24.15402f, 5.595165f, 97.26892f, -34.87865f };
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
            yield return StartCoroutine(button.ButtonClick(30));//7
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

    IEnumerator phase_15()//dout ot1 on
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

        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }

    IEnumerator phase_16()//delay 1
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

    IEnumerator phase_17()//wait
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

    IEnumerator phase_18()//movl p6
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
            MOVJCLASS.EndPos = new float[] { -91.18701f, 34.45065f, -46.46085f, 5.015003f, 93.31339f, -31.65466f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(39));//6
            yield return StartCoroutine(button.ButtonClick(36));//input
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

    IEnumerator phase_19()//movl p5
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
            MOVJCLASS.EndPos = new float[] { -91.18701f, 31.97004f, -34.39893f, 5.036433f, 83.76875f, -32.49337f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(38));//5
            yield return StartCoroutine(button.ButtonClick(36));//input
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

    IEnumerator phase_20()//dout ot2 on
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

            button.MouseMove(36, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(45));//2
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
            button.OnOutBtClick_Teach(2);//out 2

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


    IEnumerator phase_21()//wait
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
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose

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


    IEnumerator phase_22()//dout ot3 off
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

            button.MouseMove(36, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(46));//3
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
            button.OnOutBtClick_Teach(3);//out 3

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

    IEnumerator phase_23()//delay 1
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


    IEnumerator phase_24()//movl p8
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
            MOVJCLASS.EndPos = new float[] { -86.77851f, 32.13594f, -34.65929f, 4.355147f, 83.50221f, -28.06874f };
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
            yield return StartCoroutine(button.ButtonClick(31));//8
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

    IEnumerator phase_25()//movl p9
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
            MOVJCLASS.EndPos = new float[] { -78.59816f, -38.33127f, 31.95774f, 3.007779f, 86.81995f, -19.65274f };
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
            yield return StartCoroutine(button.ButtonClick(32));//9
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

    IEnumerator phase_26()//movj 1
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
            MOVJCLASS.EndPos = new float[] { 166.7227f, 35.273f, -24.26576f, -0.02574971f, 72.69278f, 47.9298f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);

            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(36));//input

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

    IEnumerator phase_27()//movl 2
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
            MOVJCLASS.EndPos = new float[] { 166.7227f, 36.75487f, -17.62833f, -0.0273132f, 64.57349f, 47.93386f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);

            button.MouseMove(0, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(9));
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(45));//2
            yield return StartCoroutine(button.ButtonClick(36));//input

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


    IEnumerator phase_28()//dout ot2 off
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

            button.MouseMove(36, 41);
            FuncPara.loopControl = 0;
            yield return StartCoroutine(WaitLoop());
            yield return StartCoroutine(button.ButtonClick(41));//add
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose
            yield return StartCoroutine(button.ButtonClick(6));//chose

            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(45));//2
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
            button.OnOutBtClick_Teach(2);//out 2

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


    IEnumerator phase_29()//delay 1
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


    IEnumerator phase_30()//movj p10
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
            MOVJCLASS.EndPos = new float[] { 167.1101f, 1.264485f, -2.876051f, -0.06721041f, 89.5118f, 48.3078f };
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
            yield return StartCoroutine(button.ButtonClick(44));//1
            yield return StartCoroutine(button.ButtonClick(52));//0
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


    IEnumerator phase_31()//还原场景
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
    IEnumerator phase_32()//再现运行
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //MOVJCLASS.EndRun();
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
