using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

namespace Motion.Encryption
{
	/*
	 * 1、运行软件就开始连接局域网解密：教师机打开服务器UDP广播信息，学生机自动连接教师机
	 * 2、通过节点数量（节点数量存储）,控制连接教师机的学生机数量
	 * 3、教师机端如何判断学生机端断开，同时连接学生机数量修改——心跳测试；
	 * 4、教师机关闭情况如何处理
	 * 5、保证可靠性：360，凌波微步等软件的影响
	 * 6、不同软件分别解密
	 */
	public class UDPLANEncryption
	{
		//是否教师机
		//public static bool isTeacherMachine = false;
		//可连接的节点数
		public static int validConnections = 0;

		//是否已经局域网解密
		public static bool isLANDecryted;
		//广播地址IP
		public static string LANBoardIP;
        public static IPAddress _LANBoardIP;
		//局域网IP
		public static IPAddress LANIP;

		//UDP
		public MyUdpServer myServer;
		MyUdpClient myClient;

		//Debug
		public static bool IsDebug = true;

		/// <summary>
		/// 开启局域网解密
		/// </summary>
		/// <param name="tmpIsTeacherMachine">是否为教师机</param>
		/// <param name="connectNumber">学生机可连接数量</param>
		public void Start(bool tmpIsTeacherMachine, int tmpConnectNumber)
		{
			isLANDecryted = false;
			validConnections = 3;

			//局域网广播IP
			IPHostEntry ihe = Dns.GetHostEntry(Dns.GetHostName());
			//局域网IP
			LANIP = ihe.AddressList[0];
			int thirdNumber = 256;
			for (int i = 0; i < ihe.AddressList.Length; i++)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log(ihe.AddressList[i]);
				//如果有多个192.168,则判断第三个参数，选择参数小的那个
				if (ihe.AddressList[i].ToString().StartsWith("192.168."))
				{
					string[] tmpStrsss2 = ihe.AddressList[i].ToString().Split('.');
					int tmpthirdNumber = int.Parse(tmpStrsss2[2]);
					if (tmpthirdNumber < thirdNumber)
					{
						thirdNumber = tmpthirdNumber;
						LANIP = ihe.AddressList[i];
					}
				}
				else if (ihe.AddressList[i].ToString().StartsWith("172."))
				{
					LANIP = ihe.AddressList[i];
				}
				else if (ihe.AddressList[i].ToString().StartsWith("10."))
				{
					LANIP = ihe.AddressList[i];
				}
			}
			string[] tmpStrsss = LANIP.ToString().Split('.');
			LANBoardIP = tmpStrsss[0] + "." + tmpStrsss[1] + "." + tmpStrsss[2] + ".255";
            _LANBoardIP = IPAddress.Parse(LANBoardIP);
			if (UDPLANEncryption.IsDebug)
				Debug.Log("Host:" + LANIP.ToString() + ",局域网广播地址IP:" + LANBoardIP);

			if (tmpIsTeacherMachine)
			{
				//设置可连接学生机数量
				validConnections = tmpConnectNumber;

				myServer = new MyUdpServer();
				myServer.Start();
			}
			else
			{
				myClient = new MyUdpClient();
				myClient.Start();
			}
		}


		/// <summary>
		/// 退出之前
		/// </summary>
		public void BeforeQuit()
		{
			if (myServer != null)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log("Server Stop");

                //服务器退出前是否发送消息给学生机
                myServer.SendMsg("ServerClose");

				myServer.Stop();
			}
			if (myClient != null)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log("Client Stop");

				myClient.Stop();

