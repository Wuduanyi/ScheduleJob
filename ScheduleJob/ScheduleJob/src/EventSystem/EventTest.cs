using ScheduleJob;

namespace EventSystem
{
    public class EventTest
    {
        // Start is called before the first frame update
        void Start()
        {
            Register();
            PostEvent();
        }

        private void OnDestroy()
        {
            Unregister();
        }

        void Register()
        {
            EventSystem.Instance.RegisterEvent<string, int>(EEvent.TestEvent, OnRecvEvent);
        }

        void Unregister()
        {
            EventSystem.Instance.UnregisterEvent<string, int>(EEvent.TestEvent, OnRecvEvent);
        }


        void PostEvent()
        {
            EventSystem.Instance.PostEvent(EEvent.TestEvent, "TestChar", 2);
        }

        void OnRecvEvent(string a, int b)
        {
            Logger.Error(a + b);
        }
    }
}