using UnityEngine;
using System.Collections;
using nodes;

namespace interpreter
{
    public interface Parser
    {
        Runnable parse();
    }
}