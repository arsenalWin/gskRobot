/// <summary>
/// FileName: ExamClass.cs
/// Author: Jiang Xiaolong
/// Created Time: 2014.04.08
/// Version: 1.0
/// Company: Sunnytech
/// Function: 用于考试状态信息存储和考试过程监控，并且最后输出考试结果文件
///
/// Changed By:
/// Modification Time:
/// Discription:
/// </summary>
using UnityEngine;
using System.Collections;
using System;
using System.Data;
using System.Collections.Generic;
using jiamitxt;

public class ExamClass
{

    private static List<string> OperationID = new List<string>();  //操作ID
    private static List<int> OperationAllow = new List<int>();  //是否为必须执行的步骤：0-任意时刻可执行；1、2-必须按排列顺序执行完一步才可以进行下一步,1为第一层，2为第二层；；
    private static List<bool> ImmediateAccount = new List<bool>();  //是否立即结算分数
    private static List<string> StepJudge = new List<string>();  //步骤判断，用于判断
    public static List<string> StepDiscription = new List<string>();  //步骤描述
    private static List<float> ScorePercent = new List<float>();  //分数占比，处理以后起始就是每一步的分数

    private static List<bool> OperationState = new List<bool>();  //操作状态记录，判断是否得分
    private static List<bool> AccountState = new List<bool>();  //是否已计分
    private static List<bool> StepPass = new List<bool>();  //是否被跳过

    public ExamClass()
    {
        OperationID = new List<string>();
        OperationAllow = new List<int>();
        ImmediateAccount = new List<bool>();
        StepJudge = new List<string>();
        StepDiscription = new List<string>();
        ScorePercent = new List<float>();

        OperationState = new List<bool>();
        AccountState = new List<bool>();
        StepPass = new List<bool>();
    }

    /// <summary>
    /// 考试信息获取.
    /// </summary>
    /// <param name='sheet_name'>
    /// 表格sheet名字，即本案例的全称.
    /// </param>
    public static void ExamDataAccess(string sheet_name)
    {
        //Data Clear
        OperationID.Clear();
        OperationAllow.Clear();
        ImmediateAccount.Clear();
        StepJudge.Clear();
        StepDiscription.Clear();
        ScorePercent.Clear();

        OperationState.Clear();
        AccountState.Clear();
        StepPass.Clear();

        //Exam information access.
        ExcelOperator excelReader = new ExcelOperator();
        DataTable examTable = excelReader.ExcelReader(Application.dataPath + "/StreamingAssets/Excel/考试设置.xls", sheet_name, "B", "H");

        //Data Initialize.
        float totalScore = 0; //分数总和
        for (int i = 0; i < examTable.Rows.Count; i++)
        {
            OperationID.Add(Convert.ToString(examTable.Rows[i][0]));
            int tempAllow = 0;
            try
            {
                tempAllow = Convert.ToInt32(examTable.Rows[i][1]);
            }
            catch
            {
                //Debug.Log(i+" :"+examTable.Rows[i][1]);
                Debug.LogError(sheet_name + ": 必须执行的步骤格式错误，位置（" + (i + 2).ToString() + ", C）；");
            }
            OperationAllow.Add(tempAllow);
            OperationState.Add(false);
            AccountState.Add(false);
            StepPass.Add(false);
            string accountStr = Convert.ToString(examTable.Rows[i][2]);
            if (accountStr == "0")
            {
                ImmediateAccount.Add(false);
            }
            else
            {
                ImmediateAccount.Add(true);
            }
            string judgeStr = Convert.ToString(examTable.Rows[i][3]);
            if (judgeStr == "")
            {
                StepJudge.Add("");
            }
            else
            {
                StepJudge.Add(judgeStr);
            }
            StepDiscription.Add(Convert.ToString(examTable.Rows[i][4]));
            string scoreStr = Convert.ToString(examTable.Rows[i][5]);
            float score = 0;
            if (scoreStr != "")
            {
                try
                {
                    score = (float)Convert.ToDouble(scoreStr);
                }
                catch
                {
                    Debug.LogError(sheet_name + ": 分数占比参数格式错误，错误信息位置（" + (i + 2).ToString() + ", G）；");
                }
            }
            ScorePercent.Add(score);
            totalScore += score;
        }
        //分数处理
        for (int i = 0; i < ScorePercent.Count; i++)
        {
            ScorePercent[i] = ScorePercent[i] / totalScore * 100f;
        }
        float timeGet = 40f;
        try
        {
            timeGet = (float)Convert.ToDouble(examTable.Rows[0][6]);
        }
        catch
        {
            Debug.LogError(sheet_name + ": 考试时间参数格式错误，错误信息位置（2, H）；");
        }
        FuncPara.examTime = timeGet;

    }

