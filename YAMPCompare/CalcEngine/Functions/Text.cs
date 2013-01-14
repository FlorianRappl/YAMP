﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    static class Text
    {
        public static void Register(CalcEngine ce)
        {
            //ce.RegisterFunction("ASC	Changes full-width (double-byte) English letters or katakana within a character string to half-width (single-byte) characters
            //ce.RegisterFunction("BAHTTEXT	Converts a number to text, using the ß (baht) currency format
            ce.RegisterFunction("CHAR", 1, _Char); // Returns the character specified by the code number
            //ce.RegisterFunction("CLEAN	Removes all nonprintable characters from text
            ce.RegisterFunction("CODE", 1, Code); // Returns a numeric code for the first character in a text string
            ce.RegisterFunction("CONCATENATE", 1, int.MaxValue, Concat); //	Joins several text items into one text item
            //ce.RegisterFunction("DOLLAR	Converts a number to text, using the $ (dollar) currency format
            //ce.RegisterFunction("EXACT	Checks to see if two text values are identical
            ce.RegisterFunction("FIND", 2, 3, Find); //Finds one text value within another (case-sensitive)
            //ce.RegisterFunction("FIXED	Formats a number as text with a fixed number of decimals
            //ce.RegisterFunction("JIS	Changes half-width (single-byte) English letters or katakana within a character string to full-width (double-byte) characters
            ce.RegisterFunction("LEFT", 1, 2, Left); // LEFTB	Returns the leftmost characters from a text value
            ce.RegisterFunction("LEN", 1, Len); //, Returns the number of characters in a text string
            ce.RegisterFunction("LOWER", 1, Lower); //	Converts text to lowercase
            ce.RegisterFunction("MID", 3, Mid); // Returns a specific number of characters from a text string starting at the position you specify
            //ce.RegisterFunction("PHONETIC	Extracts the phonetic (furigana) characters from a text string
            ce.RegisterFunction("PROPER", 1, Proper); // Capitalizes the first letter in each word of a text value
            ce.RegisterFunction("REPLACE", 4, Replace); // Replaces characters within text
            ce.RegisterFunction("REPT", 2, Rept); // Repeats text a given number of times
            ce.RegisterFunction("RIGHT", 1, 2, Right); // Returns the rightmost characters from a text value
            ce.RegisterFunction("SEARCH", 2, Search); // Finds one text value within another (not case-sensitive)
            ce.RegisterFunction("SUBSTITUTE", 3, 4, Substitute); // Substitutes new text for old text in a text string
            ce.RegisterFunction("T", 1, T); // Converts its arguments to text
            ce.RegisterFunction("TEXT", 2, _Text); // Formats a number and converts it to text
            ce.RegisterFunction("TRIM", 1, Trim); // Removes spaces from text
            ce.RegisterFunction("UPPER", 1, Upper); // Converts text to uppercase
            ce.RegisterFunction("VALUE", 1, Value); // Converts a text argument to a number
        }
#if DEBUG
        public static void Test(CalcEngine ce)
        {
            ce.Test("CHAR(65)", "A");
            ce.Test("CODE(\"A\")", 65);
            ce.Test("CONCATENATE(\"a\", \"b\")", "ab");
            ce.Test("FIND(\"bra\", \"abracadabra\")", 2);
            ce.Test("FIND(\"BRA\", \"abracadabra\")", -1);
            ce.Test("LEFT(\"abracadabra\", 3)", "abr");
            ce.Test("LEFT(\"abracadabra\")", "a");
            ce.Test("LEN(\"abracadabra\")", 11);
            ce.Test("LOWER(\"ABRACADABRA\")", "abracadabra");
            ce.Test("MID(\"abracadabra\", 1, 3)", "abr");
            ce.Test("PROPER(\"abracadabra\")", "Abracadabra");
            ce.Test("REPLACE(\"abracadabra\", 1, 3, \"XYZ\")", "XYZacadabra");
            ce.Test("REPT(\"abr\", 3)", "abrabrabr");
            ce.Test("RIGHT(\"abracadabra\", 3)", "bra");
            ce.Test("SEARCH(\"bra\", \"abracadabra\")", 2);
            ce.Test("SEARCH(\"BRA\", \"abracadabra\")", 2);
            ce.Test("SUBSTITUTE(\"abracadabra\", \"a\", \"b\")", "bbrbcbdbbrb");
            ce.Test("T(123)", "123");
            ce.Test("TEXT(1234, \"n2\")", "1,234.00");
            ce.Test("TRIM(\"   hello   \")", "hello");
            ce.Test("UPPER(\"abracadabra\")", "ABRACADABRA");
            ce.Test("VALUE(\"1234\")", 1234.0);

            ce.Test("SUBSTITUTE(\"abcabcabc\", \"a\", \"b\")", "bbcbbcbbc");
            ce.Test("SUBSTITUTE(\"abcabcabc\", \"a\", \"b\", 1)", "bbcabcabc");
            ce.Test("SUBSTITUTE(\"abcabcabc\", \"a\", \"b\", 2)", "abcbbcabc");
            ce.Test("SUBSTITUTE(\"abcabcabc\", \"A\", \"b\", 2)", "abcabcabc"); // case-sensitive!


            // test taken from http://www.codeproject.com/KB/vb/FormulaEngine.aspx
            var a1 = "\"http://j-walk.com/ss/books\"";
            var exp = "RIGHT(A1,LEN(A1)-FIND(CHAR(1),SUBSTITUTE(A1,\"/\",CHAR(1),LEN(A1)-LEN(SUBSTITUTE(A1,\"/\",\"\")))))";
            ce.Test(exp.Replace("A1", a1), "books");

        }
#endif
        static object _Char(List<Expression> p)
        {
            var c = (char)(int)p[0];
            return c.ToString();
        }
        static object Code(List<Expression> p)
        {
            var s = (string)p[0];
            return (int)s[0];
        }
        static object Concat(List<Expression> p)
        {
            var sb = new StringBuilder();
            foreach (var x in p)
            {
                sb.Append((string)x);
            }
            return sb.ToString();
        }
        static object Find(List<Expression> p)
        {
            return IndexOf(p, StringComparison.Ordinal);
        }
        static int IndexOf(List<Expression> p, StringComparison cmp)
        {
            var srch = (string)p[0];
            var text = (string)p[1];
            var start = 0;
            if (p.Count > 2)
            {
                start = (int)p[2] - 1;
            }
            var index = text.IndexOf(srch, start, cmp);
            return index > -1 ? index + 1 : index;
        }
        static object Left(List<Expression> p)
        {
            var n = 1;
            if (p.Count > 1)
            {
                n = (int)p[1];
            }
            return ((string)p[0]).Substring(0, n);
        }
        static object Len(List<Expression> p)
        {
            return ((string)p[0]).Length;
        }
        static object Lower(List<Expression> p)
        {
            return ((string)p[0]).ToLower();
        }
        static object Mid(List<Expression> p)
        {
            return ((string)p[0]).Substring((int)p[1] - 1, (int)p[2]);
        }
        static object Proper(List<Expression> p)
        {
            var s = (string)p[0];
            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }
        static object Replace(List<Expression> p)
        {
            // old start len new
            var s = (string)p[0];
            var start = (int)p[1] - 1;
            var len = (int)p[2];
            var rep = (string)p[3];

            var sb = new StringBuilder();
            sb.Append(s.Substring(0, start));
            sb.Append(rep);
            sb.Append(s.Substring(start + len));

            return sb.ToString();
        }
        static object Rept(List<Expression> p)
        {
            var sb = new StringBuilder();
            var s = (string)p[0];
            for (int i = 0; i < (int)p[1]; i++)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }
        static object Right(List<Expression> p)
        {
            var n = 1;
            if (p.Count > 1)
            {
                n = (int)p[1];
            }
            var s = (string)p[0];
            return s.Substring(s.Length - n);
        }
        static object Search(List<Expression> p)
        {
            return IndexOf(p, StringComparison.OrdinalIgnoreCase);
        }
        static object Substitute(List<Expression> p)
        {
            // get parameters
            var text = (string)p[0];
            var oldText = (string)p[1];
            var newText = (string)p[2];

            // if index not supplied, replace all
            if (p.Count == 3)
            {
                return text.Replace(oldText, newText);
            }

            // replace specific instance
            int index = (int)p[3];
            if (index < 1)
            {
                throw new Exception("Invalid index in Substitute.");
            }
            int pos = text.IndexOf(oldText);
            while (pos > -1 && index > 1)
            {
                pos = text.IndexOf(oldText, pos + 1);
                index--;
            }
            return pos > -1
                ? text.Substring(0, pos) + newText + text.Substring(pos + oldText.Length)
                : text;
        }
        static object T(List<Expression> p)
        {
            return (string)p[0];
        }
        static object _Text(List<Expression> p)
        {
            return ((double)p[0]).ToString((string)p[1], CultureInfo.CurrentCulture);
        }
        static object Trim(List<Expression> p)
        {
            return ((string)p[0]).Trim();
        }
        static object Upper(List<Expression> p)
        {
            return ((string)p[0]).ToUpper();
        }
        static object Value(List<Expression> p)
        {
            return double.Parse((string)p[0], NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
