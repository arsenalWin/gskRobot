//<summary>
//NewBehaviourScript#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public abstract class Element
{
    public UISprite SpriteBg;
    public bool selected;
    public int WinNext;

    public void SetSpriteBg(string name)
    {
        SpriteBg.spriteName = name;
    }

    public virtual int isChosen()
    {
        selected = true;
        return 1;
    }

    public virtual int noChosen()
    {
        selected = false;
        return 1;
    }

    public virtual int onChooseBt()
    {
        return 1;
    }

    public virtual string onNumBt(string number)
    {
        return "ok";
    }

    public virtual int onInputBt()
    {
        return 1;
    }

    public virtual int on_LeftBt()
    {
        return 1;
    }

    public virtual int onCancel()
    {
        return 1;
    }

    public virtual int onSwitch()
    {
        return 1;
    }

    public virtual int onGet()
    {
        return -1;
    }

    public virtual bool IsReadyToChange()
    {
        return true;
    }
}

//基本按钮
public class MyButton : Element
{
    public UILabel buttonLabel;
    private string tSprite;
    private string fSprite;
    private Color tColor;
    private Color fColor;

    public MyButton(string bt_s_name, string bt_l_name, int next_No, string t_s = "ButtonNormal", string f_s = "ButtonActive")
    {
        SpriteBg = GameObject.Find(bt_s_name).GetComponent<UISprite>();
        buttonLabel = GameObject.Find(bt_l_name).GetComponent<UILabel>();
        WinNext = next_No;
        tSprite = t_s;
        fSprite = f_s;
        tColor = Color.white;
        fColor = Color.black;
    }

    public override int isChosen()
    {
        SpriteBg.spriteName = tSprite;
        buttonLabel.color = tColor;
        selected = true;
        return 1;
    }

    public override int noChosen()
    {
        SpriteBg.spriteName = fSprite;
        buttonLabel.color = fColor;
        selected = false;
        return 1;
    }

    public override int onChooseBt()
    {
        return WinNext;
    }

    public override int onInputBt()
    {
        return 1;
    }

    public override int on_LeftBt()
    {
        return 1;
    }
}

//程序列表
public class MySprite : Element
{
    private string tSprite;
    private string fSprite;

    public MySprite(string bt_s_name, int next_No, string t_s = "greybackground", string f_s = "whitebg")
    {
        SpriteBg = GameObject.Find(bt_s_name).GetComponent<UISprite>();

        WinNext = next_No;
        tSprite = t_s;
        fSprite = f_s;
        
    }

    public override int isChosen()
    {
        SpriteBg.spriteName = tSprite;
        selected = true;
        return 1;
    }

    public override int noChosen()
    {
        SpriteBg.spriteName = fSprite;
        selected = false;
        return 1;
    }

    public override int onChooseBt()
    {
        return WinNext;
    }

    public override int onInputBt()
    {
        return 1;
    }

    public override int on_LeftBt()
    {
        return 1;
    }
}

//alpha 控制显示的text
public class MyText : Element
{
    public string tempstr;//记录输入数据
    public string originalstr;//输入之前原本的数据，
    public UILabel label;
    private UIWidget label_wid;
    public string Checktype;
    public int max;
    public int min;
    public MyText()
    {

    }
    public MyText(string label_name, string label_bg_name, string check_type)
    {
        label = GameObject.Find(label_name).GetComponent<UILabel>();
        label_wid = GameObject.Find(label_bg_name).GetComponent<UIWidget>();
        tempstr = string.Empty;
        Checktype = check_type;
        originalstr = label.text;
        //label.text = string.Empty;
        max = 0;
    }

    public MyText(string label_name, string label_bg_name, string check_type,int min_,int max_)
    {
        label = GameObject.Find(label_name).GetComponent<UILabel>();
        label_wid = GameObject.Find(label_bg_name).GetComponent<UIWidget>();
        tempstr = string.Empty;
        Checktype = check_type;
        originalstr = label.text;
        min = min_;
        max = max_;
    }
    //设置label显示的内容
    public void SetText(string content)
    {
        originalstr = content;
        label.text = content;
    }

