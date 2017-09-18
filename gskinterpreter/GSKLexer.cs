using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using interpreter;
using System;

namespace interpreter
{
    public class GSKLexer : Lexer
    {
        private static char NONE = '\0';
        private static Dictionary<string, Token> reservedKeywords = new Dictionary<string, Token>();

        private char[] text;
        private int pos = 0;

        private List<Token> tokens = new List<Token>();

        static GSKLexer()
        {
            reservedKeywords.Add("INTEGER", new Token(TokenType.INTEGER, "INTEGER"));
            reservedKeywords.Add("REAL", new Token(TokenType.INTEGER, "REAL"));
            reservedKeywords.Add("DIV", new Token(TokenType.INTEGER_DIV, "INTEGER_DIV"));
            reservedKeywords.Add("MAIN", new Token(TokenType.MAIN, "MAIN"));
            reservedKeywords.Add("MOVJ", new Token(TokenType.MOVJ, "MOVJ"));
            reservedKeywords.Add("MOVL", new Token(TokenType.MOVL, "MOVL"));
            reservedKeywords.Add("MOVC", new Token(TokenType.MOVC, "MOVC"));
            reservedKeywords.Add("DOUT", new Token(TokenType.DOUT, "DOUT"));
            reservedKeywords.Add("DIN", new Token(TokenType.DIN, "DIN"));
            reservedKeywords.Add("ON", new Token(TokenType.ON, "ON"));
            reservedKeywords.Add("OFF", new Token(TokenType.OFF, "OFF"));
            reservedKeywords.Add("WAIT", new Token(TokenType.WAIT, "WAIT"));
            reservedKeywords.Add("DELAY", new Token(TokenType.DELAY, "DELAY"));
            reservedKeywords.Add("END", new Token(TokenType.END, "END"));
            reservedKeywords.Add("PALINI", new Token(TokenType.PALINI, "PALINI"));
            reservedKeywords.Add("SET", new Token(TokenType.SET, "SET"));
            reservedKeywords.Add("PALFULL", new Token(TokenType.PALFULL, "PALFULL"));
            reservedKeywords.Add("WHILE", new Token(TokenType.WHILE, "WHILE"));
            reservedKeywords.Add("SPEED", new Token(TokenType.SPEED, "SPEED"));
            reservedKeywords.Add("DYN", new Token(TokenType.DYN, "DYN"));
            reservedKeywords.Add("PALENT", new Token(TokenType.PALENT, "PALENT"));
            reservedKeywords.Add("PALPREU", new Token(TokenType.PALPREU, "PALPREU"));
            reservedKeywords.Add("PALPRED", new Token(TokenType.PALPRED, "PALPRED"));
            reservedKeywords.Add("PALTO", new Token(TokenType.PALTO, "PALTO"));
            reservedKeywords.Add("INC", new Token(TokenType.INC, "INC"));
            reservedKeywords.Add("ENDWHILE", new Token(TokenType.ENDWHILE, "ENDWHILE"));
            reservedKeywords.Add("NE", new Token(TokenType.NE, "NE"));
            reservedKeywords.Add("DO", new Token(TokenType.DO, "DO"));
            reservedKeywords.Add("EQ", new Token(TokenType.EQ, "EQ"));
            reservedKeywords.Add("IF", new Token(TokenType.IF, "IF"));
            reservedKeywords.Add("ELSE", new Token(TokenType.ELSE, "ELSE"));
            reservedKeywords.Add("ENDIF", new Token(TokenType.ENDIF, "ENDIF"));
            reservedKeywords.Add("THEN", new Token(TokenType.THEN, "THEN"));
            reservedKeywords.Add("FALSE", new Token(TokenType.FALSE, "FALSE"));
            reservedKeywords.Add("TRUE", new Token(TokenType.TRUE, "TRUE"));
            reservedKeywords.Add("ARCON", new Token(TokenType.ARCON, "ARCON"));
            reservedKeywords.Add("ARCOF", new Token(TokenType.ARCOF, "ARCOF"));
            reservedKeywords.Add("RET", new Token(TokenType.RET, "RET"));
            reservedKeywords.Add("JUMP", new Token(TokenType.JUMP, "JUMP"));
            reservedKeywords.Add("CALL", new Token(TokenType.CALL, "CALL"));
            reservedKeywords.Add("SHIFTON", new Token(TokenType.SHIFTON, "SHIFTON"));
            reservedKeywords.Add("SHIFTOFF", new Token(TokenType.SHIFTOFF, "SHIFTOFF"));
            reservedKeywords.Add("PULSE", new Token(TokenType.PULSE, "PULSE"));
        }

