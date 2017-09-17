using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using interpreter;

namespace nodes
{
    public class Assign<T> : Runnable
    {
        private Dictionary<int, T> variableTable;

        private int varible;

        private T expression;

        public Assign(int vname, T right, Dictionary<int, T> vTable)
        {
            varible = vname;
            expression = right;
            variableTable = vTable; 
        }

        public void run()
        {
            variableTable.Add(varible, expression);
        }

        public string runCode(ref int line, int dir, List<Token> curPara)
        {
            if (dir > 0)
            {
                line++;
            }
            else
            {
                line--;
            }
            return "internalCA";
        }
    }
}
