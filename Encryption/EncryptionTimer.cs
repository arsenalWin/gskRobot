using UnityEngine;
using System.IO;
using System.Data;
using System.Net;
using System;
using Microsoft.Win32;

/*
 * 试用期
 */

public class EncryptionTimer
{

    //试用期时间
    int _probationDays;

	////是否在试用期内
	//public static bool IsInProbation;

    //文件存储路径
    string _directoryPath = "";
    string _filePath = "";

    JsonOperator jo = new JsonOperator();

    //文件加密
    AESEncryption tmpAESEncryption = new AESEncryption();

    //本次启动开始时间
    DateTime _startDateTime;

    //退出标志
    bool _isQuiting;

	bool _isDebug = false;

	/// <summary>
	/// 开启试用期解密
	/// </summary>
    public bool Start ()
    {
		if (_isDebug)
			Debug.Log("EncryptionTimer:Start");

        //_filePath = "C:\\WINDOWS\\system\\encryption.json";
        //Hcetynnus
        _directoryPath = "C:\\Documents and Settings\\All Users\\Application Data\\Hcetynnus";
        //_filePath = "Encryption.json";
        _filePath = "Noitpyrcne.json";

        //试用期
        _probationDays = 30;

        _isQuiting = false;

        //启动时间
        _startDateTime = DateTime.Now;

        //进入检测
        return Check();
    }

    /// <summary>
    /// 检测试用期
    /// </summary>
    private bool Check ()
    {
        bool IsInProbation = false;

        //创建文件夹
        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
        }

        //软件名
        string tmpSoftwareName = "";
        tmpSoftwareName = JiamiString.SoftwareName;

        DataTable dt = new DataTable();

        //如果不存在加密存储文件,并且注册表中无此信息：第一次使用
        if (!File.Exists(_directoryPath + "\\" + _filePath) && !EncryptionRegister.IsRegedited(tmpSoftwareName))
        {
            //写入文件
            WriteInfo("", "");

            //写入注册表
            EncryptionRegister.CreateRegedit(tmpSoftwareName, "", "");

            //试用期内
			IsInProbation = true;

			if (_isDebug)
			Debug.Log("1");
        }
        //如果不存在加密存储文件,但是注册表中有此信息：将注册表中信息写入加密文件，并且通过注册表来判断是否成功
        else if (!File.Exists(_directoryPath + "\\" + _filePath) && EncryptionRegister.IsRegedited(tmpSoftwareName))
        {
            //写入文件
            WriteInfo(EncryptionRegister.ReadRegedit(tmpSoftwareName, 1).ToString(), EncryptionRegister.ReadRegedit(tmpSoftwareName, 2).ToString());

            //判断试用期
			IsInProbation = EncryptionRegister.Flag(tmpSoftwareName, DateTime.Now, _probationDays, 0);

			if (_isDebug)
				Debug.Log("2");
        }
        //如果存在加密文件
        else if (File.Exists(_directoryPath + "\\" + _filePath))
        {
            dt = tmpAESEncryption.FileContentToDataTable(_directoryPath + "\\" + _filePath, tmpSoftwareName);
            //如果存在加密存储文件但是不包含当前一软件，并且注册表中没有此信息：第一次使用
            if (dt == null && !EncryptionRegister.IsRegedited(tmpSoftwareName))
            {
                //写入文件
                WriteInfo("", "");

                //写入注册表
                EncryptionRegister.CreateRegedit(tmpSoftwareName, "", "");

                //试用期内
				IsInProbation = true;

				if (_isDebug)
					Debug.Log("3");
            }
            //如果存在加密存储文件但是不包含当前软件，并且注册表中有此信息
            else if (dt == null && EncryptionRegister.IsRegedited(tmpSoftwareName))
            {
                //写入文件
                WriteInfo(EncryptionRegister.ReadRegedit(tmpSoftwareName, 1).ToString(), EncryptionRegister.ReadRegedit(tmpSoftwareName, 2).ToString());

                //判断试用期
				IsInProbation = EncryptionRegister.Flag(tmpSoftwareName, DateTime.Now, _probationDays, 0);

				if (_isDebug)
					Debug.Log("4");
            }
            //如果存在加密存储文件且包含当前一软件，并且注册表中没有此信息
            else if (dt != null && !EncryptionRegister.IsRegedited(tmpSoftwareName))
            {
                //第一次启动时间
                string _startTime;
                //最后一次启动时间
                string _lastTime;
                //记录的时间
                _startTime = dt.Rows[0][2].ToString();
                _lastTime = dt.Rows[0][3].ToString();

                //写入注册表
                EncryptionRegister.CreateRegedit(tmpSoftwareName, _startTime, _lastTime);

                //判断试用期
				IsInProbation = CheckProbation(_startTime, _lastTime, _probationDays);

				if (_isDebug)
					Debug.Log("5");
            }
            //如果存在加密存储文件且包含当前软件,并且注册表中有此信息
            else if (dt != null && EncryptionRegister.IsRegedited(tmpSoftwareName))
            {
                //第一次启动时间
                string _startTime;
                //最后一次启动时间
                string _lastTime;
                //记录的时间
                _startTime = dt.Rows[0][2].ToString();
                _lastTime = dt.Rows[0][3].ToString();

                int tmpUsedSeconds = 0;
                //软件结束
                if (_isQuiting)
                {
                    //本次使用时间
                    TimeSpan tmpSpan = DateTime.Now.Subtract(_startDateTime);
                    tmpUsedSeconds = getSeconds(tmpSpan);
                }

                //检测文件信息和注册表信息
				IsInProbation = (CheckProbation(_startTime, _lastTime, _probationDays) && EncryptionRegister.Flag(tmpSoftwareName, DateTime.Now, _probationDays, tmpUsedSeconds));

				if (_isDebug)
					Debug.Log("6");
            }
        }

