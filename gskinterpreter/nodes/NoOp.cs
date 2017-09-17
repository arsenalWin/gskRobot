using UnityEngine;
using System.Collections;
using interpreter;
using System.Collections.Generic;

namespace nodes
{
    public class NoOp : Runnable
    {
        public void run()
        {
            //
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
            return "None";
        }
    }
}
