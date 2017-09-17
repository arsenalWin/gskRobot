//<summary>
//FileReadandWrite.cs
//ROBOT
//Created by 周伟 on 7/26/2015.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Text.RegularExpressions;

public class ROBOTFILE {

    private DirectoryInfo directory;
    private string filepath;
    private List<FileInfo> listFiles;

    public ROBOTFILE(string strDirectory)
    {
        filepath = strDirectory;
        directory = new DirectoryInfo(strDirectory);
    }

    /// <summary>
    /// 获取目录下文件信息(除去.meta)
    /// </summary>
    /// <returns></returns>
    public List<FileInfo> GetFileDirectory()
    {
        listFiles = new List<FileInfo>(); //保存所有的文件信息
        GSKDATA.FileListInfo = new List<string>();
        FileInfo[] fileInfoArray = directory.GetFiles();

        //对文件进行排序
        SortAsFileCreationTime(ref fileInfoArray);

        if (fileInfoArray.Length > 0)
        {
            for (int i = 0; i < fileInfoArray.Length; i++)
            {
                if (fileInfoArray[i].Name.IndexOf(".meta") == -1 && fileInfoArray[i].Name.IndexOf(".prl") != -1)
                {
                    listFiles.Add(fileInfoArray[i]);
                    GSKDATA.FileListInfo.Add(fileInfoArray[i].Name + "," + fileInfoArray[i].Length.ToString() + "," + fileInfoArray[i].CreationTime.ToString("yyyy-MM-dd"));
                }
            }
        }
        return listFiles;
    }
    
    //获取程序第N行的程序名
    public string GetFileName(int hanghao)
    {
        Debug.Log(hanghao);
        listFiles = new List<FileInfo>(); //保存所有的文件信息
        FileInfo[] fileInfoArray = directory.GetFiles();

        //对文件进行排序
        SortAsFileCreationTime(ref fileInfoArray);

        if (fileInfoArray.Length > 0)
        {
            for (int i = 0; i < fileInfoArray.Length; i++)
            {
                if (fileInfoArray[i].Name.IndexOf(".meta") == -1 && fileInfoArray[i].Name.IndexOf(".prl") != -1)
                {
                    listFiles.Add(fileInfoArray[i]);
                }
            }
        }
        return listFiles[hanghao].Name;
    }


    /// <summary>
    /// 获取文件的内容<固定文件名>
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public int GetCodeLine(string filename)
    {
        List<string> fileContents = new List<string>();
        string path = filepath + filename;
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        string temp = null;
        while ((temp = sr.ReadLine()) != null)
        {
            if (!Regex.IsMatch(temp, @"^P[0-9]{3}"))
            {
                if (Regex.IsMatch(temp, @"\bP\*\(.+\)\s,\b"))
                {
                    //Debug.Log("P*");
                    temp = Regex.Replace(temp, @"\bP\*\(.+\)\s,\b", "P* ,", RegexOptions.IgnoreCase);
                }
                if (temp != "")
                {
                    fileContents.Add(temp);
                }
            }
        }
        sr.Close();
        fs.Close();
        return fileContents.Count-2;
    }
    public List<string> GetFileContents(string filename)
    {
        List<string> fileContents = new List<string>();
        string path = filepath + filename;
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        string temp = null;
        while ((temp = sr.ReadLine()) != null)
        {
            if (!Regex.IsMatch(temp, @"^P[0-9]{3}"))
            {
                if (Regex.IsMatch(temp, @"\bP\*\(.+\)\s,\b"))
                {
                    //Debug.Log("P*");
                    temp = Regex.Replace(temp, @"\bP\*\(.+\)\s,\b", "P* ,", RegexOptions.IgnoreCase);
                }
                if (temp != "")
                {
                    fileContents.Add(temp);
                }
            }
        }
        sr.Close();
        fs.Close();
        return fileContents;
    }
    public List<string> GetFileAllContents(string filename)
    {
        List<string> fileContents = new List<string>();
        string path = filepath + filename;
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string temp = null;
        while ((temp = sr.ReadLine()) != null)
        {
            if (temp != "")
            {
                fileContents.Add(temp);
            }
        }
        sr.Close();
        fs.Close();
        return fileContents;
    }