        public GSKLexer(string text)
        {
            this.text = text.ToCharArray();
        }

        public List<Token> tokenize()
        {
            if (tokens.Count != 0)
            {
                return tokens;
            }
            Token token = nextToken();
            while (token.type() != TokenType.EOF)
            {
                tokens.Add(token);
                if (token.type() == TokenType.NOTES)
                {
                    tokens.Add(new Token(TokenType.SEMI, ";"));
                }
                token = nextToken();
                //Debug.Log(token.type());
            }
            tokens.Add(token);
            return tokens;
        }

        private Token nextToken()
        {
            char character = currentChar();

            if (char.IsWhiteSpace(character))
            {
                skipWhitespaces();
                character = currentChar();
            }

            while (character == '#')
            {
                skipComment();
                character = currentChar();
                //return new Token(TokenType.NOTES, "#");
            }

            if (character == 'P' && char.IsDigit(peekedChar()))
            {
                return pid();
            }

            if (character == 'U' && char.IsDigit(peekedChar()))
            {
                return uid();
            }

            if (character == 'P' && peekedChar() == '*')
            {
                return tmp_pid();
            }

            if (character == 'P' && peekedChar() == 'X')
            {
                return px_num();
            }

            if (character == '-' && char.IsDigit(peekedChar()))
            {
                return neg_num();
            }

            if (character == 'V' && char.IsDigit(peekedChar()))
            {
                return vnum();
            }

            if (character == 'V' && peekedChar() == 'A')
            {
                return value();
            }

            if (character == 'Z' && char.IsDigit(peekedChar()))
            {
                return znum();
            }

            if (character == 'E' && char.IsDigit(peekedChar()))
            {
                return eNum();
            }

            if (character == 'E' && peekedChar() == 'V')
            {
                return evnum();
            }

            if (character == 'O' && peekedChar() == 'T')
            {
                return otnum();
            }

            if (character == 'I' && peekedChar() == 'N')
            {
                Token tmp = innum();
                if (tmp != null)
                {
                    return tmp;
                }
            }

            if (character == 'I' && peekedChar() == 'D')
            {
                return idnum();
            }

            if (character == 'I' && peekedChar() == 'G')
            {
                return ignum();
            }

            if (character == 'T' && peekedChar() == 'Y')
            {
                return type();
            }

            if (character == 'A' && peekedChar() == 'C')
            {
                return acnum();
            }

            if (character == 'A' && peekedChar() == 'V')
            {
                return avnum();
            }

            if (character == 'S' && peekedChar() == 'P')
            {
                Token tmp = spnum();
                if(tmp != null)
                {
                    return tmp;
                }
            }

            if (character == 'I' && char.IsDigit(peekedChar()))
            {
                return varI();
            }

            if (character == 'B' && char.IsDigit(peekedChar()))
            {
                return varB();
            }

            if (character == 'T' && char.IsDigit(peekedChar()))
            {
                return time();
            }

            if (character == 'J' && char.IsDigit(peekedChar()))
            {
                return varR();
            }

            if (character == 'R' && char.IsDigit(peekedChar()))
            {
                return jnum();
            }

            if (character == 'A' && peekedChar() == 'C')
            {
                return acc();
            }

            if (character == 'D' && peekedChar() == 'C')
            {
                return dcc();
            }

            if (character == 'L' && peekedChar() == 'A')
            {
                return lab();
            }

            if (char.IsDigit(character))
            {
                return number();
            }

            if (character == '=' && peekedChar() == '=')
            {
                character = nextChar();
                character = nextChar();
                return new Token(TokenType.EQUAL, "==");
            }

            if (character == '=')
            {
                character = nextChar();
                return new Token(TokenType.ASSIGN, "=");
            }

            if (character == ';')
            {
                character = nextChar();
                return new Token(TokenType.SEMI, ";");
            }

            if (character == ',')
            {
                character = nextChar();
                return new Token(TokenType.COMMA, ",");

            }

            if (character == ':')
            {
                nextChar();
                return new Token(TokenType.COLON, ":");
            }

            if (char.IsLetterOrDigit(character))
            {
                return id();
            }
            
            if (character == NONE)
            {
                return new Token(TokenType.EOF, "None");
            }
            else
            {
                Debug.LogError(string.Format("Illegal character '%s' at position %d, ", currentChar(), pos));
                return new Token(TokenType.END, "Warnning");
            }
        }

