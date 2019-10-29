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
            List<Token> Tokens;
            listView1.Items.Clear();
            //Remove comments
            string text = Regex.Replace(richTextBox1.Text, @"\{(.[\S\s]*?)\}", "", RegexOptions.Multiline);
            text = Regex.Replace(text, @"\/\*(.*?)\*\/", "", RegexOptions.Multiline); 
            //
            try
            {
                foreach (string line in text.Split('\n'))
                {
                    Tokens = _matcher.getTokens(line, checkBox1.Checked);
                    foreach (Token token in Tokens)
                    {
                        var item = new ListViewItem(token.value);
                        item.SubItems.Add(token.type);
                        listView1.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                listView1.Items.Clear();

                listView1.Items.Add("AN ERROR OCCURRED.");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
