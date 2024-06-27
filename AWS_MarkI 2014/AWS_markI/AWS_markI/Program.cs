using System;
using System.Threading;
using ADC;
using MicroLiquidCrystal;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using RTC;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace AWS_markI
{
    /// <summary> 
    /// Development of Automatic Weather Station. by Fahdil Ahady Hazain. Copyright 2014
    /// http://metindoinstrumen.com
    ///</summary>
    public class Program
    {
        #region Protected
        
        
        private static Shifter74Hc595LcdTransferProvider _lcdProvider = new Shifter74Hc595LcdTransferProvider
            (SPI_Devices.SPI1, Pins.GPIO_PIN_D10, Shifter74Hc595LcdTransferProvider.BitOrder.MSBFirst);
        protected static Lcd myLcd = new Lcd(_lcdProvider);
        protected static DS1307RealTimeClock clock = new DS1307RealTimeClock();
        protected static SecretLabs.NETMF.Hardware.AnalogInput A0 = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A0);
        protected static OutputPort gsmPower = new OutputPort(Pins.GPIO_PIN_D9, false);
        protected static OutputPort gsmReset = new OutputPort(Pins.GPIO_PIN_D8, false);
        #endregion

        #region PrivateProperty


        private static InputPort Rain = new InputPort(Pins.GPIO_PIN_D7, false, Port.ResistorMode.Disabled);
        private static Conversion analogIn = new Conversion(A0.Read());
        private static Tools _tools = new Tools();
        private static Timer _displayTimer;
        private static DataStorage _store = new DataStorage();
        private static GSM_UART _Gsm = new GSM_UART();

        private static int _Modulus = 10;

        private static bool _wakeUp = true;
  
        private static string _root
        {
            get { return "dataLog"; }
        }

        private static int _year
        {
            get { return DateTime.Now.Year; }
        }

        private static int _month
        {
            get { return DateTime.Now.Month; }
        }

        private static int _today
        {
            get { return DateTime.Now.Day; }
        }

        private static string _hour
        {
            get { return DateTime.Now.ToString("HH:mm"); }
        }

        private static int _minutes
        {
            get { return DateTime.Now.Minute; }
        }

        private static int _seconds
        {
            get { return DateTime.Now.Second; }
        }

        private static int _currentDay=0;

        protected static int _tipping = 0;
        protected static float readAnalog = 0;
        private static bool _LastState = false;
        private static bool _Log = false;
        #endregion

        public static void Main()
        {
            #region Initial
            
            myLcd.Begin(16, 2); // Turn On LCD
            myLcd.Backlight = false;
            myLcd.Write("AllRightReserved");            myLcd.SetCursorPosition(0, 1);            myLcd.Write("MetindoInstrumen");
           
            Thread.Sleep(3000);
            myLcd.Clear(); myLcd.Home(); myLcd.Write("Turning On GSM..");
            
            myLcd.SetCursorPosition(0, 1);
            _tools.hit_ButtonGSM(); // Turn On GSM
            while (!_Gsm.connected)
            {
                Thread.Sleep(100);
            }

            if (_Gsm.connected) { _Gsm.ATnF(); myLcd.Write("GSM connected..."); }
            else { myLcd.Write("NOT connected!"); }
            
            Thread.Sleep(1000);

            myLcd.Clear(); myLcd.Home(); myLcd.Write("Turning On RTC..");
            _tools.RTC_start(); // Turn On RTC
            Thread.Sleep(500); myLcd.Clear();

            _tools.hit_ButtonGSM(); // turn off GSM
            Thread.Sleep(500);
            myLcd.Backlight = true; // back light has a reverse logic, so this will turn it off.

            #endregion

            #region DoForever
            while (true)
            {
                #region Before Logging

                if (_today != _currentDay)
                {
                    daily(); Thread.Sleep(350);
                }
                _currentDay = _today;

                bool state = Rain.Read();

                if (state != _LastState)
                {
                    if (state) { _tipping++; Thread.Sleep(500); }
                }
                _LastState = state;

                if (_minutes % _Modulus > 0)
                {
                    #region taskBeforeRecord
                    if ((_minutes % _Modulus == _Modulus - 1) && (_seconds >= 30) && _wakeUp)
                    {
                        _tools.hit_ButtonGSM();
                        resetCommand();
                    }
                    #endregion

                    analogIn.analogread(A0.Read());
                    readAnalog = analogIn.miliVolts;
                    _displayTimer = new Timer(_tools.updateDisplay, null, 500, 1000);

                }
                
                #endregion
                else
                {
                    #region DataLogging
                    if (_Log)
                    {
                        _displayTimer.Dispose();
                        myLcd.Clear(); myLcd.SetCursorPosition(0, 1); myLcd.Write("Logging data...");
                        _store.ContentFile = _hour + "," + (analogIn.miliVolts / 10).ToString("F1") + "," + _tipping.ToString();
                        _tipping = 0;
                        if (!_store.Write())
                        {
                            myLcd.Clear(); myLcd.SetCursorPosition(0, 1); myLcd.Write("Logging Failed");
                            _store.Report(DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + " :: Logging Failed");
                        }
                        Thread.Sleep(3000);
                        try
                        {
                            if (_Gsm.connected)
                            {
                                myLcd.Clear(); myLcd.SetCursorPosition(0, 1); myLcd.Write("sending data...");
                                _Gsm.SendSMS(_store.NoDest, _store.ContentFile);
                                while (!_Gsm.SmsSent)
                                {
                                    Thread.Sleep(100);
                                }
                                Thread.Sleep(5000); // SMS not sent even reach "CMGS:OK", adding a bunch of delay to give time for SIM300 to finish its works
                            }
                            else
                            {
                                myLcd.Clear(); myLcd.SetCursorPosition(0, 1); myLcd.Write("Data Send Error");
                                _store.Report(DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + "Data Send Error");
                                Thread.Sleep(1000);
                            }
                        }

                        catch (Exception ex)
                        {
                            _store.Report(DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + ex.Message);
                        }
                        resetVar();
                        _tools.hit_ButtonGSM();
                        myLcd.Clear(); myLcd.Home(); myLcd.Write("Data Sent :"); myLcd.SetCursorPosition(0, 1); myLcd.Write(_store.ContentFile);
                    }
                    #endregion
                    if (_seconds > 57) { myLcd.Clear(); }
                }
            } 
            #endregion

        }

        private static void daily()
        {
            try
            {
                Debug.Print("Daily Task!....start");_store.Namefile = _today.ToString();
                if (_tools.CheckPath(_root, _year.ToString(), _month.ToString()))
                {
                    _store.PathFile = _tools.myPath;
                }
                Thread.Sleep(300);
                _Gsm.DeleteAllSMS();
                Debug.Print("Daily Task!.... ends");
            }
            catch (Exception ex)
            {
                myLcd.SetCursorPosition(0, 1);
                myLcd.Write(ex.Message);
                _store.Report(_hour+ ex.Message);
            }
        }

    #region reset
		
        private static void resetVar()
        {
            _wakeUp = true;
            _Gsm.connected = false;
            _Log = false;
        }
        private static void resetCommand()
        {
            _Log = true;
            _Gsm.SmsSent = false;
            _wakeUp = false;
        }
 
	#endregion    
    }
}
