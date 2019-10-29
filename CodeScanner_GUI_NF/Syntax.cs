using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeScanner_GUI_NF
{
    public class Matcher
    {
         Dictionary<string, string[]> keywordsdb = new Dictionary<string, string[]>();
         Dictionary<string, string> descriptordb = new Dictionary<string, string>();
        public Matcher()
        {
            string[] keywords = { "WRITE", "READ", "IF", "ELSE", "RETURN", "BEGIN", "END", "MAIN", "STRING", "INT", "REAL" };
            string[] separators = { ";", ",", "(", ")", "[", "]" };
            string[] single_operators = { "+", "-", "*", "/" };
            string[] multi_operators = { ":=", "==", "!=" };
            keywordsdb.Add("keyword", keywords);
            keywordsdb.Add("separator", separators);
            keywordsdb.Add("single_operator", single_operators);
            keywordsdb.Add("multi_operator", multi_operators);
            foreach (string keyword in keywords) descriptordb.Add(keyword, keyword + " token");
            foreach (string separator in separators) descriptordb.Add(separator, "separator");
            foreach (string _operator in single_operators) descriptordb.Add(_operator, "single operator");
            foreach (string _operator in multi_operators) descriptordb.Add(_operator, "multi operator");

        }
        /*
           Keywords:   WRITE READ IF ELSE RETURN BEGIN END MAIN STRING INT REAL
           Single-character separators:   ;  ,  (   )
           Single-character operators:    +  -  *   /
           Multi-character operators:    :=  ==   !=

         */
        public string getTokenType(string match, bool contOnError = false)
        {
            if (int.TryParse(match, out int number))
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
            if (Regex.Match(match, "^\"(.*?)\"$").Success)
            {
                return "string";
            }
            if (Regex.Match(match, "^\'(.*?)\'$").Success)
            {
                return "character/string";
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
            return line;
        }
        public List<Token> getTokens(string line, bool contOnError = false)
        {
            List<Token> _return = new List<Token>();
            line = filterLine(line); //Remove comments
            Debug.WriteLine(line);
            foreach (Match match in Regex.Matches(line, @"(\S*)"))
            {
                if (Regex.Match(match.Value, @"^\s*$", RegexOptions.Multiline).Success) continue;
                if (match.Value.Length.ToString().Trim().Length > 0)
                _return.Add(new Token(match.Value, getTokenType(match.Value, contOnError)));
            }
            return _return;
        }
    }
}
