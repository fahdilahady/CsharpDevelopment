using System;
using RTC;
namespace Tools
{
    class myRTC : Tools
    {

        private DS1307RealTimeClock clock = new DS1307RealTimeClock();
        private bool _ready = true;
        private string _report;

        public void Start()
        {
            // TODO: Do this only once to set your clock
            // clock.SetClock(14, 2, 7, 23, 39, 30, DayOfWeek.Friday);
            try
            {
                clock.SetLocalTimeFromRTC();

                //Debug.Print("The time is: " + DateTime.Now.ToString());
                //Debug.Print(DateTime.Today.Date.ToString());

                clock.Write(DS1307RealTimeClock.UserDataAddress, new byte[] { 0xab, 0xcd });

                var data = new byte[2]; // This can be up to DS1307RealTimeClock.UserDataLength (56)
                clock.Read(DS1307RealTimeClock.UserDataAddress, data);
                _ready = true;
            }
            catch (Exception ex)
            {
                _ready = false;
                _report = ex.Message;
            }

        }

        public override bool Ready
        {
            get
            {
                return _ready;
            }
            set
            {
                _ready = value;
            }
        }

        public override string Report
        {
            get
            {
                return _report;
            }
            set
            {
                _report = value;
            }
        }
    }
}
