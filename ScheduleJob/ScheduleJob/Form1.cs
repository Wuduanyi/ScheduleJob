using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ScheduleJob
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            DateTime dateTime = dateTimePicker.Value;
            FixedTimePointSchedule schedule = new FixedTimePointSchedule(dateTime, false);
            schedule.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                //点击时判断form是否显示,显示就隐藏,隐藏就显示
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.Hide();
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    //取消窗体关闭事件
            this.WindowState = FormWindowState.Minimized;   //最小化窗口
            this.ShowInTaskbar = false;		//在Windows任务栏中不显示窗体
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 关闭所有的线程
            this.Dispose();
            this.Close();
        }

        private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
