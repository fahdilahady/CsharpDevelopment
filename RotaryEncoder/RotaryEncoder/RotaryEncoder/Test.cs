using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace RotaryEncoder
{
    /// <summary>
    /// For use with quadrature encoders where signal A and B are 90° out of phase.
    /// </summary>
    /// <remarks>
    /// For a detented rotary encoder such as http://www.sparkfun.com/products/9117
    /// Written by Michael Paauwe
    /// Tested with .NETMF 4.2RC1 and Netduino Plus FW4.2.0RC1
    /// </remarks>
    class Encoder
    {
        private InputPort PhaseB;
        private InterruptPort PhaseA;
        private InterruptPort Button;
        public bool buttonState = false;
        public int position = 0;

        /// <summary>
        /// Constructor for encoder class
        /// </summary>
        /// <param name="pinA">The pin used for output A</param>
        /// <param name="pinB">The pin used for output B</param>
        /// <param name="pinButton">The pin used for the push contact (Optional:GPIO_NONE if not used)</param>
        public Encoder(Cpu.Pin pinA, Cpu.Pin pinB, Cpu.Pin pinButton = Pins.GPIO_NONE)
        {
            PhaseA = new InterruptPort(pinA, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
            PhaseA.OnInterrupt += new NativeEventHandler(PhaseA_OnInterrupt);
            PhaseB = new InputPort(pinB, false, Port.ResistorMode.PullUp);
            if (pinB != Pins.GPIO_NONE)
            {
                if (pinButton != Pins.GPIO_NONE)
                {
                    Button = new InterruptPort(pinButton, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
                    Button.OnInterrupt += new NativeEventHandler(Button_OnInterrupt);
                }
            }
        }

        void PhaseA_OnInterrupt(uint port, uint state, DateTime time)
        {
            if (state == 0)
            {
                if (PhaseB.Read() == true)
                    position++;
                else
                    position--;
            }
            else
            {
                if (PhaseB.Read() == true)
                    position--;
                else
                    position++;
            }
        }

        void Button_OnInterrupt(uint port, uint state, DateTime time)
        {
            buttonState = (state == 0);
        }
    }
}
