Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports Microsoft.SPOT
Imports Microsoft.SPOT.Hardware
Imports SecretLabs.NETMF.Hardware
Imports SecretLabs.NETMF.Hardware.NetduinoPlus
Imports SdLog.SdLog.IO

Namespace SdLog
    Public Class Program

        Public Shared Sub main()

            Logger.LogToFile = True
            Logger.Append = True
            Logger.PrefixDateTime = True

            Logger.Log("All", "these", "will", "be", "combined", "in", "to", "one", "string")
            Logger.Log("This should go into the second line.")
            Debug.Print(Logger.LogFilePath)
        End Sub
    End Class
End Namespace

