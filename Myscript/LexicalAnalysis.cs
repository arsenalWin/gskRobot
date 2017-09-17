//<summary>
//LexicalAnalysis.cs
//ROBOT
//Created by 周伟 on 8/21/2015.
//Company: Sunnytech
//Function:
//词法分析类
//一行一行分析
//<summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Runtime.InteropServices;


public class LexicalAnalysis{
    [DllImport("VR_RBP", EntryPoint = "?RBP_InsertParse@@YAHQAHQANH@Z")]
    private static extern int RBP_InsertParse(int[] data, double[] pathdata, int rowNum);//插入译码数据,在当前行末插入新一行程序段
    [DllImport("VR_RBP", EntryPoint = "?RBP_Set_Path_Data@@YAHHQAN@Z")]
    private static extern int RBP_Set_Path_Data(int num, double[] pathdata);

    private string filepath;
    public LexicalAnalysis(string path)
    {
        filepath = path;
    }
    
    //文件的读取
    List<string> FileRead()
    {
        List<string> tempList = new List<string>();
        FileStream file = new FileStream(filepath + GSKDATA.CurrentProgramName, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(file);
        string temp1 = null;
        while ((temp1 = sr.ReadLine()) != null)
        {
            if (!Regex.IsMatch(temp1, @"^P[0-9]{3}"))
            {
                if (temp1 != "")
                {
                    tempList.Add(temp1);
                }
            }
        }
        sr.Close();
        file.Close();
        return tempList;
    }
    /// <summary>
    /// 获取示教点信息
    /// </summary>
    /// <returns></returns>
    List<string> GetPointContents()
    {
        List<string> pointContents = new List<string>();
        string path = filepath + GSKDATA.CurrentProgramName;
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string temp = null;
        while ((temp = sr.ReadLine()) != null)
        {
            if (Regex.IsMatch(temp, @"^P[0-9]{3}"))
            {
                if (temp != "")
                {
                    pointContents.Add(temp);
                }
            }
        }
        sr.Close();
        fs.Close();
        return pointContents;
    }

    //文件的初次处理成string[]
    //关键词的划分
    List<List<string>> KeySplit()
    {
        List<List<string>> temp = new List<List<string>>();
        List<string> filecontents = FileRead();
        string Semicolon;
        string[] tempsplit;
        //一行一行划分
        for (int i = 0; i < filecontents.Count; i++)
        {
            Semicolon = Regex.Replace(filecontents[i], @";", "", RegexOptions.IgnoreCase);//去分号
            Semicolon = Regex.Replace(Semicolon, @":", "", RegexOptions.IgnoreCase);//去冒号
            tempsplit = Semicolon.Split(' ');
            List<string> tlist = new List<string>();
            for (int j = 0; j < tempsplit.Length; j++)
            {
                
                if (tempsplit[j] != "," && tempsplit[j] != "")
                {
                    tempsplit[j] = Regex.Replace(tempsplit[j], @"^,", "", RegexOptions.IgnoreCase);
                    tlist.Add(tempsplit[j]);
                }
            }
            temp.Add(tlist);
        }
        return temp;
    }

    /// <summary>
    /// 路径点的处理
    /// </summary>
    public void PointAnalysis()
    {
        double[] pathdata = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        List<string> pathInfo = GetPointContents();
        //Debug.Log(pathInfo[0]);
        if (pathInfo.Count > 0)
        {
            for (int i = 0; i < pathInfo.Count; i++)
            {
                int num = Convert.ToInt16(pathInfo[i].Substring(1, 3));
                //Debug.Log("num:" + num);
                string tempParhinfo = Regex.Replace(pathInfo[i], @"^P\d+=", "", RegexOptions.IgnoreCase);
                tempParhinfo = Regex.Replace(tempParhinfo, @";$", "", RegexOptions.IgnoreCase);
                pathdata = pathdata_(tempParhinfo);
                RBP_Set_Path_Data(num, pathdata);
            }
        }
    }



    public void KeyAnalysis()
    {
        List<List<int>> tempint = new List<List<int>>();
        List<List<string>> tempkey = KeySplit();
        int[] data = new int[100];
        double[] pathdata = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        int Dalei = 0;//指令属于什么大类
        string tempstr=string.Empty;
        //一行一行分析
        
        for (int i = 0; i < tempkey.Count; i++)
        {
            List<int> listInt = new List<int>();
            tempstr = tempkey[i][0];
            //Debug.Log(tempstr);
            if (Regex.IsMatch(tempstr, @"^MOVJ"))
            {
                Dalei = 1;
            }
            else if (Regex.IsMatch(tempstr, @"^MOVL"))
            {
                Dalei = 2;
            }
            else if (Regex.IsMatch(tempstr, @"^MOVC"))
            {
                Dalei = 3;
            }
            else if (Regex.IsMatch(tempstr, @"^DOUT"))
            {
                Dalei = 4;
            }
            else if (Regex.IsMatch(tempstr, @"^WAIT"))
            {
                Dalei = 5;
            }
            else if (Regex.IsMatch(tempstr, @"^DELAY"))
            {
                Dalei = 6;
            }
            else if (Regex.IsMatch(tempstr, @"^DIN"))
            {
                Dalei = 7;
            }
            else if (Regex.IsMatch(tempstr, @"^PULSE"))
            {
                Dalei = 8;
            }
            else if (Regex.IsMatch(tempstr, @"^LAB\d+"))
            {
                Dalei = 9;
            }
            else if (Regex.IsMatch(tempstr, @"^JUMP"))
            {
                Dalei = 10;
            }
            else if (Regex.IsMatch(tempstr, @"^#"))
            {
                Dalei = 0;
                listInt.Add(2);
                listInt.Add(3);
                listInt.Add(3);
            }
            else if (Regex.IsMatch(tempstr, @"^END"))
            {
                Dalei = 0;
                listInt.Add(2);
                listInt.Add(3);
                listInt.Add(4);
            }
            else if (Regex.IsMatch(tempstr, @"^MAIN"))
            {
                Dalei = 0;
                listInt.Add(2);
                listInt.Add(3);
                listInt.Add(5);
            }
            else if (Regex.IsMatch(tempstr, @"^R"))
            {
                Dalei = 11;
            }
            else if (Regex.IsMatch(tempstr, @"^INC"))
            {
                Dalei = 12;
            }
            else if (Regex.IsMatch(tempstr, @"^DEC"))
            {
                Dalei = 13;
            }
            else if (Regex.IsMatch(tempstr, @"^PX"))
            {
                Dalei = 14;
            }
            else if (Regex.IsMatch(tempstr, @"^SHIFTON"))
            {
                Dalei = 15;
            }
            else if (Regex.IsMatch(tempstr, @"^SHIFTOFF"))
            {
                Dalei = 16;
            }
            else if (Regex.IsMatch(tempstr, @"^MSHIFT"))
            {
                Dalei = 17;
            }
            else if (Regex.IsMatch(tempstr, @"^ADD"))
            {
                Dalei = 18;
            }
            else if (Regex.IsMatch(tempstr, @"^SUB"))
            {
                Dalei = 19;
            }
            else if (Regex.IsMatch(tempstr, @"^MUL"))
            {
                Dalei = 20;
            }
            else if (Regex.IsMatch(tempstr, @"^DIV"))
            {
                Dalei = 21;
            }
            else if (Regex.IsMatch(tempstr, @"^SET"))
            {
                Dalei = 22;
            }
            else if (Regex.IsMatch(tempstr, @"^SETE"))
            {
                Dalei = 23;
            }
            else if (Regex.IsMatch(tempstr, @"^GETE"))
            {
                Dalei = 24;
            }
            else if (Regex.IsMatch(tempstr, @"^AND"))
            {
                Dalei = 25;
            }
            else if (Regex.IsMatch(tempstr, @"^OR"))
            {
                Dalei = 26;
            }
            else if (Regex.IsMatch(tempstr, @"^NOT"))
            {
                Dalei = 27;
            }
            else if (Regex.IsMatch(tempstr, @"^XOR"))
            {
                Dalei = 28;
            }
            else if (Regex.IsMatch(tempstr, @"^ARCON"))
            {
                Dalei = 29;
            }
            else if (Regex.IsMatch(tempstr, @"^ARCOF"))
            {
                Dalei = 30;
            }
            else
            {
                Debug.Log("not found: " + tempstr);
                Dalei = 0;
                listInt.Add(2);
                listInt.Add(3);
                listInt.Add(3);
            }

            switch (Dalei)
            {
                case 1://movj
                    listInt.Add(2);
                    listInt.Add(1);
                    listInt.Add(1);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^P\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(1);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^P\*\(.+"))
                        {
                            listInt.Add(1);
                            listInt.Add(2);
                            tempstr = Regex.Replace(tempstr, @"^P\*\(", "", RegexOptions.IgnoreCase);
                            tempstr = Regex.Replace(tempstr, @"\)$", "", RegexOptions.IgnoreCase);
                            //Debug.Log(tempstr);
                            pathdata = pathdata_(tempstr);
                        }
                        else if (Regex.IsMatch(tempstr, @"^Z\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(3);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"Z", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^V\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(4);
                            //提取数字
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"V", "", RegexOptions.IgnoreCase)));
                            //Debug.Log("vvv:"+Convert.ToInt32(Regex.Replace(tempstr, @"V", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 2://movl
                    listInt.Add(2);
                    listInt.Add(1);
                    listInt.Add(2);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^P\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(1);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^P\*\(.+"))
                        {
                            listInt.Add(1);
                            listInt.Add(2);
                            tempstr = Regex.Replace(tempstr, @"^P\*\(", "", RegexOptions.IgnoreCase);
                            tempstr = Regex.Replace(tempstr, @"\)$", "", RegexOptions.IgnoreCase);
                            //Debug.Log(tempstr);
                            pathdata = pathdata_(tempstr);
                        }
                        else if (Regex.IsMatch(tempstr, @"^Z\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(3);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"Z", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^V\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(4);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"V", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 3://MOVC
                    listInt.Add(2);
                    listInt.Add(1);
                    listInt.Add(3);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^P\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(1);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^P\*\(.+"))
                        {
                            listInt.Add(1);
                            listInt.Add(2);
                            tempstr = Regex.Replace(tempstr, @"^P\*\(", "", RegexOptions.IgnoreCase);
                            tempstr = Regex.Replace(tempstr, @"\)$", "", RegexOptions.IgnoreCase);
                            //Debug.Log(tempstr);
                            pathdata = pathdata_(tempstr);
                        }
                        else if (Regex.IsMatch(tempstr, @"^Z\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(3);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"Z", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^V\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(4);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"V", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 4://DOUT
                    listInt.Add(2);
                    listInt.Add(2);
                    listInt.Add(1);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^OT\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(5);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"OT", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^OFF$"))
                        {
                            listInt.Add(2);
                            listInt.Add(15);
                            listInt.Add(0);
                        }
                        else if (Regex.IsMatch(tempstr, @"^ON$"))
                        {
                            listInt.Add(2);
                            listInt.Add(16);
                            listInt.Add(1);
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 5://WAIT
                    listInt.Add(2);
                    listInt.Add(2);
                    listInt.Add(2);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^IN\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(11);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"IN", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^T\d+$|^T\d+\.\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(12);
                            //Debug.Log(tempstr);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            float t = Convert.ToSingle(Regex.Replace(tempstr, @"T", "", RegexOptions.IgnoreCase));
                            listInt.Add((int)t * 1000);
                        }
                        else if (Regex.IsMatch(tempstr, @"^OFF$"))
                        {
                            listInt.Add(2);
                            listInt.Add(15);
                            listInt.Add(0);
                        }
                        else if (Regex.IsMatch(tempstr, @"^ON$"))
                        {
                            listInt.Add(2);
                            listInt.Add(16);
                            listInt.Add(1);
                        }
                        else
                        {
                            Debug.Log(i);
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 6://DELAY
                    listInt.Add(2);
                    listInt.Add(2);
                    listInt.Add(3);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^T\d+$|^T\d+\.\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(12);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            float t = Convert.ToSingle(Regex.Replace(tempstr, @"T", "", RegexOptions.IgnoreCase));
                            listInt.Add((int)t * 1000);
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 7://DIN
                    listInt.Add(2);
                    listInt.Add(2);
                    listInt.Add(4);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^IN\d+"))
                        {
                            listInt.Add(2);
                            listInt.Add(11);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"IN", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 8://PULSE
                    listInt.Add(2);
                    listInt.Add(2);
                    listInt.Add(5);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^OT\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(5);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"OT", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^T\d+$|^T\d+\.\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(12);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            float t = Convert.ToSingle(Regex.Replace(tempstr, @"T", "", RegexOptions.IgnoreCase));
                            listInt.Add((int)t * 1000);
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 9://LAB
                    listInt.Add(2);
                    listInt.Add(3);
                    listInt.Add(1);
                    listInt.Add(2);
                    listInt.Add(17);
                    listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"LAB", "", RegexOptions.IgnoreCase)));
                    break;
                case 10://jump
                    listInt.Add(2);
                    listInt.Add(3);
                    listInt.Add(2);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        //Debug.Log("jump");
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^LAB\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(13);
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"LAB", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else if (Regex.IsMatch(tempstr, @"^==$"))
                        {
                            listInt.Add(1);
                            listInt.Add(50);
                        }
                        else if (Regex.IsMatch(tempstr, @"^>=$"))
                        {
                            listInt.Add(1);
                            listInt.Add(51);
                        }
                        else if (Regex.IsMatch(tempstr, @"^<=$"))
                        {
                            listInt.Add(1);
                            listInt.Add(52);
                        }
                        else if (Regex.IsMatch(tempstr, @"^>$"))
                        {
                            listInt.Add(1);
                            listInt.Add(53);
                        }
                        else if (Regex.IsMatch(tempstr, @"^<$"))
                        {
                            listInt.Add(1);
                            listInt.Add(54);
                        }
                        else if (Regex.IsMatch(tempstr, @"^<>$"))
                        {
                            listInt.Add(1);
                            listInt.Add(55);
                        }
                        else if (Regex.IsMatch(tempstr, @"^IF$"))
                        {
                            listInt.Add(1);
                            listInt.Add(58);
                        }
                        else if (Regex.IsMatch(tempstr, @"^IN\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(11);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"IN", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^OFF$"))
                        {
                            listInt.Add(2);
                            listInt.Add(15);
                            listInt.Add(0);
                        }
                        else if (Regex.IsMatch(tempstr, @"^ON$"))
                        {
                            listInt.Add(2);
                            listInt.Add(16);
                            listInt.Add(1);
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 11://R
                    listInt.Add(2);
                    listInt.Add(10);
                    //提取数字
                    //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                    listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^=$"))
                        {
                            listInt.Insert(0, 1);
                            listInt.Insert(0, 6);
                            listInt.Insert(0, 2);
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 12://INC
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(1);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^I\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(8);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"I", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 13://DEC
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(2);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^I\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(8);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"I", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 14://PX
                    listInt.Add(2);
                    listInt.Add(14);
                    //提取数字
                    //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                    listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"PX", "", RegexOptions.IgnoreCase)));
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^=$"))
                        {
                            listInt.Insert(0, 1);
                            listInt.Insert(0, 6);
                            listInt.Insert(0, 2);
                            //listInt.Add(2);
                            //listInt.Add(6);
                            //listInt.Add(1);
                        }
                        else if (Regex.IsMatch(tempstr, @"^PX\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(14);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"PX", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\+$"))
                        {
                            listInt.Add(1);
                            listInt.Add(56);
                        }
                        else if (Regex.IsMatch(tempstr, @"^\-$"))
                        {
                            listInt.Add(1);
                            listInt.Add(57);
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 15://SHIFTON
                    listInt.Add(2);
                    listInt.Add(5);
                    listInt.Add(1);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^PX\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(14);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"PX", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 16://SHIFTOFF
                    listInt.Add(2);
                    listInt.Add(5);
                    listInt.Add(2);
                    break;
                case 17://MSHIFT
                    listInt.Add(2);
                    listInt.Add(5);
                    listInt.Add(3);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^PX\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(14);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"PX", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^P\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(1);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 18://ADD
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(3);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^I\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(8);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"I", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 19://sub
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(4);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^I\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(8);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"I", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 20://mul
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(5);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^I\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(8);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"I", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 21://div
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(6);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^I\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(8);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"I", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 22://set
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(7);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^R\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(10);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"R", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^I\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(8);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"I", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 23://sete
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(8);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^PX\d+\(\d+\)$"))
                        {
                            listInt.Add(3);
                            listInt.Add(80);
                            //提取数字
                            string[] tt = tempstr.Split('(');
                            listInt.Add(Convert.ToInt32(Regex.Replace(tt[0], @"[PX]", "", RegexOptions.IgnoreCase)));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tt[1], @"[\)]", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 24://gete
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(9);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^PX\d+\(\d+\)$"))
                        {
                            listInt.Add(3);
                            listInt.Add(80);
                            //提取数字
                            string[] tt = tempstr.Split('(');
                            listInt.Add(Convert.ToInt32(Regex.Replace(tt[0], @"[PX]", "", RegexOptions.IgnoreCase)));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tt[1], @"[\)]", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^D\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(9);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"D", "", RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 25://and
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(10);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 26://or
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(11);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 27://not
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(12);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 28://xor
                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(13);
                    for (int j = 1; j < tempkey[i].Count; j++)
                    {
                        tempstr = tempkey[i][j];
                        if (Regex.IsMatch(tempstr, @"^B\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(7);
                            //提取数字
                            //Convert.ToInt32(Regex.Replace(tempstr, @"P", "", RegexOptions.IgnoreCase));
                            listInt.Add(Convert.ToInt32(Regex.Replace(tempstr, @"B", "", RegexOptions.IgnoreCase)));
                        }
                        else if (Regex.IsMatch(tempstr, @"^\d+$"))
                        {
                            listInt.Add(2);
                            listInt.Add(17);
                            listInt.Add(Convert.ToInt32(tempstr));
                        }
                        else
                        {
                            Debug.Log(tempstr);
                        }
                    }
                    break;
                case 29://ARCON
                    listInt.Add(2);
                    listInt.Add(7);
                    listInt.Add(1);

                    listInt.Add(2);
                    listInt.Add(18);
                    listInt.Add(200);

                    listInt.Add(2);
                    listInt.Add(19);
                    listInt.Add(20);

                    listInt.Add(2);
                    listInt.Add(4);
                    listInt.Add(50);

                    listInt.Add(2);
                    listInt.Add(12);
                    listInt.Add(1);
                    break;
                case 30://ARCOF
                    listInt.Add(2);
                    listInt.Add(7);
                    listInt.Add(2);

                    listInt.Add(2);
                    listInt.Add(18);
                    listInt.Add(200);

                    listInt.Add(2);
                    listInt.Add(19);
                    listInt.Add(20);

                    listInt.Add(2);
                    listInt.Add(12);
                    listInt.Add(1);

                    break;
            }
            data = listInt.ToArray();
            int re = RBP_InsertParse(data, pathdata, i);
            //Debug.Log("插入指令是否成功：" + re);
            
            tempint.Add(listInt);
        }
        WriteData(tempint);//写入数据
    }

    /// <summary>
    /// 写入译码数据
    /// </summary>
    /// <param name="data"></param>
    void WriteData(List<List<int>> data)
    {
        FileStream file = new FileStream(Application.dataPath + "\\StreamingAssets\\Programs\\zw.zyy", FileMode.Create, FileAccess.Write);
        StreamWriter Streamw = new StreamWriter(file);

        for (int i = 0; i < data.Count; i++)
        {
            string temp = "";
            for (int j = 0; j < data[i].Count; j++)
            {
                temp += data[i][j].ToString();
            }
            Streamw.WriteLine(temp);
        }

        Streamw.Close();
        file.Close();
    }

    /// <summary>
    /// 返回P*存储的路径信息
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    double[] pathdata_(string str)
    {
        //Debug.Log(str);
        string[] tt = str.Split(',');
        double[] pathdata = new double[8];
        for (int i = 0; i < 8; i++)
        {
            pathdata[i] = Convert.ToDouble(tt[i]);
            //Debug.Log("dian:" + pathdata[i]);
        }
        return pathdata;
    }

}