    /// <summary>
    /// 获取示教点内容信息
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public List<string> GetPointContents(string filename)
    {
        List<string> pointContents = new List<string>();
        string path = filepath + filename;
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
   

    /// <summary>
    /// 新建一个名称为filename的文件。prl
    /// 并写入main；end；
    /// </summary>
    /// <param name="filename"></param>
    public bool CreateFile(string filename)
    {
        string path = filepath + filename;
        
        if (File.Exists(path))
        {
            return false;
        }
        else
        {
            //创建程序文件
            FileStream fs = new FileStream(path, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("MAIN;");
            sw.WriteLine("END;");
            sw.Close();
            fs.Close();
            return true;
        }

    }


    ///程序的复制+重新命名
    public string CopyProgram(string filename,string copyname)
    {
        string path1 = filepath + filename;
        string path2 = filepath + copyname;
        FileInfo fi1 = new FileInfo(path1);
        try
        {
            if (File.Exists(path2))
            {
                return "文件已存在";
            }
            else
            {
                fi1.CopyTo(path2);
                return "文件复制完成";
            }
        }
        catch
        {
            return "复制文件失败";
        }
    }
    ///程序的删除
    public string DeleteProgram(string filename)
    {
        string path = filepath + filename;
        if (File.Exists(path) && filename != "777777.prl")//777777文件不能被删除
        {
            FileInfo fi = new FileInfo(path);
            fi.Delete();
        }
        return "文件删除完成";
    } 
    ///程序查找
    // 
    public string SearchProgram(ref int row, ref int fline,int hangshu,ref string filename)
    {
        string path = filepath + filename;
        if (File.Exists(path))
        {
            //文件查找完成
            LocateCurrentname(ref row, ref fline, hangshu, ref filename);

            return "文件查找完成";
        }
        else
        {
            return "文件未找到";
        }
    }
    ///程序重命名
    public string RenameProgram(string filename,string rename)
    {
        string path1 = filepath + filename;
        string path2 = filepath + rename;
        FileInfo fi1 = new FileInfo(path1);
        try
        {
            if (File.Exists(path2))
            {
                return "当前文件已经存在";
            }
            else
            {
                fi1.CopyTo(path2);
                fi1.Delete();
                return "文件重命名完成";
            }
        }
        catch
        {
            return "文件重命名失败";
        }
    }
    /// <summary>
    /// C#按创建时间排序（顺序）
    /// </summary>
    /// <param name="arrFi">待排序数组</param>
    private void SortAsFileCreationTime(ref FileInfo[] arrFi)
    {
        Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
    }

    /// <summary>
    /// 定位文件名在列表中的位置
    /// </summary>
    /// <param name="row">行号从1开始？？</param>
    /// <param name="fline">首行从0开始</param>
    public bool LocateCurrentname(ref int row, ref int fline,int hangshu,ref string name)
    {
        //刷新获取列表文件
        FileInfo[] arrfi = GetFileDirectory().ToArray();
        int num = -1;
        if (File.Exists(filepath + name))
        {
            //在列表文件中查找
            for (int i = 0; i < arrfi.Length; i++)
            {
                if (arrfi[i].Name == name)
                {
                    num = i;
                    break;
                }
            }
            fline = num / hangshu;
            row = (num % hangshu) + 1;
            return true;
        }
        else
        {
            fline = 0;
            row = 1;
            name = "";
            return false;
        }
        
    }
    //获取程序大小
    public long GetMemotySize(List<FileInfo> fi)
    {
        long size = 0;
        for (int i = 0; i < fi.Count; i++)
        {
            size += fi[i].Length;
        }
        return size;
    }
    //写入程序
    public void writeContents(List<string> fcontents, string fileN)
    {
        string path = filepath + fileN;
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter Streamw = new StreamWriter(file);

        for (int i = 0; i < fcontents.Count; i++)
        {
            Streamw.WriteLine(fcontents[i]);
        }

        Streamw.Close();
        file.Close();
    }
    //文件是否存在
    public bool FileIsExist(string filename)
    {
        string path = filepath + filename;
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }
}
