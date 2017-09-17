using UnityEngine;
using System.Collections;

namespace interpreter
{
    public enum TokenType
    {
        PROGRAM,
        VAR,
        BEGIN,
        DOT,
        COMMA,
        COLON,
        SEMI,
        ID,
        ASSIGN,
        INTEGER_CONST,
        REAL_CONST,
        INTEGER,
        REAL,
        PLUS,
        MINUS,
        MUL,
        INTEGER_DIV,
        REAL_DIV,
        LPAREN,
        RPAREN,
        EOF,
        MAIN,
        END,
        PID,
        TMP_PID,
        MOVJ,
        MOVL,
        MOVC,
        VNUM,
        ZNUM,
        DOUT,
        DIN,
        OTNUM,
        INNUM,
        IDNUM,
        INUM,
        VALUE,
        ON,
        OFF,
        NOTES,
        WAIT,
        DELAY,
        TIME,
        TIME_REAL,
        PALINI,
        SET,
        PALFULL,
        WHILE,
        SPEED,
        DYN,
        PALENT,
        PALPREU,
        PALPRED,
        PALTO,
        INC,
        ENDWHILE,
        ACC,
        BNUM,
        NE,
        DO,
        JNUM,
        DCC,
        EQ,
        FALSE,
        TRUE,
        THEN,
        IF,
        ELSE,
        ENDIF,
        ENUM,
        EVNUM,
        ARCON,
        ARCOF,
        ACNUM,
        AVNUM,
        UID,
        RET,
        LAB,
        RNUM,
        IGNUM,
        JUMP,
        EQUAL,
        CALL,
        PXNUM,
        SHIFTON,
        SHIFTOFF,
        PULSE,
        FILE,
    }

    public class Token
    {
        
        private TokenType _type;
        private string _value;

        public Token(TokenType type, string value)
        {
            this._type = type;
            this._value = value;
        }

        public TokenType type()
        {
            return _type;
        }

        public string value()
        {
            return _value;
        }

        public string toString()
        {
            return string.Format("Token(%s, \"%s\")", _type, _value);
        }

    }
}

