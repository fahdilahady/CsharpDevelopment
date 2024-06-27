using System;
using Microsoft.SPOT.Hardware;
using System.IO;
using System.Threading;


namespace AwsClasses.Tools
{
    public class SD_card : Tools
    {
        static InterruptPort sdCardStatusPort = new InterruptPort((Cpu.Pin)57, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        private static bool _ready = true;
        private static string _report;
        protected static string _mypath;

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

        public SD_card()
        {
            sdCardStatusPort.OnInterrupt += sdCardStatusPort_OnInterrupt;
        }

        static void sdCardStatusPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (!sdCardStatusPort.Read())
            {
                _ready = true;
            }
            else { _ready = false; }
            
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
                _report = ex.Message;
                return (false);
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
