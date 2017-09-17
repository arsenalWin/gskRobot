using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using interpreter;

public interface Lexer
{
    List<Token> tokenize();
}
