using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ScheduleJob
{
    /// <summary>
    /// 固定时刻执行
    /// </summary>
    
    public class FixedTimePointSchedule : ScheduleBase
    {
        Timer m_timer;
        DateTime m_dateTime;

        public FixedTimePointSchedule()
        {

        }

        public FixedTimePointSchedule(DateTime dateTime, bool repeat)
        {
            m_dateTime = dateTime;
            Repeat = repeat;
        }

        public override void Start()
        {
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
            m_timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logger.Debug("时间到了，目前时间: " + DateTime.Now.ToString());
            // 执行事件
            DoAction();
            m_timer.Stop();
            m_timer.Dispose();
            m_timer = null;
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
                string targetDir = string.Format(@"D:\Work\Project-LiveDirector\tools\client_tools\build\win\");//这是bat存放的目录
                Process proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = "build_all.bat";  //bat文件名称
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();//读取进程的输出 
                Logger.ProcessCmdLog(output);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString()));
            }
        }

        public override void Stop()
        {
            m_timer.Stop();
            m_timer.Dispose();
            m_timer = null;
        }
    }
}
