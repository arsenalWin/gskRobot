using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using nodes;
using interpreter;
using System.IO;
using Newtonsoft.Json;

namespace interpreter
{
    public class GSKInterpreter
    {
        private Runnable program;
        private string basepath;
        private Dictionary<int, float[]> pointTable = new Dictionary<int, float[]>();
        private List<StackPoints> stackPoints = new List<StackPoints>();
        private float[] stackEntPoint;
        private int stackPointsCount = 0;
        private List<Token> orderPara = new List<Token>();
        private List<Token> curPara = new List<Token>();
        private List<Token> orderList = new List<Token>();
        private Lexer lexer;
        private float[] pathData = new float[8];
        private List<float[]> movcData = new List<float[]>();
        private List<string> dataInfo = new List<string>();
        private int movc_num = 0;
		private int[] outState = new int[GSKDATA.IO_MAX_NUM];
		private int[] inState = new int[GSKDATA.IO_MAX_NUM];
		private Dictionary<int, int> waitIN = new Dictionary<int, int>();
		private int tmpLine = 0;

        private Dictionary<int, int> varInt = new Dictionary<int, int>();
        private Dictionary<int, bool> varBool = new Dictionary<int, bool>();
        private Dictionary<int, float> varR = new Dictionary<int, float>();

        //wait
        private int waitIn = 0;
        private int waitValue = 0;
        private int waitTime = 0;
        private int waitBool = -1;

        //子程序
        public GSKInterpreter childInterpreter;
        public GSKInterpreter parentInterpreter;
        //
        public int parentLine = -1;
        public string parentName = "";
        private string childFile;
        private bool hasChild = false;

        public GSKInterpreter(string realPath)
        {
            //basepath = Application.streamingAssetsPath;
            basepath = realPath;
        }

        public void initChild(string file, int line, string name)
        {
            childInterpreter = new GSKInterpreter(basepath);
            childInterpreter.parentLine = line;
            childFile = file;
            childInterpreter.parentName = name;

            childInterpreter.run(childFile);
            //报错信息
            hasChild = true;
        }

        public void destoryChild()
        {
            childInterpreter = null;
            parentLine = -1;
            hasChild = false;
        }

        public bool getChild()
        {
            return hasChild;
        }

		//整体编译
        public int run(string path)
        {
            hasChild = false;
            GSKDATA.SHIFT = false;
            ClearData();
            string text = File.ReadAllText(basepath + path);
            lexer = new GSKLexer(text);
            program = new GSKParser(lexer, pointTable, orderPara, orderList, varBool,varR).parse();
            program.run();
            Debug.Log("总共的行数：" + orderList.Count);
            for (int i = 0; i < orderList.Count; i++)
            {
                if (orderList[i].type() == TokenType.MOVC)
                {
                    movc_num++;
                }
                else
                {
                    if (movc_num > 0 && movc_num < 3)
                    {
                        Debug.LogError("应该至少有三条圆弧指令！当前只有" + movc_num + "次");
                        return -1;
                    }
                    else
                    {
                        movc_num = 0;
                    }
                }
            }
            return 0;
        }

		//运行
        public string runCode(ref int line, int dir)
        {
			tmpLine = line;
            string result = "END";

            //Debug.Log(program.runCode(line));
            if (line >= orderList.Count || line < 0)
            {
                return result;
            }
            result = program.runCode(ref line, dir, curPara);

            dataInfo.Clear();
            foreach (Token t in curPara)
            {
                dataInfo.Add(t.value());
            }
            switch (result)
            {
                case "MOVJ":case "MOVL":case "MOVC":
                    if (curPara[0].type() == TokenType.PID)
                    {
                        pathData = pointTable[int.Parse(dataInfo[0])];
                    }
                    else
                    {
                        pathData = switchToArray(dataInfo[0]);
                    }
                    if (result == "MOVC")
                    {
						setMovcData(tmpLine);
                    }
                    break;
				case "DIN":
                    if (curPara[0].type() == TokenType.BNUM)
                    {
                        varBool[int.Parse(dataInfo[0])] = intToBool(inState[int.Parse(dataInfo[1])]);
                    }
                    else if(curPara[0].type() == TokenType.RNUM)
                    {
                        varR[int.Parse(dataInfo[0])] = int.Parse(dataInfo[1]);
                    }
                    else
                    {
                        inState[int.Parse(dataInfo[0])] = boolToInt(dataInfo[1]);

                    }
                    break;
				case "DOUT":
				    outState[int.Parse(dataInfo[0])] = boolToInt(dataInfo[1]);
					break;
				case "WAIT":
                    setWait();
					break;
                case "PALINI":
                    iniStack();
                    break;
                case "SET":
                    iniVar();
                    break;
                case "PALFULL":
                    fullStack();
                    break;
                case "INC":
                    incVarI();
                    break;
                case "PALPRED":
                    setStackPoint(0);
                    break;
                case "PALPREU":
                    setStackPoint(2);
                    break;
                case "PALTO":
                    setStackPoint(1);
                    break;
                case "PALENT":
                    setStackEntPoint();
                    break;
                case "SHIFTON":
                    GSKDATA.SHIFT = true;
                    GSKDATA.SHIFT_PX = readPX(dataInfo[0]);
                    break;
                case "SHIFTOFF":
                    GSKDATA.SHIFT = false;
                    break;

            }

            return result;
        }


