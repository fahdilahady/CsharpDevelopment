using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using MicroLiquidCrystal;
using System.Text;
using AwsClasses;
using AwsClasses.Tools;

namespace AWS_markII
{
    public class Program
    {
        #region Tools

        private static myRTC myRtc = new myRTC();
        private static GSM_modem modem = new GSM_modem();
        private static SD_Log Logger = new SD_Log();
        private static Shifter74Hc595LcdTransferProvider _lcdProvider = new Shifter74Hc595LcdTransferProvider
            (SPI_Devices.SPI1, Pins.GPIO_PIN_D10, Shifter74Hc595LcdTransferProvider.BitOrder.MSBFirst);
        private static Lcd myLcd = new Lcd(_lcdProvider);
        private static IC_4017 Switcher = new IC_4017(Pins.GPIO_PIN_D6);

        #endregion

        #region Sensors
        
        private static WindVane ArahAngin   = new WindVane(Pins.GPIO_PIN_D4, Pins.GPIO_PIN_D5, true, 20, Pins.GPIO_NONE);
        private static RainRecorder Hujan   = new RainRecorder(Pins.GPIO_PIN_D7, 0.2); //incase resolution = 0.2
        private static AnalogSensor A0      = new AnalogSensor(Pins.GPIO_PIN_A0, 4, 1);  // incase resoulution = 0.1 (1C/10mv)
        
        private static string[] _data = new string[5];
        private static string data="";
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

        private static int _Modulus = 10;

        private static int _Second
        { get { return DateTime.Now.Second; } }

        #endregion

        #region Costum
        private static bool _wakeUp = true;
        private static bool _timeToLog = true;
        private static bool _timeToSwitch = true;
        private static bool _getDistinghuiser = true;
        private static Timer _displayTimer;
        private static Container SuhuContainer      = new Container(A0.value, 0.1);
        private static Container HumidityContainer  = new Container(A0.value, 0.1);
        private static Container AirPressContainer  = new Container(A0.value, 0.1);

        private static string _msgDisplay = " ";

        private static string MsgDisplay
        {
            get { return _msgDisplay; }

            set { _msgDisplay = value; }
        }
        #endregion

