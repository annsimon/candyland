using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Candyland
{
    public partial class Form1 : Form
    {
        public Game1 game; 
        public Form1()
        {
            InitializeComponent();
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if(game != null)
            game.wndProc(ref m);
        }
    }
}
