using GameStructure;
using PreCloud9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Home
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Connection.ServerIP = this.textBox1.Text;
            this.Visible = false;
            Thread newThread = new Thread(startGame);
            newThread.Start();
        }

        public void startGame()
        {
            Game1 game = new Game1();
            game.Run();
        }

        private void btnStartAI_Click(object sender, EventArgs e)
        {
            GameManager.AI_State = true;
            Connection.ServerIP = this.textBox1.Text;
            this.Visible = false;
            Thread AIthread = new Thread(startGame);
            AIthread.Start();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
