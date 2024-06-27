Public Shared Function NTPTime(TimeServer As [String], UTC_offset As Integer) As DateTime
	' Find endpoint for timeserver
	Dim ep As New IPEndPoint(Dns.GetHostEntry(TimeServer).AddressList(0), 123)

	' Connect to timeserver
	Dim s As New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
	s.Connect(ep)

	' Make send/receive buffer
	Dim ntpData As Byte() = New Byte(47) {}
	Array.Clear(ntpData, 0, 48)

	' Set protocol version
	ntpData(0) = &H1b

	' Send Request
	s.Send(ntpData)

	' Receive Time
	s.Receive(ntpData)

	Dim offsetTransmitTime As Byte = 40

	Dim intpart As ULong = 0
	Dim fractpart As ULong = 0

	For i As Integer = 0 To 3
		intpart = (intpart << 8) Or ntpData(offsetTransmitTime + i)
	Next

	For i As Integer = 4 To 7
		fractpart = (fractpart << 8) Or ntpData(offsetTransmitTime + i)
	Next

	Dim milliseconds As ULong = (intpart * 1000 + (fractpart * 1000) / &H100000000L)

	s.Close()

	Dim timeSpan__1 As TimeSpan = TimeSpan.FromTicks(CLng(milliseconds) * TimeSpan.TicksPerMillisecond)
	Dim dateTime As New DateTime(1900, 1, 1)
	dateTime += timeSpan__1

	Dim offsetAmount As New TimeSpan(0, UTC_offset, 0, 0, 0)
	Dim networkDateTime As DateTime = (dateTime + offsetAmount)

	Return networkDateTime
End Function
'Utility.SetLocalTime(NTPTime("time-a.nist.gov", -5))