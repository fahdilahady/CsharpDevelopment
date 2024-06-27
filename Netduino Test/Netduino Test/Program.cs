using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.IO.Ports;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace Netduino_Test
{
    public class Program
    {
        
        public static void Main()
        {
            
            // write your code here
            //*** Turn Of all Digital Pins as Default....
            OutputPort D0 = new OutputPort(Pins.GPIO_PIN_D0, false);
            OutputPort D1 = new OutputPort(Pins.GPIO_PIN_D1, false);
            OutputPort D2 = new OutputPort(Pins.GPIO_PIN_D2, false);
            OutputPort D3 = new OutputPort(Pins.GPIO_PIN_D3, false);
            OutputPort D4 = new OutputPort(Pins.GPIO_PIN_D4, false);
            OutputPort D5 = new OutputPort(Pins.GPIO_PIN_D5, false);
            OutputPort D6 = new OutputPort(Pins.GPIO_PIN_D6, false);
            OutputPort D7 = new OutputPort(Pins.GPIO_PIN_D7, false);
            OutputPort D8 = new OutputPort(Pins.GPIO_PIN_D8, false);
            OutputPort D9 = new OutputPort(Pins.GPIO_PIN_D9, false);
            OutputPort D10 = new OutputPort(Pins.GPIO_PIN_D10, false);
            OutputPort D11 = new OutputPort(Pins.GPIO_PIN_D11, false);
            OutputPort D12 = new OutputPort(Pins.GPIO_PIN_D12, false);
            OutputPort D13 = new OutputPort(Pins.GPIO_PIN_D13, false);    
           
            OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
            

            while (true)
            {
                
                led.Write(true); // turn on the LED
                
                Thread.Sleep(1000); // sleep for 250ms
                led.Write(false); // turn off the LED
                Thread.Sleep(500); // sleep for 250ms
            }
        }

    }
}