        public int getWaitIn()
        {
            return waitIn;
        }

        public int getWaitValue()
        {
            return waitValue;
        }

        public int getWaitTime()
        {
            return waitTime;
        }

        public PXClass readPX(string name)
        {
            string fileName = "/StreamingAssets/PX/pxList";
            if (!File.Exists(Application.dataPath + fileName))
            {
                Debug.LogError("PX文件不存在！");
                return null;
            }
            string content = File.ReadAllText(Application.dataPath + fileName);
            PXList pxL = JsonConvert.DeserializeObject<PXList>(content);

            switch (name)
            {
                case "0":
                    return pxL.PX0;
                case "1":
                    return pxL.PX1;
                case "2":
                    return pxL.PX2;
                case "3":
                    return pxL.PX3;
                case "4":
                    return pxL.PX4;
                case "5":
                    return pxL.PX5;

            }
            return null;
        }

        private void setWait()
        {
            waitIn = int.Parse(dataInfo[0]);
            //waitValue = int.Parse(dataInfo[1]);
            waitValue = (dataInfo[1] == "ON") ? 1 : 0;
            waitTime = int.Parse(dataInfo[2]);
        }

        private bool intToBool(int v)
        {
            if(v <= 0)
            {
                return false;
            }
            return true;
        }

        public int getCurLine(){
			return tmpLine;		
		}

		public int getCurLine(int page)
		{
			return tmpLine % page;		
		}

		public int getOutState(int n){
			return 	outState [n];	
		}

		public int setOutState(int n, int state){
			outState [n] = state;
			return 0;
		}

		public int getInState(int n){
			return 	inState [n];	
		}

		public int setInState(int n, int state){
			inState [n] = state;
			return 0;
		}

		public int resetOutState(bool state = false){
			int st = 0;
			if (state) {
				st = 1;			
			}
			for (int i = 0; i < outState.Length; i++) {
				outState[i] = st;			
			}
			return 0;
		}

		public int resetInState(bool state = false){
			int st = 0;
			if (state) {
				st = 1;			
			}
			for (int i = 0; i < inState.Length; i++) {
				inState[i] = st;			
			}
			return 0;
		}

        public void ClearData()
        {
            pointTable = new Dictionary<int, float[]>();
            orderPara = new List<Token>();
            orderList = new List<Token>();
        }

        public float[] GetPathData()
        {
            Debug.Log(pathData[0] + ":" + pathData[1] + ":" + pathData[2] + ":" + pathData[3] + ":" + pathData[4] + ":" + pathData[5]);
            return pathData;
        }

        public List<string> GetDataInfo()
        {
            string str = "";
            for (int i = 0; i < dataInfo.Count; i++)
            {
                str = str + dataInfo[i] + ":";
            }
            //Debug.Log(str);
            return dataInfo;
        }

        public List<float[]> GetMovcData()
        {
            for (int i = 0; i < movcData.Count; i++)
            {
                Debug.Log(movcData[i][0] + ":" + movcData[i][1] + ":" + movcData[i][2] + ":" + movcData[i][3] + ":" + movcData[i][4] + ":" + movcData[i][5]);
            }
            return movcData;
        }

