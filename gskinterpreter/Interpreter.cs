using System.Collections.Generic;

namespace interpreter {

	public interface Interpreter {
		//already  exit -----------------
		int run (string path); 
		string runCode(ref int line, int dir);
		void ClearData();
		float[] GetPathData();
		List<string> GetDataInfo();
		List<float[]> GetMovcData();

		//add new function --------------
		int getCurLine(); //在文件中的第几行（不包括前面的位置信息）

		/// <summary>
		/// Gets the current line.
		/// </summary>
		/// <returns>在页面中的行号</returns>
		/// <param name="pageLines">页面可显示的行数</param>
		int getCurLine(int pageLines);

		// 返回第n个输出端口的状态
		//0 -- 关闭
		//1 -- 打开
		//n = {0~31}
		int getOutState(int n);

		//设置第n个输出端口的状态
		int setOutState(int n);

		// 返回第n个输入端口的状态
		//0 -- 关闭
		//1 -- 打开
		//n = {0~31}
		int getInState(int n);

		//设置第n个输入端口的状态
		int setInState(int n);

		//重置所有输出端口
		//默认情况下全部关闭
		int resetOutState(bool state = false);

		//重置所有输入端口
		//默认情况下全部关闭
		int resetInState(bool state = false);

		//判断当前程序是否存在循环
		bool existLoop();

		//跳到程序的指定行号
		//跳转失败返回false
		bool jumpToLine(int line);
	}
}