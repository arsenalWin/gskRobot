//<summary>
//CaseClass#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//案例类
//
//<summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

public  class CaseClass : MonoBehaviour
{
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_OT_Data@@YAHH_N@Z")]
    //public static extern int RBP_Set_OT_Data(int num, bool value);

    public int StageTotal = 0;
    public int stage_current = 0;
    public List<string> TitleName = new List<string>();
    public List<string> stage_name = new List<string>();
    public string caseName = "";
    public TipsWindow TipsShow;//提示的ui脚本
    public doVoiceExe Voice;//语音脚本
    public TipsMotion TipsMo;
    public TipsInfoManager TipsManager;
    public ButtonRespond button;//按钮脚本
    public ScreenBuild ScreenScript;//屏幕脚本
    public RobotMotion MotionScript;//运动脚本
    public ROBOTFILE GSKFile;//文件处理脚本
    public string FILEPATH;//文件位置
    public string FileName;//文件名称
    public int DetectNo = 0; //监测号
    public List<string> UserCode = new List<string>(); //用户写的code
    public List<string> CorrectCode = new List<string>(); //正确的code
    float[] CorrectAngles = new float[6];

    public void empty()
    {
        FILEPATH = Application.dataPath + "\\StreamingAssets\\Programs\\";
        GSKFile = new ROBOTFILE(FILEPATH);
        button = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
        ScreenScript = GameObject.Find("MyScreen").GetComponent<ScreenBuild>();
        MotionScript = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        TipsShow = GameObject.Find("MainScript").GetComponent<TipsWindow>();
        Voice = GameObject.Find("MainScript").GetComponent<doVoiceExe>();
        TipsMo = new TipsMotion();
        TipsManager = new TipsInfoManager();
    }

    public void Initial(string casename,string filename)
    {
        ObjectInitial();   
        StageTotal = 0;
        stage_current = 0;
        DetectNo = 0;
        caseName = casename;
        FileName = filename;
        TipsShow = GameObject.Find("MainScript").GetComponent<TipsWindow>();
        Voice = GameObject.Find("MainScript").GetComponent<doVoiceExe>();
        TipsMo = new TipsMotion();
        TipsManager = new TipsInfoManager();
        button = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
        FILEPATH = Application.dataPath + "\\StreamingAssets\\Programs\\";
        GSKFile = new ROBOTFILE(FILEPATH);
        ScreenScript = GameObject.Find("MyScreen").GetComponent<ScreenBuild>();
        MotionScript = GameObject.Find("MyMotion").GetComponent<RobotMotion>();

        CalculateTotalStage();
        CaseIntro.SetActive(true);
        CaseIntro.CaseStart(casename);
        PanelInitial();//面板初始化
    }
    #region-----------------需要重写的方法-------------
    public virtual void ObjectInitial()
    {
        ;
    }

    public virtual IEnumerator Teach()
    {
        yield return new WaitForSeconds(1.0f);
    }
   

    public virtual void StagePanelInitial()
    {
        PanelInitial(); 
    }
    public virtual void TransPosition()//旋转参考点位置的确定
    {
        ;
    }
    public virtual void ToolPosition()
    {
        ;
    } //末端加持装置的位置
    public virtual void ResultDetect() //结果检测部分
    {
        ;
    }
    public virtual void DevicePosition()
    {
        ;
    }//周边设备位置初始化
    public virtual void SetStageOutInfo()
    {
        ;
    }
    public virtual void SetStageInInfo()
    {
        ;
    }

    public virtual void ReturnToOriAng()//返回到原始的角度
    {
        ;
    }

