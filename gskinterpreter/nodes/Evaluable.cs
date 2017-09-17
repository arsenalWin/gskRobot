using UnityEngine;
using System.Collections;

namespace nodes
{
    public interface Evaluable<T>
    {
        T evaluate();
    }
}