        //如果在试用期内,更新文件里面的内容
		if (IsInProbation && (dt != null && dt.Rows.Count != 0))
        {
            string[] column_name = new string[] { "MachineName", "SoftwareName", "StartTime", "LastTime", "Times" };
            string[,] insert_content;
            insert_content = new string[5, 1];

            //机器名
            string tmpMachineName = "";
            tmpMachineName = Dns.GetHostName();
            insert_content[0, 0] = tmpMachineName;

            //软件名
            insert_content[1, 0] = tmpSoftwareName;

            //软件第一次运行时间
            string tmpStartTime = "";
            tmpStartTime = dt.Rows[0][2].ToString();
            insert_content[2, 0] = tmpStartTime;

            //软件最后一次启动时间
            string tmpLastTime = "";
            tmpLastTime = DateTime.Now.ToString();
            insert_content[3, 0] = tmpLastTime;

            //启动次数
            string tmpTimes = dt.Rows[0][4].ToString();
            int tmpTimes2 = int.Parse(tmpTimes) + 1;
            insert_content[4, 0] = tmpTimes2.ToString();

            /*
             * 文件加密
             * 1、加密文件还原成正常文件
             * 2、写入正常文件
             * 3、正常文件加密
             */
            //加密文件还原成正常文件
            tmpAESEncryption.FileRestore(_directoryPath + "\\" + _filePath);
            //写入正常文件
            jo.JsonWriter(_directoryPath + "\\" + _filePath, tmpSoftwareName, column_name, insert_content, false);
            //正常文件加密
            string tmpContent = tmpAESEncryption.FileReader(_directoryPath + "\\" + _filePath);
            File.Delete(_directoryPath + "\\" + _filePath);
            tmpAESEncryption.FileWriter(_directoryPath + "\\" + _filePath, tmpContent);
        }

