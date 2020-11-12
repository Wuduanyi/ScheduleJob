using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace ScheduleJob
{
    public enum ScheduleState
    {
        None = 0,
        Doing = 1,
        Done = 2,
    };

    /// <summary>
    /// 固定时刻执行
    /// </summary>

    public class FixedTimePointSchedule : ScheduleBase
    {
        Timer m_timer;
        DateTime m_dateTime;
        string m_batFilePath;
        System.Threading.SynchronizationContext m_syncContext = null;
        ScheduleState m_scheduleState = ScheduleState.None;

        public FixedTimePointSchedule()
        {
            
        }

        public FixedTimePointSchedule(DateTime dateTime, string batFilePath, bool repeat)
        {
            m_scheduleState = ScheduleState.None;
            m_dateTime = dateTime;
            m_batFilePath = batFilePath;
            Repeat = repeat;
            //获取UI线程同步上下文 
            m_syncContext = System.Threading.SynchronizationContext.Current;
        }

        public override void Start()
        {
            if (string.IsNullOrEmpty(m_batFilePath))
            {
                System.Windows.Forms.MessageBox.Show("请先选择Bat文件");
                return;
            }

            if (m_scheduleState == ScheduleState.Doing)
            {
                System.Windows.Forms.MessageBox.Show("当前任务正在执行");
                return;
            }

            DateTime dateTime = m_dateTime;
            DateTime nowTime = DateTime.Now;
            if (DateTime.Compare(dateTime, nowTime) < 0)
            {
                // 添加24个小时
                dateTime = dateTime.AddHours(24);
            }
            Logger.Debug("下一个任务的时间点: " + dateTime.ToString());
            TimeSpan timeSpan = dateTime.Subtract(nowTime);
            Interval = (long)timeSpan.TotalMilliseconds;
            Logger.Debug("距离下一个任务的时间点，还需要毫秒数: " + Interval);
            m_timer = new Timer();
            m_timer.Enabled = true;
            m_timer.Interval = Interval; //执行间隔时间,单位为毫秒    
            m_timer.Start();
            m_timer.AutoReset = false;
            m_timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            PostEvent(true);
            m_scheduleState = ScheduleState.Doing;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logger.Debug("时间到了，目前时间: " + DateTime.Now.ToString());
            // 执行事件
            DoAction();

            Stop();
            if (Repeat)
            {
                m_dateTime = m_dateTime.AddHours(24);
                Start();
            }
        }

        private void DoAction()
        {
            // 执行Unity的Build_All
            try
            {
                // 关闭所有的Unity程序
                CloseUnityProcess();
                string targetDir = Path.GetDirectoryName(m_batFilePath); //这是bat存放的目录
                Process proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = Path.GetFileName(m_batFilePath); //bat文件名称
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();//读取进程的输出 
                Logger.ProcessCmdLog(output);
                m_scheduleState = ScheduleState.Done;
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString()));
            }
        }

        public void CloseUnityProcess()
        {
            string ProcessName = "Unity";//"VRP-Player" 这里换成你需要删除的进程名称
                                         //获取你要关闭的进程 系统中的每个进程有个唯一的id 
            Process[] processArray = Process.GetProcessesByName(ProcessName);
            if (processArray.Length <= 0)
            {
                return;
            }
            else
            {
                // 所有的Unity程序都干掉
                for (int i = 0; i < processArray.Length; i++)
                {
                    Process process = processArray[i];
                    process.Kill();
                }
            }
        }

        public override void Stop()
        {
            if (m_timer != null)
            {
                m_timer.Stop();
                m_timer.Dispose();
                m_timer = null;
                m_syncContext.Post(PostEvent, false);
            }
        }

        private void PostEvent(object obj)
        {
            bool state = bool.Parse(obj.ToString());
            EventSystem.EventSystem.Instance.PostEvent(EventSystem.EEvent.TimerStateChange, state);
        }
    }
}
