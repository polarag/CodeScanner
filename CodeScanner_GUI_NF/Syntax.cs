using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace CodeScanner_GUI_NF
{
    public class Matcher
    {
        private Random rnd = new Random();


        Dictionary<string, string[]> keywordsdb = new Dictionary<string, string[]>();
         Dictionary<string, string> descriptordb = new Dictionary<string, string>();
         Dictionary<string, Color> colorsdb = new Dictionary<string, Color>();
        public Matcher()
        {
            string[] keywords = { "WRITE", "READ", "IF", "ELSE", "RETURN", "BEGIN", "END", "MAIN", "STRING", "INT", "REAL", "THEN", "REPEAT", "UNTIL" };
            string[] separators = { ";", ",", "(", ")", "[", "]" };
            string[] single_operators = { "+", "-", "*", "/" };
            string[] multi_operators = { ":=", "==", "!=", "<", "=", ">", ">=", "<=" };
            keywordsdb.Add("keyword", keywords);
            keywordsdb.Add("separator", separators);
            keywordsdb.Add("single_operator", single_operators);
            keywordsdb.Add("multi_operator", multi_operators);
            foreach (string keyword in keywords)
            {
                colorsdb.Add(keyword + " TOKEN", Color.Blue);
                descriptordb.Add(keyword, keyword + " token");
            }
            colorsdb.Add("SEPARATOR", Color.Magenta);
            foreach (string separator in separators) descriptordb.Add(separator, "separator");
            colorsdb.Add("SINGLE OPERATOR", Color.Orange);
            foreach (string _operator in single_operators) descriptordb.Add(_operator, "single operator");
            colorsdb.Add("MULTI OPERATOR", Color.DarkOrange);
            foreach (string _operator in multi_operators) descriptordb.Add(_operator, "multi operator");
            colorsdb.Add("STRING", Color.Red);
            colorsdb.Add("CHARACTER/STRING", Color.Red);
            colorsdb.Add("IDENTIFIER", Color.DarkBlue);
            colorsdb.Add("NUMBER", Color.Green);
        } 
        public Color getColor(string keyword)
        {
            keyword = keyword.ToUpper();
            
            return colorsdb.Keys.Contains(keyword) ? colorsdb[keyword] : Color.Black ;
        }
        /*
           Keywords:   WRITE READ IF ELSE RETURN BEGIN END MAIN STRING INT REAL
           Single-character separators:   ;  ,  (   )
           Single-character operators:    +  -  *   /
           Multi-character operators:    :=  ==   !=

         */
        public string getTokenType(string match, bool contOnError = false)
        {
            if (Regex.Match(match, "^\"(.*?)\"$").Success)
            {
                return "string";
            }
            if (Regex.Match(match, "^\'(.*?)\'$").Success)
            {
                return "character/string";
            }
            if (float.TryParse(match, out float number))
            {
                return "number";
            }
            foreach (string keyword in keywordsdb.Keys)
            {
                if (keywordsdb[keyword].Contains(match.ToUpper()))
                {
                    return descriptordb.Keys.Contains(match.ToUpper()) ? descriptordb[match.ToUpper()] : "keyword";
                }
            }
            if (Regex.Match(match, @"^[A-z]+[0-9]*$").Success)
            {
                return "identifier";
            }
            return contOnError? "unknown" : throw new Exception(" Unknown token type: " + match);

        }
        public string filterLine(string line)
        {
            foreach (string keyword in keywordsdb.Keys)
            {
                foreach (string value in keywordsdb[keyword])
                    line = line.Replace(value, " " + value+ " ");
            }
            line = Regex.Replace(line, @":\s+=", ":=");
            line = Regex.Replace(line, @"<\s+=", "<=");
            line = Regex.Replace(line, @">\s+=", ">=");
            return line;
        }
        public List<Token> getTokens(string line, bool contOnError = false)
        {
            List<Token> _return = new List<Token>();
            if (line.Contains('"') || line.Contains("'"))
            {
                foreach (Match match in Regex.Matches(line, "\"(.*?)\""))
                {
                    _return.Add(new Token(match.Value, "string"));
                    line = line.Replace(match.Value, "");
                }
                foreach (Match match in Regex.Matches(line, "\'(.*?)\'"))
                {
                    _return.Add(new Token(match.Value, "character/string"));
                    line = line.Replace(match.Value, "");
                }
            }
            line = filterLine(line); //Remove comments
            foreach (Match match in Regex.Matches(line, @"(\S+)"))
            {
                if (match.Value.Length.ToString().Trim().Length > 0)
                _return.Add(new Token(match.Value, getTokenType(match.Value, contOnError)));
            }
            return _return;
        }
    }
}