        return IsInProbation;
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    void WriteInfo (string str1, string str2)
    {
        string[] column_name = new string[] { "MachineName", "SoftwareName", "StartTime", "LastTime", "Times" };
        string[,] insert_content;
        insert_content = new string[5, 1];

        //机器名
        string tmpMachineName = "";
        tmpMachineName = Dns.GetHostName();
        insert_content[0, 0] = tmpMachineName;

        //软件名
        string tmpSoftwareName = "";
        //tmpSoftwareName = Application.loadedLevelName;
        tmpSoftwareName = JiamiString.SoftwareName;
        insert_content[1, 0] = tmpSoftwareName;

        //软件第一次运行时间
        string tmpStartTime = "";
        tmpStartTime = DateTime.Now.ToString();
        insert_content[2, 0] = tmpStartTime;
        if (str1 != "")
        {
            insert_content[2, 0] = str1;
        }

        //软件最后一次启动时间
        string tmpLastTime = "";
        tmpLastTime = DateTime.Now.ToString();
        insert_content[3, 0] = tmpLastTime;
        if (str2 != "")
        {
            insert_content[3, 0] = str2;
        }

        //启动次数
        string tmpTimes = "";
        tmpTimes = "1";
        insert_content[4, 0] = tmpTimes;

        //jo.JsonWriter(_directoryPath + "\\" + _filePath, tmpSoftwareName, column_name, insert_content, false);
        //文件存在
        if (File.Exists(_directoryPath + "\\" + _filePath))
        {
            //加密文件还原成正常文件
            tmpAESEncryption.FileRestore(_directoryPath + "\\" + _filePath);
        }

        //写入正常文件
        jo.JsonWriter(_directoryPath + "\\" + _filePath, tmpSoftwareName, column_name, insert_content, false);

        //正常文件加密
		string tmpContent = tmpAESEncryption.FileReader(_directoryPath + "\\" + _filePath);
		File.Delete(_directoryPath + "\\" + _filePath);
		tmpAESEncryption.FileWriter(_directoryPath + "\\" + _filePath, tmpContent);
    }

    /// <summary>
    /// 验证软件的试用期
    /// </summary>
    /// <returns></returns>
    bool CheckProbation (string tmpStartTime, string tmpLastTime, int tmpProbationDays)
    {
		//Debug.Log("CheckProbation:" + tmpStartTime + "," + tmpLastTime);
        DateTime dt1 = DateTime.Parse(tmpStartTime, System.Globalization.CultureInfo.InvariantCulture);
        DateTime dt2 = DateTime.Parse(tmpLastTime, System.Globalization.CultureInfo.InvariantCulture);

        //当前时间
        DateTime currentTime = DateTime.Now;

        TimeSpan span = currentTime.Subtract(dt2);
        //Debug.Log("离上次启动的时间是：" + span.Days + ":" + span.Minutes + ":" + span.Seconds + ",秒数：" + getSeconds(span));
        //上次启动时间 在当前时间之前，则修改过系统时间
        if (getSeconds(span) < 0)
        {
            //Debug.Log("系统时间被修改，无法使用试用版");
            return false;
        }

        TimeSpan span2 = currentTime.Subtract(dt1);
        //Debug.Log("离第一次启动的时间是：" + span2.Days + ":" + span2.Minutes + ":" + span2.Seconds + ",秒数：" + getSeconds(span2));
        if (getSeconds(span2) <= ( tmpProbationDays * 3600 ) && getSeconds(span2) >= 0)
        {
            //Debug.Log("剩余试用期天数：" + ( tmpProbationDays - span2.Days ));
            return true;
        }

        return false;
    }

    //获得时间差的秒差
    public static int getSeconds (TimeSpan tmpSpan)
    {
        int result = 0;

        result += ( tmpSpan.Days * 3600 );

        result += ( tmpSpan.Minutes * 60 );

        result += ( tmpSpan.Seconds * 1 );

        return result;
    }

    //TODO:程序退出前，记录时间
    public void BeforeQuit ()
    {
        //程序退出前处理,记录计时
        _isQuiting = true;
        Check();
    }
}


public class EncryptionRegister
{

