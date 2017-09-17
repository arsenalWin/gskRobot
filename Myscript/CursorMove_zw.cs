/// <summary>
/// FileName: CursorMove.cs
/// Author: Jiang Xiaolong
/// Created Time: 2014.03.02
/// Version: 1.0
/// Company: Sunnytech
/// Function: 鼠标贴图移动
///
/// Changed By:
/// Modification Time:
/// Discription:
/// </summary>
using UnityEngine;
using System.Collections;

public class CursorMove_zw : MonoBehaviour {
	
	public static Rect cursorRect; //虚拟鼠标的位置
	bool movingCompute = false; //计时中
	bool rotateCompute = false;  //旋转中
	bool cwFlag = false;  //flase：逆时针；true：顺时针
	float timeEnd = 0;  //每个动作的时间
	float timeStart = 0;  //时间记录
	float timeSpeed = 0;  //用于运动时控制速度
	const float SPEED_MOVE = 200F; //移动速度
	const float SPEED_ROTATE = 1F;  //旋转角速度
	float xSpeed = 0;  //X轴移动速度
	float ySpeed = 0;  //Y轴移动速度
	float xStart = 0;
	float xEnd = 0;
	float yStart = 0;
	float yEnd = 0;
	float radiusRotate = 0;  //旋转半径
	Vector2 startPoint = Vector2.zero;  //旋转起始点
	Vector2 centerPoint = Vector2.zero;   //旋转圆心点
	// Use this for initialization
	void Start () {
		cursorRect = new Rect(100, 100, 33, 33);
	}
	
	void OnGUI () {
		if(FuncPara.cursorShow){
			GUI.Window(25, cursorRect, CursorWindow, "", FuncPara.sty_Cursor);
			GUI.BringWindowToFront(25);
		}
	}

	void Update(){
		if(FuncPara.isPlaying && FuncPara.cursorShow){
			if(movingCompute){  //平移
				timeSpeed += Time.deltaTime;  //移动速度控制
				timeStart += Time.deltaTime;  //处理加减速操作
				cursorRect.x = xStart + xSpeed * timeSpeed * FuncPara.speedRate;
				cursorRect.y = yStart + ySpeed * timeSpeed * FuncPara.speedRate;
				FuncPara.cursorPosition.x = cursorRect.x + 16f - FuncPara.rightRect.x;
				FuncPara.cursorPosition.y = cursorRect.y + 16f - FuncPara.rightRect.y;
				if(timeStart >= timeEnd){  //超过时间，位置矫正
					cursorRect.x = xEnd;
					cursorRect.y = yEnd;
					movingCompute = false;
					FuncPara.loopControl = 14;  //循环停止
				}
			}
			
			if(rotateCompute){  //旋转
				timeStart += Time.deltaTime;  //处理加减速操作
				if(cwFlag)  //顺时针
                    FuncPara.CursorRotateSpeed = -SPEED_ROTATE * FuncPara.speedRate * timeStart;
				else  //逆时针
                    FuncPara.CursorRotateSpeed = SPEED_ROTATE * FuncPara.speedRate * timeStart;
                cursorRect.x = radiusRotate * Mathf.Sin(FuncPara.CursorRotateSpeed) + centerPoint.x;
                cursorRect.y = radiusRotate * Mathf.Cos(FuncPara.CursorRotateSpeed) + centerPoint.y;
			}
		}
	}
	
	//以窗口的形式显示光标
	void CursorWindow(int WindowID){
		
	}
	
	/// <summary>
	/// 光标移动.
	/// </summary>
	/// <param name='start_position'>
	/// Start_position.
	/// </param>
	/// <param name='end_position'>
	/// End_position.
	/// </param>
	public float MovingStart(Vector2 start_position, Vector2 end_position){
		xStart = start_position.x; 
		cursorRect.x = xStart;
		xEnd = end_position.x;
		yStart = start_position.y;
		cursorRect.y = yStart;
		yEnd = end_position.y;
		timeEnd = (end_position - start_position).magnitude / SPEED_MOVE;
		xSpeed = (xEnd - xStart) / timeEnd;
		ySpeed = (yEnd - yStart) / timeEnd;
		timeStart = 0;
		timeSpeed = 0;
		timeEnd = timeEnd / FuncPara.speedRate;
		movingCompute = true;
		rotateCompute = false;  //旋转失效
		FuncPara.cursorShow = true;
		return timeEnd;
	}
	
	/// <summary>
	/// 光标旋转.
	/// </summary>
	/// <param name='start_position'>
	/// 起始点位置.
	/// </param>
	/// <param name='center_point'>
	/// 旋转中心位置.
	/// </param>
	/// <param name='cw'>
	/// false：逆时针；true：顺时针.
	/// </param>
	public void RotateStart(Vector2 start_position, Vector2 center_point, bool cw){
		radiusRotate = (start_position - center_point).magnitude;
		cwFlag = cw;
		startPoint = start_position;
		centerPoint = center_point;
        FuncPara.CursorRotateSpeed = 0;
		timeStart = 0;
		timeSpeed = 0;
		rotateCompute = true;
		movingCompute = false;
		FuncPara.cursorShow = true;
		
	}
	
	/// <summary>
	/// 光标移动停止，消失.
	/// </summary>
	public void MovingStop() {
		FuncPara.cursorShow = false;
		movingCompute = false;
		rotateCompute = false;
	}
	
	/// <summary>
	/// 光标旋转停止，但是光标未消失.
	/// </summary>
	public void RotateStop(){
		rotateCompute = false;
		movingCompute = false;
	}
	
	public void ChangeRate (float set_rate, float origin_rate){
		timeEnd = timeStart + (timeEnd - timeStart) * origin_rate / set_rate;  //重新计算终点时间
		xStart = cursorRect.x;
		yStart = cursorRect.y;
		timeSpeed = 0;
	}
	
	
}
