//<summary>
//FileReadandWrite.cs
//ROBOT
//Created by ��ΰ on 7/26/2015.
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
    /// ��ȡĿ¼���ļ���Ϣ(��ȥ.meta)
    /// </summary>
    /// <returns></returns>
    public List<FileInfo> GetFileDirectory()
    {
        listFiles = new List<FileInfo>(); //�������е��ļ���Ϣ
        GSKDATA.FileListInfo = new List<string>();
        FileInfo[] fileInfoArray = directory.GetFiles();

        //���ļ���������
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
    
    //��ȡ�����N�еĳ�����
    public string GetFileName(int hanghao)
    {
        Debug.Log(hanghao);
        listFiles = new List<FileInfo>(); //�������е��ļ���Ϣ
        FileInfo[] fileInfoArray = directory.GetFiles();

        //���ļ���������
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
    /// ��ȡ�ļ�������<�̶��ļ���>
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
    /// ��ȡʾ�̵�������Ϣ
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
    /// �½�һ������Ϊfilename���ļ���prl
    /// ��д��main��end��
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
            //���������ļ�
            FileStream fs = new FileStream(path, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("MAIN;");
            sw.WriteLine("END;");
            sw.Close();
            fs.Close();
            return true;
        }

    }


    ///����ĸ���+��������
    public string CopyProgram(string filename,string copyname)
    {
        string path1 = filepath + filename;
        string path2 = filepath + copyname;
        FileInfo fi1 = new FileInfo(path1);
        try
        {
            if (File.Exists(path2))
            {
                return "�ļ��Ѵ���";
            }
            else
            {
                fi1.CopyTo(path2);
                return "�ļ��������";
            }
        }
        catch
        {
            return "�����ļ�ʧ��";
        }
    }
    ///�����ɾ��
    public string DeleteProgram(string filename)
    {
        string path = filepath + filename;
        if (File.Exists(path) && filename != "777777.prl")//777777�ļ����ܱ�ɾ��
        {
            FileInfo fi = new FileInfo(path);
            fi.Delete();
        }
        return "�ļ�ɾ�����";
    } 
    ///�������
    // 
    public string SearchProgram(ref int row, ref int fline,int hangshu,ref string filename)
    {
        string path = filepath + filename;
        if (File.Exists(path))
        {
            //�ļ��������
            LocateCurrentname(ref row, ref fline, hangshu, ref filename);

            return "�ļ��������";
        }
        else
        {
            return "�ļ�δ�ҵ�";
        }
    }
    ///����������
    public string RenameProgram(string filename,string rename)
    {
        string path1 = filepath + filename;
        string path2 = filepath + rename;
        FileInfo fi1 = new FileInfo(path1);
        try
        {
            if (File.Exists(path2))
            {
                return "��ǰ�ļ��Ѿ�����";
            }
            else
            {
                fi1.CopyTo(path2);
                fi1.Delete();
                return "�ļ����������";
            }
        }
        catch
        {
            return "�ļ�������ʧ��";
        }
    }
    /// <summary>
    /// C#������ʱ������˳��
    /// </summary>
    /// <param name="arrFi">����������</param>
    private void SortAsFileCreationTime(ref FileInfo[] arrFi)
    {
        Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
    }

    /// <summary>
    /// ��λ�ļ������б��е�λ��
    /// </summary>
    /// <param name="row">�кŴ�1��ʼ����</param>
    /// <param name="fline">���д�0��ʼ</param>
    public bool LocateCurrentname(ref int row, ref int fline,int hangshu,ref string name)
    {
        //ˢ�»�ȡ�б��ļ�
        FileInfo[] arrfi = GetFileDirectory().ToArray();
        int num = -1;
        if (File.Exists(filepath + name))
        {
            //���б��ļ��в���
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
    //��ȡ�����С
    public long GetMemotySize(List<FileInfo> fi)
    {
        long size = 0;
        for (int i = 0; i < fi.Count; i++)
        {
            size += fi[i].Length;
        }
        return size;
    }
    //д�����
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
    //�ļ��Ƿ����
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
