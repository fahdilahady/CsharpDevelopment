using System;
using Microsoft.SPOT;


namespace AWS_markII
{
    class Container
    {
        private double _valueLatest;
        private double Value;
        private double cumullate;
        public string valueToString
        {
            get 
            {
                cumullate = Value * Resolution;
                return cumullate.ToString("F1"); 
            }
        }

        /// <summary>
        /// To Collecting Value From sensors
        /// </summary>
        /// <param name="input">Input to Collect and Average</param>
        /// <param name="_resolution">Give Resollution of The Sensor "Unit/mV"</param>
        public Container(double input, double _resolution)
        {
            Value  = _valueLatest = input;
            Resolution  = _resolution;
        }

        public void Collect(double input)
        {
            Value = (Value + _valueLatest)/2;
            _valueLatest = input;            
        }

        public void Reset()
        {
            Value = 0;
        }
        public double Resolution { get; set; }
    }
}
