using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using interpreter;

namespace nodes
{
    public class Compound : Runnable
    {
        private List<Runnable> children;

        public Compound(List<Runnable> children)
        {
            this.children = children;
        }

        public void run()
        {
            children.ForEach(c => c.run());
        }

        public string runCode(ref int line, int dir, List<Token> curPara)
        {
            return children[line].runCode(ref line, dir, curPara);
        }
    }
}