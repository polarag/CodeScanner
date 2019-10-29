using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeScanner_GUI_NF
{
    class Token
    {

    }
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



        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
