Imports System
Imports System.Text
Imports System.Threading
Imports System.IO.Ports
'Imports Microsoft.VisualBasic.Constants
Imports Microsoft.SPOT
Imports Microsoft.SPOT.Hardware
Imports SecretLabs.NETMF.Hardware
Imports SecretLabs.NETMF.Hardware.NetduinoPlus

Namespace Uart
    Public Class Com_UART
        Private Shared GSM_UART As SerialPort
        Private Shared OnboardLed As OutputPort
        Private Shared D4, D5, D6, D7, D8, D9, D10, D11, D12, D13 As OutputPort
        Private Shared Respons As String
        Private Shared bytesTosend As Byte()
        Private Shared SendingSMS As Boolean

        Public Shared Sub Main()
            ' write your code here
            OnboardLed = New OutputPort(Pins.ONBOARD_LED, False)
            D4 = New OutputPort(Pins.GPIO_PIN_D4, False)
            D5 = New OutputPort(Pins.GPIO_PIN_D5, False)
            D6 = New OutputPort(Pins.GPIO_PIN_D6, False)
            D7 = New OutputPort(Pins.GPIO_PIN_D7, False)
            D8 = New OutputPort(Pins.GPIO_PIN_D8, False)
            D9 = New OutputPort(Pins.GPIO_PIN_D9, False)
            D10 = New OutputPort(Pins.GPIO_PIN_D10, False)
            D11 = New OutputPort(Pins.GPIO_PIN_D11, False)
            D12 = New OutputPort(Pins.GPIO_PIN_D12, False)
            D13 = New OutputPort(Pins.GPIO_PIN_D13, False)
            SendingSMS = False

            D9.Write(True)
            Thread.Sleep(500)
            D9.Write(False)
            GSM_UART = New SerialPort("COM1", 9600, Parity.None, 8, StopBits.One)
            AddHandler GSM_UART.DataReceived, New SerialDataReceivedEventHandler(AddressOf RecieveData)


            If GSM_UART.IsOpen = False Then

                GSM_UART.Open()
                Debug.Print("Port Opening" & " : at baudRate= " & GSM_UART.BaudRate)
                Thread.Sleep(250)
                GSM_UART_dataWrite("AT+CMGF=1" & vbCr)
                Thread.Sleep(250)
                GSM_UART_dataWrite("AT+CMEE=1" & vbCr)
                Thread.Sleep(250)
                GSM_UART_dataWrite("AT&W" & vbCr)
            Else
                Debug.Print("Port is ready...")
            End If
            SendingSMS = True

            While SendingSMS = True

                Thread.Sleep(250)
                GSM_UART_dataWrite("AT+CMGS=")
                Thread.Sleep(250)
                GSM_UART.WriteByte(34)
                GSM_UART_dataWrite("+6285218877358")
                GSM_UART.WriteByte(34)
                GSM_UART_dataWrite("" & vbLf)
                Thread.Sleep(500)
                GSM_UART_dataWrite("This is the messege body!!" + "\r")
                Thread.Sleep(250)
                PrintEnd()
                GSM_UART_dataWrite("" & vbLf)
                Thread.Sleep(1000)


                SendingSMS = False

            End While
            Debug.Print(Respons & "?")
            Thread.Sleep(Timeout.Infinite)



        End Sub
        Private Shared Sub PrintEnd()
            Dim EndToSend As Byte() = New Byte(0) {}
            EndToSend(0) = 26
            GSM_UART.Write(EndToSend, 0, 1)

            Thread.Sleep(200)
        End Sub
        Private Shared Sub GSM_UART_dataWrite(ByVal Command As String)
            bytesTosend = Encoding.UTF8.GetBytes(Command)
            Debug.Print("Send port : " & Command)
            GSM_UART.Write(bytesTosend, 0, bytesTosend.Length)
            Debug.Print("Port is writing...")
        End Sub
        Private Shared Sub RecieveData(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
            Dim Length As Integer = GSM_UART.BytesToRead
            Dim bufferData As Byte() = New Byte(Length - 1) {}
            GSM_UART.Read(bufferData, 0, Length)
            Respons += New String(Encoding.UTF8.GetChars(bufferData))

            If Respons.IndexOf(vbLf) >= 0 Then
                Respons = Respons.Trim()          ' Get rid of the line feed
                If Respons <> "" Then
                    OnboardLed.Write(state:=True)
                    Debug.Print(Respons)
                End If
                Respons = ""
                If Respons = "" Then
                    OnboardLed.Write(state:=False)
                    Debug.Print(Respons)
                End If
            End If
        End Sub
    End Class
End Namespace