    public override int isChosen()
    {
        label_wid.alpha = 1;
        selected = true;
        return 1;
    }

    public override int noChosen()
    {
        label_wid.alpha = 0;
        selected = false;
        return 1;
    }

    public override string onNumBt(string number)
    {
        return RegularCheck(number);
    }

    public override int onInputBt()
    {
        if (label.text == "")
        {
            return 2;//请输入值
        }
        if (max == 0)
        {
            label.text = OutRegularText(tempstr);
            originalstr = tempstr;
            tempstr = string.Empty;
            return 1;
        }
        else
        {
            int inputvalue = Convert.ToInt16(tempstr);
            if (inputvalue <= max && inputvalue >= min)
            {
                label.text = OutRegularText(tempstr);
                tempstr = string.Empty;
                return 1;
            }
            else
            {
                label.text = max.ToString();
                tempstr = string.Empty;
                return min*1000+max;
            }
        }
        
    }

    public override int on_LeftBt()
    {
        if(tempstr.Length!=0)
        {
            tempstr = tempstr.Substring(0,tempstr.Length-1);
        }
        label.text = tempstr;
        return 1;
    }

    public override int onCancel()
    {
        label.text = originalstr;
        tempstr = string.Empty;
        return 1;
    }

    public override bool IsReadyToChange()
    {
        if (tempstr == "" && label.text != "")
        {
            return true;
        }
        return false;
    }

    protected string RegularCheck(string num)
    {
        string regex_str = @"^0\d";
        string xx=tempstr+num;
        if (Regex.IsMatch(xx, regex_str))
        {
            tempstr = num;
            label.text = tempstr;
            return "ok";
        }

        switch (Checktype)
        {
            case "3.0":
                if (Regex.IsMatch(xx, @"^\d{1,3}$"))
                {
                    tempstr = xx;
                    label.text = tempstr;
                }
                else
                {
                    return "输入格式3.0";
                }
                break;
            case "4.2":
                if (Regex.IsMatch(xx, @"^-?\d{1,4}\.\d{0,2}$|^-$|^-?\d{1,4}\.?$"))
                {
                    tempstr = xx;
                    label.text = tempstr;
                }
                else
                {
                    if (Regex.IsMatch(xx, @"^-?\d{5,}|\.\d{2,}$"))
                    {
                        return "输入格式4.2";
                    }
                }
                break;
            case "filename":
                if (Regex.IsMatch(xx, @"^\w{1,8}$"))
                {
                    tempstr = xx;
                    label.text = tempstr;
                }
                else
                {
                    return "输入格式8.0";
                }
                break;
        }

        return "ok";
    }

    protected string OutRegularText(string inputstr)
    {
        string outstr = string.Empty;
        switch (Checktype)
        {
            case "4.2":
                if (Regex.IsMatch(inputstr, @"^-?\d{1,4}\.$"))
                {
                    outstr = inputstr + "00";
                }
                else if (inputstr == "")
                {
                    outstr = "0.00";
                }
                else
                {
                    outstr = Convert.ToSingle(inputstr).ToString("0.00");
                }
                break;
            default:
                outstr = inputstr;
                break;
        }
        return outstr;
    }

}

//sprite控制显示的text,可以按确认
public class MyText2 : MyText
{
    private string tSprite;
    private string fSprite;
    private UISprite myTextBg;
    public MyText2(string label_name,string bg_name, string check_type,int next_win,  string t_s = "yellowbg", string f_s = "whitebg")
    {
        label = GameObject.Find(label_name).GetComponent<UILabel>();
        myTextBg = GameObject.Find(bg_name).GetComponent<UISprite>();
        tSprite = t_s;
        fSprite = f_s;
        tempstr = string.Empty;
        Checktype = check_type;
        originalstr = label.text;
        WinNext = next_win;
        max = 0;
    }

    public override int isChosen()
    {
        myTextBg.spriteName = tSprite;
        selected = true;
        return 1;
    }

    public override int noChosen()
    {
        myTextBg.spriteName = fSprite;
        selected = false;
        return 1;
    }
    public override int onChooseBt()
    {
        return WinNext;
    }

}