        private Token px_num()
        {
            nextChar();
            char character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.PXNUM, num);
        }

        private Token ignum()
        {
            char character = nextChar();
            character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.IGNUM, num);
        }

        private Token varR()
        {
            string r_num = "";
            char character = nextChar();
            while (character != NONE && char.IsDigit(character))
            {
                r_num = r_num + character;
                character = this.nextChar();
            }
            return new Token(TokenType.RNUM, r_num);
        }

        private Token lab()
        {
            nextChar();
            nextChar();
            string lab_num = "";
            char character = nextChar();
            while (character != NONE && char.IsDigit(character))
            {
                lab_num = lab_num + character;
                character = this.nextChar();
            }
            return new Token(TokenType.LAB, lab_num);
        }

        private Token uid()
        {
            string uid_num = "";
            char character = nextChar();
            while (character != NONE && char.IsDigit(character))
            {
                uid_num = uid_num + character;
                character = this.nextChar();
            }
            return new Token(TokenType.UID, uid_num);
        }

        private Token avnum()
        {
            char character = nextChar();
            character = nextChar();

            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.AVNUM, num);
        }

        private Token acnum()
        {
            char character = nextChar();
            character = nextChar();

            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.ACNUM, num);
        }

        private Token evnum()
        {
            char character = nextChar();
            character = nextChar();

            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.EVNUM, num);
        }

        private Token eNum()
        {
            char character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.ENUM, num);
        }

        private Token dcc()
        {
            char character = nextChar();
            character = nextChar();
            character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.DCC, num);
        }

        private Token acc()
        {
            char character = nextChar();
            character = nextChar();
            character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.ACC, num);
        }

        private Token jnum()
        {
            char character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.RNUM, num);
        }

        private Token spnum()
        {
            char character = nextChar();
            character = nextChar();
            if(!char.IsDigit(character))
            {
                character = lastChar();
                character = lastChar();
                return null;
            }

            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.ACC, num);
        }

        private Token varB()
        {
            string b_num = "";
            char character = nextChar();
            while (character != NONE && char.IsDigit(character))
            {
                b_num = b_num + character;
                character = this.nextChar();
            }
            return new Token(TokenType.BNUM, b_num);
        }

        private Token value()
        {
            char character = nextChar();
            character = nextChar();
            character = nextChar();
            character = nextChar();
            character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.VALUE, num);
        }

        private Token varI()
        {
            string i_num = "";
            char character = nextChar();
            while (character != NONE && char.IsDigit(character))
            {
                i_num = i_num + character;
                character = this.nextChar();
            }
            return new Token(TokenType.INUM, i_num);
        }

        private Token type()
        {
            char character = nextChar();
            character = nextChar();
            character = nextChar();
            character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.ACC, num);
        }

        private char currentChar()
        {
            if (pos > text.Length - 1)
            {
                return NONE;
            }
            else
            {
                return text[pos];
            }
        }

        private char nextChar()
        {
            pos = pos + 1;
            return currentChar();
        }

        private char lastChar()
        {
            pos = pos - 1;
            return currentChar();
        }

        private char peekedChar()
        {
            if (pos + 1 > text.Length - 1)
            {
                return NONE;
            }
            else
            {
                return text[pos + 1];
            }
        }

        private void skipWhitespaces()
        {
            char character = currentChar();
            while (character != NONE && char.IsWhiteSpace(character))
            {
                character = this.nextChar();
            }
        }

        private void skipComment()
        {
            char character = currentChar();
            while (character != '\n' && character != NONE)
            {
                character = this.nextChar();
            }
            nextChar();
        }

        private Token number()
        {
            char character = currentChar();
            Token token;
            string number = "";

            while (character != NONE && char.IsDigit(character))
            {
                number = number + character;
                character = this.nextChar();
            }

            if (character == '.')
            {
                number = number + character;
                character = this.nextChar();

                while (character != NONE && char.IsDigit(character))
                {
                    number = number + character;
                    character = this.nextChar();
                }

                token = new Token(TokenType.REAL_CONST, number);
            }
            else
            {
                token = new Token(TokenType.INTEGER_CONST, number);
            }

            if (char.IsLetter(character))
            {
                Debug.LogError(string.Format("Illegal character '%s' at position %d, ", currentChar(), pos));
            }

            return token;
        }

        private Token pid()
        {
            string pid_num = "";
            char character = nextChar();
            while (character != NONE && char.IsDigit(character))
            {
                pid_num = pid_num + character;
                character = this.nextChar();
            }
            return new Token(TokenType.PID, pid_num);
        }

        private Token tmp_pid()
        {
            char character = nextChar();
            character = nextChar();
            string tmp_pid_array = "";
            if (character == '(')
            {
                character = nextChar();
                while (character != NONE && character != ')')
                {
                    tmp_pid_array += character;
                    character = nextChar();
                }
                character = nextChar();
            }
            else
            {
                Debug.LogError("Invail P*");
            }
            return new Token(TokenType.TMP_PID, tmp_pid_array);
        }

        private Token vnum()
        {
            char character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.VNUM, num);
        }

        private Token znum()
        {
            char character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.ZNUM, num);
        }

        private Token otnum()
        {
            char character = nextChar();
            character = nextChar();
            
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.OTNUM, num);
        }

        private Token innum()
        {
            char character = nextChar();
            character = nextChar();
            if (!char.IsDigit(character))
            {
                lastChar();
                lastChar();
                return null;
            }
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.INNUM, num);
        }

        private Token idnum()
        {
            char character = nextChar();
            character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            return new Token(TokenType.IDNUM, num);
        }

        private Token time()
        {
            char character = nextChar();
            string num = "";
            while (character != NONE && char.IsDigit(character))
            {
                num += character;
                character = nextChar();
            }
            if (character == '.')
            {
                num = num + character;
                character = this.nextChar();
                while (character != NONE && char.IsDigit(character))
                {
                    num = num + character;
                    character = this.nextChar();
                }
                return new Token(TokenType.TIME_REAL, num);
            }
            return new Token(TokenType.TIME, num);
        }

        private Token neg_num()
        {
            string num = "-";
            char character = nextChar();
            while (character != NONE && char.IsDigit(character))
            {
                num = num + character;
                character = this.nextChar();
            }

            if (character == '.')
            {
                num = num + character;
                character = this.nextChar();
                while (character != NONE && char.IsDigit(character))
                {
                    num = num + character;
                    character = this.nextChar();
                }
                return new Token(TokenType.REAL_CONST, num);
            }
            else
                return new Token(TokenType.INTEGER_CONST, num);
        }

        private Token id()
        {
            char character = currentChar();
            string name = "";
            while (character != NONE && char.IsLetter(character))
            {
                name = name + character;
                character = this.nextChar();
            }
            //Debug.Log("name:" + name);
            if (reservedKeywords.ContainsKey(name))
            {
                return reservedKeywords[name];
            }
            while(character != NONE && char.IsLetterOrDigit(character))
            {
                name = name + character;
                character = this.nextChar();
            }
            return new Token(TokenType.FILE, name);
        }
    }
}

