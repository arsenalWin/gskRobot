/// <summary>
/// FileName: Motion.Encryption.Softdog.cs
/// Author: Jiang Xiaolong
/// Created Time: 2015.01.13
/// Version: 1.0
/// Company: Sunnytech
/// Function: 解密模块
///
/// Changed By:
/// Modification Time:
/// Discription:
/// </summary>
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System;

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

/*
 * 
 * 加密狗函数：
 * 1、int FindPort( int start, char *OutPath ):
 * 功能：查找系统中是否存在第N个加密锁，如果存在，则返回该加密锁所在的设备路径
 * 参数1－－start(in)； 要查找的第N个加密锁，例：当start=0时，指的是要查找第一个加密锁所在的设备路径，当start=1时，指的是要查找系统中的第二个加密锁所在的设备路径。
 * 参数2――OutPath(out)；如果系统中存在第N个加密锁，则该参数中包含有该加密锁所在的设备路径
 * 返回结果－－若函数返回0，则表示函数执行成功，系统中存在着第N个加密锁，若返回其它值，则表示函数失败，错误原因可以参见错误代码
 * 
 * 2、int FindPort_2(  int start,DWORD in_data,DWORD verf_data,char *OutPath); 
 * 功能：查找系统中是否存在符合指定条件的第N个加密锁，如果存在，则返回该加密锁所在的设备路径，符合指定条件指的是：送入一个要加密的数据（in_data）让该加密进行加密运算，返回的结果与我们期待的加密结果相符(verf_data)
 * 参数1－－start(in)； 要查找的第N个符合指定条件的加密锁，例：当start=0时，指的是要查找第一个符合指定条件的加密锁所在的设备路径，当start=1时，指的是要查找系统中的第二个符合指定条件的加密锁所在的设备路径。
 * 参数2――in_data(in)；   要送入到加密锁进行加密运算的数据，
 * 参数3――verf_data (in)；   要进行比较校验的数据，
 * 参数4――OutPath(out)；如果系统中存在第N个符合指定条件的加密锁，则该参数中包含有该加密锁所在的设备路径
 * 返回结果－－若函数返回0，则表示函数执行成功，系统中存在着第N个符合指定条件的加密锁，若返回其它值，则表示函数失败，错误原因可以参见错误代码含义。
 * 
 * 3、int YReadString(char *outstring ,short Address,int Len,char * HKey,char *LKey,char *Path );
 * 功能：从加密锁中的指定起始位置读出字符串。
 *
 * 参数１－－outstring(out)；若函数正确返回时，从加密锁address的储存空间中读出字符串到outstring中。
 * 参数2 address(in)；用户指定的加密锁内部地址空间。即字符串在加密锁中储存的的起始位置。
 * 参数3 Len(in)；   要读取的字符串的长度。
 * 参数4 HKey(in)；读密码的高八位密码
 * 参数5 Lkey(in)；读密码的低八位密码
 * 参数6 InPath (in)；加密锁所在的设备路径
 * 返回结果－－若函数返回0，则表示函数执行成功，若返回其它值， 则表示函数失败，错误原因可以参见错误代码含义。
 * 备注：在读加密锁的储存器空间时，需要输入读密码，所有加密锁出厂时的读密码均为“FFFFFFFF”――“FFFFFFF”，如果密码错误，将不能正确从储存器中读出数据。
 * 
 */

namespace Motion.Encryption
{
	/// <summary>
	/// 解密标志位；
	/// </summary>
	public enum DecodingFlag { Fail, LANDecoding, SoftdogDecoding, FileDecoding, NetworkDecoding, TrialVersionDecoding } 
	 

	/// <summary>
	/// 加密狗解密处理类；
	/// </summary>
	public class Softdog
	{
		[DllImport("Syunew3D", CallingConvention = CallingConvention.StdCall)]
		private static extern int FindPort_2(int start, int in_data, int verf_data, StringBuilder OutPath);