public class MyWid : Element
{
    public UIWidget mywid;
    public MyWid(string bt_s_name, int getpoint = 1)
    {
        mywid = GameObject.Find(bt_s_name).GetComponent<UIWidget>();
        WinNext = getpoint;
        selected = false;
    }
    public override int isChosen()
    {
        mywid.alpha = 1;
        selected = true;
        return 1;
    }

    public override int noChosen()
    {
        mywid.alpha = 0;
        selected = false;
        return 1;
    }

    public override int onChooseBt()
    {
        return WinNext;
    }


    public override int onInputBt()
    {
        return 1;
    }

    public override int on_LeftBt()
    {
        return 1;
    }
    public override int onGet()
    {
        return WinNext;
    }
}

public abstract class Part
{
    public int partNum;//当前位于part的第几个button
    public bool selected;
    public int memberCount;
    public List<Element> partMembers;

    public virtual int isChosen()
    {
        return 1;
    }

    public virtual int noChosen()
    {
        return 1;
    }

    public virtual int onChooseBt()
    {
        return 1;
    }

    public virtual string onNumBt(string number)
    {
        return "ok";
    }

    public virtual int onInputBt()
    {
        return 1;
    }

    public virtual int on_LeftBt()
    {
        return 1;
    }

    public virtual int onUpBt()
    {
        return 1;
    }

    public virtual int onLeftBt()
    {
        return 1;
    }
    public virtual int onDownBt()
    {
        return 1;
    }

    public virtual int onRightBt()
    {
        return 1;
    }

    //互斥按钮
    public virtual int doMutexBt()
    {
        return 1;
    }

    public virtual int InitalPart()
    {
        partNum = 0;
        doMutexBt();
        return 1;
    }

    public virtual int onCancel()
    {
        return 1;
    }

    public virtual int onSwitch()
    {
        return 1;
    }

    public virtual bool IsReadyToTab()
    {
        return true;
    }
    public virtual int onGet()
    {
        return partMembers[partNum].onGet();
    }
}

//一级菜单栏
public class ButtonPart : Part
{
    public ButtonPart(List<Element> members)
    {
        partMembers = members;
        memberCount = partMembers.Count;
        partNum = 0;
    }
    public override int isChosen()
    {
        selected = true;
        doMutexBt();
        return 1;
        
    }

    public override int noChosen()
    {
        selected = false;
        return partMembers[partNum].noChosen();
    }

    public override int onChooseBt()
    {
        return partMembers[partNum].onChooseBt();
    }

    public override string onNumBt(string number)
    {
        return partMembers[partNum].onNumBt(number);
    }

    public override int onInputBt()
    {
        return partMembers[partNum].onInputBt();
    }

    public override int on_LeftBt()
    {
        return partMembers[partNum].on_LeftBt();
    }

    public override int onUpBt()
    {
        if (--partNum < 0)
        {
            partNum = memberCount - 1;
        }
        doMutexBt();
        return 1;
    }

    public override int onLeftBt()
    {
        if (--partNum < 0)
        {
            partNum = memberCount - 1;
        }
        doMutexBt();
        return 1;
    }
    public override int onDownBt()
    {
        if (++partNum > memberCount - 1)
        {
            partNum = 0;
        }
        doMutexBt();
        return 1;
    }

    public override int onRightBt()
    {
        if (++partNum > memberCount - 1)
        {
            partNum = 0;
        }
        doMutexBt();
        return 1;
    }

    public override int doMutexBt()
    {
        foreach (Element element in partMembers)
        {
            element.noChosen();
        }
        partMembers[partNum].isChosen();
        return 1;
    }
   
}

//标签列表
public class LabelPart : Part
{
    private int labelNo;
    public LabelPart(List<Element> members,int label_No)
    {
        partMembers = members;
        memberCount = partMembers.Count;
        labelNo = label_No;
        partNum = 0;
    }
    public override int isChosen()
    {
        selected = true;
        doMutexBt();
        return 1;

    }

