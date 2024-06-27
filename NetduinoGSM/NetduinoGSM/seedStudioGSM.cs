using System;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;

namespace seedStudioGSM
{
    class seedStudioGSM
    {
        static string awatingResponseString = "";
        static bool awaitingResponse = false;
        static bool connected = false;

        static SerialPort serialPort;
        const int bufferMax = 1024;
        static byte[] buffer = new Byte[bufferMax];
        static StringBuilder output = new StringBuilder();

        //static int bufferLength = 0;
        public seedStudioGSM(string portName = SerialPorts.COM1, int baudRate = 19200, Parity parity = Parity.Odd, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.ReadTimeout = -1;
            serialPort.WriteTimeout = 10;
            serialPort.Handshake = Handshake.XOnXOff;
            serialPort.Open();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }
        static byte[] readbuff(byte[] inputBytes)
        {
            for (int i = 0; i < inputBytes.Length; i++)
            {
                if (inputBytes[i] == 230)
                {
                    inputBytes[i] = 32;
                }
            }

            return inputBytes;
        }
        static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Check if Chars are received
            if (e.EventType == SerialData.Chars)
            {
                // Create new buffer
                byte[] ReadBuffer = new byte[serialPort.BytesToRead];
                // Read bytes from buffer
                serialPort.Read(ReadBuffer, 0, ReadBuffer.Length);


                if (ReadBuffer.Length > 0 && (ReadBuffer[ReadBuffer.Length - 1] == 10 || ReadBuffer[ReadBuffer.Length - 1] == 0))
                {
                    // New line or terminated string.
                    output.Append(GetUTF8StringFrombytes(ReadBuffer));


                    if (!awatingResponseString.Equals("") && output.ToString().IndexOf(awatingResponseString) > -1)
                    {
                        Debug.Print("Response Matched : " + output.ToString());
                        awatingResponseString = "";

                    }
                    else
                    {
                        if (output.ToString().IndexOf("CONNECT OK") > -1)
                        {
                            connected = true;
                            Debug.Print("CONNECTED !!!!!!!!!!!");
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

                        //output.Append(UTF8Encoding.UTF8.GetChars(readbuff(ReadBuffer)));
                        output.Append(GetUTF8StringFrombytes(ReadBuffer));
                        //UTF8Encoding.UTF8.GetChars(
                    }
                    catch (Exception ecx)
                    {

                        Debug.Print("Cannot parse : " + ecx.StackTrace);
                    }
                }
            }
        }