                //学生机退出前发送消息
                //myClient.SendExitMsg();
			}

		}
	}

	public class MyUdpClient
	{
		//UdpClient udpClient;
		//发送消息
		bool _sendCircle = true;
		//发送消息的进程
		Thread myThread;
		//接受消息
		bool _receiveCircle = true;
		//接受消息的进程
		Thread myReceiveThread;

		//心跳间隔
		int spaceTime;
		//教师机断开局域网的时间间隔
		int checkLimitTime;
		bool _checkTeacherCircle;
		Thread myCheckTeacherThread;
		System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();

        //
        Socket _sendSocket;
        Socket _listenSocket;

		/// <summary>
		/// 开始局域网解密
		/// </summary>
		public void Start()
		{
			if (UDPLANEncryption.IsDebug)
				Debug.Log("MyUdpClient Start");
			//发送消息的间隔
			spaceTime = 10;

			//开始发送消息的线程
			_sendCircle = true;
			myThread = new Thread(SendMsg);
			myThread.Start();

			//开启接受消息的线程
			_receiveCircle = true;
			myReceiveThread = new Thread(ReceiveMsg);
			myReceiveThread.Start();

            
           //检测教师机是否断开局域网
           _checkTeacherCircle = true;
           checkLimitTime = 120;
           myCheckTeacherThread = new Thread(CheckTeather);
           myCheckTeacherThread.Start();
		}

		/// <summary>
		/// 向教师机发送消息
		/// </summary>
		void SendMsg()
		{

			try
			{
                _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, 11011);
                IPEndPoint iep = new IPEndPoint(UDPLANEncryption._LANBoardIP, 11011);
                _sendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                while (_sendCircle)
                {
                    try
                    {
                        byte[] data = Encoding.ASCII.GetBytes(UDPLANEncryption.LANIP + "~11011~" + "Connect Server~" + JiamiString.SoftwareName + "~" + UDPLANEncryption.isLANDecryted);
                        _sendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                        _sendSocket.SendTo(data, iep);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("SendMsg Exception");
                        if (UDPLANEncryption.IsDebug)
                            Debug.Log(e.Message);
                    }

                    Thread.Sleep(1000 * spaceTime);
                }
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
                    Debug.Log("SendMsg:" + e.Message);
			}
		}

		/// <summary>
		/// 向教师机发送消息
		/// </summary>
		public void SendExitMsg()
		{
			try
			{
                _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, 11011);
                IPEndPoint iep = new IPEndPoint(UDPLANEncryption._LANBoardIP, 11011);
                _sendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                //发送信息里面包含软件ID,是否解密成功
                byte[] data = Encoding.ASCII.GetBytes(UDPLANEncryption.LANIP + "~11011~" + "ClientQuit~" + JiamiString.SoftwareName + "~" + UDPLANEncryption.isLANDecryted);
                _sendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                _sendSocket.SendTo(data, iep);

				if (UDPLANEncryption.IsDebug)
					Debug.Log("ClientQuit:" + UDPLANEncryption.isLANDecryted);
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log(e.Message);
			}
            

            if (UDPLANEncryption.IsDebug)
                Debug.Log("SendExitMsg End");
		}

		/// <summary>
		/// 接收教师机端的信息
		/// </summary>
		void ReceiveMsg()
		{
			try
			{
                _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 11012);
                _listenSocket.Bind(ipep);//定义一个网络端点

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);//定义要发送的计算机的地址
                EndPoint remote = (EndPoint)(sender);//远程

                if (UDPLANEncryption.IsDebug)
                    Debug.Log("开始接收教师机反馈信息");
                //一直接收教师机的消息
                while (_receiveCircle)
                {
                    try
                    {
                        //TODO:UdpClient接收信息
                        byte[] data = new byte[1024];
                        int recv = _listenSocket.ReceiveFrom(data, ref remote);
                        string returnData = Encoding.ASCII.GetString(data, 0, recv);

                        //接收信息
                        IPAddress ip = UDPLANEncryption.LANIP;
                        string correctInfo = JiamiString.SoftwareName + "~Connect Success " + ip.ToString();
                        string softdogErrorInfo = JiamiString.SoftwareName + "~Connect Success " + "SoftDogOut";
                        string serverCloseInfo = JiamiString.SoftwareName + "~Connect Success " + "ServerClose";

                        string[] tmpStringArray = UDPLANEncryption.LANIP.ToString().Split('.');
                        string teacherConfirmInfo = JiamiString.SoftwareName + "~" + tmpStringArray[0] + "." + tmpStringArray[1] + "~";

                        if (UDPLANEncryption.IsDebug)
                        {
                            if (!returnData.StartsWith(teacherConfirmInfo))
                            {
                                Debug.Log("receive message:" + returnData + "," + DateTime.Now.ToString());
                            }
                            else
                            {
                                Debug.Log("教师机发送的统一的通知在线信息:" + returnData + "," + DateTime.Now.ToString());
                            }
                        }

                        //根据接收信息判断是否成功
                        if (returnData == correctInfo)
                        {
                            UDPLANEncryption.isLANDecryted = true;
                            //局域网解密成功
                            EncryptionManager._isDecoding = DecodingFlag.LANDecoding;
                            //发送间隔时间延长为30s
                            spaceTime = 30;
                            //计时器归零
                            myStopwatch.Reset();
                            //如果重新连上服务器，需要重启教师机检查
                            if (myCheckTeacherThread == null)
                            {
                                if (UDPLANEncryption.IsDebug)
                                    Debug.Log("如果重新连上服务器，需要重启教师机检查");
                                _checkTeacherCircle = true;
                                myCheckTeacherThread = new Thread(CheckTeather);
                                myCheckTeacherThread.Start();
                            }

                            if (UDPLANEncryption.IsDebug)
                                Debug.Log("局域网解密成功");
                        }
                        //如果接收到教师机加密狗被拔出的信息 或者 教师机端关闭
                        else if (returnData == softdogErrorInfo || returnData == serverCloseInfo)
                        {
                            UDPLANEncryption.isLANDecryted = false;
                            //局域网解密失败
                            EncryptionManager._isDecoding = DecodingFlag.Fail;

                            //不用检查服务器超时
                            _checkTeacherCircle = false;
                            myCheckTeacherThread.Abort();
                            myCheckTeacherThread = null;
                            if (UDPLANEncryption.IsDebug)
                            {
                                if (returnData == softdogErrorInfo)
                                {
                                    Debug.Log("教师机加密狗被拔出,不用检查服务器超时");
                                }
                                else if (returnData == serverCloseInfo)
                                {
                                    Debug.Log("教师机已关闭,不用检查服务器超时");
                                }
                            }
                        }
                        //收到教师机发送的统一的通知在线信息
                        else if (returnData.StartsWith(teacherConfirmInfo))
                        {
                            string[] tmpStringArray2 = returnData.Substring(teacherConfirmInfo.Length).Split('|');
                            List<string> tmpList2 = new List<string>(tmpStringArray2);
                            string currentIPStr = tmpStringArray[2] + "." + tmpStringArray[3];
                            if (tmpList2.Contains(currentIPStr))
                            {
                                UDPLANEncryption.isLANDecryted = true;
                                //局域网解密成功
                                EncryptionManager._isDecoding = DecodingFlag.LANDecoding;
                                //发送间隔时间延长为30s
                                spaceTime = 30;
                                //计时器归零
                                myStopwatch.Reset();
                                //如果重新连上服务器，需要重启教师机检查
                                if (myCheckTeacherThread == null)
                                {
                                    if (UDPLANEncryption.IsDebug)
                                        Debug.Log("如果重新连上服务器，需要重启教师机检查");
                                    _checkTeacherCircle = true;
                                    myCheckTeacherThread = new Thread(CheckTeather);
                                    myCheckTeacherThread.Start();
                                }

                                if (UDPLANEncryption.IsDebug)
                                    Debug.Log("局域网解密成功");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (UDPLANEncryption.IsDebug)
                            Debug.Log("ReceiveMsg Exception:" + e.Message);
                    }
                }
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
                    Debug.Log("ReceiveMsg:" + e.Message);
			}

			
		}

		/// <summary>
		/// 检查教师机局域网是否断开连接
		/// </summary>
		void CheckTeather() {
			while (_checkTeacherCircle)
			{
				if (UDPLANEncryption.isLANDecryted)
				{
					//计算等待连接的时间
					myStopwatch.Stop();
					myStopwatch.Start();

					float tmpTime = (myStopwatch.ElapsedMilliseconds / 1000f);
					//教师机局域网是否断开连接
					if (tmpTime > checkLimitTime)
					{
						UDPLANEncryption.isLANDecryted = false;
						//局域网解密失败
						EncryptionManager._isDecoding = DecodingFlag.Fail;

						if (UDPLANEncryption.IsDebug)
							Debug.Log("教师机局域网断开连接：" + ",超时：" + tmpTime);

						//教师机局域网是否断开连接，学生机如何处理
						_checkTeacherCircle = false;
						myCheckTeacherThread.Abort();
						myCheckTeacherThread = null;
					}

					Thread.Sleep(1000 * 10);
				}
				else
				{
					//TODO:减少CPU消耗
					Thread.Sleep(1000 * 10);
				}
			}

            if (UDPLANEncryption.IsDebug)
                Debug.Log("CheckTeather End");
		}


		/// <summary>
		/// 停止
		/// </summary>
		public void Stop()
		{
			try
			{
				_sendCircle = false;
				_receiveCircle = false;
				_checkTeacherCircle = false;

                myThread.Abort();
                _sendSocket.Close();
                myReceiveThread.Abort();
                _listenSocket.Close();
                myCheckTeacherThread.Abort();
                if (UDPLANEncryption.IsDebug)
				    Debug.Log("Client Stop End");
			}
			catch(Exception e) {
				Debug.Log("Client Stop Exception:" + e.Message);
			}
		}
	}

	public class MyUdpServer
	{
		//UdpClient udpClient;

		//接收消息进程
		Thread myThread;
		//计时进程
		Thread timeThread;
		//断开连接超时
		int limitTime;

		//接收学生机消息
		bool _receiveCircle = true;
		//检查学生机是否断开连接
		bool _checkStudentCircle = true;

        //监听Socket
        Socket _listenSocket;
        Socket _sendSocket;

		//连接的学生机
		public Dictionary<string, float> dicConnectMachines = new Dictionary<string, float>();
		//每个学生机对应的计时器
		Dictionary<string, System.Diagnostics.Stopwatch> dicStopWatch = new Dictionary<string, System.Diagnostics.Stopwatch>();

		//断开连接的学生机
		List<string> disconnectMachines = new List<string>();

		//检查加密狗
		private bool _checkSoftdog = true;
		//检查加密狗的时间间隔
		int _checkSoftdogSpaceTime;
		//是否拔出过加密狗
		bool _isSoftdogOut = false;
		//计时进程
		Thread checkSoftDogThread;

		public void Start()
		{
			if (UDPLANEncryption.IsDebug)
				Debug.Log("MyUdpServer Start");
			_receiveCircle = true;
			_checkStudentCircle = true;
			limitTime = 60;
			_checkSoftdogSpaceTime = 60;

			disconnectMachines = new List<string>();

			try
			{
				//接受学生机的信息
				myThread = new Thread(Receive);
				myThread.Start();
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log(e.Message);
			}

			try
			{
				//检测学生机是否断开
				timeThread = new Thread(CheckConnections);
				timeThread.Start();
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log(e.Message);
			}

			try
			{
				//检查加密狗是否断开
				checkSoftDogThread = new Thread(CheckSoftdog);
				checkSoftDogThread.Start();
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log(e.Message);
			}
		}

		
		/// <summary>
		/// 接收学生机的信息
		/// </summary>
		void Receive()
		{
			if (UDPLANEncryption.IsDebug)
				Debug.Log("Receive");

			try
			{
                _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 11011);
                _listenSocket.Bind(ipep);

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)sender;

                while (_receiveCircle)
                {
                    //一直接收客户端的信息
                    if (dicConnectMachines.Count <= UDPLANEncryption.validConnections)
                    {
                        if (dicConnectMachines.Count == UDPLANEncryption.validConnections)
                        {
                            if (UDPLANEncryption.IsDebug)
                                Debug.Log("dicConnectMachines.Count连接数值达到最大:" + dicConnectMachines.Count);
                        }
                        else
                        {
                            if (UDPLANEncryption.IsDebug)
                                Debug.Log("dicConnectMachines.Count:" + dicConnectMachines.Count);
                        }
                        try
                        {
                            byte[] data = new byte[1024];
                            int recv = _listenSocket.ReceiveFrom(data, ref remote);
                            string returnData = Encoding.ASCII.GetString(data, 0, recv);

                            //学生机发送软件ID是否正确
                            string[] tmpStrsss = returnData.Split('~');
                            if (UDPLANEncryption.IsDebug)
                                Debug.Log("receive message:" + returnData + ",from :" + tmpStrsss[0] + ",port:" + tmpStrsss[1] + "," + DateTime.Now);
                            if (tmpStrsss[3] == JiamiString.SoftwareName)
                            {
                                string tmpMachineInfo = (tmpStrsss[0] + "~" + tmpStrsss[1]);
                                if (!dicConnectMachines.ContainsKey(tmpMachineInfo))
                                {
                                    //连接未达到最大数量，可允许添加连接
                                    if (dicConnectMachines.Count < UDPLANEncryption.validConnections)
                                    {
                                        //记录学生机，并设置未连接时间为0
                                        dicConnectMachines.Add(tmpMachineInfo, 0);
                                        dicStopWatch.Add(tmpMachineInfo, new System.Diagnostics.Stopwatch());

                                        //重新开启的教师机，对已解密成功的学生机只记录不重新发送解密成功信息
                                        SendMsg(tmpStrsss[0]);
                                    }
                                }
                                else
                                {
                                    if (tmpStrsss[2] == "ClientQuit")
                                    {
                                        if (!disconnectMachines.Contains(tmpMachineInfo))
                                        {
                                            disconnectMachines.Add(tmpMachineInfo);
                                        }
                                    }
                                    else
                                    {
                                        dicConnectMachines[tmpMachineInfo] = 0;

                                        if (tmpStrsss[4].ToLower() == "false")
                                        {
                                            //重新开启的学生机，对已解密成功的学生机只记录不重新发送解密成功信息
                                            SendMsg(tmpStrsss[0]);
                                        }
                                    }
                                }
                                dicStopWatch[tmpMachineInfo].Reset();
                            }
                        }
                        catch (Exception e)
                        {
                            if (UDPLANEncryption.IsDebug)
                                Debug.Log(e.Message);
                        }
                    }

                }
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log(e.Message);
			}
		}

		/// <summary>
		/// 向学生机发送解密成功的信息
		/// </summary>
		public void SendMsg(object msg)
		{
			try
			{
                _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
               // IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, 11012);
                IPEndPoint iep = new IPEndPoint(UDPLANEncryption._LANBoardIP, 11012);
                _sendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(JiamiString.SoftwareName + "~Connect Success " + msg.ToString());
                    if (msg.ToString().StartsWith(JiamiString.SoftwareName))
                    {
                        data = Encoding.ASCII.GetBytes(msg.ToString());
                    }
                    _sendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                    _sendSocket.SendTo(data, iep);
                    
                    if (UDPLANEncryption.IsDebug)
                    {
                        if (msg.ToString().StartsWith(JiamiString.SoftwareName))
                        {
                            Debug.Log("统一在线通知信息：" + msg.ToString() + "," + DateTime.Now.ToString());
                        }
                        else
                        {
                            Debug.Log("向学生机发送反馈信息：" + JiamiString.SoftwareName + "~Connect Success " + msg.ToString() + "," + DateTime.Now.ToString());
                        }
                    }

                    _sendSocket.Close();
                }
                catch (Exception e)
                {
                    if (UDPLANEncryption.IsDebug)
                        Debug.Log(e.Message);
                }
			}
			catch (Exception e)
			{
				if (UDPLANEncryption.IsDebug)
					Debug.Log(e.Message);
			}

		

			//教师机加密狗被拔出，继续检查加密狗是否插入
			if (msg.ToString() == "SoftDogOut")
			{
				_receiveCircle = false;
				_checkStudentCircle = false;
                //停止监听
				myThread.Abort();
                _listenSocket.Close();
                //停止检测学生机是否断开
				timeThread.Abort();
			}
		}

		/// <summary>
		/// 检测学生机是否断开
		/// </summary>
		void CheckConnections()
		{
			if (UDPLANEncryption.IsDebug)
				Debug.Log("CheckConnections");
			while (_checkStudentCircle)
			{
				try
				{
					if (dicConnectMachines.Count > 0)
					{
						//计算等待连接的时间
						foreach (KeyValuePair<string, System.Diagnostics.Stopwatch> kv in dicStopWatch)
						{
							kv.Value.Stop();
						}
						foreach (KeyValuePair<string, System.Diagnostics.Stopwatch> kv in dicStopWatch)
						{
							kv.Value.Start();
						}

						if (disconnectMachines.Count > 0)
						{
							for (int i = 0; i < disconnectMachines.Count; i++)
							{
								if (UDPLANEncryption.IsDebug)
									Debug.Log("学生机退出：" + disconnectMachines[i]);
								dicConnectMachines.Remove(disconnectMachines[i]);
								dicStopWatch.Remove(disconnectMachines[i]);
								if (UDPLANEncryption.IsDebug)
									Debug.Log("dicConnectMachines.Count:" + dicConnectMachines.Count);
							}
							disconnectMachines.Clear();
						}

						Dictionary<string, float> tmpDicConnectMachines = new Dictionary<string, float>(dicConnectMachines);
						foreach (KeyValuePair<string, float> kv in tmpDicConnectMachines)
						{
							dicConnectMachines[kv.Key] = (dicStopWatch[kv.Key].ElapsedMilliseconds / 1000f);

							//学生机连接超时，则服务器的连接机器序列中删掉改机器
							if (dicConnectMachines[kv.Key] > limitTime)
							{
								if (UDPLANEncryption.IsDebug)
									Debug.Log("学生机断开局域网连接：" + kv.Key + ",超时：" + dicConnectMachines[kv.Key]);
								dicConnectMachines.Remove(kv.Key);
								dicStopWatch.Remove(kv.Key);
								if (UDPLANEncryption.IsDebug)
									Debug.Log("dicConnectMachines.Count:" + dicConnectMachines.Count);
							}
						}

						Thread.Sleep(1000 * 10);
					}
					else
					{
						//stopwatch.Reset();
						//TODO:减少CPU消耗
						Thread.Sleep(1000 * 10);
					}
				}
				catch (Exception e)
				{
					if (UDPLANEncryption.IsDebug)
						Debug.Log(e.Message);
				}
			}
		}

        /// <summary>
        /// 检查加密狗
        /// </summary>
        void CheckSoftdog()
        {
            if (UDPLANEncryption.IsDebug)
                Debug.Log("开始检查教师机加密狗：————————");
            while (_checkSoftdog)
            {
                //检查间隔60s
                Thread.Sleep(1000 * _checkSoftdogSpaceTime);

                //加密狗解密尝试
                Softdog _softDog = new Softdog();
                DecodingFlag tmpDecodingFlag = _softDog.DogReturn();

                //如果重新插入加密狗
                if (_isSoftdogOut && tmpDecodingFlag == DecodingFlag.SoftdogDecoding)
                {
                    if (UDPLANEncryption.IsDebug)
                        Debug.Log("重新插入加密狗,：" + tmpDecodingFlag);

                    _checkSoftdog = false;
                    checkSoftDogThread.Abort();
                }
                //统一向学生机发送确认在线的消息
                else if (!_isSoftdogOut && tmpDecodingFlag == DecodingFlag.SoftdogDecoding)
                {
                    if (dicConnectMachines.Keys.Count > 0)
                    {
                        string tmpMachineInfo = JiamiString.SoftwareName + "~";
                        string[] tmpStringArray = UDPLANEncryption.LANIP.ToString().Split('.');
                        tmpMachineInfo += (tmpStringArray[0] + "." + tmpStringArray[1] + "~");
                        List<string> tmpList = new List<string>(dicConnectMachines.Keys);
                        for (int i = 0; i < tmpList.Count; i++)
                        {
                            string[] tmpStringArray2 = tmpList[i].Split('~');
                            string[] tmpStringArray3 = tmpStringArray2[0].Split('.');
                            if (i == 0)
                            {
                                tmpMachineInfo += (tmpStringArray3[2] + "." + tmpStringArray3[3]);
                            }
                            else
                            {
                                tmpMachineInfo += ("|" + tmpStringArray3[2] + "." + tmpStringArray3[3]);
                            }
                        }

                        //TODO:不再单独开启线程
                        SendMsg(tmpMachineInfo);
                    }
                }

                if (tmpDecodingFlag == DecodingFlag.Fail && !_isSoftdogOut)
                {
                    if (UDPLANEncryption.IsDebug)
                        Debug.LogWarning("教师机加密狗被拔出");

                    //服务器解密实例为null
                    EncryptionManager.ServerEncryption = null;
                    //加密狗被拔出
                    _isSoftdogOut = true;

                    //TODO:向局域网发送教师机加密狗失效的消息
                    SendMsg("SoftDogOut");
                }
            }
        }

		/// <summary>
		/// 杀死进程
		/// </summary>
		public void Stop()
		{
			_receiveCircle = false;
			_checkStudentCircle = false;
			_checkSoftdog = false;
            /*
			if (_receiveUdpClient != null)
			{
				_receiveUdpClient.Close();
			}
			if (_sendUdpClient != null)
			{
				_sendUdpClient.Close();
			}
			myThread.Abort();
			timeThread.Abort();
			checkSoftDogThread.Abort();
             * */

            //停止监听
            myThread.Abort();
            _listenSocket.Close();
            //停止检查学生
            timeThread.Abort();
            checkSoftDogThread.Abort();
		}
	}

}
