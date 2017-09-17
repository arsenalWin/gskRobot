using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using interpreter;

namespace nodes
{
    public class Block : Runnable
    {
        private Runnable compoundStatment;

        public Block(Runnable compoundStatment)
        {
            this.compoundStatment = compoundStatment;
        }

        public void run()
        {
            compoundStatment.run();
        }

        public string runCode(ref int line, int dir, List<Token> curPara)
        {
            return compoundStatment.runCode(ref line, dir, curPara);
        }
    }
}

