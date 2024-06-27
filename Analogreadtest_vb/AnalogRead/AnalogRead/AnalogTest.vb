Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports Microsoft.SPOT
Imports Microsoft.SPOT.Hardware
Imports SecretLabs.NETMF.Hardware
Imports SecretLabs.NETMF.Hardware.NetduinoPlus


Namespace AnalogTest
    Public Class Program

        Private Const MaxVal_int As Integer = 1024
        Private Const Analog_ref As Single = 3.3F
        Private Const VoltsPerCount As Single = Analog_ref / MaxVal_int


        Public Shared Sub Main()
            ' write your code here
            Dim adcPort_A0 As New SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A0)
            Dim ADC_digitalValueRead_int As Integer = 0
            Dim ADC_analogValueConverted_float As Single = 0

            Dim ADC_digitalValueRead0_int As Integer = 0
            Dim ADC_digitalValueRead1_int As Integer = 0
            Dim ADC_digitalValueRead2_int As Integer = 0
            Dim ADC_digitalValueRead3_int As Integer = 0
            Dim ADC_digitalValueReadSum_int As Integer = 0
            Dim ADC_digitalValueReadAverage_int As Integer = 0
            Dim ADC_analogValueAverageConverted_float As Single = 0


            'init the values
            ADC_digitalValueRead_int = adcPort_A0.Read()
            ADC_digitalValueRead0_int = ADC_digitalValueRead_int
            ADC_digitalValueRead1_int = ADC_digitalValueRead_int
            ADC_digitalValueRead2_int = ADC_digitalValueRead_int
            ADC_digitalValueRead3_int = ADC_digitalValueRead_int
            ADC_digitalValueReadAverage_int = 0


            ' do forever...
            While True
                ' read from ADC Port_A0
                ADC_digitalValueRead_int = adcPort_A0.Read()
                ' convert digital value to analog voltage
                ADC_analogValueConverted_float = CSng(ADC_digitalValueRead_int) * VoltsPerCount
                ' Print the values in the debug window
                Debug.Print("Raw Value: " & ADC_digitalValueRead_int.ToString() & "  ; Volts: " & ADC_analogValueConverted_float.ToString())


                ' Slightly more advanced, simple average of the last four ADC values.
                ' Shift values, sum, and average
                ADC_digitalValueRead0_int = ADC_digitalValueRead1_int
                ADC_digitalValueRead1_int = ADC_digitalValueRead2_int
                ADC_digitalValueRead2_int = ADC_digitalValueRead3_int
                ADC_digitalValueRead3_int = ADC_digitalValueRead_int
                'this methode called moving average we take 4 point on it
                ADC_digitalValueReadSum_int = ADC_digitalValueRead0_int + ADC_digitalValueRead1_int + ADC_digitalValueRead2_int + ADC_digitalValueRead3_int
                ADC_digitalValueReadAverage_int = ADC_digitalValueReadSum_int >> 2 ' ADC_digitalValueReadAverage_int = (ADC_digitalValueReadSum_int/4);
                ' convert digital value to analog voltage
                ADC_analogValueAverageConverted_float = CSng(ADC_digitalValueReadAverage_int) * VoltsPerCount
                ' Print the values in the debug window
                Debug.Print("Averaged Value: " & ADC_digitalValueReadAverage_int.ToString() & "    ; Averaged Volts: " & ADC_analogValueAverageConverted_float.ToString())
                Debug.Print(" ")
            End While

        End Sub
    End Class
End Namespace