        private void iniStack()
        {
            iniStackPoints(readStackFile(curPara[0].value()));
        }

        private string readStackFile(string n)
        {
            string fileName = "/simpleStack_" + n;
            Debug.Log("the file name of stack is " + fileName);
            if (!File.Exists(Application.dataPath + fileName))
            {
                Debug.LogError("码垛工业文件不存在！");
                return null;
            }
            return File.ReadAllText(Application.dataPath + fileName);
        }

        private void iniStackPoints(string file)
        {
            SimpleStack simple = JsonConvert.DeserializeObject<SimpleStack>(file);
            SimpleStackWork simpleWork = new SimpleStackWork(simple);
            stackPoints = simpleWork.getStackPoints();
            stackPointsCount = stackPoints.Count;
            Debug.Log("the count of stack is " + stackPointsCount);
            stackEntPoint = simple.enterPoint;
        }

        private void setStackEntPoint()
        {
            int iNum = Int32.Parse(curPara[1].value());
            int pNum = Int32.Parse(curPara[0].value());

            pointTable[pNum] = stackEntPoint;
        }

        private void incVarI()
        {
            int varNum = Int32.Parse(curPara[0].value());
            varInt[varNum]++;
        }

        private void setStackPoint(int v)
        {
            int iNum = Int32.Parse(curPara[1].value());
            int iValue = varInt[iNum] - 1;
            int pNum = Int32.Parse(curPara[0].value());
            Debug.Log("the number of stack is: " + iValue);
            switch (v)
            {
                case 0:
                    pointTable[pNum] = stackPoints[iValue].p1;
                    break;
                case 1:
                    pointTable[pNum] = stackPoints[iValue].p2;
                    break;
                case 2:
                    pointTable[pNum] = stackPoints[iValue].p3;
                    break;
            }
        }

        private void iniVar()
        {
            int varNum = Int32.Parse(curPara[0].value());
            int value = Int32.Parse(curPara[1].value());

            switch (curPara[0].type())
            {
                case TokenType.INUM:
                    varInt.Add(varNum, value);
                    break;
                case TokenType.BNUM:
                    if (value == 0)
                    {
                        varBool[varNum] = false;
                    }
                    else
                    {
                        varBool[varNum] = true;
                    }
                    break;
                case TokenType.RNUM:
                    varR[varNum] = value;
                    break;

            }
        }

        private void fullStack()
        {
            int bNum = Int32.Parse(curPara[0].value());
            int iNum = Int32.Parse(curPara[1].value());
            int iValue = varInt[iNum];
            if (iValue <= stackPointsCount)
            {
                varBool[bNum] = false;
            }
            else
            {
                varBool[bNum] = true;
            }
        }

        private float[] switchToArray(string str)
        {
            string[] strs = str.Split(',');
            List<float> list = new List<float>();
            foreach (string s in strs)
            {
                list.Add(Convert.ToSingle(s));
            }

            return list.ToArray();
        }

        private void setMovcData(int line)
        {
            movcData.Clear();
            movcData.Add(pathData);
            float[] tmpData;
            for (int i = 1; ; i++)
            {
                int tmp = line + i;
                if (orderList[tmp].type() == TokenType.MOVC)
                {
                    program.runCode(ref tmp, 1, curPara);
                    if (curPara[0].type() == TokenType.PID)
                    {
                        tmpData = pointTable[int.Parse(curPara[0].value())];
                    }
                    else
                    {
                        tmpData = switchToArray(curPara[0].value());
                    }
                    movcData.Add(tmpData);
                }
                else
                {
                    break;
                }
            }

            for (int i = 1; ; i++)
            {
                int tmp = line - i;
                if (orderList[tmp].type() == TokenType.MOVC)
                {
                    program.runCode(ref tmp, 1, curPara);
                    if (curPara[0].type() == TokenType.PID)
                    {
                        tmpData = pointTable[int.Parse(curPara[0].value())];
                    }
                    else
                    {
                        tmpData = switchToArray(curPara[0].value());
                    }
                    movcData.Insert(0,tmpData);
                }
                else
                {
                    break;
                }
            }
        }
	    
	    private int boolToInt(string bo) {
			if (bo == "false") {
				return 0;
			}
			return 1;
		}
    }
}
