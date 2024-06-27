using System;
using Microsoft.SPOT.Time;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.IO.Ports;
using System.Text;
using System.IO;
//using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;

namespace SutronListener
{
    public class aMniFastTrackModem : aMniSerialCom
    {
        protected static string _SIM_APN="internet";
        protected static string _SIM_userName;
        protected static string _SIM_Pass;
        protected static string _FTP_server;
        protected static string _FTP_userName;
        protected static string _FTP_Pass;
        private static DateTime modemDT;
        private static string _cLineFeedChar = "\r\n";
        private bool WIPBR_active = false;
        private bool responseOK = false;
        private bool isRuntime = false;
        protected override string cLineFeedChar
        {
            get { return _cLineFeedChar; }
        }

        public DateTime myDT
        {
            get { return modemDT; }
            set { modemDT = value; }
        }
        private static bool timeSynced = false;

        public bool isTimeSynced
        {
            get { return timeSynced; }
            set { timeSynced = value; }
        }
        protected static StringBuilder output = new StringBuilder();
        
        public aMniFastTrackModem(string thePortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(thePortName, baudRate, parity, dataBits, stopBits)
        {
            ///<summary>
            ///this is SIERRA WIRELESS MODEM 
            ///MODEL : FXT009 EDGE
            ///ATCOMMAND VERSION : WIPSOFt 5.41
            ///</summary            
               
        }

        public void initSets()
        {
            PrintLine("AT"); Thread.Sleep(500);          
            this.ATnF(); Thread.Sleep(200);
            
            WOpenAT(0); Thread.Sleep(5000);
            WOpenAT(1); Thread.Sleep(15000);
            //WOpenAT(0); Thread.Sleep(5000); 
            //WOpenAT(1); Thread.Sleep(15000);
        }
        protected static void ParseStringToDateTime(string ThisString)
        {
            char []separator =  {',','"'};
            string[] words;
            string tempStr = "";
            string date = ""; string time = "";
            int Year, Month, Day, Hour,Minute,Second;
            //example to parse: +WIND: 15,1,"TELKOMSEL",4,"16/01/09,06:30:14+28",6,"0"
            if (ThisString.IndexOf("+WIND: 15") > -1)
            {
                tempStr = ThisString.Substring(ThisString.IndexOf("+WIND: 15"), 53);
                words = tempStr.Split(separator);
                  string [] dates = words[7].Split('/');
                  string[] times = words[8].Split(':');
                  Year = 2000 + Int32.Parse(dates[0]);
                  Month = Int32.Parse(dates[1]);
                  Day = Int32.Parse(dates[2]);
                  Hour = Int32.Parse(times[0]);
                  Minute = Int32.Parse(times[1]);
                  Second = Int32.Parse(times[2].Substring(0,2));
                  //if (modemDT.Equals(null))
                  //{
                  DateTime UTC= new DateTime(Year, Month, Day, Hour, Minute, Second);
                  modemDT = DateTime.SpecifyKind(UTC, DateTimeKind.Utc);
                  TimeSpan span = new TimeSpan(7, 0, 0);
                  modemDT.Add(span); 

                  foreach (string s in dates)
                  {
                      date +=s + "/";
                  }Debug.Print(date);
                  foreach (string s in times)
                  {
                      time += s + ":";
                  }Debug.Print(time);
                
                
            }
        }
        public override void OpenCommunication()
        {
            //base.OpenCommunication();
            if (!isOpenned())
            {
                ComPort.Open(); Debug.Print(ComPort.PortName.ToString() + " opened");
                ComPort.DataReceived += new SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            }

        }
        protected override void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) 
        {
            // Check if Chars are received

            if (e.EventType == SerialData.Chars)
            {
                // Create new buffer
                byte[] ReadBuffer = new byte[ComPort.BytesToRead];
                // Read bytes from buffer
                ComPort.Read(ReadBuffer, 0, ReadBuffer.Length);
                

                if (ReadBuffer.Length > 0 && (ReadBuffer[ReadBuffer.Length - 1] == 10 || ReadBuffer[ReadBuffer.Length - 1] == 0))
                {
                    // New line or terminated string.
                    output.Append(GetUTF8StringFrombytes(ReadBuffer));


                    if (!awatingResponseString.Equals("") && output.ToString().IndexOf(awatingResponseString) > -1)
                    {
                        Debug.Print("Response Matched : " + output.ToString()); responseOK = true;
                        awatingResponseString = "";

                    }
                    else
                    {
                        if (output.ToString().IndexOf("OK") > -1)
                        {
                            Debug.Print(output.ToString());
                        }
                        if (output.ToString().IndexOf("ERROR") > -1)
                        {
                            _error = true;
                            Debug.Print("Command Error");
                        }
                        if (output.ToString().IndexOf("+CMGS:") > -1)
                        {
                            _sent = true;
                            Debug.Print("SMS Sent !!!");
                        }
                        if (output.ToString().IndexOf("+WIND: 15") > -1)
                        {
                            if (!isRuntime)
                            {
                                ParseStringToDateTime(output.ToString());
                            }
                            timeSynced = true;
                        }
                        if (output.ToString().IndexOf("+WIPBR: 6,1") > -1)
                        {
                            WIPBR_active = true;
                            Debug.Print("WIPBR activated");
                        }
                        Debug.Print("Recieved : " + output.ToString());

                    }
                    output.Clear();
                    awaitingResponse = false;
                    
                }
                else
                {
                    try
                    {
                        output.Append(GetUTF8StringFrombytes(ReadBuffer));
                        
                        Debug.Print(output.ToString());
                    }
                    catch (Exception ecx)
                    {

                        Debug.Print("Cannot parse : " + ecx.StackTrace);
                    }
                }
            }
        }

