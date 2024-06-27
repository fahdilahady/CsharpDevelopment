using System;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using MicroLiquidCrystal;

namespace RTC_Test
{
    public class Program
    {
        private static Lcd myLcd;
        private static Timer _displayTimer;
        
        public static void Main()
        {
            
            using (var clock = new DS1307RealTimeClock())
            {
                // TODO: Do this only once to set your clock
               clock.SetClock(14, 9, 6, 11, 37, 0, DayOfWeek.Saturday);
               
                clock.SetLocalTimeFromRTC();
                
                Debug.Print("The time is: " + DateTime.Now.ToString("HH:mm:ss"));
                Debug.Print(DateTime.Today.Date.ToString("dd/mm/yyyy"));

                clock.Write(DS1307RealTimeClock.UserDataAddress, new byte[] { 0xab, 0xcd });

                var data = new byte[2]; // This can be up to DS1307RealTimeClock.UserDataLength (56)
                clock.Read(DS1307RealTimeClock.UserDataAddress, data);

                Debug.Print("Breakpoint here to check out the data array in the debugger");
            }
            AnalogInput A0 = new AnalogInput(AnalogChannels.ANALOG_PIN_A0);

            var _lcdProvider = new Shifter74Hc595LcdTransferProvider(SPI_Devices.SPI1, Pins.GPIO_PIN_D10,
                Shifter74Hc595LcdTransferProvider.BitOrder.MSBFirst);
            myLcd = new Lcd(_lcdProvider);
            _displayTimer = new Timer(updateDisplay, null, 500, 500);

            Thread.Sleep(1000);

            myLcd.Begin(16, 2);
            myLcd.SetCursorPosition(0, 1);
            myLcd.Write("Hi Grand Fahdil!!");
            myLcd.Backlight = false;
            Thread.Sleep(2000);
            InputPort d7 = new InputPort(Pins.GPIO_PIN_D7, false, Port.ResistorMode.Disabled);
            int f = 0;
            while (true)
            {
                if (d7.Read()) { f++;}
               Debug.Print(DateTime.Now.ToString());
               Thread.Sleep(500);
               myLcd.SetCursorPosition(0, 1); myLcd.Write(f.ToString());
            }
        }

        private static void updateDisplay(object state)
        {
            var dt = DateTime.Now;
            
            myLcd.Home();
            myLcd.Write(FillLine(dt.ToString("HH:mm:ss")), 0, 16);
 
        }

        static readonly byte[] _lineBuffer = new byte[16];

        private static byte[] FillLine(string text)
        {
            // fill empty space
            for (int i = 0; i < _lineBuffer.Length; i++)
                _lineBuffer[i] = (byte)' ';

            // write new text
            var bytes = Encoding.UTF8.GetBytes(text);
            bytes.CopyTo(_lineBuffer, 0);

            return _lineBuffer;
        }
    }
}
