using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
namespace SutronListener
{
    public class aMniSutronCom : aMniSerialCom
    {
        protected static StringBuilder output = new StringBuilder();

        public aMniSutronCom(string thePortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(thePortName, baudRate, parity, dataBits, stopBits)
        { }
        private static bool _ready;
        private static bool _isDataRecieved;
        private static string dataRecieved;
        private static string _cLineFeedChar = "\r";
        protected override string cLineFeedChar
        {
            get { return _cLineFeedChar; }
        }

        public string DataRecieved
        {
            get { return dataRecieved; }
        }
        public bool IsDataRecieved
        {
            get { return _isDataRecieved; }
            set { _isDataRecieved = value; }
        }
        public bool Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }

        public override void OpenCommunication()
        {
            //base.OpenCommunication();
            if (!isOpenned())
            {
                ComPort.Open(); Debug.Print(ComPort.PortName.ToString() + " opened");
                ComPort.DataReceived += new SerialDataReceivedEventHandler(this.serialPort_DataReceived);
                Thread.Sleep(150); Print("\r");
            }
        }
        protected override void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) 
        {
            if (e.EventType == SerialData.Chars)
            {
                // Create new buffer
                byte[] ReadBuffer = new byte[ComPort.BytesToRead];
                // Read bytes from buffer
                ComPort.Read(ReadBuffer, 0, ReadBuffer.Length);


                if (ReadBuffer.Length > 0 && (ReadBuffer[ReadBuffer.Length - 1] == 62 || ReadBuffer[ReadBuffer.Length - 1] == 10 ||  ReadBuffer[ReadBuffer.Length - 1] == 0))
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
                        if (output.ToString().IndexOf("Flash Disk") > -1)
                        {
                            _ready = true;
                            Debug.Print(output.ToString());
                        }
                        if (output.ToString().IndexOf("TipBktAcc") > -1)
                        {

                            dataRecieved = output.ToString(); _isDataRecieved = true;
                            Debug.Print(output.ToString());
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
        
        public void RetrieveData()
        {
            try
            {
                this.PrintLine("Get /Newest"); Thread.Sleep(150);
            }
            catch (Exception e)
            { 
                Debug.Print(e.Message);
                
            }
        
        }
    }
}