    /// <summary>
    /// 加密成绩输出.
    /// </summary>
    /// <param name='sheet_name'>
    /// 表格sheet名字，即本案例名称.
    /// </param>
    public static void ScoreOutput(string sheet_name)
    {
        //For Excel
        List<string> columnName = new List<string>();
        columnName.Add("序号"); columnName.Add("操作说明"); columnName.Add("得分");
        List<List<string>> outputInfo = new List<List<string>>();
        List<string> singleStep = new List<string>();
        //For TXT
        string encryptedString = "";
        //Time and File Path.
        string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string timeToWirte = "";
        string currentTime = "";
        currentTime += System.DateTime.Now.Year + "_"; timeToWirte += System.DateTime.Now.Year + "年";
        currentTime += System.DateTime.Now.Month + "_"; timeToWirte += System.DateTime.Now.Month + "月";
        currentTime += System.DateTime.Now.Day + "_"; timeToWirte += System.DateTime.Now.Day + "日 ";
        currentTime += System.DateTime.Now.Hour + "_"; timeToWirte += System.DateTime.Now.Hour + ":";
        currentTime += System.DateTime.Now.Minute + "_"; timeToWirte += System.DateTime.Now.Minute + ":";
        currentTime += System.DateTime.Now.Second; timeToWirte += System.DateTime.Now.Second;
        string fileName = FuncPara.studentName + "-" + FuncPara.studentID + "-" + currentTime;

        //Score Information
        string indexStr = "";
        string descriptionStr = "";
        string socreStr = "";
        for (int i = 0; i < StepDiscription.Count; i++)
        {
            singleStep = new List<string>();
            indexStr = i.ToString();
            descriptionStr = StepDiscription[i];
            if (OperationState[i])
                socreStr = ScorePercent[i].ToString();
            else
                socreStr = "0";
            encryptedString += indexStr + "\t" + descriptionStr + "\t" + socreStr + "\n";
            singleStep.Add(indexStr); singleStep.Add(descriptionStr); singleStep.Add(socreStr);
            outputInfo.Add(singleStep);
        }
        //总成绩，姓名，学号，考试日期
        float tempScore = 0;
        for (int i = 0; i < OperationState.Count; i++)
        {
            if (OperationState[i])
                tempScore += ScorePercent[i];
        }
        FuncPara.examScore = (float)Math.Round(tempScore, 0);
        indexStr = ""; descriptionStr = sheet_name + "总成绩"; socreStr = Convert.ToInt32(FuncPara.examScore).ToString();
        singleStep = new List<string>();
        encryptedString += indexStr + "\t" + descriptionStr + "\t" + socreStr + "\n";
        singleStep.Add(indexStr); singleStep.Add(descriptionStr); singleStep.Add(socreStr);
        outputInfo.Add(singleStep);
        indexStr = ""; descriptionStr = "姓名"; socreStr = FuncPara.studentName;
        singleStep = new List<string>();
        encryptedString += indexStr + "\t" + descriptionStr + "\t" + socreStr + "\n";
        singleStep.Add(indexStr); singleStep.Add(descriptionStr); singleStep.Add(socreStr);
        outputInfo.Add(singleStep);
        indexStr = ""; descriptionStr = "学号"; socreStr = FuncPara.studentID;
        singleStep = new List<string>();
        encryptedString += indexStr + "\t" + descriptionStr + "\t" + socreStr + "\n";
        singleStep.Add(indexStr); singleStep.Add(descriptionStr); singleStep.Add(socreStr);
        outputInfo.Add(singleStep);
        indexStr = ""; descriptionStr = "考试日期"; socreStr = timeToWirte;
        singleStep = new List<string>();
        encryptedString += indexStr + "\t" + descriptionStr + "\t" + socreStr + "\n";
        singleStep.Add(indexStr); singleStep.Add(descriptionStr); singleStep.Add(socreStr);
        outputInfo.Add(singleStep);

        //Write To File.
        ExcelOperator excelWriter = new ExcelOperator();
        excelWriter.ExcelWriter(deskPath + "\\" + fileName + ".xls", sheet_name, columnName, outputInfo, true);
        jiami encryptOutput = new jiami();
        encryptOutput.WriteTxt(deskPath, fileName, encryptedString);
    }