    /// <summary>
    /// 读取注册表
    /// </summary>
    /// <param name="tmpStr">软件名</param>
    /// <param name="i">索引</param>
    /// <returns></returns>
    public static DateTime ReadRegedit (string tmpStr, int i)
    {
        DateTime recordDateTime = DateTime.Now;

        //获取注册表里面的时间信息
        try
        {
            RegistryKey Huser = Registry.CurrentUser;
            RegistryKey zcb = Huser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\Service\Number_" + tmpStr, true);

            //获取当前软件第一次使用的时间
            if (i == 1)
            {
				recordDateTime = DateTime.Parse(zcb.GetValue("ShellValue").ToString());
            }
            //获取当前软件最后一次使用的时间
            else if (i == 2)
            {
				recordDateTime = DateTime.Parse(zcb.GetValue("ShellValue3").ToString());
            }
        }
        catch
        {
            CreateRegedit(tmpStr, "", "");
        }

        return recordDateTime;
    }

    /// <summary>
    /// 检测是否写入注册表
    /// </summary>
    /// <param name="tmpStr"></param>
    /// <returns></returns>
    public static bool IsRegedited (string tmpStr)
    {
        //获取注册表里面的软件使用秒数
        try
        {
            RegistryKey Huser = Registry.CurrentUser;
            RegistryKey zcb = Huser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\Service\Number_" + tmpStr, true);

			//Debug.Log("zcb:" + zcb);
            if (zcb != null)
            {
                return true;
            }
        }
        catch
        {
            Debug.Log("注册表");
        }

        return false;
    }

    //判断试用期
    public static bool Flag (string tmpStr, DateTime tmpCurrentTime, int tmpProbationDays, int tmpSeconds)
    {

        //软件结束的时间，需要记录的使用时间,不需要验证
        if (tmpSeconds != 0)
        {
            //获取注册表里面的软件使用秒数
            try
            {
                RegistryKey Huser = Registry.CurrentUser;
                RegistryKey zcb = Huser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\Service\Number_" + tmpStr, true);

                //获取当前软件第一次使用的时间
                int recordSeconds = int.Parse(zcb.GetValue("ShellValue2").ToString());
                recordSeconds += tmpSeconds;
                zcb.SetValue("ShellValue2", recordSeconds.ToString());

                //最后一次使用时间
                zcb.SetValue("ShellValue3", tmpCurrentTime.ToString());
            }
            catch
            {
                Debug.Log("注册表累加时间出错");
            }

            return true;
        }
        //软件开始的时间，需要验证试用期
        else
        {
            //记录第一次使用时间
            DateTime recordDateTime = ReadRegedit(tmpStr, 1);
            //记录最后一次使用时间
            DateTime recordDateTime2 = ReadRegedit(tmpStr, 2);

            TimeSpan span2 = tmpCurrentTime.Subtract(recordDateTime2);
            //Debug.Log("离上次启动的时间是：" + span.Days + ":" + span.Minutes + ":" + span.Seconds + ",秒数：" + getSeconds(span));
            //上次启动时间 在当前时间之前，则修改过系统时间
            if (EncryptionTimer.getSeconds(span2) < 0)
            {
                //Debug.Log("系统时间被修改，无法使用试用版");
                return false;
            }

            //当前时间和记录第一次使用时间的时间差
            TimeSpan span = tmpCurrentTime.Subtract(recordDateTime);
            //当前时间和记录时间比较
            if (EncryptionTimer.getSeconds(span) >= 0 && EncryptionTimer.getSeconds(span) <= ( tmpProbationDays * 3600 ))
            {
                return true;
            }

            return false;
        }
    }

    //创建注册表
    public static void CreateRegedit (string tmpStr, string dt1, string dt2)
    {
        RegistryKey tmpRegistryKey = Registry.CurrentUser;
        RegistryKey ssub = tmpRegistryKey.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\Service\Number_" + tmpStr);

        //记录当前软件第一次使用的时间
        ssub.SetValue("ShellValue", DateTime.Now.ToString());
        if (dt1 != "")
        {
            ssub.SetValue("ShellValue", dt1);
        }
        //累计数使用时间
        ssub.SetValue("ShellValue2", "0");
        //最后一次使用时间
        ssub.SetValue("ShellValue3", DateTime.Now.ToString());
        if (dt2 != "")
        {
            ssub.SetValue("ShellValue3", dt2);
        }
    }
}