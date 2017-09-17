using UnityEngine;
using System.Collections;
using interpreter;
using System.Collections.Generic;

namespace nodes
{
    public class Var<T> : Evaluable<T>
    {
        private string _name;
        private Dictionary<int, T> symbolTable = new Dictionary<int, T>();

        public Var(Token token, Dictionary<int, T> symbolTable)
        {
            this._name = token.value();
            this.symbolTable = symbolTable;
        }

        public string name()
        {
            return _name;
        }

        public T evaluate()
        {
            return symbolTable[int.Parse(_name)];
        }
    }
}
