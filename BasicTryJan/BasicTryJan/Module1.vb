Imports Microsoft.SPOT
Imports Microsoft.SPOT.Hardware
Imports SecretLabs.NETMF.Hardware
Imports SecretLabs.NETMF.Hardware.NetduinoPlus
'additional Reference


Module Module1
    Dim LED As New OutputPort(Pins.ONBOARD_LED, False)
    Dim D0 As New OutputPort(Pins.GPIO_PIN_D0, False)
    Dim D1 As New OutputPort(Pins.GPIO_PIN_D1, False)
    Dim D2 As New OutputPort(Pins.GPIO_PIN_D2, False)
    Dim D3 As New OutputPort(Pins.GPIO_PIN_D3, False)
    Dim D4 As New OutputPort(Pins.GPIO_PIN_D4, False)
    Dim D5 As New OutputPort(Pins.GPIO_PIN_D5, False)
    Dim D6 As New OutputPort(Pins.GPIO_PIN_D6, False)
    Dim D7 As New OutputPort(Pins.GPIO_PIN_D7, False)
    Dim D8 As New OutputPort(Pins.GPIO_PIN_D8, False)
    Dim D9 As New OutputPort(Pins.GPIO_PIN_D9, False)
    Dim D10 As New OutputPort(Pins.GPIO_PIN_D10, False)
    Dim D11 As New OutputPort(Pins.GPIO_PIN_D11, False)
    Dim D12 As New OutputPort(Pins.GPIO_PIN_D12, False)
    Dim D13 As New OutputPort(Pins.GPIO_PIN_D13, False)


    Sub Main()
        Dim i, u As Integer
        ' write your code here

        Dim A0 As New AnalogInput(AnalogChannels.ANALOG_PIN_A0)
        'Dim mV As Double
        Dim maxV As Double = 3300
        Dim ADC As Integer = 4096
        D9.Write(True)
        Debug.Print("true")
        Thread.Sleep(400)
        D9.Write(False)
        Debug.Print("False")
        Do
            'mV = A0.Read * maxV

            'LED.Write(True)
            'D13.Write(False)
            'Thread.Sleep(1000)
            'Debug.Print(mV.ToString)
            'LED.Write(False)
            'D13.Write(True)
            'Thread.Sleep(1000)
            i = i + 1
            Select Case i
                Case 1 To 50
                    XLED()
                    Thread.Sleep(1000 \ i)
                    D13on()
                    Thread.Sleep(1000 \ i)
                    D12on()
                    Thread.Sleep(1000 \ i)
                    D11on()
                    Thread.Sleep(1000 \ i)
                    'D10on()
                    'Thread.Sleep(1000 \ i)
                Case 51 To 100
                    u = 101 - i
                    'D10on()
                    'Thread.Sleep(1000 \ u)
                    D11on()
                    Thread.Sleep(1000 \ u)
                    D12on()
                    Thread.Sleep(1000 \ u)
                    D13on()
                    Thread.Sleep(1000 \ u)
                    XLED()
                    Thread.Sleep(1000 \ u)
                Case Else
                    i = 10
            End Select

        Loop

    End Sub


    Sub XLED()
        LED.Write(True)
        D13.Write(False)
        D12.Write(False)
        D11.Write(False)
        D10.Write(False)
    End Sub
    Sub D13on()
        LED.Write(False)
        D13.Write(True)
        D12.Write(False)
        D11.Write(False)
        D10.Write(False)
    End Sub
    Sub D12on()
        LED.Write(False)
        D13.Write(False)
        D12.Write(True)
        D11.Write(False)
        D10.Write(False)
    End Sub
    Sub D11on()
        LED.Write(False)
        D13.Write(False)
        D12.Write(False)
        D11.Write(True)
        D10.Write(False)
    End Sub
    Sub D10on()
        LED.Write(False)
        D13.Write(False)
        D12.Write(False)
        D11.Write(False)
        D10.Write(True)
    End Sub

End Module