    public override int noChosen()
    {
        selected = false;
        return partMembers[partNum].noChosen();
    }

   
    public override int onUpBt()
    {
        if (--partNum < 0)
        {
            partNum = memberCount - 1;
        }
        doMutexBt();
        return labelNo;
    }

    public override int onLeftBt()
    {
        return 1;
    }
    public override int onDownBt()
    {
        if (++partNum > memberCount - 1)
        {
            partNum = 0;
        }
        doMutexBt();
        return labelNo;
    }

    public override int onRightBt()
    {
        return 1;
    }

    public override int doMutexBt()
    {
        foreach (Element element in partMembers)
        {
            element.noChosen();
        }
        partMembers[partNum].isChosen();
        return 1;
    }

}

//软键盘专用
public class ButtonMulti : Part
{
    private int row;
    private int column;
    public ButtonMulti(List<Element> members,int r,int c)
    {
        partMembers = members;
        memberCount = partMembers.Count;
        partNum = 0;
        row = r;
        column = c;
    }
    public override int isChosen()
    {
        selected = true;
        doMutexBt();
        return 1;

    }

    public override int noChosen()
    {
        selected = false;
        return partMembers[partNum].noChosen();
    }

    public override int onChooseBt()
    {
        return partMembers[partNum].onChooseBt();
    }

    public override string onNumBt(string number)
    {
        return "214";
    }

    public override int onInputBt()
    {
        return 214;
    }

    public override int on_LeftBt()
    {
        return 214;
    }

    public override int onUpBt()
    {
        partNum -= column;
        if (partNum < 0)
        {
            partNum = memberCount + partNum;
        }
        doMutexBt();
        return 1;
    }

    public override int onLeftBt()
    {
        if (--partNum < 0)
        {
            partNum = memberCount - 1;
        }
        doMutexBt();
        return 1;
    }
    public override int onDownBt()
    {
        partNum += column;
        if (partNum > memberCount - 1)
        {
            partNum = -memberCount + partNum;
        }
        doMutexBt();
        return 1;
    }

    public override int onRightBt()
    {
        if (++partNum > memberCount - 1)
        {
            partNum = 0;
        }
        doMutexBt();
        return 1;
    }

    public override int doMutexBt()
    {
        foreach (Element element in partMembers)
        {
            element.noChosen();
        }
        partMembers[partNum].isChosen();
        return 1;
    }

}

public class OneButtonPart : Part
{
    private Element one;
    private int returnNum;
    public OneButtonPart(Element member,int return_num,bool select=false)
    {
        one = member;
        returnNum = return_num;
        selected = select;
        if(selected)
        {
            isChosen();
        }else{
            noChosen();
        }
    }
    public override int isChosen()
    {
        selected = true;
        one.isChosen();
        return 1;

    }

    public override int noChosen()
    {
        selected = false;
        return one.noChosen();
    }

    public override int onUpBt()
    {
        return returnNum;
    }

    public override int onLeftBt()
    {
        return returnNum;
    }
    public override int onDownBt()
    {
        return returnNum;
    }

    public override int onRightBt()
    {
        return returnNum;
    }

}



//二级菜单栏,暂时和ButtonPart没有什么区别
public class ButtonPartSecond : ButtonPart
{
    public ButtonPartSecond(List<Element> members)
        : base(members)
    {

    }

    public override int onLeftBt()
    {
        noChosen();
        partNum = 0;
        return 1;
    }

    public override int onRightBt()
    {
        noChosen();
        partNum = 0;
        return 1;
    }

}

//程序列表
public class ScrollPart : Part
{
    private int listcount;//列表内容的数目
    public int firstline;//列表的第一行序号
    private UIScrollBar scrollbar;
    private string activesprite;

    //特有的函数部分
    public int SetlistCount
    {
        set
        {
            listcount = value;
            scrollbar.barSize = getBarsize();
            scrollbar.value = getBarvalue();
        }
    }

    private float getBarsize()
    {
        if (memberCount - listcount < 0)
        {
            return (float)memberCount / listcount;
        }
        else
        {
            return 1;
        }
    }

    private float getBarvalue()
    {
        if (listcount - memberCount > 0)
        {
            return ((float)firstline / (listcount - memberCount));
        }
        else
        {
            return 1;
        }
    }

