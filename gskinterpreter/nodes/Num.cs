using UnityEngine;
using System.Collections;

namespace nodes
{
    public class Num<T> : Evaluable<T> where T : struct
    {
        private T value;

        public Num(T value)
        {
            this.value = value;
        }

        public T evaluate()
        {
            return value;
        }
    }
}
