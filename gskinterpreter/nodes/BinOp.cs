using UnityEngine;
using System.Collections;
using interpreter;

namespace nodes
{
    public class BinOp : Evaluable<decimal>
    {
        private Token op;
        private Evaluable<decimal> left;
        private Evaluable<decimal> right;

        public BinOp(Token op, Evaluable<decimal> left, Evaluable<decimal> right)
        {
            this.op = op;
            this.left = left;
            this.right = right;
        }

        public decimal evaluate()
        {
            if (op.type() == TokenType.PLUS)
            {
                return left.evaluate() + right.evaluate();
            }
            else if (op.type() == TokenType.MINUS)
            {
                return left.evaluate() - right.evaluate();
            }
            else if (op.type() == TokenType.MUL)
            {
                return left.evaluate() * right.evaluate();
            }
            else
            {
                Debug.LogError("Invail operation");
                return left.evaluate();
            }
        }
    }

}