    #endregion
    //下一步
    public void NextStage()
    {
        if (++stage_current == StageTotal)
        {
            stage_current = StageTotal - 1;
        }
        else
        {
            StageInitial();
        }
    }
    //上一步
    public void LastStage()
    {
        if (--stage_current < 0)
        {
            stage_current = 0;
        }
        else
        {
            StageInitial();
        }
    }
    //步骤选择
    public void SetStage(int num)
    {
        Debug.Log("当前步奏:" + num);
        stage_current = num;
        StageInitial();
    }
    //阶段的初始化
    public void StageInitial()
    {
        //stop coroutines
        StopAllCoroutines();
        button.StopAllCoroutines();
        //鼠标
        button.MouseStop();
        button.OriginalDisableColor();
        //tips
        DisableTips();
        ReadTips("");
        //camera axis
        CameraPosition();
        MOVJCLASS.EndRun();
        AxisPosition();
        //device
        DevicePosition();
        //code 
        CodeInitial();
        //screen show
        //StagePanelInitial();
        PanelStageInitial();
        button.CloseIOPanel_Teach();
        //rightmenu
        FuncPara.rightclick_menu_on = false;
        //out in
        SetStageInInfo();
        SetStageOutInfo();
    }
    //退出案例
    public void ExitCase()
    {
        StopAllCoroutines();
        button.StopAllCoroutines();
        //axis initial
        MOVJCLASS.EndRun();
        RobotReturnZero();
        //close teachwindow
        FuncPara.showTeachWindow = false; 
        //删除程序 设置当前程序
        GSKFile.DeleteProgram(FileName);
        //rightmenu
        FuncPara.rightclick_menu_on = false;
        //voice tips mouse
        DisableTips();
        ReadTips("");
        button.MouseStop();
        button.OriginalDisableColor();
        //panel
        button.ClosePanel();
        button.CloseIOPanel();
        button.BtAllow();
        //reset scene
        ResetScene();
        ReturnToOriAng();
        Debug.Log("ExitCase");
    }
    //场景还原
    public void ResetScene()
    {
        GameObject.Find("CameraFree").GetComponent<Camerascript>().ResetScene();
    }
    //加减速
    public void SpeedControl(bool plus)
    {
        if (plus)
        {  //加速
            if (FuncPara.speedRate < 1f)
            {
                Speed(FuncPara.speedRate + 0.1f);
            }
            else if (FuncPara.speedRate == 1f)
            {
                Speed(FuncPara.speedRate + 0.2f);
            }
            else if (FuncPara.speedRate < 2f)
            {
                Speed(FuncPara.speedRate + 0.2f);
            }
        }
        else
        {
            //减速
            if (FuncPara.speedRate > 1f)
            {
                Speed(FuncPara.speedRate - 0.2f);
            }
            else if (FuncPara.speedRate == 1f)
            {
                Speed(FuncPara.speedRate - 0.1f);
            }
            else if (FuncPara.speedRate > 0.5f)
            {
                Speed(FuncPara.speedRate - 0.1f);
            }
        }
    }
    void Speed(float speed)
    {
        if (FuncPara.cursorShow)
        {
            button.MouseSpeed(speed, FuncPara.speedRate);//mouse
        }

        FuncPara.speedRate = speed;
    }
    //播放暂停
    public void Pause(bool isplay)
    {
        FuncPara.isPlaying = isplay;
    }
    //同步练习
    public void StartExercise()
    {
        button.ShowPanel();
        button.BtAllow();//allow
        DetectNo = 0;
    }
    //开始考试
    public void StartExam()
    {
        GSKDATA.SoftCurrentMode = "Exam";
        StageInitial();
        stage_current = 0;
        button.ShowPanel();
        button.BtAllow();//allow
        DetectNo = 0;
    }
    //panel初始化
    public void PanelInitial()
    {
        button.ShowPanel();
        button.PanelInitial();
        button.BtForbid();
    }
    //机器人返回零点
    public void RobotReturnZero()
    {
        GameObject.Find("MyMotion").GetComponent<RobotMotion>().ReturnToZero();
        
    }
    //等待鼠标到位,其他动作完成
    public IEnumerator WaitLoop()
    {
        while (FuncPara.loopControl <= 4 || (!FuncPara.isPlaying && GSKDATA.SoftCurrentMode == "Teach"))
        {
            yield return new WaitForSeconds(0.01f);
        }
    }
    //计算总的步数，并记录每一大步的名称
    public void CalculateTotalStage()
    {
        TitleName.Clear();
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyStage.xls";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable TeachWData = new DataTable("TeachWData");
        TeachWData = excelClass.ExcelReader(ExcelPath, caseName);
        StageTotal = TeachWData.Rows.Count;
        for (int i = 0; i < StageTotal; i++)
        {
            TitleName.Add((string)TeachWData.Rows[i][0]);
        }
    }
    //读取每一阶段具体小步骤的信息
    public void LstageRead()
    {
        stage_name = new List<string>();
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyStage.xls";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable TeachWData = new DataTable("TeachWData");
        TeachWData = excelClass.ExcelReader(ExcelPath, caseName);
        int i = 0;
        while (i < TeachWData.Rows.Count)
        {
            stage_name.Add(TeachWData.Rows[i][stage_current+1].ToString());
            i++;
        }
    }
    //等待到语音读完（包含加减速）
    public IEnumerator Timer(float time_value)
    {
        if (GSKDATA.SoftCurrentMode == "Exam")
        {
            yield return new WaitForSeconds(0.01f);
        }
        else
        {
            if (!FuncPara.VoiceHelp)
            {  //帮助禁止时不等待
                yield return new WaitForSeconds(1.0f);
            }
            else
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
                while (!FuncPara.isPlaying && GSKDATA.SoftCurrentMode == "Teach")//wait when pause
                {
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
        
    }
    public IEnumerator WaitTime(float time_value)
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
        while (!FuncPara.isPlaying)//wait when pause
        {
            yield return new WaitForSeconds(0.01f);
        }
    }
    //设置相机位置
    public void CameraPosition()
    {
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyCamera.xls";
        Camera camera = GameObject.Find("CameraFree").GetComponent<Camera>();
        TransPosition();
        Vector3 position = Vector3.zero;
        Vector3 posture = Vector3.zero;
        float size = 0;
        ExcelOperator excelClass = new ExcelOperator();
        DataTable CameraData = new DataTable("CameraData");
        CameraData = excelClass.ExcelReader(ExcelPath, caseName);
        if (stage_current > CameraData.Rows.Count)
        {
            Debug.LogError("当前步骤相机没有设置参数");
        }
        else
        {
            //deal with the data from the excel
            //Debug.Log(CameraData.Rows[stage_current][1]);
            size = Convert.ToSingle(CameraData.Rows[stage_current][1]);
            position = StrToVector(CameraData.Rows[stage_current][2]);
            posture = StrToVector(CameraData.Rows[stage_current][3]);
        }
        camera.orthographicSize = size;
        camera.transform.position = position;
        camera.transform.eulerAngles = posture;

    }
    //设置各个轴的位置
    public void AxisPosition()
    {
        ToolPosition();
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyAxis.xls";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable AxisData = new DataTable("AxisTable");
        AxisData = excelClass.ExcelReader(ExcelPath, caseName);
        if (stage_current > AxisData.Rows.Count)
        {
            Debug.LogError("当前步骤轴没有设置参数");
        }
        else
        {
            //deal with the data from the excel
            float j1 = Convert.ToSingle(AxisData.Rows[stage_current][1]); float j4 = Convert.ToSingle(AxisData.Rows[stage_current][4]);
            float j2 = Convert.ToSingle(AxisData.Rows[stage_current][2]); float j5 = Convert.ToSingle(AxisData.Rows[stage_current][5]);
            float j3 = Convert.ToSingle(AxisData.Rows[stage_current][3]); float j6 = Convert.ToSingle(AxisData.Rows[stage_current][6]);
            //J1_6Initial(j1, j2, j3, j4, j5, j6, z1, z2);
            MotionScript.AxisPositionSet(j1, j2, j3, j4, j5, j6);
        }
    }
    //将Excel表中的内容转换成Vector3
    public Vector3 StrToVector(object str)
    {
        string tstr = (string)str;
        Vector3 vect = Vector3.zero;
        tstr = Regex.Replace(tstr, @"\(", "", RegexOptions.IgnoreCase);
        tstr = Regex.Replace(tstr, @"\)", "", RegexOptions.IgnoreCase);
        string[] strs = tstr.Split(',');
        try
        {
            vect.x = Convert.ToSingle(strs[0]);
            vect.y = Convert.ToSingle(strs[1]);
            vect.z = Convert.ToSingle(strs[2]);
        }
        catch
        {
            Debug.LogError("convert the object into vetor3 error");
        }
        return vect;
    }
    //设置阶段状态栏的状态
    public void PhaseStateSetting()
    {
        GameObject.Find("MainScript").GetComponent<TeachWinow>().StageSetting(stage_current);
    }
    //显示大的阶段提示信息
    public void ShowTips()
    {
        if (!FuncPara.WordHelp || GSKDATA.SoftCurrentMode == "Exam")
        {
            TipsShow.MyTipsWindowsShow("", false, true, 35, new Color(255, 0, 0), "msyh");
        }
        else
        {
            TipsShow.MyTipsWindowsShow(TitleName[stage_current], true, true, 35, new Color(255, 0, 0), "msyh");

        }
    }
    //显示小的阶段提示信息
    public void showtips(string tipinfo)
    {
        if (!FuncPara.WordHelp || GSKDATA.SoftCurrentMode == "Exam")
        {
            TipsShow.MyTipsWindowsShow("", false, true, 35, new Color(255, 255, 255), "msyh");
        }
        else
        {
            TipsShow.MyTipsWindowsShow(tipinfo, true, false, 25, new Color(255, 255, 255), "msyh");
        }
    }
    //取消tips的显示
    public void DisableTips()
    {
        TipsShow.MyTipsWindowsShow("", false, true, 35, new Color(255, 0, 0), "msyh");
    }
    //读语音
    public void ReadTips(string tips)
    {
        if (!FuncPara.VoiceHelp || GSKDATA.SoftCurrentMode == "Exam")
        {  //帮助禁止时不播放语音
            return;
        }
        else
        {
            Voice.doVoice(tips, TipsMo.TipsSpeed);
        }
    }
    //获取语音读取的时间
    public float GetVoiceTime(string tips)
    {
        TipsMo.StandardTime = TipsManager.GetPlayTime(tips);
        TipsMo.TipsTimeControl();
        return TipsMo.EndTime;
    }

    //代码初始化
    public void CodeInitial()
    {
        List<string> stage_code = new List<string>();
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyCode.xls";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable CodeData = new DataTable("CodeData");
        CodeData = excelClass.ExcelReader(ExcelPath, caseName);
        int i = 0;
        while (i < CodeData.Rows.Count)
        {
            string tempcode = CodeData.Rows[i][stage_current].ToString();
            if (tempcode != "") 
                stage_code.Add(CodeData.Rows[i][stage_current].ToString());
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
    }
    //编辑界面初始化
    public void PanelStageInitial()
    {
        //整体界面初始化
        PanelInitial();
        string winname= "";
        string Speed_Bt ="";
        string Zuobiao_Bt = ""; 
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyScreen.xls";
        if (stage_current > 0)
        {
            GSKDATA.CurrentProgramName = FileName;
        }
        else
        {
            GSKDATA.CurrentProgramName = "777777.prl";
        }
        ExcelOperator excelClass = new ExcelOperator();
        DataTable PanelData = new DataTable("PanelData");
        PanelData = excelClass.ExcelReader(ExcelPath, caseName);
        winname = PanelData.Rows[stage_current][0].ToString();
        Speed_Bt = PanelData.Rows[stage_current][1].ToString();
        Zuobiao_Bt = PanelData.Rows[stage_current][2].ToString();
        WinInitial(winname);
    }
    //代码检测
    public void GetCurrentCode()
    {
        CorrectCode = new List<string>(); //正确的code
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyCode.xls";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable CodeData = new DataTable("CodeData");
        CodeData = excelClass.ExcelReader(ExcelPath, caseName);
        int i = 0;
        while (i < CodeData.Rows.Count)
        {
            string temps = CodeData.Rows[i][stage_current + 1].ToString();
            if (temps != "" && temps != null)
            {
                if (!Regex.IsMatch(temps, @"^P[0-9]{3}"))
                {
                    CorrectCode.Add(CodeData.Rows[i][stage_current + 1].ToString());
                }
            }
            i++;
        }
    }
    public bool CodeDetect()
    {
        UserCode = new List<string>(); //用户写的code
        
        if (CorrectCode.Count > 0)
        {
            if (GSKFile.FileIsExist(FileName))
            {
                UserCode = GSKFile.GetFileContents(FileName);
                if (UserCode.Count != CorrectCode.Count)
                {
                    return false;
                }
                else
                {
                    for(int i=0;i<UserCode.Count;i++)
                    {
                        if(UserCode[i]!=CorrectCode[i])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    public void GetCurrentPosition()
    {
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyAxis.xls";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable AxisData = new DataTable("AxisTable");
        AxisData = excelClass.ExcelReader(ExcelPath, caseName);
        if (stage_current > AxisData.Rows.Count)
        {
            Debug.LogError("当前步骤轴没有设置参数");
        }
        else
        {
            //deal with the data from the excel
            float j1 = Convert.ToSingle(AxisData.Rows[stage_current + 1][1]); float j4 = Convert.ToSingle(AxisData.Rows[stage_current + 1][4]);
            float j2 = Convert.ToSingle(AxisData.Rows[stage_current + 1][2]); float j5 = Convert.ToSingle(AxisData.Rows[stage_current + 1][5]);
            float j3 = Convert.ToSingle(AxisData.Rows[stage_current + 1][3]); float j6 = Convert.ToSingle(AxisData.Rows[stage_current + 1][6]);
            CorrectAngles = new float[6] { j1, j2, j3, j4, j5, j6 };
            Debug.Log("currentangles:" + CorrectAngles[0] + " " + CorrectAngles[1] + " " + CorrectAngles[2] + " " + CorrectAngles[3] + " " + CorrectAngles[4] + " " + CorrectAngles[5]);
        }
    }
    //位置检测
    public bool PositionDetect()
    {
        float[] UserAngles = MotionScript.CurrentAngle_All();
        for (int i = 0; i < 6; i++)
        {
            if (Mathf.Abs(UserAngles[i] - CorrectAngles[i]) > 2)
            {
                return false;
            }
        }
        return true;
    }
    //当前位置与目标位置偏差
    public float[] PosDeviation()
    {
        float[] UserAngles = MotionScript.CurrentAngle_All();
        float[] tempAngles = new float[6];
        for (int i = 0; i < 6; i++)
        {
            tempAngles[i] = CorrectAngles[i] - UserAngles[i];
        }
        return tempAngles;
    }
    //窗口初始化
    public void WinInitial(string win)
    {
        ScreenScript.SetCurrentWin_Teach(win);
    }
    
    //完成案例的教学
    public void CaseTeachOver(string content)
    {
        TipsShow.MyTipsWindowsShow(content, true, true, 35, new Color(255, 0, 0), "msyh");
        ReadTips(content);
        button.MouseStop();
    }
    //练习和考试
    public IEnumerator DoubleE()
    {
        DetectNo = stage_current + 1;
        Debug.Log("DetectNo" + DetectNo);
        float waittime = 0;
        ShowTips();
        ReadTips(TitleName[stage_current]);
        waittime = GetVoiceTime(TitleName[stage_current]);
        yield return StartCoroutine(Timer(waittime));
        FuncPara.loopControl = 0;
        yield return StartCoroutine(WaitLoop());
        Debug.Log("end");
    }
    //记录分数
    public void RecordScore()
    {
        if (GSKDATA.SoftCurrentMode == "Exam")
        {
            ExamClass.ScoreAccounts(stage_current, true);
        }
    }
    //考试跳步
    public void ExamSkip()
    {
        if (++stage_current == StageTotal)
        {
            stage_current = StageTotal - 1;
        }
        else
        {
            ExamClass.ScoreAccounts(stage_current-1, false);
            StageInitial();
            button.BtAllow();//allow
            DetectNo = 0;
            StartCoroutine("Teach");
        }
    }
}