    //当前总体上执行到的步骤
    public static int CurrentExecuteStep()
    {
        int sequenceNumber = -1;
        for (int i = 0; i < AccountState.Count; i++)
        {
            if (!AccountState[i])
            {
                sequenceNumber = i;
                break;
            }
        }
        return sequenceNumber;
    }

    #region Scoring Area

    /// <summary>
    /// 考试得分处理入口.
    /// </summary>
    /// <param name='exam_commamd'>
    /// 步骤代码ID.
    /// </param>
    public static void Scoring(string exam_commamd)
    {
        //Determing whether it is in the Examining mode.
        //if (FuncPara.currentMotion == MotionState.Examining)
        if (GSKDATA.SoftCurrentMode == "Exam")
        {
            
            //Obtain the appropriate id index where it has not be graded.
            int idIndex = IndexObtain(exam_commamd, 0);
            //End the scoring stage if the exam command isn't in the OperationID list.
            if (idIndex == -1)
                return;
            //Enter the corresponding task.
            //if (FuncPara.taskActive == Task.Task1 || FuncPara.taskActive == Task.Task2)
            if (GSKDATA.CaseName != "")
            {
                ScoringProcess(idIndex, exam_commamd);
            }
        }
    }

    //获取ID Index.
    private static int IndexObtain(string exam_commamd, int index)
    {
        //for (int i = 0; i < OperationID.Count; i++)
        //{
        //    Debug.Log("bb:"+OperationID[i]);
        //}
        int aimInt = -1;
        if (index > OperationID.Count - 1)
            return -1;
        aimInt = OperationID.IndexOf(exam_commamd, index);
        //Debug.Log("aimInt:" + aimInt);
        if (aimInt != -1)
        {
            if (AccountState[aimInt])
            {  //当前已被计分，递归寻找下一个（重复的ID根据是否已计分来进行判断）
                if (aimInt + 1 > OperationID.Count - 1)
                    return -1;
                else
                    aimInt = IndexObtain(exam_commamd, aimInt + 1);  //函数递归调用
            }
        }
        return aimInt;
    }

    //具体计分过程
    private static void ScoringProcess(int id_index, string exam_command)
    {
        //首先判断当前步骤没有被跳过
        if (!StepPass[id_index])
        {
            //判断是否记过分
            if (!AccountState[id_index])
            {
                //判断是否为必须执行的步骤
                //if (OperationAllow[id_index] > 0)
                //{
                //    //判断是否允许执行
                //    if (!CurrentAllow(id_index, OperationAllow[id_index]))  //不允许，则返回，不作处理
                //        return;
                //}
                //判断是否立即计分
                if (ImmediateAccount[id_index])
                {  //YES
                    //判断是否含有之前步骤结算
                    //if (StepJudge[id_index] != "")
                    //{  //YES
                    //    //之前步骤分数结算
                    //    PrevScoreAccounts(id_index, exam_command);  //内含递归调用
                    //}
                    //当前步骤分数结算（有的立即结算步骤需要判断）
                    TaskManage(id_index, exam_command, -1, "");
                }
                else
                {  //NO
                    //判断是否含有之前步骤结算
                    if (StepJudge[id_index] != "")
                    {  //YES
                        //之前步骤分数结算
                        PrevScoreAccounts(id_index, exam_command);  //内含递归调用
                    }
                }
            }
        }
    }

