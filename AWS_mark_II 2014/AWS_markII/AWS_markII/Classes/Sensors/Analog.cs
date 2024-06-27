using Microsoft.SPOT.Hardware;
namespace Sensors
{
    abstract class Analog:Sensors
    {
        private SecretLabs.NETMF.Hardware.AnalogInput _analogInput;
        private static int MaxVal = 1024;
        private static float Analog_ref = 3.30f * 1000;
        private static float VoltsPerCount = Analog_ref / MaxVal;
        private float _millivolts;
        private static int maxIndex = 4;
        private int[] _adc_ValueRead = new int[maxIndex];
        
        private int[] ADC_ValueRead
        {
            get { return _adc_ValueRead; }
            set { _adc_ValueRead = value; }
        }

        protected Analog(Cpu.Pin _inputPin, int Index)
        {
            _analogInput = new SecretLabs.NETMF.Hardware.AnalogInput(_inputPin);
            maxIndex = Index;
            foreach (int i in ADC_ValueRead)
            {
                ADC_ValueRead[i] = _analogInput.Read();
            }
        }
  
        public void Read()
        {
            analogread(_analogInput.Read());
        }

        public override double value
        {
            get { return _millivolts; }
        }
         
        private void analogread(int _inputAnalog)
        {

            int ADC_ValueReadSum = 0;
            int ADC_ValueReadAverage = 0;

            for (int i = 0; i < maxIndex -1; ++i)
            {
                ADC_ValueRead[i] = ADC_ValueRead[i + 1];
            }
            ADC_ValueRead[maxIndex-1] = _inputAnalog;

            for (int x = 0; x < maxIndex; ++x)
            {
                ADC_ValueReadSum = ADC_ValueReadSum + ADC_ValueRead[x];
            }

            ADC_ValueReadAverage = ADC_ValueReadSum >> AkarKuadrat(maxIndex);

            _millivolts = (ADC_ValueReadAverage * VoltsPerCount); //- 39.74F

        }

        private int AkarKuadrat(int _Value)
        {
            int x = _Value;
            int result = 0;
            while (x != 1)
            {
                x = x / 2;
                result = result + 1;

            }
            return result;
        }
    }
}
