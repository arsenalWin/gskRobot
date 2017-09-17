/// <summary>
/// FileName: EncryptionManager.cs
/// Author: Jiang Xiaolong
/// Created Time: 2015.01.13
/// Version: 1.0
/// Company: Sunnytech
/// Function: 加密功能管理
///
/// Changed By:
/// Modification Time:
/// Discription:
/// </summary>
using UnityEngine;
using System.Collections;
using Motion.Encryption;
using System.Threading;

using System.Net;
using System.Net.Sockets;

public class EncryptionManager : MonoBehaviour 
{
	/* *************
	 * 1、局域网解密；
	 * 2、文件解密；
	 * 3、加密狗解密；
	 * 4、网络解密；
	 * ************/

	private bool _checkNetwork = false;
	private bool _decodingResult = false;
	//局域网解密
	//private bool _checkLAN = false;
	public static DecodingFlag _isDecoding = DecodingFlag.Fail;

	//GUI参数
	private Rect _checkNetworkRect = new Rect(200, 200, 320, 230);
	private Rect _resultRect = new Rect(200, 200, 320, 230);
	//局域网解密
	//private Rect _checkLANRect = new Rect(200, 200, 320, 230);
	private string _userName = "";
	private string _passWord = "";

	//MotionManager st_motionmanager;

	//是否是教师机
	public static bool _isTeacherMachine = false;
	//局域网解密
	private UDPLANEncryption _clientEncryption = null;
    //服务器解密实例
    public static UDPLANEncryption ServerEncryption = null;
	//是否在试用期内
	public static bool IsInProbation = false;
	private EncryptionTimer _encryptionTimer;
	

	// Use this for initialization
	void Awake () 
	{

		//程序开始时查询一次，不使用网络解密；
        CheckEncryption(false);
	}

	void Start() 
    {

	}

	 /// <summary>
	 /// 是否加密判断；
	 /// </summary>
	public static bool IsEncryped
	{
		get
		{
			if (_isDecoding == DecodingFlag.Fail)
			{
				return false;
			}
			else
			{
				return false;
			}
		}
	}

	/// <summary>
	/// 解密入口，如果加入局域网解密，此处也要修改；
	/// </summary>
	/// <param name="check_network_encryption">是否最后查询网络解密；</param>
	public void CheckEncryption(bool check_network_encryption)
	{
		//加密狗解密尝试
		Softdog _softDog = new Softdog();
		_isDecoding = _softDog.DogReturn();

		if (_isDecoding == DecodingFlag.Fail)
		{
			//文件解密尝试
			FileDecode filedecode = new FileDecode();
			_isDecoding = filedecode.IsFileIdentitySuccess();

			//如果解密成功，关闭学生机局域网
			if (_isDecoding != DecodingFlag.Fail)
			{
				if (_clientEncryption != null)
				{
					_clientEncryption.BeforeQuit();
				}
			}

			if (_isDecoding == DecodingFlag.Fail)
			{
				//局域网解密会消耗一些时间，采用协程处理后续试用期和网络解密
				StartCoroutine(StartLANEncryption(check_network_encryption));
			}
		}
		else {
			//如果解密成功，关闭学生机局域网
			if (_clientEncryption != null)
			{
				_clientEncryption.BeforeQuit();
			}
		}
	}

	/// <summary>
	/// 开启一个协程来获得局域网解密结果
	/// </summary>
	/// <returns></returns>
	IEnumerator StartLANEncryption(bool check_network_encryption)
	{
		//学生机局域网解密
		if (!_isTeacherMachine)
		{
			if (_clientEncryption == null)
			{
				_clientEncryption = new UDPLANEncryption();
				_clientEncryption.Start(false, 0);
			}
		}

		//等待局域网解密5s
		yield return new WaitForSeconds(5f);

		//试用期加载
        //if (_isDecoding == DecodingFlag.Fail)
        //{
        //    _encryptionTimer = new EncryptionTimer();
        //    IsInProbation = _encryptionTimer.Start();

        //    Debug.Log("_isInProbation:" + IsInProbation);
        //}

		//if (_isDecoding == DecodingFlag.Fail && !IsInProbation)
		if (_isDecoding == DecodingFlag.Fail)
		{
			//开启网络解密才显示结果
			if (check_network_encryption)
				_checkNetwork = true;
		}
	}

	/// <summary>
	/// 程序退出前处理
	/// </summary>
	public void OnApplicationQuit() {
		//试用期时间更新
		if (_encryptionTimer != null)
		{
			_encryptionTimer.BeforeQuit();
		}

		//局域网端Close
		if (_clientEncryption != null)
		{
			_clientEncryption.BeforeQuit();
		}

        //服务器端Colse
        if (ServerEncryption != null)
        {
            ServerEncryption.BeforeQuit();
        }
	}

	void OnGUI()
	{
        GUI.skin.window = FuncPara.skin_hiMenu.window;

		//网上注册认证窗口
		if (_checkNetwork)
		{
			_checkNetworkRect = GUI.Window(30, _checkNetworkRect, CheckNetworkWindow, "");
			GUI.BringWindowToFront(30);
		}

		//解密结果窗口
		if (_decodingResult)
		{
			_resultRect = GUI.Window(31, _resultRect, DecodingResultWindow, "");
		}

        GUI.skin.window = FuncPara.defaultSkin.window;
	}

