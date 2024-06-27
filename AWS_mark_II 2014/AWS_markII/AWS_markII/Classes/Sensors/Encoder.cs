using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;


namespace Sensors
{
    class Encoder : Digital
    {
        /// <summary>
        /// pointTo for Encoder Class
        /// </summary>
        public override double value
        {
            get { return position; }
        }
        private InputPort PhaseB;
        private InterruptPort PhaseA;
        private InterruptPort Button;
        private int _maxVal;
        private bool _limitedRotation;
        protected bool buttonState = false;
        protected int position = 0;

        public int windDegree
        {
            get
            {
                if (degree <= 180) { return degree + 180; }
                else { return degree - 180; }
            }
        }
        public int degree
        {
            get { return position * (360/_maxVal); }
        }
        public int counter = 0;

        /// <summary>
        /// Constructor for encoder class
        /// </summary>
        public Encoder() { }
        /// <param name="pinA">The pin used for output A</param>
        /// <param name="pinB">The pin used for output B</param>
        /// <param name="LimitedRotation">Is Limited Value per Rotation? defaultMaxvalue = 40</param
        public Encoder(Cpu.Pin pinA, Cpu.Pin pinB, bool LimitedRotation = true)
            : this(pinA, pinB, LimitedRotation, 40)
        { }

        /// <param name="MaxVal">Maximum Value per Rotation</param>
        public Encoder(Cpu.Pin pinA, Cpu.Pin pinB, bool LimitedRotation = true, int MaxVal = 40)
            : this(pinA, pinB, LimitedRotation, MaxVal, Pins.GPIO_NONE)
        { }
        /// <param name="pinButton">The pin used for the push contact (Optional:GPIO_NONE if not used)</param>
        public Encoder(Cpu.Pin pinA, Cpu.Pin pinB, bool LimitedRotation = true, int MaxVal = 40, Cpu.Pin pinButton = Pins.GPIO_NONE)
            : base()
        {

            _limitedRotation = LimitedRotation;
            _maxVal = MaxVal;
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
            if (_limitedRotation)
            {
                if (position == _maxVal || position == 0)
                {
                    position = 0;
                }
                if (position == -1) { position = _maxVal - 1; }
            }
        }

        void Button_OnInterrupt(uint port, uint state, DateTime time)
        {
            buttonState = (state == 0);
        }

        public override void Count()
        {
            if (buttonState)
            {
                counter++;
            }
        }

        public override void Reset()
        {
            position = 0;
        }

        protected override double Resolution
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
    
}
