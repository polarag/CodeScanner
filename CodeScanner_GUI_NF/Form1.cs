using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeScanner_GUI_NF
{
    public partial class Form1 : Form
    {

        List<Token> tokensdictionary = new List<Token>();

        bool CheckSyntax(string code)
        {
            return true;
        }

        List<Token> FindTokens(string code)
        {
            List<Token> tokens = new List<Token>();

            return tokens;
        }



        private Matcher _matcher;
        public Form1()
        {
            InitializeComponent();
            _matcher = new Matcher();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> Tokens;
            listView1.Items.Clear();
            string text = Regex.Replace(richTextBox1.Text, @"\/\*(.*?)\*\/", "", RegexOptions.Multiline); //Remove comments
            foreach (string line in text.Split('\n'))
            {
                Tokens = _matcher.getTokens(line);
                foreach (string token in Tokens)
                {
                    var item = new ListViewItem(token.Split('~')[0]);
                    item.SubItems.Add(token.Split('~')[1]);
                    listView1.Items.Add(item);
                }

            }
        }
    }
}
