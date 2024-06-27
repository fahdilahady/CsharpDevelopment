using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Text;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;
using System.IO.Ports;

namespace SutronListener
{
    public abstract class aMniSerialCom 
    {
        protected static  string cReadyString ="";
        protected static string awatingResponseString = "";
        protected static bool awaitingResponse = false;
        protected static bool _connected = false;
        protected static bool _error = false;
        protected static bool _sent = false;
        protected static string _report;
        
        private static SerialPort comPort;
        public SerialPort ComPort { get { return comPort; } set { comPort = value; } }
        public static string ReadyString { get { return cReadyString; } set { cReadyString = value; } }

        protected abstract string cLineFeedChar
        { get;  }
        

        public aMniSerialCom(string thePortName, int baudRate, Parity parity , int dataBits , StopBits stopBits)
        {
            ComPort = new SerialPort(thePortName, baudRate, parity, dataBits, stopBits);
            ComPort.Handshake = Handshake.XOnXOff;
            ComPort.ReadTimeout = -1;
            ComPort.WriteTimeout = 10;
                 
        }    
        ~aMniSerialCom()
        { }
        
        public bool isOpenned()
        {
            if (!comPort.Equals(null))
            {
                return ComPort.IsOpen;
            }
            else { return false; }
        }

        public virtual void OpenCommunication() { }
        protected virtual void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) { }
        
        protected static string GetUTF8StringFrombytes(byte[] byteVal)
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

        protected void PrintByte(byte line)
        {
            try
            {
                ComPort.WriteByte(line);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
        }

        protected void Print(string line)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                byte[] bytesToSend = encoder.GetBytes(line);
                if (bytesToSend.Length > 0)
                {
                    comPort.Write(bytesToSend, 0, bytesToSend.Length);
                }
                
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
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
            Print(line + cLineFeedChar);
            //Print(line); PrintByte(13); PrintByte(10);
            if (awaitResponse)
            {
                awaitingResponse = true;
                while (awaitingResponse)
                {
                    Thread.Sleep(100);
                }

            }
        }

        protected void PrintEnd()
        {
            byte[] bytesToSend = new byte[1];
            bytesToSend[0] = 26;
            ComPort.Write(bytesToSend, 0, 1);
            Thread.Sleep(200);
        }


    }
}
