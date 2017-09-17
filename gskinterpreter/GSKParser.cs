using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using nodes;
using System.IO;
using Newtonsoft.Json;
using System;

namespace interpreter
{
    public class GSKParser : Parser
    {
        private IEnumerator<Token> tokens;
        private Token currentToken;
        private Dictionary<int, float[]> pointTable = new Dictionary<int, float[]>();
        private List<Token> orderPara = new List<Token>();
        private List<Token> orderList = new List<Token>();
        private List<StackPoints> stackPoints = new List<StackPoints>();
        private float[] stackEntPoint;
        private Dictionary<int, int> varInt = new Dictionary<int, int>();
        private Dictionary<int, bool> varBool;
        private Dictionary<int, float> varReal;
        private int stackPointsCount = 0;


        public GSKParser(Lexer lexer, Dictionary<int, float[]> pointTable, List<Token> orderPara, List<Token> orderList, Dictionary<int, bool> varBool, Dictionary<int, float> varR)
        {
            this.tokens = lexer.tokenize().GetEnumerator();
            this.tokens.MoveNext();
            this.currentToken = this.tokens.Current;
            this.pointTable = pointTable;
            this.orderPara = orderPara;
            this.orderList = orderList;
            this.varBool = varBool;
            this.varReal = varR;
        }

        public Runnable parse()
        {
            Runnable astTree = program();
            if (currentToken.type() != TokenType.EOF)
            {
                error(currentToken.type());
            }
            return astTree;
        }

        private Program program()
        {
            //
            PointsInfo points = point();
            Block blocks = block();
            return new Program(points, blocks);
        }

        private PointsInfo point()
        {
            return new PointsInfo(pointStatement());
        }

        private Block block()
        {
            return new Block(compoundStatement());
        }

        private Runnable pointStatement()
        {
            List<Runnable> nodesU = uPointList();
            List<Runnable> nodesT = tPointList();
            List<Runnable> nodes = pointList();
            return new Compound(nodes);
        }

        private Runnable compoundStatement()
        {
            List<Runnable> nodes = statementList();
            return new Compound(nodes);
        }

        private List<Runnable> statementList()
        {
            List<Runnable> statements = new List<Runnable>();
            Runnable node = statement();
            statements.Add(node);
            while (currentToken.type() == TokenType.SEMI || currentToken.type() == TokenType.COLON) {
                if(currentToken.type() == TokenType.SEMI)
                {
                    eat(TokenType.SEMI);
                }
                else
                {
                    eat(TokenType.COLON);
                }
                node = statement();
                if(node.GetType() != typeof(NoOp)){
                    statements.Add(node);
                }
                else
                {
                    break;
                }
            }
            return statements;
        }

        private Runnable statement()
        {
            Token curtoken = currentToken;
            switch (currentToken.type())
            {
                case TokenType.MAIN: case TokenType.END:
                case TokenType.NOTES:
                case TokenType.RET:
                case TokenType.ENDWHILE:
                case TokenType.ELSE:
                case TokenType.ENDIF:
                    eat(currentToken.type());
                    return new Order(curtoken, orderPara, orderList);
                case TokenType.MOVJ:
                case TokenType.MOVL:
                case TokenType.MOVC:
                case TokenType.DOUT:
                case TokenType.DIN:
                case TokenType.WAIT:
                case TokenType.SPEED:
                case TokenType.DYN:
                case TokenType.DELAY:
                case TokenType.PALINI:
                case TokenType.SET:
                case TokenType.PALFULL:
                case TokenType.INC:
                case TokenType.PALPREU:
                case TokenType.PALPRED:
                case TokenType.PALTO:
                case TokenType.PALENT:
                case TokenType.ARCON:
                case TokenType.ARCOF:
                case TokenType.CALL:
                case TokenType.SHIFTOFF:
                case TokenType.SHIFTON:
                case TokenType.PULSE:
                    eat(currentToken.type());
                    restorePara();
                    return new Order(curtoken, orderPara, orderList);
                case TokenType.WHILE:
                case TokenType.IF:
                    eat(currentToken.type());
                    restorePara();
                    return new Order(curtoken, orderPara, orderList, varBool);
                case TokenType.JUMP:
                    eat(currentToken.type());
                    restorePara();
                    return new Order(curtoken, orderPara, orderList, varReal);
                case TokenType.LAB:
                    eat(currentToken.type());
                    return new Order(curtoken, orderPara, orderList);
                default:
                    return empty();
            }
        }

