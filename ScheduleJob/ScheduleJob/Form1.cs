using System;
using System.Windows.Forms;
using EventSystem;

namespace ScheduleJob
{
    public partial class Form1 : Form
    {
        private string m_batFilePath;
        private bool m_repeat;
        private FixedTimePointSchedule m_schedule;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EventSystem.EventSystem.Instance.RegisterEvent<bool>(EventSystem.EEvent.TimerStateChange, OnTimerStateChange);
            OnTimerStateChange(false);
        }

        private void OnTimerStateChange(bool state)
        {
            stateLabel.Text = state ? "任务开启中" : "任务已经暂停了";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            DateTime dateTime = dateTimePicker.Value;
            if (m_schedule == null)
            {
                m_schedule = new FixedTimePointSchedule();
            }
            m_schedule.Reset(dateTime, m_batFilePath, m_repeat);
            m_schedule.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (m_schedule != null)
                m_schedule.Stop();
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
            this.Hide();
            this.ShowInTaskbar = false;		//在Windows任务栏中不显示窗体
        }

        //调整窗体大小的时候触发的事件
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)//当窗体设置值为最小化时
            {
                this.Hide();
                this.ShowInTaskbar = false;//在任务栏中显示该窗口
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择文件路径";
            dialog.Filter = "批处理文件(*.bat)|*.bat";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_batFilePath = dialog.FileName;
                label4.Text = dialog.FileName;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            m_repeat = checkBox1.Checked;
        }
    }
}
