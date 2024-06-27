using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using CompassDirection;

namespace RotaryEncoder
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            //RotaryEncoder myEncoder = new RotaryEncoder(Cpu.Pin.GPIO_Pin4, Cpu.Pin.GPIO_Pin5, true, 40, Cpu.Pin.GPIO_Pin6);
            Encoder MyEncoder = new Encoder(Pins.GPIO_PIN_D4, Pins.GPIO_PIN_D5, Pins.GPIO_NONE);
            //Direction windDirection = new Direction();
            InputPort exit = new InputPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled);
            while (!exit.Read())
            {
                //string _direction = windDirection.Degreeto16Direction(myEncoder.windDegree);
                Debug.Print("position=" + MyEncoder.position.ToString() + "   Direction" );
                //if (myencoder.buttonState == true)
                //    myencoder.position = 0;
                Thread.Sleep(1000);
            }

        }

    }
}