    //public 

    //公共部分
    public ScrollPart(List<Element> members, int count, string barname,string activeS="yellowbg",bool select = false)
    {
        partNum = 0;
        firstline = 0;
        partMembers = members;
        memberCount = partMembers.Count;
        listcount = count;
        activesprite = activeS;
        selected = select;
        if (selected)
        {
            isChosen();
        }
        else
        {
            noChosen();
        }
        //进度条设置
        scrollbar = GameObject.Find(barname).GetComponent<UIScrollBar>();
        scrollbar.barSize = getBarsize();
        scrollbar.value = getBarvalue();
    }

    public override int isChosen()
    {
        selected = true;
        doMutexBt();
        return 1;

    }

    public override int noChosen()
    {
        selected = false;
        return partMembers[partNum].isChosen();
    }

    public override int onChooseBt()
    {
        return partMembers[partNum].onChooseBt();
    }

    public override string onNumBt(string number)
    {
        return partMembers[partNum].onNumBt(number);
    }

    public override int onInputBt()
    {
        return partMembers[partNum].onInputBt();
    }

    public override int on_LeftBt()
    {
        return partMembers[partNum].on_LeftBt();
    }

    public override int onUpBt()
    {
        if (listcount > memberCount)
        {
            if (--partNum < 0)
            {
                partNum ++;
                if (--firstline < 0)
                {
                    partNum = memberCount - 1;
                    firstline = listcount - memberCount;
                }
                scrollbar.value = getBarvalue();
                doMutexBt();
                return 520;//更新列表
            }
        }
        else
        {
            if (--partNum < 0)
            {
                partNum = listcount-1;
            }
        }
        scrollbar.value = getBarvalue();
        doMutexBt();
        return 1;
    }

    public override int onLeftBt()
    {
        return 1;
    }
    public override int onDownBt()
    {
        if (listcount > memberCount)
        {
            if (++partNum > memberCount - 1)
            {
                partNum --;
                if (++firstline > listcount - memberCount)
                {
                    firstline = 0;
                    partNum = 0;
                }
                scrollbar.value = getBarvalue();
                doMutexBt();
                return 520;//更新列表
            }
        }
        else
        {
            if (++partNum > listcount - 1)
            {
                partNum = 0;
            }
        }
        scrollbar.value = getBarvalue();
        doMutexBt();
        return 1;
    }

    public override int onRightBt()
    {
        return 1;
    }

    public override int doMutexBt()
    {
        foreach (Element element in partMembers)
        {
            element.noChosen();
        }
        partMembers[partNum].isChosen();
        partMembers[partNum].SetSpriteBg(activesprite);
        return 1;
    }    
}

//选项part

public class OptionPart : Part
{
    List<Element> partMembers2;
    public OptionPart(List<Element> member1,List<Element> member2)
    {
        memberCount = member1.Count;
        partNum = 0;
        partMembers = member1;
        partMembers2 = member2;
    }

    public override int isChosen()
    {
        selected = true;
        doMutexBt();
        return 1;

    }

    public override int noChosen()
    {
        selected = false;
        partMembers2[partNum].noChosen();
        partMembers[partNum].isChosen();
        return 1;
    }

    public override int onChooseBt()
    {
        return partMembers[partNum].onChooseBt();
    }

    public override string onNumBt(string number)
    {
        return partMembers[partNum].onNumBt(number);
    }

    public override int onInputBt()
    {
        return partMembers[partNum].onInputBt();
    }

    public override int on_LeftBt()
    {
        return partMembers[partNum].on_LeftBt();
    }

    public override int onUpBt()
    {
        if (--partNum < 0)
        {
            partNum = memberCount - 1;
        }
        doMutexBt();
        return 1;
    }

    public override int onLeftBt()
    {
        if (--partNum < 0)
        {
            partNum = memberCount - 1;
        }
        doMutexBt();
        return 1;
    }
    public override int onDownBt()
    {
        if (++partNum > memberCount - 1)
        {
            partNum = 0;
        }
        doMutexBt();
        return 1;
    }