    //当前是否允许执行考试判断
    private static bool CurrentAllow(int index_number, int allow_number)
    {
        if (allow_number >= 10)
        {  //主要层次对应的子过程
            int unitsDigit = allow_number / 10;  //个位数
            for (int i = index_number - 1; i >= 0; i--)
            {
                if (OperationAllow[i] == unitsDigit)
                {
                    if (!AccountState[i])
                    {  //没有被计算过
                        return false;
                    }
                }
            }
        }
        else
        {  //主要层次
            for (int i = 0; i < index_number; i++)
            {
                if (OperationAllow[i] > 0 && OperationAllow[i] <= allow_number)
                {
                    if (!AccountState[i])
                    {  //没有被计算过
                        return false;
                    }
                }
            }
        }
        //		Debug.Log(index_number.ToString() + ": " + allow_number.ToString());
        return true;
    }

    //分数计算部分
    //public函数，在跳步时需要在外部调用
    public static void ScoreAccounts(int id_index, bool scoring_or_not)
    {
        AccountState[id_index] = true;
        if (scoring_or_not)
        {
            OperationState[id_index] = true;
            GameObject.Find("MainScript").GetComponent<MyAward>().StartEatFruit((int)ScorePercent[id_index]);//吃水果
        }
        else
        {
            OperationState[id_index] = false;
        }
        float tempScore = 0;
        for (int i = 0; i < OperationState.Count; i++)
        {
            if (OperationState[i])
            {
                tempScore += ScorePercent[i];
            }               
        }
        FuncPara.examScore = (float)Math.Round(tempScore, 0);
    }

    //非立即步骤分数结算
    private static void PrevScoreAccounts(int id_index, string exam_command)
    {
        string[] stepInfoArray = StepJudge[id_index].Split(',');
        for (int i = 0; i < stepInfoArray.Length; i++)
        {
            int tempID = int.Parse(stepInfoArray[i]);
            if (!AccountState[tempID])
            {  //如果没有被结算过
                //判断是否含有之前步骤结算
                if (StepJudge[tempID] != "")
                {  //YES
                    PrevScoreAccounts(tempID, exam_command);  //递归调用
                }
                //相应案例处理流程
                TaskManage(id_index, exam_command, tempID, OperationID[tempID]);
            }
        }
    }

    /// <summary>
    /// 相应案例处理流程.
    /// </summary>
    /// <param name='id_index'>
    /// 当前序号.
    /// </param>
    /// <param name='exam_command'>
    /// 当前操作ID.
    /// </param>
    /// <param name='sub_index'>
    /// 非立即步骤结算序号.
    /// </param>
    /// <param name='sub_command'>
    /// 非立即步骤操作ID.
    /// </param>
    private static void TaskManage(int id_index, string exam_command, int sub_index, string sub_command)
    {
        ////案例1处理
        //if (Sharedscript.Casename=="Chess")
        //{
        //    Task1Manage(id_index, exam_command, sub_index, sub_command);
        //}
        //else if (Sharedscript.Casename == "Load")
        //{
        //    Task2Manage(id_index, exam_command, sub_index, sub_command);
        //}
        //else if (FuncPara.taskActive == Task.Task2)
        //{  //案例2处理
        //    Task2Manage(id_index, exam_command, sub_index, sub_command);
        //}
    }

    //用计分是否完全来判断考试是否结束
    private static bool IsExamFinish()
    {
        for (int i = 0; i < AccountState.Count; i++)
        {
            if (!AccountState[i])
            {
                return false;
            }
        }
        return true;
    }

