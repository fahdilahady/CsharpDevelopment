using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.IO.Ports;

namespace SutronListener
{
    public class Program
    {
        static OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
        private static bool _timeToLog=true;
        private static bool _timeToSend=false;
        private static bool _timeSyncToLocal = false;
        private static bool _NeedUpdateSettings;
        
        private static Thread _LoggerSeq = new Thread(new ThreadStart(LoggerSequence));
        protected static aMniSDCardManager _SD;
        private static aMniFastTrackModem _modem = new aMniFastTrackModem("COM1", 115200, Parity.None, 8, StopBits.One);
        private static aMniSutronCom _sutron = new aMniSutronCom("COM2", 115200, Parity.None, 8, StopBits.One); 
        #region StatSetupInfo
        private static string cStatID = "sta0171";

        private static string cGPRS_APN = "internet";
        private static string cGPRS_UserName = ""; 
        private static string cGPRS_Password = "";

        private static string cFTP_ServerAddres = "192.185.197.129";// FTP Server: ftp.metindoinstrumen.com "202.90.198.212"
        private static string cFTP_UserName = "RnD@metindoinstrumen.com"; //FTP Username: metftp@metindoinstrumen.com  "aaws"
        private static string cFTP_Password = "met123indo";//"aaws";

        #endregion
        #region Time

        private static int _Year
        { get { return DateTime.Now.Year; } }

        private static int _Month
        { get { return DateTime.Now.Month; } }

        private static int _Today
        { get { return DateTime.Now.Day; } }
        private static int _CurrentDay = 0;

        private static int _Hour
        { get { return DateTime.Now.Hour; } }

        private static int _Minute
        { get { return DateTime.Now.Minute; } }

        private static int _MinuteMod = 2;

        private static int _Second
        { get { return DateTime.Now.Second; } }

        #endregion
        private static void daily() 
        {
            ///Check SD card directory
            ///reset controller if possible
            try
            {
                if (!_SD.Equals(null) && _SD.Ready)
                {
                    if (!_SD.CheckPath("DataLogging", _Year.ToString(), _Month.ToString(), _Today.ToString()))
                    {
                        /// if Failed send error or notif
                        Debug.Print("Check Path has failed, error on SD card");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            } 
        }

        private static void BlinkLed()
        {

            while (true)
            {

                
               // Debug.Print(DateTime.Now.ToString());
                if (!_SD.Ready)
                {
                    _NeedUpdateSettings = true;
                }
                else
                {
                    if (_NeedUpdateSettings)
                    {
                        //Do Update Necessary Variable by read the settings from SD card
                        _NeedUpdateSettings = false;
                    }
                }
                
            }
        }

        private static void doParseAndStore(string fromThisStr)
        {
            char[] separator = { ' ' };
            string[] tempStrs;
            string clearStr = "";
            Int32 pos;
            bool useClear = false;

            // 01/13/2016 02:40:00 TipBktAcc      0.0   G mm
            if (fromThisStr.IndexOf("DATA.LOG")>0)
            {
                pos = fromThisStr.IndexOf(_Year.ToString())-6;
                clearStr = fromThisStr.Substring(pos, fromThisStr.IndexOf("mm") + 1);
                useClear = true;
            }
            if (useClear)
            {
                tempStrs = clearStr.Split(separator);
            }
            else
            {
                tempStrs = fromThisStr.Split(separator);
            }
            string[] dates = tempStrs[0].Split('/');
            string hari = dates[0]; string bln = dates[1]; string thn = dates[2].Substring(2, 2);

            string[] times = tempStrs[1].Split(':');
            string jam = times[0]; string mnt = times[1]; string dtk = times[2];

            if (!_SD.Equals(null))
            {
                //SET Namefile   //"sta0171"+hari+bln+thn+jam+mnt+".txt"
                _SD.Namefile = cStatID + hari + bln + thn + jam + mnt + dtk ;
                _SD.Message = tempStrs[0] + ";" + tempStrs[1] + ";" + tempStrs[8]; //date;time;Rain
                _SD.Write();
            }
        }
        private static void ActivateModem()
        {
            _modem.sendFTPFile(_SD.FullPathOfLoggedFile); Thread.Sleep(1000);           
        }
        private static void LoggerSequence()
        {
            while (true) 
            {
                led.Write(true); // turn on the LED
                Thread.Sleep(150); // sleep for 250ms
                led.Write(false); // turn off the LED
                Thread.Sleep(750); // sleep for 250ms

                if (_Today != _CurrentDay) { daily(); }
                _CurrentDay = _Today;

                if (_Minute % _MinuteMod > 0) //MinuteMod = 10 minute
                {
                    _timeToLog = true;
                    if (_timeToSend)
                    {
                        if (!_modem.Equals(null))
                        {

                            /// Activate GPRS modem.setAPN
                            /// do send data via FTP to server modem.setFTPServer
                           
                            if (_SD.FileExist())
                            {
                                Debug.Print("Sending File through FTP protocol");
                                ActivateModem();
                            }
                            
                        } _timeToSend = false;
                    }
                }
                else 
                {
                    if (_timeToLog)
                    {                    
                        
                       Debug.Print("wait until _sutron Ready");     
                        while (!_sutron.Ready)
                        {
                            Thread.Sleep(7000); _sutron.Ready = true;
                            //Debug.Print(DateTime.Now.ToString());
                        }
                        if (!_sutron.Equals(null))
                        {
                            //Log Data To Sd card
                            if (_sutron.isOpenned())
                            {
                                _sutron.RetrieveData();
                                if (_sutron.IsDataRecieved)
                                {
                                    //parse data
                                    Debug.Print("saving file into local SD card");
                                    doParseAndStore(_sutron.DataRecieved); Thread.Sleep(2000); _sutron.IsDataRecieved = false;
                                    _timeToSend = true;
                                }
                                else
                                {
                                    Debug.Print("data failed to retrieved");
                                }
                            } 
                        }
                        _timeToLog = false;
                    }
                }
            }
        }
        private static void initProcModem()
        {
            int i = 0; Debug.Print("wait until _modemReady");
            while (!_timeSyncToLocal)
            {
                if (!_modem.Equals(null) && _modem.isTimeSynced)
                {
                    if (!_modem.myDT.Equals(null))
                    {
                        Utility.SetLocalTime(_modem.myDT.AddHours(7));
                        Debug.Print("sync at: " + DateTime.Now.ToString());

                        _modem.isTimeSynced = false; _timeSyncToLocal = true;
                    }
                }
                i++;
                Thread.Sleep(500);
                Debug.Print(i.ToString());
            }
        }
        public static void Main()
        {
            _SD = new aMniSDCardManager();
            #region InitProcedure
            _sutron.OpenCommunication(); Thread.Sleep(3500);
            _modem.OpenCommunication(); Thread.Sleep(3500);
            _modem.setAPN(cGPRS_APN, cGPRS_UserName, cGPRS_Password);
            _modem.setFTPServer(cFTP_ServerAddres, cFTP_UserName, cFTP_Password);
            
            _modem.initSets();
            initProcModem(); _modem.isTimeSynced = false;
            _LoggerSeq.Start();
            #endregion
        }

    }
}
