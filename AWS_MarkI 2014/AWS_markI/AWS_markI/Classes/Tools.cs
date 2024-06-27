using System;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using RTC;

namespace AWS_markI
{
    class Tools:Program
    {
        private static string _mypath;

        public string myPath
        {
            get { return _mypath; }
            set { _mypath = value; }
        }
    
        public Tools()   {          
        }
        public void hit_ButtonGSM()
        {
            gsmPower.Write(true);
            Thread.Sleep(550);
            gsmPower.Write(false);
            Thread.Sleep(500);
        }

        public void RTC_start()
        {
            
            // TODO: Do this only once to set your clock
            //clock.SetClock(14, 2, 7, 23, 39, 30, DayOfWeek.Friday);
          
            clock.SetLocalTimeFromRTC();
            
            Debug.Print("The time is: " + DateTime.Now.ToString());
            Debug.Print(DateTime.Today.Date.ToString());
            
            clock.Write(DS1307RealTimeClock.UserDataAddress, new byte[] { 0xab, 0xcd });

            var data = new byte[2]; // This can be up to DS1307RealTimeClock.UserDataLength (56)
            clock.Read(DS1307RealTimeClock.UserDataAddress, data);

            Thread.Sleep(550);
        }

        public void updateDisplay(object state)
        {
            var dt = DateTime.Now;

            myLcd.Home();
            myLcd.Write(FillLine(dt.ToString("dd MMM HH:mm:ss")), 0, 16);
            myLcd.SetCursorPosition(0, 1); myLcd.Write(readAnalog.ToString("F1") + "mV" + "  ++" + _tipping.ToString());

        }

        static readonly byte[] _lineBuffer = new byte[16];

        public byte[] FillLine(string text)
        {
            // fill empty space
            for (int i = 0; i < _lineBuffer.Length; i++)
                _lineBuffer[i] = (byte)' ';

            // write new text
            var bytes = Encoding.UTF8.GetBytes(text);
            bytes.CopyTo(_lineBuffer, 0);

            return _lineBuffer;
        }

        public bool CheckPath(string rootDir, string YDir, string MDir)
        {
                                  
            try
            {
                Again:
                if (!Directory.Exists(@"\SD\" + rootDir))
                {
                    Directory.CreateDirectory(@"\SD\" + rootDir);
                    Thread.Sleep(150);
                   
                    goto Again;
                }

                else if (!Directory.Exists(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir)) 
                {
                    Directory.CreateDirectory(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir);
                    Thread.Sleep(150);
                    goto Again;
                }

                else if (!Directory.Exists(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir))
                {
                    Directory.CreateDirectory(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir);
                    Thread.Sleep(150);
                    goto Again;
                }

                else
                {
                    _mypath = rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir;
                    return true;
                }

                
            }
            catch (Exception ex)
            {
                myLcd.SetCursorPosition(0, 1);
                myLcd.Write(ex.Message);
                Debug.Print(ex.Message);
                return (false);
            }
        }
    }
    
}