        public static void Main()
        {
            #region Initial

            myLcd.Begin(16, 2); // Turn On LCD
            myLcd.Backlight = false;
            myLcd.Write("AllRightReserved"); myLcd.SetCursorPosition(0, 1); 
            myLcd.Write("MetindoInstrumen"); Thread.Sleep(3000);
            myLcd.Clear(); myLcd.Home(); myLcd.Write("Turning On GSM..");
            bool IsSMSError = false;
            myLcd.SetCursorPosition(0, 1);
            modem.Hit_pwrBtn(500); Debug.Print("Hit Modem power");

            while (!modem.Ready) { Thread.Sleep(100); myLcd.SetCursorPosition(0, 1); myLcd.Write("Please Wait For a sec.."); }

            if (modem.Ready) { modem.ATnF(); modem.DeleteAllSMS(); myLcd.Write(FillLine("Connected....."), 0, 16); }
            else { myLcd.Write(FillLine("NOT connected!!!"),0,16); } Thread.Sleep(2000);
            
            myLcd.Clear(); myLcd.Home(); myLcd.Write("Turning On RTC.."); myRtc.Start();
            Thread.Sleep(500); myLcd.Clear();
            modem.Hit_pwrBtn(2500); modem.Ready = false; Debug.Print("Hit Modem power"); //turn off Gsm 
            myLcd.Backlight = false;// LCD back light has a reverse logic, so this will turn it off.

            #endregion 
            _displayTimer = new Timer(updateDisplay, null, 500, 500);

            while (true)
            {
                if (_Today != _CurrentDay) { daily(); }
                _CurrentDay = _Today;

                if (_Minute % _Modulus > 0)
                {
                    #region BeforLogging
                    #region ModemWakeUp
                    if ((_Minute % _Modulus == _Modulus - 1) && (_Second >= 30) && _wakeUp)
                    {
                        modem.Hit_pwrBtn(10);//turn on modem
                        ResetBeforeLogging();
                        MsgDisplay = "TurningOn GSM";
                    }
                    #endregion

                    if (_getDistinghuiser)
                    {
                        int x = 0;
                        while (A0.value < 2800)
                        {
                            int n = 0;
                            Switcher.Switch();
                            while (n < 6) { A0.Read(); Thread.Sleep(50); n++; }
                            Debug.Print(A0.valueToString); x++;

                            if (x < 8) { MsgDisplay = "R: Locating Mark"; }
                            else { MsgDisplay = "E: Switch Failed"; Logger.Reporting("Stuck: Switch Failed"); }
                            Thread.Sleep(50);
                        }
                        if (A0.value > 2800)
                        { Switcher.Channel = 0; _getDistinghuiser = false; }
                    }
                    if (_Second % 15 == 0)
                    {
                        if (_timeToSwitch)
                        {
                            Switcher.Switch();
                            if (Switcher.Channel == 0 || Switcher.Channel == 4) { Switcher.Channel = 0; _getDistinghuiser = true; }
                            _timeToSwitch = false;
                        }
                    }
                    else { _timeToSwitch = true; }

                    switch (Switcher.Channel)
                    {
                        case 0:
                            A0.Read();
                            MsgDisplay = "M :" + A0.valueToString + " mV";
                            break;
                        case 1:
                            A0.Read();
                            SuhuContainer.Collect(A0.value);
                            MsgDisplay = "T :" + SuhuContainer.valueToString + "C";
                            break;
                        case 2:
                            A0.Read();
                            HumidityContainer.Collect(A0.value);
                            MsgDisplay = "RH :" + HumidityContainer.valueToString + "%";
                            break;
                        case 3:
                            A0.Read();
                            AirPressContainer.Collect(A0.value);
                            MsgDisplay = "P :" + AirPressContainer.valueToString + " mb";
                            break;
                        default:
                            break;
                    }
                    _data[0] = SuhuContainer.valueToString; _data[1] = HumidityContainer.valueToString; 
                    _data[2] = AirPressContainer.valueToString;
                    _data[3] = Hujan.valueToString; _data[4] = ArahAngin.pointTo16;

                    #endregion
                }

                else
                {
                    #region Logging
                    if (_timeToLog)
                    {
                        MsgDisplay = "R: Logging Data!";
                        for (int i = 0; i < 5; i++)
                        {
                            data = data + "," + _data[i];
                        }
                        Logger.ContentFile = _Hour.ToString() + ":" + _Minute.ToString() + data;
                        Hujan.Reset();
                        if (!Logger.Write())
                        {
                            MsgDisplay = "E:LoggingFailed";
                            if (Logger.Report != "") { Logger.Reporting(Logger.Report); } 
                            else { Logger.Reporting("Logging Failed"); }
                        }
                        try
                        {
                            if (modem.Ready)
                            {
                                MsgDisplay = "R:SendingData...";
                                modem.SendSMS(Logger.NoDest, Logger.ContentFile);
                                while (!modem.SmsSent && !modem.CommandError)
                                {
                                    Thread.Sleep(100);
                                }
                                if (modem.CommandError)
                                {
                                    Logger.Reporting("Sending Data Failed... Please Check Simcard or Balance");
                                    MsgDisplay = "E:SendDataFailed";
                                    IsSMSError = true;
                                }
                                Thread.Sleep(3000); // SMS not sent even reach "CMGS:OK", 
                                //adding a bunch of delay to give time for SIM300 to finish its works before it's turn off
                            }
                            else
                            {
                                MsgDisplay = "E: GSMnotReady";
                                Logger.Reporting("E: GSMnotReady");
                                Thread.Sleep(1000);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Reporting(ex.Message);
                        }
                        ResetAfterLogging(); modem.Hit_pwrBtn(100);
                        if (IsSMSError)
                        {
                            MsgDisplay = "E:SendDataFailed"; 
                        }
                        else
                        {
                            MsgDisplay = "Data Sent!";
                        }
                    }
                    #endregion
                }
            }
        }
        #region Class
        static readonly byte[] _lineBuffer = new byte[16];
        private static int StringStart, StringStop = 0;
        private static int ScrollCursor = 16;
        private static byte[] FillLine(string text)
        {
            // fill empty space
            try
            {
                    for (int i = 0; i < _lineBuffer.Length; i++)
                        _lineBuffer[i] = (byte)' ';

                    // write new text
                    var bytes = Encoding.UTF8.GetBytes(text);
                    bytes.CopyTo(_lineBuffer, 0);

                    return _lineBuffer;
                
            }
            catch (Exception ex)
            {
                Logger.Reporting(ex.Message); Debug.Print(ex.Message);
                return Encoding.UTF8.GetBytes(text);
            }
        }
        private static void daily()
        {
            try
            {
                Debug.Print("Daily Task!....start @ :" + DateTime.Now); Logger.Namefile = _Today.ToString();
                if (Logger.CheckPath("dataLog", _Year.ToString(), _Month.ToString()))
                {
                    Debug.Print("Path Logging Ok!"); MsgDisplay = "Path Logging Ok!"; Thread.Sleep(500);
                }
                if (modem.Ready)
                {
                    modem.DeleteAllSMS();
                }
                Thread.Sleep(300); 
                Debug.Print("Daily Task!.... ends");
            }
            catch (Exception ex)
            {
                MsgDisplay = "Error" + ex.Message;
                Logger.Reporting(ex.Message);
            }
        }
        private static void updateDisplay(object state)
        {
            var dt = DateTime.Now;
            myLcd.Home();
            myLcd.Write(FillLine(dt.ToString("ddMMMyy HH:mm:ss")), 0, 16);
            myLcd.SetCursorPosition(0, 1); myLcd.Write(FillLine(MsgDisplay),0,16);
            
        }

        private static void ScrolMsg(String Message)
        {
            myLcd.SetCursorPosition(ScrollCursor, 1);
            myLcd.Write(Message.Substring(StringStart));

            if (StringStart == 0 && ScrollCursor > 0)
            {
                ScrollCursor--;
                StringStop++;
            }
            else if (StringStart == StringStop)
            {
                StringStart = StringStop = 0;
                ScrollCursor = 16;
            }
            else if (StringStop == Message.Length && ScrollCursor == 0)
            {
                StringStart++;
            }
            else
            {
                StringStart++;
                StringStop++;
            }
        }
        private static void ResetBeforeLogging()
        {
            _wakeUp = false;
            _timeToLog = true;
            modem.CommandError = false;
        }
        private static void ResetAfterLogging()
        {
            _wakeUp = true;
            modem.Ready = false;
            _timeToLog = false;
            data = "";
            AirPressContainer.Reset();
            SuhuContainer.Reset();
            HumidityContainer.Reset();
        }
        #endregion
    }
}