        #region CommonGSMCommand
        public void ATnF()
        {
            try
            {
                Print("AT&F" + "\r\n"); awatingResponseString = "OK";
                Thread.Sleep(100);
                //Print("AT+IPR?" + "\r");
            }
            catch
            {
            }
        }

        public void SendSMS(string msisdn, string message)
        {
            _sent = false;

            try
            {
                Print("AT+CMGF=1" + "\r");
                Thread.Sleep(300);
                //PrintLine("AT+CMGS=\"" + msisdn + "\"", false);
                Print("AT+CMGS="); Thread.Sleep(100);
                PrintByte(34); Print(msisdn); PrintByte(34); Print("\r");
                Thread.Sleep(250);
                Print(message + "\r");
                Thread.Sleep(100);
                PrintByte(26); Print("\r");
                Thread.Sleep(600);
            }
            catch (Exception ecx)
            {
                Debug.Print("SendSMS: " + ecx.Message);
                _report = ecx.Message;
            }
        }

        public void DeleteAllSMS()
        {
            try
            {
                Debug.Print("SMS All delete");
                PrintLine("AT+CMGD=0,4", true);
                Thread.Sleep(100);
                PrintEnd();
                Thread.Sleep(500);
            }
            catch (Exception ecx)
            {
                Debug.Print("DeleteAllSMS : " + ecx.Message.ToString());
            }
        }

        public void placeCall(string msisdn)
        {
            PrintLine("ATD " + msisdn + ';');
            Thread.Sleep(500);
            Debug.Print("Calling....." + msisdn);
        } 
        #endregion

        #region WGPRSSetUp