    public override int onRightBt()
    {
        if (++partNum > memberCount - 1)
        {
            partNum = 0;
        }
        doMutexBt();
        return 1;
    }

    public override int doMutexBt()
    {
        foreach (Element element in partMembers)
        {
            element.noChosen();
        }
        foreach (Element element in partMembers2)
        {
            element.noChosen();
        }
        partMembers[partNum].isChosen();
        partMembers2[partNum].isChosen();
        return 1;
    }

}

public class TextPart : Part
{
    public TextPart(List<Element> members)
    {
        partMembers = members;
        memberCount = partMembers.Count;
        partNum = 0;
    }
    public override int isChosen()
    {
        selected = true;
        doMutexBt();
        return 1;

    }
    public override int noChosen()
    {
        selected = false;
        return partMembers[partNum].noChosen();
    }

    public override int onChooseBt()
    {
        return partMembers[partNum].onChooseBt();
    }
    public override string onNumBt(string number)
    {
        return partMembers[partNum].onNumBt(number);
    }

    public override int onInputBt()
    {
        return partMembers[partNum].onInputBt();
    }

    public override int on_LeftBt()
    {
        return partMembers[partNum].on_LeftBt();
    }

    public override int onUpBt()
    {
        if (partMembers[partNum].IsReadyToChange())
        {
            if (--partNum < 0)
            {
                partNum = memberCount - 1;
            }
            doMutexBt();
            return 1;
        }
        return -1;//请按输入键进行确认
    }

    public override int onLeftBt()
    {
        if (partMembers[partNum].IsReadyToChange())
        {
            if (--partNum < 0)
            {
                partNum = memberCount - 1;
            }
            doMutexBt();
            return 1;
        }
        return -1;//请按输入键进行确认
    }
    public override int onDownBt()
    {
        if (partMembers[partNum].IsReadyToChange())
        {
            if (++partNum > memberCount - 1)
            {
                partNum = 0;
            }
            doMutexBt();
            return 1;
        }
        return -1;//请按输入键进行确认
    }

    public override int onRightBt()
    {
        if (partMembers[partNum].IsReadyToChange())
        {
            if (++partNum > memberCount - 1)
            {
                partNum = 0;
            }
            doMutexBt();
            return 1;
        }
        return -1;//请按输入键进行确认
    }

    public override int doMutexBt()
    {
        foreach (Element element in partMembers)
        {
            element.noChosen();
        }
        partMembers[partNum].isChosen();
        return 1;
    }
    public override bool IsReadyToTab()
    {
        return partMembers[partNum].IsReadyToChange();
    }
}

public abstract class Win
{
    public int winNum;//当前位于window的第几个part
    public bool selected;
    public int memberCount;
    public List<Part> winMembers;
    public int lastWin;
    public string nowname;

    public virtual int isChosen()
    {
        return 1;
    }

    public virtual int noChosen()
    {
        return 1;
    }

    public virtual int onChooseBt()
    {
        return 1;
    }

    public virtual string onNumBt(string number)
    {
        return "ok";
    }
      
    public virtual int onInputBt()
    {
        return 1;
    }

    public virtual int on_LeftBt()
    {
        return 1;
    }

    public virtual int onUpBt()
    {
        return 1;
    }

    public virtual int onLeftBt()
    {
        return 1;
    }
    public virtual int onDownBt()
    {
        return 1;
    }

    public virtual int onRightBt()
    {
        return 1;
    }

    public virtual int doMutexPart()
    {
        return 1;
    }

    public virtual int onTabBt()
    {
        return 1;
    }

    public virtual int onFlipBt()
    {
        return 1;
    }

    public virtual int onSwitch()
    {
        return 1;
    }

    public virtual int onCancelBt()
    {
        return 1;
    }
    public virtual int onGet()
    {
        return winMembers[winNum].onGet();
    }
    public virtual int closeWin()
    {
        return 1;
    }

    public virtual int showWin()
    {
        return 1;
    }

    public virtual int InitialWin()
    {
        return 1;
    }
}

