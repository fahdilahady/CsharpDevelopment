using System;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using System.IO.Ports;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using ADC;

//using RTC;

namespace AnalogReadClass
{
    public class Program
    {
        public static int X = 0;
        public static void Main()
        {
            // write your code here
            

            AnalogInput A0 = new AnalogInput(Pins.GPIO_PIN_A0);
            ADC.Conversion Suhu;
            DataStorage Log = new DataStorage("testFile.csv", "testPath");
            Suhu = new ADC.Conversion(A0.Read());
            Container Contain = new Container();
            

            while (true)
            {
            
            X = X + 1;
            Suhu.analogread(A0.Read());

            //Log.ContentFile = Contain.MovingAverage(Suhu.miliVolts).ToString("F");
            //if (Log.Write())
            //{
                Debug.Print( Suhu.miliVolts.ToString("F") + " " + Contain.MovingAverage(Suhu.miliVolts).ToString("F"));
            //}
            //else { Debug.Print("Write Data Error"); }
            
            Thread.Sleep(1000);
            }
        }

    }
}
