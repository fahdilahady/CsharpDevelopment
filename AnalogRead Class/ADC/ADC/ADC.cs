using System;
using Microsoft.SPOT;

namespace ADC
{
     public class Conversion
    {
        private static int MaxVal = 1023;
        private static float Analog_ref = 3.30f*1000;
        private static float VoltsPerCount = Analog_ref / MaxVal;
        private float _valueConverted;
        private int[] _adc_ValueRead = new int[4];


        public Conversion(int _inputAnalog)
        {
            //Constructor

            for (int i = 0; i < 4; ++i)
            {
                ADC_ValueRead[i] = _inputAnalog;
            }

        }

        private int[] ADC_ValueRead
        {
            get { return _adc_ValueRead; }
            set { _adc_ValueRead = value; }
        }


        public float miliVolts
        {
            get{return _valueConverted;}
            set { _valueConverted = value; }
        }

        public void analogread(int _inputAnalog)
        {

            int ADC_ValueReadSum = 0;
            int ADC_ValueReadAverage = 0;

            for (int i = 0; i < 3; ++i)
            {
                ADC_ValueRead[i] = ADC_ValueRead[i + 1];
            }
            ADC_ValueRead[3] = _inputAnalog;
            
                for (int x = 0; x < 4; ++x)
                {
                    ADC_ValueReadSum = ADC_ValueReadSum + ADC_ValueRead[x];
                }
                
            ADC_ValueReadAverage = ADC_ValueReadSum >> 2;

            _valueConverted = (ADC_ValueReadAverage * VoltsPerCount); //- 39.74F
                       
        }
    }
}


