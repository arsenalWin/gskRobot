using UnityEngine;
using System.Collections;
using System.Data;

public class Case4 : CaseClass
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

    public override void DevicePosition() //焊缝
    {
        GameObject.Find("CameraFree").GetComponent<WeldLine>().RemoveWeldLine();
    }

    public override void SetStageOutInfo()
    {
        ;
    }
    public override void ReturnToOriAng()
    {
        MotionScript.AxisPositionSet(0f, -35f, 37, 0, 35, 0);
    }

    IEnumerator phase_0()
    {
        if (GSKDATA.SoftCurrentMode == "Teach")
        {
            //delete the orginal program
            GSKFile.DeleteProgram("a4.prl");
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
            yield return StartCoroutine(button.ButtonClick(37));//4
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[2]);
            ReadTips(stage_name[2]);
            waittime = GetVoiceTime(stage_name[2]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(12));//tab
            yield return StartCoroutine(button.ButtonClick(6));//chose
            FileName = "a4.prl";//程序名称
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
            MOVJCLASS.EndPos = new float[] { -8.742731f, -14.61436f, 35.77437f, 23.71783f, 17.56529f, -28.01431f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movj p2 v50
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
            showtips(stage_name[3]); 
            ReadTips(stage_name[3]);
            waittime = GetVoiceTime(stage_name[3]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(38));//5
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


    IEnumerator phase_3()//movl p3 v20
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
            MOVJCLASS.EndPos = new float[] { -8.742731f, -14.47661f, 36.62946f, 25.04917f, 16.66096f, -29.40728f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movl p3 v20
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
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(45));//2
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


    IEnumerator phase_4()//arcof 
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
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose           
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

    IEnumerator phase_5()//movl p4 v20
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
            MOVJCLASS.EndPos = new float[] { -12.0844f, -35.44544f, 50.34214f, 23.70327f, 24.57695f, -29.10693f };
            MOVJCLASS.StartRun();
            yield return new WaitForSeconds(3.0f);
            //add movj p2 v50
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
            yield return StartCoroutine(button.ButtonClick(37));//4
            yield return StartCoroutine(button.ButtonClick(36));//input
            showtips(stage_name[3]);
            ReadTips(stage_name[3]);
            waittime = GetVoiceTime(stage_name[3]);
            yield return StartCoroutine(Timer(waittime));
            yield return StartCoroutine(button.ButtonClick(8));//right
            yield return StartCoroutine(button.ButtonClick(38));//5
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


    IEnumerator phase_6()//arcof
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
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(9));//down
            yield return StartCoroutine(button.ButtonClick(6));//chose           
            yield return StartCoroutine(button.ButtonClick(9));//down
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

    IEnumerator phase_7()//movl p5 v50
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
            MOVJCLASS.EndPos = new float[] { -12.0844f, -35.41929f, 45.85467f, 20.36539f, 28.71445f, -25.37515f };
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
            yield return StartCoroutine(button.ButtonClick(38));//4
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

    IEnumerator phase_8()//movj p1 v100
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
            MOVJCLASS.EndPos = new float[] { 0f, -35f, 37f, 0, 35, 0};
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

    IEnumerator phase_9()//再现运行
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