        protected void WOpenAT(int mode)
        {
            /// <summary>
            /// This Wavecom proprietary command performs the start, stop, delete, and get information about the current Open AT®embedded application.//
            /// </summary>
            PrintLine("at+wopen=" + mode.ToString());
            awatingResponseString = "OK"; Thread.Sleep(230);
        }
        protected void WIPStackHandling(int mode)
        {
            ///<summary>
            ///the +WIPCFG command is used for performing the following ops:
            ///    - Start TCP/IP stack
            ///    - Stop TCP/IP stack
            ///    - Configuring TCP/IP stack
            ///    - displayingg version information
            ///</summary>

            PrintLine("at+wipcfg=" + mode.ToString()); awatingResponseString = "OK";
            Thread.Sleep(1000);
        }
        protected void WBearerHandling(bool open, int bid)
        {
            ///<summary>
            /// - start/close the bearer
            ///     - bid 5 = GSM
            ///     - bid 6 = GPRS
            /// </summary>
            if (open)
            {
                PrintLine("at+wipbr=1," + bid.ToString());
            }
            else
            {
                PrintLine("at+wipbr=0," + bid.ToString());
            } awatingResponseString = "OK";
            Thread.Sleep(500);
        }
        protected void WBearerSetValueGPRS(string APN, string userName, string password)
        {
            Print("at+wipbr=2,6,11,"); Thread.Sleep(500);//APN
            PrintByte(34); Print(APN); PrintByte(34); awatingResponseString = "OK"; Print("\r\n"); Thread.Sleep(550);

            Print("at+wipbr=2,6,0,"); Thread.Sleep(500); //UserName
            PrintByte(34); Print(userName); PrintByte(34); awatingResponseString = "OK"; Print("\r\n"); Thread.Sleep(550);

            Print("at+wipbr=2,6,1,"); Thread.Sleep(500); //Pass
            PrintByte(34); Print(password); PrintByte(34); awatingResponseString = "OK"; Print("\r\n"); Thread.Sleep(550);

        }
        protected void WStartBearer(int bid)
        {
            ///bid 5 GSM
            ///bid 6 GPRS
            PrintLine("at+wipbr=4,6,0"); awatingResponseString = "OK"; Thread.Sleep(5000);
        }
        protected void WCreateFTPConnection(int bid, string serverIPConfig, string userName, string password)
        {
            //string command = "";
            //command = "AT+WIPCREATE=4,1," + bid.ToString() + "," + '"' + serverIPConfig + '"' + ",21," + '"' + userName + '"' + "," + '"' + password + '"';
            //PrintLine(command); Thread.Sleep(10000);
            Print("at+wipcreate=4,1," + bid.ToString() + ","); Thread.Sleep(50);
            PrintByte(34); Print(serverIPConfig); PrintByte(34);
            Print(",21,"); //Port
            PrintByte(34); Print(userName); PrintByte(34); Print(",");
            PrintByte(34); Print(password); PrintByte(34); Print("\r\n"); awatingResponseString = "OK"; Thread.Sleep(250);
        }
        protected void WConstructFile(string fileName)
        {
           // string command = "";
           // command = "AT+WIPFILE=4,1,6," + '"' + '@' + fileName + '"';
           // PrintLine(command); Thread.Sleep(5000);
            Print("at+wipfile=4,1,2,"); Thread.Sleep(50);
            PrintByte(34); Print('@'+fileName); PrintByte(34); Print("\r\n"); awatingResponseString = "OK"; Thread.Sleep(500);
            Print("at+wipfile=4,1,1,"); Thread.Sleep(50);
            PrintByte(34); Print('@' + fileName); PrintByte(34); Print("\r\n"); awatingResponseString = "OK"; Thread.Sleep(500);

        }
        protected void WFTPCloseSession()
        {
            PrintLine("AT+WIPCLOSE=4,1"); Thread.Sleep(500);
        }
        public void setAPN(string APN, string userName, string password)
        {
            _SIM_APN = APN;
            _SIM_Pass = password;
            _SIM_userName = userName;
        }
        public void setFTPServer(string Server, string userName, string password)
        {
            _FTP_server = Server;
            _FTP_Pass = password;
            _FTP_userName = userName;
        }
        
        #endregion

        public void sendFTPFile(string FileName)
        {
            try
            {
                _error = false; isRuntime = false; timeSynced = false;
                
                //ATnF(); Thread.Sleep(200);
                PrintLine("AT");
                WOpenAT(0); Thread.Sleep(5000);
           
                WOpenAT(1); Thread.Sleep(15000);
           
                WIPStackHandling(1);
                WBearerHandling(true, 6); //open GPRS mode
                if (responseOK)
                {
                    responseOK = false;
                    WBearerSetValueGPRS(_SIM_APN, _SIM_userName, _SIM_Pass); //GPRS authentication
                    WStartBearer(6); //startGPRS Connection
                    if (responseOK)
                    {
                        responseOK = false;
                        WCreateFTPConnection(6, _FTP_server, _FTP_userName, _FTP_Pass); //FTP authentication via GPRS bid =6
                        WConstructFile(FileName);
                    }
                    else if (_error)
                    { }
                }
                WFTPCloseSession();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
        }

    }
}