        private void Print(string line)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            byte[] bytesToSend = encoder.GetBytes(line);
            serialPort.Write(bytesToSend, 0, bytesToSend.Length);
        }
        public void PrintLine(string line, bool awaitResponse = false, string awaitResponseString = "")
        {
            if (!awaitResponseString.Equals(""))
            {
                awatingResponseString = awaitResponseString;
                while (!awatingResponseString.Equals(""))
                {
                    Thread.Sleep(1000);
                }
            }
            Print(line + "\r");
            if (awaitResponse)
            {
                awaitingResponse = true;
                while (awaitingResponse)
                {
                    Thread.Sleep(100);
                }

            }
        }
        public void ReadAllSMS()
        {
            output.Clear();
            PrintLine("AT+CSCS=\"8859-1\"", true);
            //PrintLine("AT+CSCS?", true);

            PrintLine("AT+CMGL=\"ALL\",0");
            Thread.Sleep(1000);
        }
        public SMS ReadSMS(int number)
        {
            output.Clear();
            SMS response = new SMS();
            PrintLine("AT+CMGR=" + number.ToString() + ",0");
            Thread.Sleep(1000);
            throw new NotImplementedException();

            return response;
        }
        public void postRequest(string apn, string gatewayip, string host, string page, string port, string parameters)
        {
            //setup_start:
            PrintEnd();
            serialPort.Flush();
            PrintLine("ATE1", true);
            PrintLine("AT+CIPMUX=0", true); //We only want a single IP Connection at a time.
            PrintLine("AT+CIPMODE=0", true); //Selecting "Normal Mode" and NOT "Transparent Mode" as the TCP/IP Application Mode
            PrintLine("AT+CGDCONT=1,\"IP\",\"" + apn + "\",\"" + gatewayip + "\",0,0", true); //Defining the Packet Data
            //Protocol Context - i.e. the Protocol Type, Access Point Name and IP Address
            PrintLine("AT+CSTT=\"" + apn + "\"", true);//Start Task and set Access Point Name (and username and password if any)
            PrintLine("AT+CIPSHUT", true); //Close any GPRS Connection if open
            PrintLine("AT+CIPSTART=\"TCP\",\"" + host + "\",\"" + port + "\"", false);
            Thread.Sleep(300);
            while (connected == false)
            {
                Thread.Sleep(1000);
            }
            connected = false;
            //Thread.Sleep(10000);// Wait until CONNECT OK is recieved can be up to 8 secs (should make something smarter)
            serialPort.Flush();

            PrintLine("AT+CIPSEND");//Start data through TCP connection
            serialPort.Flush();
            Thread.Sleep(1000);
            StringBuilder POST = new StringBuilder();
            POST.Append(parameters);
            Print("POST " + page + " HTTP/1.1\r\nHost: " + host + "\r\nContent-Length: " + POST.Length.ToString() + "\r\n\r\n" + POST.ToString());
            PrintEnd();
            Thread.Sleep(5000);
            PrintLine("AT+CIPACK", true);
            Thread.Sleep(300);
            PrintLine("AT+CIPSHUT", true); //Close any GPRS Connection if open
            Thread.Sleep(1000000);
        }
        public void getRequest(string apn, string gatewayip, string host, string page, string port)
        {
            //setup_start:
            PrintEnd();
            serialPort.Flush();
            PrintLine("ATE1", true);
            PrintLine("AT+CIPMUX=0", true); //We only want a single IP Connection at a time.
            PrintLine("AT+CIPMODE=0", true); //Selecting "Normal Mode" and NOT "Transparent Mode" as the TCP/IP Application Mode
            PrintLine("AT+CGDCONT=1,\"IP\",\"" + apn + "\",\"" + gatewayip + "\",0,0", true); //Defining the Packet Data
            //Protocol Context - i.e. the Protocol Type, Access Point Name and IP Address
            PrintLine("AT+CSTT=\"" + apn + "\"", true);//Start Task and set Access Point Name (and username and password if any)
            PrintLine("AT+CIPSHUT", true); //Close any GPRS Connection if open
            PrintLine("AT+CIPSTART=\"TCP\",\"" + host + "\",\"" + port + "\"", false);
            Thread.Sleep(300);
            Thread.Sleep(10000);// Wait until CONNECT OK is recieved can be up to 8 secs (should make something smarter)
            serialPort.Flush();
            PrintLine("AT+CIPSEND");//Start data through TCP connection
            serialPort.Flush();
            Thread.Sleep(1000);
            StringBuilder POST = new StringBuilder();
            POST.Append("channel=1&rnd=329999932324&colorCode=33;120;255");
            //Print("POST " + page + " HTTP/1.1\r\nHost: " + host + "\r\nContent-Length: " + POST.Length.ToString() + "\r\n\r\n" + POST.ToString());
            Print("GET " + page + " HTTP/1.1\\r\\n");
            Thread.Sleep(300);
            Print("Host: " + host + "\\r\\n");
            Thread.Sleep(300);
            Print("Connection: close\\r\\n");
            Thread.Sleep(300);
            PrintEnd();
            Thread.Sleep(5000);
            PrintLine("AT+CIPACK", true);
            Thread.Sleep(300);
            PrintLine("AT+CIPSHUT", true); //Close any GPRS Connection if open
            Thread.Sleep(1000);
        }
        public void SendSMS(string msisdn, string message)
        {
            //PrintLine("");
            PrintLine("AT+CMGF=1", true);
            PrintLine("AT+CMGS=\"" + msisdn + "\"", false);
            PrintLine(message);
            Thread.Sleep(100);
            PrintEnd();
            Thread.Sleep(500);
            //Debug.Print("SMS Sent!");
        }
        public void placeCall(string msisdn)
        {
            PrintLine("ATD" + msisdn);
            Thread.Sleep(100);
            Debug.Print("Calling....." + msisdn);
        }
        private void PrintEnd()
        {
            byte[] bytesToSend = new byte[1];
            bytesToSend[0] = 26;
            serialPort.Write(bytesToSend, 0, 1);
            Thread.Sleep(200);
        }
        private static string GetUTF8StringFrombytes(byte[] byteVal)
        {
            byte[] btOne = new byte[1];
            StringBuilder sb = new StringBuilder("");
            char uniChar;
            for (int i = 0; i < byteVal.Length; i++)
            {
                btOne[0] = byteVal[i];
                if (btOne[0] > 127)
                {
                    uniChar = Convert.ToChar(btOne[0]);
                    sb.Append(uniChar);
                }
                else
                    sb.Append(new string(Encoding.UTF8.GetChars(btOne)));
            }
            return sb.ToString();
        }
    }
}