public class MyWindow : Win
{
    public MyWindow()
    {

    }
    public MyWindow(List<Part> members, string now_win_name, int last_w_No)
    {
        winMembers = members;
        winNum = 0;
        memberCount = winMembers.Count;
        lastWin = last_w_No;
        nowname = now_win_name;
        noChosen();
    }

    public override int isChosen()
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
        return winMembers[winNum].onChooseBt();
    }

    public override string onNumBt(string number)
    {
        return winMembers[winNum].onNumBt(number);
    }

    public override int onInputBt()
    {
        return winMembers[winNum].onInputBt();
    }

    public override int on_LeftBt()
    {
        return winMembers[winNum].on_LeftBt();
    }

    public override int onUpBt()
    {
        return winMembers[winNum].onUpBt();
    }

    public override int onLeftBt()
    {
        return winMembers[winNum].onLeftBt();
    }
    public override int onDownBt()
    {
        return winMembers[winNum].onDownBt();
    }

    public override int onRightBt()
    {
        return winMembers[winNum].onRightBt();
    }

    public override int doMutexPart()
    {
        foreach (Part parts in winMembers)
        {
            parts.noChosen();
        }
        winMembers[winNum].isChosen();
        return 1;
    }

    public override int onTabBt()
    {
        if (winMembers[winNum].IsReadyToTab())
        {
            if (++winNum > memberCount - 1)
            {
                winNum = 0;
            }
            doMutexPart();
            return 1;
        }
        return -1;//请按输入键进行确认
    }

    public override int onFlipBt()
    {
        return 1;
    }

    public override int onCancelBt()
    {
        return 1;
    }

    public override int closeWin()
    {
        return 1;
    }

    public override int showWin()
    {
        return 1;
    }

    public override int InitialWin()
    {
        winNum = 0;
        foreach (Part parts in winMembers)
        {
            parts.noChosen();
            parts.InitalPart();
        }
        winMembers[winNum].InitalPart();
        noChosen();
        return 1;
    }
}

//通过depth来控制的window窗口
public class Window_depth : MyWindow
{
    public Window_depth(List<Part> members, string now_win_name, int last_w_No)
        : base(members, now_win_name, last_w_No)
    {

    }

    public override int onCancelBt()
    {
        closeWin();
        return lastWin;
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

}

public class Window_depth_no : Win
{
    public Window_depth_no( string now_win_name, int last_w_No)
    {
        lastWin = last_w_No;
        nowname = now_win_name;
        noChosen();
    }

    public override int isChosen()
    {
        selected = true;
        showWin();
        return 1;
    }

    public override int noChosen()
    {
        selected = false;
        closeWin();
        return 1;
    }

    public override int onCancelBt()
    {
        closeWin();
        return lastWin;
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

}

//通过alpha值来控制显示的window窗口
public class Window_a : MyWindow
{
    private GameObject objectWin;
    public Window_a(List<Part> members, string now_win_name, int last_w_No)
        : base(members, now_win_name, last_w_No)
    {

    }

    public override int onCancelBt()
    {
        closeWin();
        return lastWin;
    }

    public override int closeWin()
    {
        winNum = 0;
        GameObject.Find(nowname).GetComponent<UIWidget>().alpha = 0;
        return 1;
    }

    public override int showWin()
    {
        winNum = 0;
        GameObject.Find(nowname).GetComponent<UIWidget>().alpha = 1;
        return 1;
    }
}


//左右键取消
public class Window_alpha : MyWindow
{
    public Window_alpha(List<Part> members, string now_win_name, int last_w_No)
        : base(members, now_win_name, last_w_No)
    {

    }

    public override int onLeftBt()
    {
        return onCancelBt();
    }

    public override int onRightBt()
    {
        return onCancelBt();
    }

    public override int onCancelBt()
    {
        closeWin();
        return lastWin;
    }

    public override int closeWin()
    {
        winNum = 0;
        GameObject.Find(nowname).GetComponent<UIWidget>().alpha = 0;
        return 1;
    }

    public override int showWin()
    {
        winNum = 0;
        GameObject.Find(nowname).GetComponent<UIWidget>().alpha = 1;
        return 1;
    }
}



