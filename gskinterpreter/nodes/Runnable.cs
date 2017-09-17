using UnityEngine;
using System.Collections;
using interpreter;
using System.Collections.Generic;

namespace nodes
{
    public interface Runnable
    {
        void run();
        string runCode(ref int line, int dir, List<Token> curPara);
    }
}
