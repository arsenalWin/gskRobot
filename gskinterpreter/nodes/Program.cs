using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using interpreter;

namespace nodes
{
    public class Program : Runnable
    {
        private PointsInfo pointsInfo;
        private Block block;

        public Program(PointsInfo pointsInfo, Block block) {
            this.pointsInfo = pointsInfo;
            this.block = block;
        }

        public void run() {
            pointsInfo.run();
            block.run();
        }

        public string runCode(ref int line, int dir, List<Token> curPara)
        {
            return block.runCode(ref line, dir, curPara);
        }
    }
}
