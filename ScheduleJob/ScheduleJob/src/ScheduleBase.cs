namespace ScheduleJob
{
    public abstract class ScheduleBase
    {
        public bool Repeat { get; set; }
        public long Interval { get; set; }
        public abstract void Start();
        public abstract void Stop();
    }
}