        private void setStackEntPoint()
        {
            orderPara.Clear();
            int iNum = 0;
            int pNum = 0;
            while (currentToken.type() != TokenType.SEMI)
            {
                if (currentToken.type() == TokenType.PID)
                {
                    pNum = Int32.Parse(currentToken.value());
                }
                else if (currentToken.type() == TokenType.INUM)
                {
                    iNum = Int32.Parse(currentToken.value());
                }
                orderPara.Add(currentToken);
                eat(currentToken.type());
            }
            pointTable[pNum] = stackEntPoint;
        }

        private void setStackPoint(int v)
        {
            orderPara.Clear();
            int iNum = 0;
            int pNum = 0;
            while (currentToken.type() != TokenType.SEMI)
            {
                if (currentToken.type() == TokenType.PID)
                {
                    pNum = Int32.Parse(currentToken.value());
                }
                else if(currentToken.type() == TokenType.INUM)
                {
                    iNum = Int32.Parse(currentToken.value());
                }
                orderPara.Add(currentToken);
                eat(currentToken.type());
            }

            switch (v)
            {
                case 0:
                    pointTable[pNum] = stackPoints[iNum].p1;
                    break;
                case 1:
                    pointTable[pNum] = stackPoints[iNum].p2;
                    break;
                case 2:
                    pointTable[pNum] = stackPoints[iNum].p3;
                    break;
            }
        }

        private void incVarI()
        {
            orderPara.Clear();
            int varNum = 0;
            while (currentToken.type() != TokenType.SEMI)
            {
                if (currentToken.type() == TokenType.INUM)
                {
                    varNum = Int32.Parse(currentToken.value());
                    varInt[varNum]++;
                }
                orderPara.Add(currentToken);
                eat(currentToken.type());
            }
        }

        private void fullStack()
        {
            orderPara.Clear();
            int bNum = 0;
            int iNum = 0;
            while (currentToken.type() != TokenType.SEMI)
            {
                if(currentToken.type() == TokenType.BNUM)
                {
                    bNum = Int32.Parse(currentToken.value());
                }
                else if (currentToken.type() == TokenType.INUM)
                {
                    iNum = Int32.Parse(currentToken.value());
                }
                orderPara.Add(currentToken);
                eat(currentToken.type());
            }
            if(iNum <= stackPointsCount)
            {
                varBool[iNum] = true;
            }
            else
            {
                varBool[bNum] = false;
            }
        }

        private void iniVar()
        {
            orderPara.Clear();
            int varNum = 0;
            int value = 0;
            bool varI = false;
            while (currentToken.type() != TokenType.SEMI)
            {
                if (currentToken.type() == TokenType.INUM)
                {
                    varNum = Int32.Parse(currentToken.value());
                    varI = true;
                    
                }
                else if (currentToken.type() == TokenType.BNUM)
                {
                    varNum = Int32.Parse(currentToken.value());
                    varI = false;
                }
                else if (currentToken.type() == TokenType.VALUE)
                {
                    value = Int32.Parse(currentToken.value());
                }
                orderPara.Add(currentToken);
                eat(currentToken.type());
            }

            if (varI)
            {
                varInt.Add(varNum, value);
            }
            else
            {
                if(value == 0)
                {
                    varBool[varNum] = false;
                }
                else
                {
                    varBool[varNum] = true;
                }
            }
        }

