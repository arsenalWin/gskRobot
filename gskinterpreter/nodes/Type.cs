using UnityEngine;
using System.Collections;
using interpreter;

namespace nodes
{
    public class Type
    {
        private string value;

        public Type(Token token)
        {
            this.value = token.type().ToString();
        }

        public string type()
        {
            return value;
        }
    }
}