    //案例1加工
    private static void Task1Manage(int id_index, string exam_command, int sub_index, string sub_command)
    {
        ScoreAccounts(id_index, true);  //注意是id_index

        //立即得分步骤
        //if (id_index == 0 || id_index == 1 || id_index == 2)
        //{
        //    ScoreAccounts(id_index, true);  //注意是id_index
        //    Task1 st_Task1;
        //    st_Task1 = GameObject.Find("Main Camera").GetComponent<Task1>();
        //    st_Task1.colorControl[id_index] = true;
        //    st_Task1.testState[id_index] = true;
        //}
        //else if (id_index == 5)
        //{
        //    Task1 st_Task1;
        //    st_Task1 = GameObject.Find("Main Camera").GetComponent<Task1>();
        //    if (sub_index == 3)
        //    {
        //        if (st_Task1.testState[sub_index])  //状态检查
        //            ScoreAccounts(sub_index, true);  //sub_index
        //        else
        //            ScoreAccounts(sub_index, false);  //sub_index
        //        st_Task1.colorControl[sub_index] = true;
        //    }
        //    else if (sub_index == -1)
        //    {  //5的立即结算步骤
        //        st_Task1.colorControl[id_index] = true;  //注意是id_index
        //        st_Task1.testState[id_index] = true;  //注意是id_index
        //        ScoreAccounts(id_index, true);  //注意是id_index
        //    }
        //}
        //else if (id_index == 6)
        //{
        //    Task1 st_Task1;
        //    st_Task1 = GameObject.Find("Main Camera").GetComponent<Task1>();
        //    if (sub_index == 4)
        //    {
        //        if (st_Task1.testState[sub_index])
        //            ScoreAccounts(sub_index, true);  //sub_index
        //        else
        //            ScoreAccounts(sub_index, false);  //sub_index
        //        st_Task1.colorControl[sub_index] = true;
        //    }
        //    else if (sub_index == -1)
        //    {  //6的立即结算步骤
        //        st_Task1.colorControl[id_index] = true;  //注意是id_index
        //        st_Task1.testState[id_index] = true;  //注意是id_index
        //        ScoreAccounts(id_index, true);  //注意是id_index
        //    }
        //}
        ////本次考试结束
        //if (IsExamFinish())
        //{
        //    ExamControl st_Exam = GameObject.Find("Main Camera").GetComponent<ExamControl>();
        //    st_Exam.ExamFinish();
        //    D.Log("Finish");
        //}
        //		Debug.Log(id_index + ", " + exam_command + ", " + sub_index + ", " + sub_command);
    }

    //案例2加工
    private static void Task2Manage(int id_index, string exam_command, int sub_index, string sub_command)
    {
        ScoreAccounts(id_index, true);  //注意是id_index
        //立即得分步骤
        //if (id_index == 2 || id_index == 4 || id_index == 7 || id_index == 11)
        //{
        //    ScoreAccounts(id_index, true);  //注意是id_index
        //    Task2 st_Task2;
        //    st_Task2 = GameObject.Find("Main Camera").GetComponent<Task2>();
        //    st_Task2.colorControl[id_index] = true;
        //    st_Task2.testState[id_index] = true;
        //}
        //else if (id_index == 1)
        //{
        //    Task2 st_Task2;
        //    st_Task2 = GameObject.Find("Main Camera").GetComponent<Task2>();
        //    if (sub_index == 0)
        //    {
        //        if (st_Task2.testState[sub_index])  //状态检查
        //            ScoreAccounts(sub_index, true);  //sub_index
        //        else
        //            ScoreAccounts(sub_index, false);  //sub_index
        //        st_Task2.colorControl[sub_index] = true;
        //    }
        //    else if (sub_index == -1)
        //    {
        //        st_Task2.colorControl[id_index] = true;  //注意是id_index
        //        st_Task2.testState[id_index] = true;  //注意是id_index
        //        ScoreAccounts(id_index, true);  //sub_index
        //    }
        //}
        //else if (id_index == 10)
        //{
        //    Task2 st_Task2;
        //    st_Task2 = GameObject.Find("Main Camera").GetComponent<Task2>();
        //    if (sub_index == 3 || sub_index == 5 || sub_index == 6 || sub_index == 8 || sub_index == 9)
        //    {
        //        if (st_Task2.testState[sub_index])
        //            ScoreAccounts(sub_index, true);  //sub_index
        //        else
        //            ScoreAccounts(sub_index, false);  //sub_index
        //        st_Task2.colorControl[sub_index] = true;
        //    }
        //    else if (sub_index == -1)
        //    {
        //        st_Task2.colorControl[id_index] = true;  //注意是id_index
        //        st_Task2.testState[id_index] = true;  //注意是id_index
        //        ScoreAccounts(id_index, true);  //sub_index
        //    }
        //}
        ////本次考试结束
        //if (IsExamFinish())
        //{
        //    ExamControl st_Exam = GameObject.Find("Main Camera").GetComponent<ExamControl>();
        //    st_Exam.ExamFinish();
        //    D.Log("Finish");
        //}
        //		Debug.Log(id_index + ", " + exam_command + ", " + sub_index + ", " + sub_command);

    }

    #endregion
}
