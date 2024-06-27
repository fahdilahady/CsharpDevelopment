using System;
using Microsoft.SPOT;

namespace AnalogReadClass
{
    public class ADC
    {
        private static int MaxVal = 1023;
        private static float Analog_ref = 3.3F*1000;
        private static float VoltsPerCount = Analog_ref / MaxVal;
        private float _valueConverted;
        private bool _getValue;
 
        public ADC()
        {
        }

        public bool GetValue
        {
            get { return _getValue; }
            set { _getValue = value; }
        }
        public float ValueConverted
        {
            get{return _valueConverted;}
            set { _valueConverted = value; }
        }

        public void analogread(int _inputAnalog)
        {
            int[] ADC_ValueRead = new int[4];
            int ADC_Read = 0;

            int ADC_ValueReadSum = 0;
            int ADC_ValueReadAverage = 0;
            //float ADC_ValueConverted = 0;

            //init the values
            ADC_Read = _inputAnalog;
            for (int i = 0; i < 4; ++i)
            {
                ADC_ValueRead[i] = ADC_Read;
            }
                              

                for (int i = 0; i < 3; ++i)
                {
                    ADC_ValueRead[i] = ADC_ValueRead[i+1];
                }
                ADC_ValueRead[3] = ADC_Read;

                for (int x = 0; x < 4; ++x)
                {
                    ADC_ValueReadSum = ADC_ValueReadSum + ADC_ValueRead[x];
                }
                
                ADC_ValueReadAverage = ADC_ValueReadSum >> 2;

                ValueConverted = (int) ADC_ValueReadAverage * VoltsPerCount;

                // Print the values in the debug window
                //Debug.Print("Averaged Value: " + ADC_ValueReadAverage.ToString() + "    ; Averaged Volts: " + ValueConverted.ToString());
                //Debug.Print(" ");
                        
        }
    }
}
