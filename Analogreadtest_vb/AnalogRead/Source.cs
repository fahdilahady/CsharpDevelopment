using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace AnalogTest {
    public class Program {
        private const int MaximumValue_int = 1024;
        private const float AnalogReference_float = 3.30f;
        private const float VoltsPerCount_float = AnalogReference_float / MaximumValue_int;

        public static void Main() {

            // Define and initialize variables
            int ADC_digitalValueRead_int = 0;
            float ADC_analogValueConverted_float = 0;

            int ADC_digitalValueRead0_int = 0;
            int ADC_digitalValueRead1_int = 0;
            int ADC_digitalValueRead2_int = 0;
            int ADC_digitalValueRead3_int = 0;
            int ADC_digitalValueReadSum_int = 0;
            int ADC_digitalValueReadAverage_int = 0;
            float ADC_analogValueAverageConverted_float = 0;

            SecretLabs.NETMF.Hardware.AnalogInput adcPort_A0 = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A0);

            //init the values
            ADC_digitalValueRead_int = adcPort_A0.Read();
            ADC_digitalValueRead0_int = ADC_digitalValueRead_int;
            ADC_digitalValueRead1_int = ADC_digitalValueRead_int;
            ADC_digitalValueRead2_int = ADC_digitalValueRead_int;
            ADC_digitalValueRead3_int = ADC_digitalValueRead_int;
            ADC_digitalValueReadAverage_int = 0;


            // do forever...
            while (true) {
                // read from ADC Port_A0
                ADC_digitalValueRead_int = adcPort_A0.Read();
                // convert digital value to analog voltage
                ADC_analogValueConverted_float = ((float)ADC_digitalValueRead_int) * VoltsPerCount_float;
                // Print the values in the debug window
                Debug.Print("Raw Value: " + ADC_digitalValueRead_int.ToString() +
                            "  ; Volts: " + ADC_analogValueConverted_float.ToString());


                // Slightly more advanced, simple average of the last four ADC values.
                // Shift values, sum, and average
                ADC_digitalValueRead0_int = ADC_digitalValueRead1_int;
                ADC_digitalValueRead1_int = ADC_digitalValueRead2_int;
                ADC_digitalValueRead2_int = ADC_digitalValueRead3_int;
                ADC_digitalValueRead3_int = ADC_digitalValueRead_int;
                ADC_digitalValueReadSum_int = ADC_digitalValueRead0_int + ADC_digitalValueRead1_int + ADC_digitalValueRead2_int + ADC_digitalValueRead3_int;
                ADC_digitalValueReadAverage_int = ADC_digitalValueReadSum_int >> 2;  // ADC_digitalValueReadSum_int = (ADC_digitalValueReadSum_int/4);
                // convert digital value to analog voltage
                ADC_analogValueAverageConverted_float = ((float)ADC_digitalValueReadAverage_int) * VoltsPerCount_float;
                // Print the values in the debug window
                Debug.Print("Averaged Value: " + ADC_digitalValueReadAverage_int.ToString() +
                            "    ; Averaged Volts: " + ADC_analogValueAverageConverted_float.ToString());
                Debug.Print(" ");
            }
        }
    }
}