using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO.Ports;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using SutronListener;

namespace SutronListener_Mark_II
{
    public class Program
    {
        private static aMniFastTrackModem _aModem = new aMniFastTrackModem(SerialPorts.COM1, 115200, Parity.None, 8, StopBits.One);
        private static aMniSutronCom _sutron = new aMniSutronCom(SerialPorts.COM2, 115200, Parity.None, 8, StopBits.One);
        private static aMniSDCardManager _SD = new aMniSDCardManager();
        private static Boolean _timeSyncToLocal = false;

        private static void initProcModem()
        {
            int i = 0; Debug.Print("wait until _aModemReady");
            while (!_timeSyncToLocal)
            {
                if (!_aModem.Equals(null) && _aModem.isTimeSynced)
                {
                    if (!_aModem.myDT.Equals(null))
                    {
                        Utility.SetLocalTime(_aModem.myDT.AddHours(7));
                        Debug.Print("sync at: " + DateTime.Now.ToString());

                        _aModem.isTimeSynced = false; _timeSyncToLocal = true;
                    }
                }
                i++;
                Thread.Sleep(500);
                Debug.Print(i.ToString());
            }
        }

        public static void Main()
        {
            // write your code here
            #region Init
            _aModem.ComPort.Close(); Thread.Sleep(4500);
            _aModem.OpenCommunication(); Thread.Sleep(3500);
            _aModem.initSets();
            initProcModem(); Thread.Sleep(150);
            _aModem.isTimeSynced = false;
            #endregion

            #region Sequence
            while (true)
            {
                _aModem.PrintLine("AT"); Thread.Sleep(120);
                _sutron.PrintLine("\r\n)"); Thread.Sleep(150);
            }
            #endregion

        }

    }
}
