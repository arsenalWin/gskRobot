using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using interpreter;
using UnityEngine;

namespace nodes
{
    class Order : Runnable
    {
        private Token tokens;
        private List<Token> orderPara = new List<Token>();
        private List<Token> orderList;
        private Dictionary<int, bool> varBList;
        private Dictionary<int, float> varRList;

        public Order(Token tokens, List<Token> orderPara, List<Token> orderList)
        {
            this.tokens = tokens;
            this.orderList = orderList;
            foreach (Token t in orderPara)
            {
                this.orderPara.Add(t);
            }
            varBList = null;
        }

        public Order(Token tokens, List<Token> orderPara, List<Token> orderList, Dictionary<int, bool> varB)
        {
            this.tokens = tokens;
            this.varBList = varB;
            this.orderList = orderList;
            foreach (Token t in orderPara)
            {
                this.orderPara.Add(t);
            }
        }

        public Order(Token tokens, List<Token> orderPara, List<Token> orderList, Dictionary<int, float> varR)
        {
            this.tokens = tokens;
            this.varRList = varR;
            this.orderList = orderList;
            foreach (Token t in orderPara)
            {
                this.orderPara.Add(t);
            }
        }

        public void run()
        {
            orderList.Add(tokens);
        }

        public string runCode(ref int line, int dir, List<Token> curPara)
        {
            int lineTmp = line;
            if (dir > 0)
            {
                line++;
            }
            else
            {
                line--;
            }

            if (tokens.type() == TokenType.WHILE && varBList != null)
            {
                if(orderPara.Count > 3)
                {
                    if(orderPara[1].type() == TokenType.NE)
                    {
                        if(!(varBList[Int32.Parse(orderPara[0].value())] != varBList[Int32.Parse(orderPara[2].value())]))
                        {
                            //end
                            line = findEndWhile(lineTmp);
                        }
                    }
                }
                else
                {
                    if (!varBList[Int32.Parse(orderPara[0].value())])
                    {
                        //end
                        line = findEndWhile(lineTmp);
                    }
                }
            }

            if(tokens.type() == TokenType.ENDWHILE)
            {
                line = findWhile(lineTmp);
            }

            if(tokens.type() == TokenType.IF && varBList != null)
            {
                if (orderPara.Count > 3)
                {
                    if (orderPara[1].type() == TokenType.EQ)
                    {
                        bool tmp = false;
                        if(orderPara[2].type() == TokenType.FALSE)
                        {
                            tmp = false;
                        }
                        else if (orderPara[2].type() == TokenType.TRUE)
                        {
                            tmp = true;
                        }

                        if (!(varBList[Int32.Parse(orderPara[0].value())] == tmp))
                        {
                            //findELSE
                            line = findELSE(lineTmp);
                        }
                    }
                }
            }

            if (tokens.type() == TokenType.JUMP)
            {
                if (orderPara.Count == 5)
                {
                    if (varRList.Count > 0)
                    {
                        switch (orderPara[3].type())
                        {
                            case TokenType.EQUAL:
                                if(varRList[Int32.Parse(orderPara[2].value())] == float.Parse(orderPara[4].value()))
                                {
                                    line = findLAB(int.Parse(orderPara[0].value()));
                                }
                                break;
                        }
                    }
                }


            }

            if (tokens.type() == TokenType.ELSE)
            {
                line = findENDIF(lineTmp);

            }

            if(tokens.type() == TokenType.END)
            {
                line = -1;
            }

            curPara.Clear();
            foreach (Token t in orderPara)
            {
                curPara.Add(t);
            }
            return tokens.type().ToString();
        }

        private int findENDIF(int lineTmp)
        {
            for (int i = lineTmp; i < orderList.Count; i++)
            {
                if (orderList[i].type() == TokenType.ENDIF)
                {
                    return ++i;
                }
            }
            return -1;
        }

        private int findELSE(int lineTmp)
        {
            for (int i = lineTmp; i < orderList.Count; i++)
            {
                if (orderList[i].type() == TokenType.ELSE)
                {
                    return ++i;
                }
            }
            return -1;
        }

        private int findEndWhile(int line)
        {
            for (int i = line; i < orderList.Count; i++)
            {
                if (orderList[i].type() == TokenType.ENDWHILE)
                {
                    return ++i;
                }
            }
            return -1;
        }

        private int findWhile(int line)
        {
            for (int i = line; i >0; i--)
            {
                if (orderList[i].type() == TokenType.WHILE)
                {
                    return i;
                }
            }
            return -1;
        }

        private int findLAB(int num)
        {
            for(int i = 0; i < orderList.Count; i++)
            {
                if (orderList[i].type() == TokenType.LAB && orderList[i].value() == num.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
