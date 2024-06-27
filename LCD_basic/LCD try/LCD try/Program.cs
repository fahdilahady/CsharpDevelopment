using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using MicroLiquidCrystal;

namespace LCD_try
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            var lcdProvider = new GpioLcdTransferProvider(Pins.GPIO_PIN_D7, Pins.GPIO_PIN_D8, Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D10, Pins.GPIO_PIN_D11, Pins.GPIO_PIN_D12);
            OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);


            // Option 2: Use shift register provider
            //var lcdProvider = new Shifter74Hc595LcdTransferProvider(SPI_Devices.SPI1, Pins.GPIO_PIN_D10,
            //    Shifter74Hc595LcdTransferProvider.BitOrder.MSBFirst);
          
            // create the LCD interface
            var lcd = new Lcd(lcdProvider);

            // set up the LCD's number of columns and rows: 
            lcd.Begin(16, 2);

            // Print a message to the LCD.
            lcd.Write("hello, world!");

            while (true)
            {
                // set the cursor to column 0, line 1
                lcd.SetCursorPosition(0, 1);
                led.Write(true); lcd.Backlight = true;

                Thread.Sleep(250);
                lcd.Backlight = false;
                led.Write(false);
                Thread.Sleep(250);

                // print the number of seconds since reset:
                lcd.Write((Utility.GetMachineTime().Ticks / 10000).ToString());

                Thread.Sleep(100);
            }
        }

    }
}