		[DllImport("Syunew3D", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		private static extern int FindPort(int start, StringBuilder outpath);

		[DllImport("Syunew3D", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int YReadString(StringBuilder string1, short Address, int len, StringBuilder HKey, StringBuilder LKey, StringBuilder Path1);

		public Softdog()
		{

		}

        /// <summary>
        /// 加密狗解密，根据写入字符串第一个字符是否为数字，判断是否为教师机；
        /// </summary>
        /// <returns>加密狗解密是否成功；</returns>
        public DecodingFlag DogReturn ()
        {

			//UnityEngine.Debug.Log("Softdog: DogReturn");

            DecodingFlag _dogInfo = DecodingFlag.Fail;
            StringBuilder DevicePath = new StringBuilder(500);
            //加密狗第一阶段解密是否成功
            bool firstStateSuccess = false;

            if (FindPort(0, DevicePath) == 0)
            {
                if (FindPort_2(0, 1, 74532473, DevicePath) == 0)
                {
                    firstStateSuccess = true;
                }
                else if (FindPort_2(0, 1, -379880570, DevicePath) == 0)
                {
                    firstStateSuccess = true;
                }

                //第一阶段验证成功
                if (firstStateSuccess)
                {
                    //读取存储器里面的字符串
                    StringBuilder outString = new StringBuilder(500);
                    StringBuilder hKey = new StringBuilder("20D30D25");
                    StringBuilder lKey = new StringBuilder("FFB706AF");

					bool tmpIsTeachermachine = false;

                    if (YReadString(outString, 0, 494, hKey, lKey, DevicePath) == 0)
                    {
						//UnityEngine.Debug.LogWarning(outString);
                        string[] softNameArray = outString.ToString().Split('~');
						//Dictionary<string, bool> nameDic = new Dictionary<string, bool>();
						Dictionary<string, int> nameDic = new Dictionary<string, int>();
                        for (int i = 0; i < softNameArray.Length; i++)
                        {
							string[] tmpStrsss = softNameArray[i].Split('|');
							if (tmpStrsss.Length == 2)
							{
								string tmpSoftwareName = tmpStrsss[0];

								try 
                                {
									int num = int.Parse(tmpStrsss[1]);
									if (!nameDic.ContainsKey(tmpSoftwareName))
									{
										nameDic.Add(tmpSoftwareName, num);
									}
                                    if(num > 0)
									    tmpIsTeachermachine = true;
								}
								catch
                                {
									UnityEngine.Debug.LogError(outString + "加密狗节点数填写错误：" + softNameArray[i]);
								}
							}
							else if (tmpStrsss.Length == 1)
							{
								if (!nameDic.ContainsKey(softNameArray[i]))
								{
									nameDic.Add(softNameArray[i], 0);
								}
							}
                        }
                        //根据当前软件ID和加密狗读取出来的字符串中是否含有该ID来判断该软件是否解密；
                        if (nameDic.ContainsKey(JiamiString.SoftwareName))
                        {
                            _dogInfo = DecodingFlag.SoftdogDecoding;

							//UnityEngine.Debug.Log("Softdog: 加密狗解密");
							if (tmpIsTeachermachine && nameDic[JiamiString.SoftwareName] > 0)
							{
                                //UnityEngine.Debug.Log("节点数：" + nameDic[JiamiString.SoftwareName]);
								//当前为教师机
								EncryptionManager._isTeacherMachine = true;
								if (EncryptionManager.ServerEncryption == null)
								{
									//局域网教师机服务器开启
									EncryptionManager.ServerEncryption = new UDPLANEncryption();
									EncryptionManager.ServerEncryption.Start(true, nameDic[JiamiString.SoftwareName]);
								}
							}
                        }
                    }
                }
            }
            return _dogInfo;
        }
		
	}

    /// <summary>
    /// 文件解密类；
    /// </summary>
    public class FileDecode
    {
        private string _decodePath = "C:\\SoftwareDecode.exe";
        private string _resultPath = "C:\\result.txt";
        private AutoResetEvent aEvent = new AutoResetEvent(false);

        public FileDecode ()
        { 
        
        }

        /// <summary>
        /// 文件解密入口；
        /// </summary>
        /// <returns></returns>
        public DecodingFlag IsFileIdentitySuccess ()
        {
			//UnityEngine.Debug.Log("FileDecode:IsFileIdentitySuccess");

            if (File.Exists(_decodePath))
            {
                //删除result.txt文件
                File.Delete(_resultPath);
                Process fileCheckProc = new Process();
                fileCheckProc.StartInfo.FileName = _decodePath;
                fileCheckProc.StartInfo.Arguments = JiamiString.SoftwareName;
                fileCheckProc.StartInfo.WorkingDirectory = "C:\\";
                fileCheckProc.StartInfo.CreateNoWindow = true;
                fileCheckProc.StartInfo.UseShellExecute = false;
                fileCheckProc.Start();
                //启动CheckFile线程，等待生成result.txt文件
                Thread checkT = new Thread(CheckFileThread);
                checkT.Start();
                //等待生成result.txt文件主线程再往下执行
                aEvent.WaitOne();
                //超时，判断失败
                if (!File.Exists(_resultPath))
                {
                    return DecodingFlag.Fail;
                }
                //读取result.txt信息
                string result = File.ReadAllText(_resultPath);
                File.Delete(_resultPath);
                if (result.StartsWith("0"))  //失败
                {
                    return DecodingFlag.Fail;
                }
                else
                {
                    if (result == "1" + JiamiString.SoftwareName)  //成功
                    {
                        return DecodingFlag.FileDecoding;
                    }
                    else  //失败
                    {
                        return DecodingFlag.Fail;
                    }
                }
            }
            else
            {
                return DecodingFlag.Fail;
            }
        }

        private void CheckFileThread ()
        {
            int timer = 0;
            while (!File.Exists(_resultPath))
            {
                Thread.Sleep(10);
                timer += 10;  //10秒定时，超出失败
                if (timer > 10000)
                {
                    break;
                }
            }
            Thread.Sleep(20);
            aEvent.Set();
        }

    }

}

