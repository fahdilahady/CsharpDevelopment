Private Sub InitNetwork()
	' write your code here 
	Dim networkInterfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
	For Each networkInterface__1 As NetworkInterface In networkInterfaces
		If networkInterface__1.NetworkInterfaceType = NetworkInterfaceType.Ethernet Then
			If Not networkInterface__1.IsDhcpEnabled Then
				' Switch to DHCP ... 
				networkInterface__1.EnableDhcp()
				networkInterface__1.RenewDhcpLease()
				Thread.Sleep(10000)
			End If
			Debug.Print("IP Address: " + networkInterface__1.IPAddress)
			Debug.Print("Subnet mask " + networkInterface__1.SubnetMask)
		End If
	Next
End Sub