	//网络解密窗口
	private void CheckNetworkWindow(int WindowID)
	{
		GUI.skin.label = FuncPara.defaultSkin.label;
		GUI.skin.label.font = FuncPara.defaultFont;
		GUI.skin.label.fontSize = 19;
		GUI.skin.label.normal.textColor = Color.white;
		GUI.Label(new Rect(10, 5, 200, 30), "软件解密窗口");

		GUI.skin.label.fontSize = 17;
		GUI.skin.label.normal.textColor = Color.black;
		GUI.Label(new Rect(30, 65, 100, 30), "用户名：");
		GUI.Label(new Rect(30, 120, 100, 30), "密   码：");

		GUI.skin.settings.cursorColor = Color.black;
		GUI.SetNextControlName("UserName");
		//_userName = GUI.TextField(new Rect(100, 65, 180, 32), _userName, 30);
		_userName = GUI.TextField(new Rect(100, 65, 180, 32), _userName, 30, FuncPara.sty_InputBar);
		_userName = _userName.Replace("\n", "");
		GUI.SetNextControlName("Password");
		//_passWord = GUI.PasswordField(new Rect(100, 120, 180, 32), _passWord, "*"[0], 30);
		_passWord = GUI.PasswordField(new Rect(100, 120, 180, 32), _passWord, "*"[0], 30, FuncPara.sty_InputBar);
		_passWord = _passWord.Replace("\n", "");

		//if (GUI.Button(new Rect(35, 177, 100, 35), "确定"))
		if (GUI.Button(new Rect(35, 177, 100, 35), "确定", FuncPara.sty_SquareBtn))
		{
			StartCoroutine(NetworkDecoding(_userName, _passWord));
			_checkNetwork = false;
		}
		//if (GUI.Button(new Rect(185, 177, 100, 35), "取消"))
		if (GUI.Button(new Rect(185, 177, 100, 35), "取消", FuncPara.sty_SquareBtn))
		{
			_checkNetwork = false;
		}
		if (_checkNetworkRect.x < 0)
		{
			_checkNetworkRect.x = 0;
		}
		if (_checkNetworkRect.y < 0)
		{
			_checkNetworkRect.y = 0;
		}
		if (_checkNetworkRect.x > Screen.width - _checkNetworkRect.width)
		{
			_checkNetworkRect.x = Screen.width - _checkNetworkRect.width;
		}
		if (_checkNetworkRect.y > Screen.height - _checkNetworkRect.height)
		{
			_checkNetworkRect.y = Screen.height - _checkNetworkRect.height;
		}

		GUI.DragWindow();
		GUI.skin.label = null;

	}

	//解密结果窗口
	private void DecodingResultWindow(int WindowID)
	{
		GUI.skin.label = FuncPara.defaultSkin.label;
		GUI.skin.label.font = FuncPara.defaultFont;
		GUI.skin.label.fontSize = 19;
		GUI.skin.label.normal.textColor = Color.white;
		GUI.Label(new Rect(10, 5, 200, 30), "软件解密状态");

		GUI.skin.label.fontSize = 17;
		GUI.skin.label.normal.textColor = Color.black;

		if (IsEncryped)
			GUI.Label(new Rect(35, 60, 240, 200), "权限认证失败！\n您可以使用加密狗、认证证书或网上注册获得权限。");
		else
		{
			GUI.Label(new Rect(35, 60, 260, 120), "权限认证成功！\n您现在可以使用加密模块。");
			_checkNetwork = false;
			//conn = false;
		}

		//if (GUI.Button(new Rect(35, 177, 100, 35), "确定"))
		if (GUI.Button(new Rect(35, 177, 100, 35), "确定", FuncPara.sty_SquareBtn))
		{
			_decodingResult = false;
		}

		//if (GUI.Button(new Rect(185, 177, 100, 35), "取消"))
		if (GUI.Button(new Rect(185, 177, 100, 35), "取消", FuncPara.sty_SquareBtn))
		{
			_decodingResult = false;
		}

		if (_resultRect.x < 0)
		{
			_resultRect.x = 0; 
		}
		if (_resultRect.y < 0)
		{ 
			_resultRect.y = 0; 
		}
		if (_resultRect.x > Screen.width - _resultRect.width)
		{ 
			_resultRect.x = Screen.width - _resultRect.width;
		}
		if (_resultRect.y > Screen.height - _resultRect.height)
		{ 
			_resultRect.y = Screen.height - _resultRect.height;
		}

		GUI.DragWindow();
		GUI.skin.label = null;
	}

	

	/// <summary>
	/// 网络解密，因为使用了Unity的WWW类，暂时无法模块化，要模块化可使用C#的相关API；
	/// </summary>
	/// <param name="user_name">用户名；</param>
	/// <param name="pass_word">密码；</param>
	/// <returns></returns>
	IEnumerator NetworkDecoding(string user_name, string pass_word)
	{
		string url = "http://www.51cax.com/vt/authen.jsp?username=" + user_name;
		url += "&password=" + pass_word + "&id=mold";

		WWW networkCheck = new WWW(url);
		yield return networkCheck;

		if (networkCheck.error != null)
		{
			Debug.Log(networkCheck.error);
		}
		else
		{
			if (networkCheck.text.Length < 16)
			{
				string x = networkCheck.text;
				int xx = int.Parse(x);
				if (xx == 1)
				{
					_isDecoding = DecodingFlag.NetworkDecoding;
				}
			}
		}
		_decodingResult = true;
	}

}
