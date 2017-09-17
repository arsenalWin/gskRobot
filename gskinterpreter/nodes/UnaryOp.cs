using UnityEngine;
using System.Collections;
using interpreter;

namespace nodes
{
    public class UnaryOp : Evaluable<decimal>
    {
        private Token op;
        private Evaluable<decimal> right;

        public UnaryOp (Token op, Evaluable<decimal> right) {
            this.op = op;
            this.right = right;
        }

        public decimal evaluate()
        {
            if (op.type() == TokenType.PLUS)
            {
                return right.evaluate();
            }
            else if (op.type() == TokenType.MINUS)
            {
                return -right.evaluate();
            }
            else
            {
                Debug.LogError("Invalid operation");
                return 0;
            }
        }
    }
}