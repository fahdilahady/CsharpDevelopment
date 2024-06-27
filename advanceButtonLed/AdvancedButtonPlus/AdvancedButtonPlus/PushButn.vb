Imports System
Imports System.Threading
Imports Microsoft.SPOT
Imports Microsoft.SPOT.Hardware
Imports SecretLabs.NETMF.Hardware
Imports SecretLabs.NETMF.Hardware.NetduinoPlus
Namespace ButtonPush
    Module PushButn
        Dim LED As New OutputPort(Pins.ONBOARD_LED, False)
        Sub Main()
            ' write your code here
            Dim Tombol As New InterruptPort(Pins.ONBOARD_SW1, False, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth)
            NativeEventHandler(button_OnInterrupt)
            Thread.Sleep(Timeout.Infinite)

        End Sub

    End Module
End Namespace