        private void iniStack()
        {
            orderPara.Clear();
            while (currentToken.type() != TokenType.SEMI)
            {
                if(currentToken.type() == TokenType.IDNUM)
                {
                    iniStackPoints(readStackFile(currentToken.value()));
                }
                orderPara.Add(currentToken);
                eat(currentToken.type());
            }
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
            stackEntPoint = simple.enterPoint;
        }

        private void restorePara()
        {
            orderPara.Clear();
            while (currentToken.type() != TokenType.SEMI && currentToken.type() != TokenType.COLON)
            {
                orderPara.Add(currentToken);
                eat(currentToken.type());
            }        
        }

        private List<Runnable> uPointList()
        {
            List<Runnable> pointAssign = new List<Runnable>();
            while (currentToken.type() == TokenType.UID)
            {
                eat(TokenType.UID);
                eat(TokenType.ASSIGN);
                List<float> tmp_data = new List<float>();
                float result = const_num();
                while (result != 10000)
                {
                    tmp_data.Add(result);
                    result = const_num();
                }
                eat(TokenType.SEMI);
            }
            return pointAssign;
        }

        private List<Runnable> tPointList()
        {
            List<Runnable> pointAssign = new List<Runnable>();
            while (currentToken.type() == TokenType.TIME)
            {
                eat(TokenType.TIME);
                eat(TokenType.ASSIGN);
                List<float> tmp_data = new List<float>();
                float result = const_num();
                while (result != 10000)
                {
                    tmp_data.Add(result);
                    result = const_num();
                }
                eat(TokenType.SEMI);
            }
            return pointAssign;
        }


        private List<Runnable> pointList()
        {
            List<Runnable> pointAssign = new List<Runnable>();
            while (currentToken.type() == TokenType.PID)
            {
                Var<float[]> left = variable();
                eat(TokenType.ASSIGN);
                List<float> tmp_data = new List<float>();
                float result = const_num();
                while (result != 10000)
                {
                    tmp_data.Add(result);
                    result = const_num();
                }
                //for (int i = 0; i < 8; i++)
                //{
                //    tmp_data.Add(const_num());
                //}
                pointAssign.Add(new Assign<float[]>(int.Parse(left.name()), tmp_data.ToArray(), pointTable));
                eat(TokenType.SEMI);
            }
            return pointAssign;
        }

        private Var<float[]> variable()
        {
            Var<float[]> node = new Var<float[]>(currentToken, pointTable);
            eat(TokenType.PID);
            return node;
        }

        //private Var<float[]> variableU()
        //{
        //    Var<float[]> node = new Var<float[]>(currentToken, pointTable);
        //    eat(TokenType.UID);
        //    return node;
        //}

        //private Var<float[]> variableT()
        //{
        //    Var<float[]> node = new Var<float[]>(currentToken, pointTable);
        //    eat(TokenType.TIME);
        //    return node;
        //}

        private void eat(TokenType type)
        {
            if (currentToken.type() == type)
            {
                Debug.Log(string.Format("eat the token {0} successfully", currentToken.type().ToString()));
                tokens.MoveNext();
                currentToken = tokens.Current;
                while (currentToken.type() == TokenType.COMMA)
                {
                    tokens.MoveNext();
                    currentToken = tokens.Current;
                }
            }
            else
            {
                error(type);
            }
        }

        private float const_num()
        {
            Token token = currentToken;
            if (token.type() == TokenType.REAL_CONST)
            {
                eat(TokenType.REAL_CONST);
            }
            else
            {
                eat(TokenType.INTEGER_CONST);
            }
            float result;
            if (float.TryParse(token.value(), out result))
            {
                return result;
            }
            else
            {
                Debug.Log("string could not be parsed");
                return 10000;
            }
        }

        private void error(TokenType type)
        {
            string message = string.Format("Invalid syntax: expected token type is {0} but actual one is {1}.",
                    type.ToString(), currentToken.type().ToString());
            Debug.Log(message);
        }

        private Runnable empty()
        {
            return new NoOp();
        }
    }
}