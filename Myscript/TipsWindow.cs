/// <summary>
/// FileName: TipsWindow.cs
/// Author: Jiang Xiaolong
/// Created Time: 2014.02.24
/// Version: 1.0
/// Company: Sunnytech
/// Function: 提示窗口
///
/// Changed By:
/// Modification Time:
/// Discription:
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsWindow : MonoBehaviour {
	
	private Rect tipsRect;
	const float LINT_HEIGHT = 19f;
	const float ORIGIN_HEIGHT = 60f;
	private static GUIText tipsText;  //GUI Text用于字符宽度计算
	string displayString = "";  //提示信息内容
	Rect helpRect;
	
	private static Rect componentsRect;  //部件提示窗口
	private static Rect componentsLabel;  //部件提示信息label大小
	private static float singleCharLength = 0f;

	//TODO:框架修改——Tips新UI
	bool _newTipsWindow;
	Rect _newTipsRect;
	string _newTipsString;
	int MyFontSize;
	Color MyFontColor;
	Font MyFont;
	Dictionary<string, Font> myTipsFontDic = new Dictionary<string, Font>();
	
	// Use this for initialization
	void Awake () {
		tipsRect = new Rect(100f, 100f, 263f, 60f);
		GameObject TipsObj = new GameObject("TipsText");
		TipsObj.AddComponent<GUIText>();
		tipsText = TipsObj.GetComponent<GUIText>();
		tipsText.enabled = false;

		tipsText.font = (Font)Resources.Load ("Font/msyh");
		if (tipsText.font == null)
		{
			Debug.LogError("无法加载Font/msyh中是微软雅黑字体。");
		}
		tipsText.fontSize = 15;
		helpRect = new Rect(100f, 100f, 249f, 60f);
		componentsRect = new Rect(100f, 100f, 90f, 60f);
		componentsLabel = new Rect(0f, 15f, 90f, 40f);


		//TODO:框架修改——Tips新UI
		MyFontSize = 28;
		MyFontColor = Color.white;
		myTipsFontDic.Add("msyh", FuncPara.defaultFont);

		//测试
		//MyTipsWindowsShow("依次旋开按钮C、B、A", true, true, 35, new Color(255, 0, 0), "msyh");
	}
	
	void Start () {
		 tipsText.text = "四";
		 singleCharLength = tipsText.GetScreenRect().width;
	}
	
	void OnGUI () {

		GUI.skin = FuncPara.defaultSkin;
		GUI.skin.label.font = FuncPara.defaultFont;
		GUI.skin.label.fontSize = 13;
		GUI.skin.label.normal.textColor = Color.white;
		GUI.skin.button.font = FuncPara.defaultFont;
		GUI.skin.button.fontSize = 13;
		GUI.skin.button.normal.textColor = Color.white;

		//TODO:框架修改——Tips新UI
		if(_newTipsWindow){
			//设置字体
			GUI.skin.label.font = MyFont;
			GUI.skin.label.fontSize = MyFontSize;
			GUI.skin.label.normal.textColor = MyFontColor;

			GUI.Label(_newTipsRect, _newTipsString);

			GUI.skin.label.font = FuncPara.defaultFont;
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.normal.textColor = Color.white;
		}

		GUI.skin = null;
	}
	
	/// <summary>
	/// TODO:框架修改——Tips新UI
	/// </summary>
	/// <param name="tmpStr">字符串</param>
	/// <param name="tmpShow">是否显示</param>
	/// <param name="tmpIsTitle">是否作为标题：标题显示在正中间，非标题显示在下方中间</param>
	/// <param name="tmpFontSize">字体大小</param>
	/// <param name="tmpFontColor">字体颜色</param>
	/// /// <param name="tmpFontName">字体名字</param>
	public void MyTipsWindowsShow(string tmpStr, bool tmpShow, bool tmpIsTitle, int tmpFontSize, Color tmpFontColor, string tmpFontName) {

		_newTipsWindow = tmpShow;

		//显示Tips
		if (_newTipsWindow)
		{
			_newTipsString = tmpStr;

			MyFontSize = tmpFontSize;
			MyFontColor = tmpFontColor;
			if (!myTipsFontDic.ContainsKey(tmpFontName))
			{
				myTipsFontDic.Add(tmpFontName, (Font)Resources.Load("Font/" + tmpFontName));
			}
			MyFont = myTipsFontDic[tmpFontName];

			//测试字体宽度
			tipsText.font = MyFont;
			tipsText.fontSize = MyFontSize;
			tipsText.text = tmpStr;

			if (!tmpIsTitle)
			{
				//设置显示区域
				_newTipsRect = new Rect(0, 0, 100, 30);
				_newTipsRect.width = tipsText.GetScreenRect().width;
				_newTipsRect.x = (Screen.width - _newTipsRect.width) / 2;
				_newTipsRect.y = Screen.height - (MyFontSize + 30);
				_newTipsRect.height = (MyFontSize + 30);
			}
			else {
				//设置显示区域
				_newTipsRect = new Rect(0, 0, 100, 30);
				_newTipsRect.width = tipsText.GetScreenRect().width;
				_newTipsRect.x = (Screen.width - _newTipsRect.width) / 2;
				_newTipsRect.y = (Screen.height - (MyFontSize + 30)) / 2;
				_newTipsRect.height = MyFontSize + 30;
			}
		}
	}

	public static void ComponentsFormat()
	{ 
		tipsText.text = FuncPara.componentString;
		componentsRect.width = tipsText.GetScreenRect().width + 5 * singleCharLength;
		componentsLabel.width = componentsRect.width;
	}
	
	/// <summary>
	/// 显示并格式化提示信息.
	/// </summary>
	/// <param name='tips_message'>
	/// Tips_message.
	/// </param>
	/// <param name='move_allow'>
	/// 是否允许移动窗口.
	/// </param>
	public void RectAdjust(string tips_message, bool move_allow)
	{
		displayString = tips_message;
		if(FuncPara.sty_TipsWindow.normal.background != FuncPara.t2d_tipsWindow){
			FuncPara.sty_TipsWindow.normal.background = FuncPara.t2d_tipsWindow;
		}
		//stringCount = TipsFormat(tips_message);
		TipsFormat(tips_message);
		FuncPara.tipsWindow = true;
		FuncPara.tipsMove = move_allow;
		tipsRect.x = Screen.width - tipsRect.width;
		tipsRect.y = Screen.height - tipsRect.height;
	}
	
	/// <summary>
	/// 显示并格式化提示信息.
	/// </summary>
	/// <param name='tips_message'>
	/// Tips_message.
	/// </param>
	/// <param name='move_allow'>
	/// 是否允许移动窗口.
	/// </param>
	/// <param name='window_color'>
	/// 背景颜色选择.
	/// </param>
	public void RectAdjust(string tips_message, bool move_allow, WindowColor window_color)
	{
		displayString = tips_message;
		FuncPara.sty_TipsWindow.normal.background = FuncPara.t2d_colorWindow[window_color];
		//stringCount = TipsFormat(tips_message);
		TipsFormat(tips_message);
		FuncPara.tipsWindow = true;
		FuncPara.tipsMove = move_allow;
		tipsRect.x = Screen.width - tipsRect.width;
		tipsRect.y = Screen.height - tipsRect.height;
	}
	
	/// <summary>
	/// 显示并格式化提示信息.
	/// </summary>
	/// <param name='tips_message'>
	/// Tips_message.
	/// </param>
	/// <param name='move_allow'>
	/// 是否允许移动窗口.
	/// </param>
	/// <param name='aim_rect'>
	/// 显示位置
	/// </param>
	public void RectAdjust(string tips_message, bool move_allow, Vector2 target_position)
	{
		RectAdjust(tips_message, move_allow);
		tipsRect.x = target_position.x;
		tipsRect.y = target_position.y;
	}
	
	/// <summary>
	/// 显示并格式化提示信息.
	/// </summary>
	/// <param name='tips_message'>
	/// Tips_message.
	/// </param>
	/// <param name='move_allow'>
	/// 是否允许移动窗口.
	/// </param>
	/// <param name='aim_rect'>
	/// 显示位置
	/// </param>
	/// <param name='window_color'>
	/// 背景颜色选择.
	/// </param>
	public void RectAdjust(string tips_message, bool move_allow, Vector2 target_position, WindowColor window_color)
	{
		RectAdjust(tips_message, move_allow, window_color);
		tipsRect.x = target_position.x;
		tipsRect.y = target_position.y;
	}
	
	/// <summary>
	/// 显示并格式化提示信息.
	/// </summary>
	/// <param name='tips_message'>
	/// Tips_message.
	/// </param>
	/// <param name='move_allow'>
	/// 是否允许移动窗口.
	/// </param>
	/// <param name='position_string'>
	/// 显示位置： down_left, down_right, top_right, top_left, center
	/// </param>
	public void RectAdjust(string tips_message, bool move_allow, string position_string)
	{
		RectAdjust(tips_message, move_allow);
		if(position_string == "down_left"){
			tipsRect.x = 0;
			tipsRect.y = Screen.height - tipsRect.height;
		}else if(position_string == "down_right"){
			tipsRect.x = Screen.width - tipsRect.width;
			tipsRect.y = Screen.height - tipsRect.height;
		}else if(position_string == "top_right"){
			tipsRect.x = Screen.width - tipsRect.width;
			tipsRect.y = 0;
		}else if(position_string == "top_left"){
			tipsRect.x = 0;
			tipsRect.y = 0;
		}else{
			tipsRect.x = (Screen.width - tipsRect.width) / 2;
			tipsRect.y = (Screen.height - tipsRect.height) / 2;
		}
	}
	
	/// <summary>
	/// 显示并格式化提示信息.
	/// </summary>
	/// <param name='tips_message'>
	/// Tips_message.
	/// </param>
	/// <param name='move_allow'>
	/// 是否允许移动窗口.
	/// </param>
	/// <param name='position_string'>
	/// 显示位置： down_left, down_right, top_right, top_left, center
	/// </param>
	/// <param name='window_color'>
	/// 背景颜色选择.
	/// </param>
	public void RectAdjust(string tips_message, bool move_allow, string position_string, WindowColor window_color)
	{
		RectAdjust(tips_message, move_allow, window_color);
		if(position_string == "down_left"){
			tipsRect.x = 0;
			tipsRect.y = Screen.height - tipsRect.height;
		}else if(position_string == "down_right"){
			tipsRect.x = Screen.width - tipsRect.width;
			tipsRect.y = Screen.height - tipsRect.height;
		}else if(position_string == "top_right"){
			tipsRect.x = Screen.width - tipsRect.width;
			tipsRect.y = 0;
		}else if(position_string == "top_left"){
			tipsRect.x = 0;
			tipsRect.y = 0;
		}else{
			tipsRect.x = (Screen.width - tipsRect.width) / 2;
			tipsRect.y = (Screen.height - tipsRect.height) / 2;
		}
	}
	
	//提示窗口字符格式化
	private int TipsFormat(string tips_message)
	{
		string tempStr = "";
		int rowCount = 0;
		for(int i = 0; i < tips_message.Length; i++){
			tempStr += tips_message[i].ToString();
			tipsText.text = tempStr;
			if(tipsText.GetScreenRect().width > 220f){  //超过了一行
				rowCount++;
				tempStr = "";
			}
		}
		if(tempStr != "")
			rowCount++;
		if(rowCount < 2){
			tipsRect.height = ORIGIN_HEIGHT;
			tipsText.text = displayString;
			tipsRect.width = (19f * 2f + tipsText.GetScreenRect().width);
		}else{
			tipsRect.height = ORIGIN_HEIGHT + (rowCount - 2) * LINT_HEIGHT + 20f;
			tipsRect.width = 263f;
		}
		return rowCount;
	}
	
	// /// <summary>
	// /// 帮助提示信息格式化.
	// /// </summary>
	 public void HelpInfoFormat(){
	 	string tempStr = "";
	 	int rowCount = 0;
	 	for(int i = 0; i < FuncPara.helpString.Length; i++){
	 		if(FuncPara.helpString[i] == '\n'){
	 			rowCount++;
	 			tempStr = "";
	 			continue;
	 		}
	 		tempStr += FuncPara.helpString[i].ToString();
	 		tipsText.text = tempStr;
	 		if(tipsText.GetScreenRect().width > 220f){  //超过了一行
	 			rowCount++;
	 			tempStr = "";
	 		}
	 	}
	 	if(tempStr != "")
	 		rowCount++;
	 	if(rowCount < 3)
	 		helpRect.height = ORIGIN_HEIGHT;
	 	else
	 		helpRect.height = ORIGIN_HEIGHT + (rowCount - 2) * LINT_HEIGHT;
	 }
	
}
