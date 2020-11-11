using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleJob
{
    public class Logger
    {
        private const string Log_File = "Player.log";

        private const string Error_File = "Error.log";

        private const string Cmd_Log_File = "Cmd.log";
        public static void Debug(string msg)
        {
            //启动任务,并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            Task task = DebugAsync(Log_File, msg);
            if (!task.IsCompleted)
                task.Start();
        }

        public static void ProcessCmdLog(string msg)
        {
            //启动任务,并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            Task task = DebugAsync(Cmd_Log_File, msg);
            if (!task.IsCompleted)
                task.Start();
        }

        public static void Error(string msg)
        {
            //启动任务,并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            Task task = DebugAsync(Error_File, msg);
            if (!task.IsCompleted)
                task.Start();
        }

        static async Task DebugAsync(string filePath, string text)
        {
            // filename is a string with the full path
            // true is to append        
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
            {
                // Can write either a string or char array
                await file.WriteAsync(text + "\n");
            }
        }
    }
}
