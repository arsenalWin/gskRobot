using UnityEngine;
using System.Collections;
using System.Data;

public class Case2 : CaseClass
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
        GameObject.Find("MainScript").GetComponent<PaintScript>().ClearPanel();
    }
    public override void SetStageOutInfo()
    {
        ;
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
            GSKFile.DeleteProgram("a2.prl");
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
            yield return StartCoroutine(button.ButtonClick(45));//2
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(12));//tab
            yield return StartCoroutine(button.ButtonClick(6));//chose
            FileName = "a2.prl";//程序名称
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
            MOVJCLASS.EndPos = new float[] { -13.25663f, 10.14561f, 24.79338f, -22.36026f, -37.07446f, 18.17154f };
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
            MOVJCLASS.EndPos = new float[] { -12.80403f, 13.11521f, 21.27956f, -21.91608f, -36.43026f, 17.93833f };
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
    IEnumerator phase_3()//
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
            MOVJCLASS.EndPos = new float[] { -12.77493f, 21.61144f, 27.10003f, -16.79407f, -49.95327f, 10.99096f };
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
    IEnumerator phase_4()
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
            MOVJCLASS.EndPos = new float[] { -13.14824f, 19.40651f, 30.1102f, -17.07535f, -50.79452f, 10.98974f };
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
    IEnumerator phase_5()//others
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            CameraPosition();
            float waittime = 0;
            ShowTips();
            ReadTips(TitleName[stage_current]);
            waittime = GetVoiceTime(TitleName[stage_current]);
            yield return StartCoroutine(Timer(waittime));
            SetStage(6);
            StartCoroutine("Teach");
        }
        else
        {
            SetStage(6);
            StartCoroutine("Teach");
        }
    }

    IEnumerator phase_6()//clear
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
            GameObject.Find("MainScript").GetComponent<PaintScript>().ClearPanel();

        }
        else
        {
            Debug.Log("pass" + stage_current);
            GetCurrentCode();
            GetCurrentPosition();
            yield return StartCoroutine(DoubleE());
        }
    }
    IEnumerator phase_7()//再现运行
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
