using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class ProgramWinClass : Win
{
    public int copy = 0;//复制的 步骤
    public int cut = 0;//剪切的步骤
    public int fline = 0; //显示的首行序号，10++
    public int line = 0;//当前序号
    public int column = 0;//栏
    public bool IsReadyToChange = true;//在修改模式下是否允许移动
    private int startrow;//开始行
    private int endrow;//结束行
    private int insertrow;//插入行
    private bool Modify; //是否进入修改模式
    private UILabel key;//关键词
    private UILabel value;//值
    public string tempstr;
    public string originalstr;
    private List<string> CopyCutContent = new List<string>();//复制剪切内容
    public UILabel[,] Label = new UILabel[10, 10];//展示代码的label
    public List<string> fileContents = new List<string>();  //存储文件内容的list
    public List<string> pointContents = new List<string>();  //存储示教点信息内容的list
    public string filepath;

	public string[] pTmp = new string[10];

    private bool showAxis7;

    public ProgramWinClass(string now_win_name, int last_w_No, string path)
    {
        showAxis7 = false;

        lastWin = last_w_No;
        nowname = now_win_name;
        noChosen();
        for (int i = 0; i < 10; i++)
        {
			pTmp[i]="";
            for (int j = 0; j < 10; j++)
            {
                string labelname = "Label" + i + j;
                Label[i, j] = GameObject.Find(labelname).GetComponent<UILabel>();
            }

        }
        modifyClose();
        key = GameObject.Find("key").GetComponent<UILabel>();
        value = GameObject.Find("value").GetComponent<UILabel>();

        filepath = path;
        ContentsSplit();
    }

    //
    private List<string> readAllContents()
    {
        List<string> temp = new List<string>();
        FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(file);
        string temp1 = null;
        while ((temp1 = sr.ReadLine()) != null)
        {
            if (!Regex.IsMatch(temp1, @"^P[0-9]*") && !Regex.IsMatch(temp1, @"^U[0-9]*") && !Regex.IsMatch(temp1, @"^T[0-9]*") && !Regex.IsMatch(temp1, @"^#"))
            {
                if (temp1 != "")
                {
                    temp.Add(temp1);
                }
            }
        }
        sr.Close();
        file.Close();
        return temp;
    }

    //
    private List<string> readContents()
    {
        List<string> temp = new List<string>();
        FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(file);
        string temp1 = null;
        while ((temp1 = sr.ReadLine()) != null)
        {
            if (!Regex.IsMatch(temp1, @"^P[0-9]*"))
            {
                if (Regex.IsMatch(temp1, @"\bP\*\(.+\)\s,\b"))
                {
                    temp1 = Regex.Replace(temp1, @"\bP\*\(.+\)\s,\b", "P* ,", RegexOptions.IgnoreCase);
                }
                if (temp1 != "")
                {
                    temp.Add(temp1);
                }
            }
        }
        sr.Close();
        file.Close();
        return temp;
    }
    private void writeContents(List<string> fcontents)
    {
        FileStream file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
        StreamWriter Streamw = new StreamWriter(file);

        for (int i = 0; i < fcontents.Count; i++)
        {
            Streamw.WriteLine(fcontents[i]);
        }

        Streamw.Close();
        file.Close();

    }
    private List<string> readPointContents()
    {
        List<string> temp = new List<string>();
        FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(file);
        string temp1 = null;
        while ((temp1 = sr.ReadLine()) != null)
        {
            if (Regex.IsMatch(temp1, @"^P[0-9]{3}"))
            {
                if (temp1 != "")
                {
                    temp.Add(temp1);
                }
            }
        }
        sr.Close();
        file.Close();
        return temp;
    }

    //复制或者是剪切代码行的高亮显示
    private void chosenShow()
    {
        //Debug.Log("startrow:" + startrow);
        //Debug.Log("endrow:" + endrow);
        int min, max;
        if (endrow > startrow)
        {
            max = endrow - fline * 10;
            min = startrow - fline * 10;
        }
        else
        {
            min = endrow - fline * 10;
            max = startrow - fline * 10;
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Label[i, j].color = Color.black;
            }
        }
        if (max >= 0&&min<10)
        {
            max = max < 9 ? max : 9;
            min = min > 0 ? min : 0;
            for (int i = min; i < max + 1; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Label[i, j].color = Color.yellow;
                }
            }
        }
        
    }
    //插入行的红色高亮显示
    private void insertShow()
    {
        int insert = insertrow - fline * 10;
        if (insert >= 0 && insert < 10)
        {
            for (int j = 0; j < 5; j++)
            {
                Label[insert, j].color = Color.red;
            }
        }
    }
    private void modifyClose()
    {
        GameObject.Find("ModifyLabel").GetComponent<UIWidget>().alpha = 0;
        Modify = false;
        doMutexPart();
    }
    //prl文件显示到label上
    public void ContentsSplit()
    {
        int firstline = fline * 10;
        //读取内容
        fileContents = new List<string>();
		fileContents = readAllContents();

        //划分字符串
        if (fileContents.Count == 0)
        {
            return;
        }
        else
        {
            //先将label里面的内容清除
            for (int i = 0; i < 10; i++)
            {
				pTmp[i]="";
                for (int j = 0; j < 10; j++)
                {
                    Label[i, j].text = "";
                }
            }
            //一行一行划分
            for (int i = firstline; i < fileContents.Count; i++)
            {
                if (i < (firstline + 10))
                {
                    string[] _label = fileContents[i].Split(new char[1] { ' ' });
                    if (_label.Length > 12)
                    {
                        Debug.LogError("有一行的元素超过12个");
                    }
                    for (int j = 0; j < _label.Length; j++)
                    {
						if(_label[j].Contains("*"))
						{
							pTmp[i]=_label[j].Substring(2);
							_label[j]="P*";
						}
                        if (j < _label.Length - 1)
                        {
                            Label[i - firstline, j].text = _label[j] + " ";
                        }
                        else
                            Label[i - firstline, j].text = _label[j];
                    }
                    //将labe排列整齐
                    for (int m = 1; m < _label.Length; m++)
                    {
                        Label[i - firstline, m].transform.localPosition = new Vector3(Label[i - firstline, m - 1].transform.localPosition.x - Label[i - firstline, m - 1].width, Label[i - firstline, m].transform.localPosition.y, Label[i - firstline, m].transform.localPosition.z);
                    }
                }
            }
        }
    }
    //复制键的功能实现
    public int CopyFunction()
    {
        if (!Modify)
        {
            switch (copy)
            {
                case 0:
                    if (line + fline * 10 == 0)
                    {
                        return -5;//首行不可以复制
                    }
                    else if (line + fline * 10 == fileContents.Count - 1)
                    {
                        return -6;//末行不可以复制
                    }
                    startrow = line + fline * 10;
					endrow = startrow;
                    break;
                case 1:
                    //确定复制的内容
                    CopyCutContent = new List<string>();
                    for (int i = startrow; i < endrow + 1; i++)
                    {
                        CopyCutContent.Add(fileContents[i]);
                    }
                    //红色高亮显示插入行
                    insertrow = endrow;
                    insertShow();
                    break;
                case 2:
                    if (line + fline * 10 == fileContents.Count - 1 || (insertrow <= endrow-1 && insertrow >= startrow))
                    {
                        return -7;//粘贴位置不对
                    }
                    break;
            }
            if (++copy > 2)
            {
                //执行复制命令
                fileContents.InsertRange(insertrow + 1, CopyCutContent);
                //加上point信息再写入文件
                pointContents = readPointContents();
                List<string> allContents = new List<string>();
                allContents.InsertRange(0, fileContents);
                allContents.InsertRange(0, pointContents);
                writeContents(allContents);
                ContentsSplit();
                doMutexPart();
                insertrow = 0;
                startrow = 0;
                endrow = 0;
                copy = 0;
            }
        }
        
        return 1;
    }
    public int CutFunction()
    {
        if (!Modify)
        {
            switch (cut)
            {
                case 0:
                    if (line + fline * 10 == 0)
                    {
                        return -5;//首行不可以剪切
                    }
                    else if (line + fline * 10 == fileContents.Count - 1)
                    {
                        return -6;//末行不可以剪切
                    }
                    startrow = line + fline * 10;
					endrow = startrow;
                    break;
                case 1:
                    //确定剪切的内容
                    CopyCutContent = new List<string>();
                    for (int i = startrow; i < endrow + 1; i++)
                    {
                        CopyCutContent.Add(fileContents[i]);
                    }
                    //红色高亮显示插入行
                    insertrow = endrow;
                    insertShow();
                    break;
                case 2:
                    if (line + fline * 10 == fileContents.Count - 1 || (insertrow <= endrow-1 && insertrow >= startrow))
                    {
                        return -7;//粘贴位置不对
                    }
                    break;
            }
            if (++cut > 2)
            {
                //执行剪切命令
                fileContents.InsertRange(insertrow + 1, CopyCutContent);//添加
                if (endrow <= insertrow)
                {
                    fileContents.RemoveRange(startrow, endrow - startrow + 1);
                }
                else
                {
                    fileContents.RemoveRange(endrow + 1, endrow - startrow + 1);
                }
                writeContents(fileContents);
                ContentsSplit();
                doMutexPart();
                insertrow = 0;
                startrow = 0;
                endrow = 0;
                cut = 0;
            }
        }
        
        return 1;
    }

    //选择删除的行
    public int DeleteChoose()
    {
        if (!Modify)
        {
            if (fline * 10 + line == 0)
            {
                return -1;//首行不能删除
            }
            else if (fline * 10 + line == fileContents.Count - 1)
            {
                return -2; //尾行不能删除
            }
            return 220;//调出删除确认窗口
        }
        return 1;
        
    }
    //执行删除操作
    public int DeleteDone()
    {
        List<string> tempcontent = new List<string>();
        fileContents.RemoveAt(fline * 10 + line);
        pointContents = readPointContents();
        tempcontent.InsertRange(0, fileContents);
        tempcontent.InsertRange(0, pointContents);
        tempcontent.Insert(0, "");
        writeContents(tempcontent);
        ContentsSplit();
        return 1;
    }


    //修改模式的label显示
    private void modifyShow()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Label[i, j].color = Color.black;
            }
        }
        Label[line, column].color = Color.yellow;
        //修改label
        GameObject.Find("ModifyLabel").GetComponent<UIWidget>().alpha = 1;
        key.text = Regex.Replace(Label[line, column].text, @"[0-9,\s,*,;]", "", RegexOptions.IgnoreCase);
        value.text = Regex.Replace(Label[line, column].text, @"[a-z,\s,=,>,<,+,-,;]", "", RegexOptions.IgnoreCase);
        value.transform.localPosition = new Vector3(key.transform.localPosition.x + key.width, value.transform.localPosition.y, value.transform.localPosition.z);
        
        originalstr = value.text;
        tempstr = "";
    }
    //修改按钮功能
    public void ModifyFunction()
    {
        Modify = true;
        column = 0;
        modifyShow();
    }

    //添加程序
    public void AddContent(string keyword)
    {     
        string temp_ = "";
        switch (keyword)
        {
            case "MOVJ":
                temp_ = "MOVJ P*" + getPointInfo() + " ,V100 ,Z0 ;";
                break;
            case "MOVL":
                temp_ = "MOVL P*" + getPointInfo() + " ,V100 ,Z0 ;";
                break;
            case "MOVC":
                temp_ = "MOVC P*" + getPointInfo() + " ,V100 ,Z0 ;";
                break;
            case "DOUT":
                temp_ = "DOUT OT0 ,ON ;";
                break;
            case "DIN":
                temp_ = "DIN R0 ,IN0 ;";
                break;
            case "DELAY":
                temp_ = "DELAY T0 ;";
                break;
            case "WAIT":
                temp_ = "WAIT IN0 ,ON ,T0 ;";
                break;
            case "PULSE":
                temp_ = "PULSE OT0 ,T0";
                break;
            case "LAB":
                temp_ = "LAB0 :";
                break;
            case "JUMP":
                temp_ = "JUMP LAB0 ;";
                break;
            case "JUMP_R":
                temp_ = "JUMP LAB0 ,IF R0 <= 0 ;";
                break;
            case "JUMP_IN":
                temp_ = "JUMP LAB0 ,IF IN0 == ON ;";
                break;
            case "JH": //#
                break;
            case "R":
                temp_ = "R0 = 0 ;";
                break;
            case "INC":
                temp_ = "INC R0 ;";
                break;
            case "DEC":
                temp_ = "DEC R0 ;";
                break;
            case "PX":
                temp_ = "PX0 = PX0 - PX0 ;";
                break;
            case "SHIFTON":
                temp_ = "SHIFTON PX0 ;";
                break;
            case "SHIFTOFF":
                temp_ = "SHIFTOFF ;";
                break;
            case "MSHIFT":
                temp_ = "MSHIFT  PX0 ,P001 ,P002 ;";
                break;
            case "ARCON":
                temp_ = "ARCON  AC200 ,AV20 , T1 ,V50 ;";
                break;
            case "ARCOF":
                temp_ = "ARCOF AC150 ,AV18 T1 ;";
                break;
        }
        line++;
        if (line == 10)
        {
            line = 0;
            fline++;
        }
        List<string> allContents = new List<string>();
        allContents = readAllContents();
        //Debug.Log(allContents.Count);
        //Debug.Log(fline + line);
        allContents.Insert(fline * 10 + line, temp_);
        //加上point信息再写入文件
        pointContents = readPointContents();
        allContents.InsertRange(0, pointContents);
        allContents.Insert(0, "");
        writeContents(allContents);
    }

    private string getPointInfo()
    {
        RobotMotion Motionscript = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        float[] angle8 = Motionscript.CurrentAngle_All();
        string point = "(";
        for (int i = 0; i < 8; i++)
        {
            point += angle8[i].ToString();
            if (i != 7)
            {
                point += ",";
            }
        }
        point += ")";
        return point;
    }

    public override int isChosen()
    {
        selected = true;
        showWin();
        doMutexPart();
        if (Modify)
        {
            modifyShow();
        }
        return 1;
    }

    public int issChosen()
    {
        selected = true;
        showWin();
        doMutexPart();
        return 1;
    }

    public override int noChosen()
    {
        selected = false;
        closeWin();
        return 1;
    }

    public override int onChooseBt()
    {
        if (!Modify)
        {
            return -2;
        }
        else
        {
            switch (key.text)
            {
                case "MOVJ":
                    tempstr = "MOVL";
                    key.text = tempstr;
                    break;
                case "MOVL":
                    tempstr = "MOVC";
                    key.text = tempstr;
                    break;
                case "MOVC":
                    tempstr = "MOVJ";
                    key.text = tempstr;
                    break;
                case "OFF":
                    tempstr = "ON";
                    key.text = tempstr;
                    break;
                case "ON":
                    tempstr = "OFF";
                    key.text = tempstr;
                    break;
                case "==":
                    tempstr = ">";
                    key.text = tempstr;
                    break;
                case ">":
                    tempstr = "<";
                    key.text = tempstr;
                    break;
                case "<":
                    tempstr = ">=";
                    key.text = tempstr;
                    break;
                case ">=":
                    tempstr = "<=";
                    key.text = tempstr;
                    break;
                case "<=":
                    tempstr = "<>";
                    key.text = tempstr;
                    break;
                case "<>":
                    tempstr = "==";
                    key.text = tempstr;
                    break;
                case "+":
                    tempstr = "-";
                    key.text = tempstr;
                    break;
                case "-":
                    tempstr = "+";
                    key.text = tempstr;
                    break;
                default:
                    break;
            }
        }
            return 1;
    }

    public override int onSwitch()
    {
        if (!Modify)
        {
            return -2;
        }
        else
        {
            switch (key.text)
            {
                case "MOVJ":
                case "MOVL":
                case "MOVC":
                    if(Label[line,5].text == "")
                    {
                        showAxis7 = false;
                    }
                    else
                    {
                        showAxis7 = true;
                    }

                    if (!showAxis7)
                    {
                        Label[line, 4].text = ",E1 ";
                        Label[line, 5].text = ",EV10 ";
                        Label[line, 6].text = ";";
                    }
                    else
                    {
                        Label[line, 4].text = ";";
                        Label[line, 5].text = "";
                        Label[line, 6].text = "";
                    }
                    //排列
                    for (int m = 1; m < 10; m++)
                    {
                        Label[line, m].transform.localPosition = new Vector3(Label[line, m - 1].transform.localPosition.x - Label[line, m - 1].width, Label[line, m].transform.localPosition.y, Label[line, m].transform.localPosition.z);
                    }
				NotCoverPoint();
                    break;
                default:
                    break;
            }
        }
        return 1;
    }

    public override string onNumBt(string number)
    {
        if (Modify)
        {
            IsReadyToChange = false;
            return RegularCheck(number);
        }
        return "请切换到编辑模式";
    }

    public override int onInputBt()
    {
        if (Modify)
        {
            if (tempstr != originalstr)//做出修改
            {
                pointContents = readPointContents();
                switch (key.text)
                {
                    case "MOVJ":
                    case "MOVL":
                    case "MOVC":
                    case "OFF":
                    case "ON":
                    case "==":
                    case ">":
                    case "<":
                    case ">=":
                    case "<=":
                    case "<>":
                    case "+":
                    case "-":
                        NotCoverPoint2();
                        break;
                    case "V":
                    case "Z":
                        if (tempstr == "")
                        {
                            break;
                        }
                        NotCoverPoint();
                        break;
                    case "P":
                        //Debug.Log("原始的P:" + originalstr);
					    if (tempstr == "")
                        {
							break;
                        }
                        ///对文档进行操作
                        ///删除P*添加P1
                        string Pstr = string.Empty;
                        string P_str = string.Empty;
                        List<string> allContents = new List<string>();
                        allContents = readAllContents();
                        if (pointContents.Count > 0)
                        {
                            for (int i = 0; i < pointContents.Count; i++)
                            {
                                if (pointContents[i].Substring(1, 3) == Convert.ToInt16(tempstr).ToString("000"))//比较示教点序号是否相等
                                {
                                    Debug.Log("示教点重复");
                                    return 370;
                                }
                            }
                        }

                        //新点
                        //Regex regex = new Regex(@"\bP\*\(.+\)\s,\b");
                        //Match m = regex.Match(allContents[line + fline * 10]);
                        //Debug.Log(m.Groups.Count);
                        //if (m.Groups.Count >= 1)
                        if (originalstr == "*")
                        {
						Pstr = "P" + Convert.ToInt16(tempstr).ToString("000") + "=" + pTmp[line].Substring(1,pTmp[line].Length-2);
						pTmp[line] = "";
						pointContents.Add(Pstr);
                            //Debug.Log(m.Groups[0].Value);
                            //Pstr = Regex.Replace(m.Groups[0].Value, @"\bP\*\(", "P" + Convert.ToInt16(tempstr).ToString("000") + "=", RegexOptions.IgnoreCase);
                            //Debug.Log("xindian1:" + Pstr);
                            //Pstr = Regex.Replace(Pstr, @"\b\)\s,", ";", RegexOptions.IgnoreCase);
                            //Debug.Log("xindian2:" + Pstr);
                            //allContents[line + fline * 10] = Regex.Replace(allContents[line + fline * 10], @"\bP\*\(.+\)\s,\b", "P" + tempstr + " ,", RegexOptions.IgnoreCase);
                            //pointContents.Add(Pstr);
                            //Debug.Log("xindian3:" + Pstr);
							
                        }
                        //改点的序号
                        else
                        {
                            Debug.Log("change point");
                            Debug.Log("原始的P:" + originalstr);
                            for (int i = 0; i < pointContents.Count; i++)
                            {
                                Debug.Log(pointContents[i].Substring(1, 3));
                                Debug.Log(Convert.ToInt16(originalstr).ToString("000"));
                                if (pointContents[i].Substring(1, 3) == Convert.ToInt16(originalstr).ToString("000"))//比较示教点序号是否相等
                                {
                                    Pstr = Regex.Replace(pointContents[i], @"^P[0-9]{3}", "P" + Convert.ToInt16(tempstr).ToString("000"), RegexOptions.IgnoreCase);
                                    Debug.Log(Pstr);
                                    break;
                                }
                            }
                            pointContents.Add(Pstr); 
                        }
                        pointContents.Sort();//排序

                        NotCoverPoint();
                        break;
                    default:
                        NotCoverPoint();
                        break;
                }
            }
            else//没有做出修改
            {
                tempstr = "";
                IsReadyToChange = true;
            }
            return 1;
        }
        return -2;//切换到编辑模式
    }

    public void CoverPoint()
    {
        string Pstr = string.Empty;
        string P_str = string.Empty;
        List<string> allContents = new List<string>();
        allContents = readAllContents();
        pointContents = readPointContents();
        Debug.Log("tempstr" + tempstr);
        for (int i = 0; i < pointContents.Count; i++)
        {
            if (pointContents[i].Substring(1, 3) == Convert.ToInt16(tempstr).ToString("000"))//比较示教点序号是否相等
            {
                Debug.Log("删除之前的示教点");
                pointContents.RemoveAt(i);
            }
        }
        Regex regex = new Regex(@"\bP\*\(.+\)\s,\b");
        Match m = regex.Match(allContents[line + fline * 10]);
        if (originalstr == "*")
        {
            Pstr = Regex.Replace(m.Groups[0].Value, @"\b\*\(\b", Convert.ToInt16(tempstr).ToString("000") + "=", RegexOptions.IgnoreCase);
            Pstr = Regex.Replace(Pstr, @"\b\)\s,\b", ";", RegexOptions.IgnoreCase);
            allContents[line + fline * 10] = Regex.Replace(allContents[line + fline * 10], @"\bP\*\(.+\)\s,\b", "P" + tempstr + " ,", RegexOptions.IgnoreCase);
            pointContents.Add(Pstr);
        }
        else
        {
            for (int i = 0; i < pointContents.Count; i++)
            {
                if (pointContents[i].Substring(1, 3) == Convert.ToInt16(originalstr).ToString("000"))//比较示教点序号是否相等
                {
                    pointContents[i] = Regex.Replace(pointContents[i], @"^P[0-9]{3}", "P" + Convert.ToInt16(tempstr).ToString("000"), RegexOptions.IgnoreCase);
                }
            }
        }

        NotCoverPoint();
    }

    public void NotCoverPoint()
    {
        if (Label[line, column].text.Contains(","))
        {
            Label[line, column].text = "," + key.text + tempstr + " ";
        }
        else
        {
            Label[line, column].text = key.text + tempstr + " ";
        }

        originalstr = tempstr;
        tempstr = "";
        IsReadyToChange = true;
        string new_code = string.Empty;
        //写入
        for (int i = column + 1; i < 10; i++)
        {
            Label[line, i].transform.localPosition = new Vector3(Label[line, i - 1].transform.localPosition.x - Label[line, i - 1].width, Label[line, i].transform.localPosition.y, Label[line, i].transform.localPosition.z);
        }

        //保存到文件流中
        for (int i = 0; i < 10; i++)
        {
            if (Label[line, i].text != "")//是不是应该考虑空格
            {
				if(Label[line, i].text.Contains("P*"))
				{
					new_code += "P*"+pTmp[line];
				}
				else
					new_code += Label[line, i].text;
            }
        }

        List<string> allContents = new List<string>();
        allContents = readAllContents();
        allContents[fline * 10 + line] = new_code;
        //加上point信息再写入文件
        Debug.Log(pointContents.Count);
        allContents.InsertRange(0, pointContents);
        allContents.Insert(0, "");
        writeContents(allContents);
    }

    public void NotCoverPoint2(string pTemp="")
    {
        if (Label[line, column].text.Contains(","))
        {
            Label[line, column].text = "," + tempstr + " ";
        }
        else
        {
            Label[line, column].text = tempstr + " ";
        }

        originalstr = tempstr;
        tempstr = "";
        IsReadyToChange = true;
        string new_code = string.Empty;
        //写入
        for (int i = column + 1; i < 10; i++)
        {
            Label[line, i].transform.localPosition = new Vector3(Label[line, i - 1].transform.localPosition.x - Label[line, i - 1].width, Label[line, i].transform.localPosition.y, Label[line, i].transform.localPosition.z);
        }

        //保存到文件流中
        for (int i = 0; i < 10; i++)
        {
			if (Label[line, i].text != "")//是不是应该考虑空格
			{
				if(Label[line, i].text.Contains("P*"))
				{
					new_code += "P*"+pTmp[line];
				}
				else
					new_code += Label[line, i].text;
			}
        }

        List<string> allContents = new List<string>();
        allContents = readAllContents();
        allContents[fline * 10 + line] = new_code;
        //加上point信息再写入文件
        Debug.Log(pointContents.Count);
        allContents.InsertRange(0, pointContents);
        allContents.Insert(0, "");
        writeContents(allContents);
    }

    public override int onGet()
    {
        switch (key.text)
        {
            case "P":
                //获取示教点
			if(value.text == "*")
			{
				Debug.Log("get the point of P*");
				pTmp[line] = getPointInfo();
				break;
			}
            return Convert.ToInt16(originalstr);
        }
        return -1;
    }
    public override int on_LeftBt()
    {
        if (Modify)
        {
            if (tempstr != "")
            {
                tempstr = tempstr.Substring(0, tempstr.Length - 1);
                value.text = tempstr;
            }
        }
        return 1;
    }

    public override int onDownBt()
    {
        if (!Modify)
        {
            if (copy == 0 && cut == 0)
            {
                if (++line + fline * 10 < fileContents.Count)
                {
                    if (line > 9)
                    {
                        fline++;
                        line = 0;
                        ContentsSplit();
                    }
                }
                else
                {
                    line--;
                }
                doMutexPart();
            }
            else if (copy == 1 || cut == 1)//复制的第一步
            {
                if (fline * 10 + line == fileContents.Count - 2)
                {
                    return -4;//已到达尾行
                }
                if (++line + fline * 10 < fileContents.Count)
                {
                    if (line > 9)
                    {
                        fline++;
                        line = 0;
                        ContentsSplit();
                    }
                }
                endrow = fline * 10 + line;
                //黄色显示被选中的代码行
                chosenShow();
            }
            else if (copy == 2 || cut == 2)//复制的第二步
            {
                if (fline * 10 + line == fileContents.Count - 2)
                {
                    return -4;//已到达尾行
                }

                if (++line + fline * 10 < fileContents.Count)
                {
                    if (line > 9)
                    {
                        fline++;
                        line = 0;
                        ContentsSplit();
                    }
                }
                else
                {
                    line--;
                }
                insertrow = fline * 10 + line;
                //黄色显示被选中的代码行
                chosenShow();
                //红色高亮显示插入行
                insertShow();
            }
        }
        else
        {
            if (++line + fline * 10 < fileContents.Count)
            {
                if (line > 9)
                {
                    fline++;
                    line = 0;
                    ContentsSplit();
                }
            }
            else
            {
                line--;
            }
            column = 0;
            modifyShow();
        }
        
        return 1;
    }
    

    public override int onLeftBt()
    {
        if (Modify)
        {
            if (column > 0)
            {
                column--;
            }
            modifyShow();
        }
        return 1;
    }
    public override int onUpBt()
    {
        if (!Modify)
        {
            if (copy == 0 && cut == 0)
            {
                if (--line < 0)
                {
                    line = 9;
                    if (--fline < 0)
                    {
                        fline = 0;
                        line = 0;
                    }
                    ContentsSplit();
                }
                doMutexPart();
            }
            else if (copy == 1 || cut == 1)//复制的第一步
            {
                if (fline + line == 1)
                {
                    return -3;//已到达首行
                }

                if (--line < 0)
                {
                    line = 9;
                    if (--fline < 0)
                    {
                        fline = 0;
                        line = 0;
                    }
                    ContentsSplit();
                }
                endrow = fline * 10 + line;
                //黄色显示被选中的代码行
                chosenShow();
            }
            else if (copy == 2 || cut == 2)//复制的第二步
            {
                if (--line < 0)
                {
                    line = 9;
                    if (--fline < 0)
                    {
                        fline = 0;
                        line = 0;
                    }
                    ContentsSplit();
                }
                insertrow = fline * 10 + line;
                //黄色显示被选中的代码行
                chosenShow();
                //红色高亮显示插入行
                insertShow();
            }
        }
        else
        {
            if (--line < 0)
            {
                line = 9;
                if (--fline < 0)
                {
                    fline = 0;
                    line = 0;
                }
                ContentsSplit();
            }
            column = 0;
            modifyShow();
        }
        
        return 1;
    }

    public override int onRightBt()
    {
        if (Modify)
        {
            if (column < 10)
            {
                if (Label[line, column + 1].text != ";" && Label[line, column + 1].text != "" && Label[line, column + 1].text != ":")
                {
                    column++;
                }
            }
            modifyShow();
        }
        return 1;
    }

    public override int doMutexPart()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Label[i, j].color = Color.black;
            }
        }
        for (int i = 0; i < 10; i++)
        {
            //Debug.Log(line);
            Label[line, i].color = Color.yellow;
        }
        return 1;
    }

    public override int onTabBt()
    {
        return 1;
    }

    public override int onFlipBt()
    {
        return 1;
    }


    public override int InitialWin()
    {
        fline = 0;
        line = 0;
        column = 0;
        doMutexPart();
        IsReadyToChange = true;
        modifyClose();
        noChosen();
        showAxis7 = false;
        return 1;
    }

    public override int onCancelBt()
    {
        modifyClose();
        copy = 0;
        cut = 0;
        return 1;
    }

    public override int closeWin()
    {
        GameObject.Find(nowname).GetComponent<UIPanel>().depth = 0;
        return 1;
    }

    public override int showWin()
    {
        GameObject.Find(nowname).GetComponent<UIPanel>().depth = 1;
        return 1;
    }

    private string RegularCheck(string num)
    {
        string regex_str = @"^0\d";
        string Regexstr = string.Empty;
        string xx = tempstr + num;
        if (Regex.IsMatch(xx, regex_str))
        {
            tempstr = num;
            value.text = tempstr;
            return "ok";
        }
        switch (key.text)
        {
            case "V":
                Regexstr = @"^[1-9]\d?$|^100$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入1-100";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "EV":
                Regexstr = @"^[1-9]\d?$|^100$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入1-100";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;

            case "Z":
                Regexstr = @"^[0-4]$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0-4";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "E":
                Regexstr = @"^[0-4]$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0-4";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "P":
                Regexstr = @"^[0-9]{1,3}$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0-999";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "OT":
                Regexstr = @"^[1-2]?[0-9]$|^30$|^31$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入1-31";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "IN":
                Regexstr = @"^[1-2]?[0-9]$|^30$|^31$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入1-31";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "T":
                Regexstr = @"^[0-8]?\d{1,2}(\.\d)?$|^900(\.\d)?$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0.0-900.0";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "R":
                Regexstr = @"^\d{1,2}$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0-99";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "LAB":
                Regexstr = @"^[0-9]{1,3}$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0-999";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "==":
            case ">=":
            case "<=":
            case "<>":
            case ">":
            case "<":
                Regexstr = @"^[0-9]{1,4}$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0-9999";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            case "PX":
                Regexstr = @"^\d{1,2}$";
                if (!Regex.IsMatch(xx, Regexstr))
                {
                    return "请输入0-99";
                }
                else
                {
                    tempstr = xx;
                    value.text = tempstr;
                }
                break;
            default:
                break;
        }

        return "ok";
    }

}

