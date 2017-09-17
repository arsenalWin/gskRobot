using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using interpreter;

namespace nodes
{
    public class PointsInfo : Runnable
    {
        private Runnable pointStatment;

        public PointsInfo(Runnable pointStatment)
        {
            this.pointStatment = pointStatment;
        }

        public void run()
        {
            pointStatment.run();
        }

        public string runCode(ref int line, int dir, List<Token> curPara)
        {
            return "PointsInfo";
        }
    }
}
