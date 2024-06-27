﻿using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;
using System;
namespace Tools
{
    class IC_4017 : Tools
    {
        private static OutputPort _trigger;
        
        public int Channel = 0;
        private string _report;
        /// <summary>
        /// This is Digital Switching using 4017 and 4066 IC.
        /// every trigger event will switch 4066 to drive analog input channel.
        /// </summary>
        /// <param name="Trigger">Digital Output</param>
        /// <param name="numChannels">How many sensors used in a single Trigger? defalut:4</param>
        public IC_4017(Cpu.Pin Trigger= Pins.GPIO_PIN_D6)
        {
            try
            {
                _trigger = new OutputPort(Trigger, false);
            }
            catch (Exception ex)
            {
                _report = ex.Message;
            }
        }

        public void Switch()
        {
            _trigger.Write(true); Thread.Sleep(300); _trigger.Write(false);
            Channel++;
        }
        public override bool Ready
        { get; set; }

        public override string Report
        {
            get { return _report; }
            set { _report = value; }
           
        }
    }
}
