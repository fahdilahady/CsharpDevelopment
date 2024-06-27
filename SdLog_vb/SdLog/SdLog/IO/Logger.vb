Imports System
Imports System.IO
Imports Microsoft.SPOT

Namespace SdLog.IO
    Public Class Logger
#Region "Public Static Methods"
        Public Shared Sub Log(ByVal ParamArray strings As Object())
            Dim message As String = String.Empty
            For i As Integer = 0 To strings.Length - 1
                message = message & strings(i).ToString() & " "
            Next
            WriteLog(message, StreamWriter, PrefixDateTime, LogToFile)
        End Sub
        Public Shared Sub Flush()
            If _streamWriter Is Nothing Then
                Return
            End If
            StreamWriter.Flush()
        End Sub
        Public Shared Sub Close()
            If _streamWriter Is Nothing Then
                Return
            End If
            StreamWriter.Flush()
            StreamWriter.Close()
            StreamWriter.Dispose()
        End Sub
#End Region

#Region "Private Static Methods"
        Private Shared Function GetDirectoryPath(ByVal trimmedDirectoryPath As String) As String
            If Not Directory.Exists(SDCardDirectory) Then
                Throw New Exception("SD card (directory) not found")
            End If

            Dim directoryPath As String = SDCardDirectory & Path.DirectorySeparatorChar & trimmedDirectoryPath

            If Not Directory.Exists(directoryPath) Then
                Directory.CreateDirectory(directoryPath)
            End If

            Return directoryPath
        End Function
        Private Shared Function GetFilePath(ByVal fullFileName As String, ByVal append As Boolean) As String
            If Not File.Exists(fullFileName) OrElse Not append Then
                File.Create(fullFileName)
            End If
            Return fullFileName
        End Function
        Private Shared Sub WriteLog(ByVal message As String, ByVal streamWriter As StreamWriter, ByVal addDateTime As Boolean, ByVal logToFile As Boolean)
            If addDateTime Then
                Dim current As DateTime = DateTime.Now
                message = "[" & current & ":" & current.Millisecond & "] " & message
            End If

            Debug.Print(message)
            If logToFile Then
                streamWriter.WriteLine(message)
            End If
        End Sub
#End Region

#Region "Public Static Properties"
        Public Shared Property PrefixDateTime() As Boolean
            Get
                Return m_PrefixDateTime
            End Get
            Set(ByVal value As Boolean)
                m_PrefixDateTime = value
            End Set
        End Property
        Private Shared m_PrefixDateTime As Boolean
        Public Shared Property LogToFile() As Boolean
            Get
                Return m_LogToFile
            End Get
            Set(ByVal value As Boolean)
                m_LogToFile = value
            End Set
        End Property
        Private Shared m_LogToFile As Boolean
        Public Shared Property Append() As Boolean
            Get
                Return m_Append
            End Get
            Set(ByVal value As Boolean)
                m_Append = value
            End Set
        End Property
        Private Shared m_Append As Boolean
        Public Shared ReadOnly Property LogFilePath() As String
            Get
                If _logFilePath Is Nothing Then
                    _logFilePath = GetFilePath(GetDirectoryPath("Report") & Path.DirectorySeparatorChar & "Log.txt", Append)
                End If
                Return _logFilePath
            End Get
        End Property
#End Region

#Region "Private Static Properties"
        Private Shared ReadOnly Property SDCardDirectory() As String
            Get
                Return "SD"
            End Get
        End Property
        Private Shared ReadOnly Property StreamWriter() As StreamWriter
            Get
                If _streamWriter Is Nothing Then
                    _streamWriter = New StreamWriter(LogFilePath, CBool(Append))
                End If
                Return _streamWriter
            End Get
        End Property
#End Region


#Region "Constructor"
        Public Sub New(ByVal directoryName As String, ByVal fileNameWithExtension As String, Optional ByVal append As Boolean = True)
            CustomDirectoryName = directoryName
            CustomFileNameWithExtension = fileNameWithExtension
            CustomAppend = append
        End Sub
#End Region

#Region "Public Methods"
        Public Sub LogCustom(ByVal ParamArray strings As Object())
            Dim message As String = String.Empty
            For i As Integer = 0 To strings.Length - 1
                message = message & strings(i).ToString() & " "
            Next
            WriteLog(message, CustormStreamWriter, CustomPrefixDateTime, CustomLogToFile)
        End Sub
        Public Sub FlushCustomLogger()
            If CustormStreamWriter Is Nothing Then
                Return
            End If
            CustormStreamWriter.Flush()
        End Sub
        Public Sub CloseCustomStreamWriter()
            If CustormStreamWriter Is Nothing Then
                Return
            End If
            CustormStreamWriter.Flush()
            CustormStreamWriter.Close()
            CustormStreamWriter.Dispose()
        End Sub
#End Region

#Region "Private Properties"
        Private Property CustomDirectoryName() As String
            Get
                Return m_CustomDirectoryName
            End Get
            Set(ByVal value As String)
                m_CustomDirectoryName = value
            End Set
        End Property
        Private m_CustomDirectoryName As String
        Private Property CustomFileNameWithExtension() As String
            Get
                Return m_CustomFileNameWithExtension
            End Get
            Set(ByVal value As String)
                m_CustomFileNameWithExtension = value
            End Set
        End Property
        Private m_CustomFileNameWithExtension As String
        Private Property CustomAppend() As Boolean
            Get
                Return m_CustomAppend
            End Get
            Set(ByVal value As Boolean)
                m_CustomAppend = value
            End Set
        End Property
        Private m_CustomAppend As Boolean
        Private ReadOnly Property CustormStreamWriter() As StreamWriter
            Get
                If _customStreamWriter Is Nothing Then
                    _customStreamWriter = New StreamWriter(CustomFilePath, CustomAppend)
                End If
                Return _customStreamWriter
            End Get
        End Property
#End Region

#Region "Public Properties"
        Public ReadOnly Property CustomFilePath() As String
            Get
                If CustomDirectoryName = String.Empty Then
                    Throw New Exception("Custom directory cannot be blank")
                End If
                If CustomFileNameWithExtension = String.Empty Then
                    Throw New Exception("File name cannot be blank")
                End If

                If _customLogFilePath Is Nothing Then
                    _customLogFilePath = GetFilePath(GetDirectoryPath(CustomDirectoryName) & Path.DirectorySeparatorChar & CustomFileNameWithExtension, CustomAppend)
                End If

                Return _customLogFilePath
            End Get
        End Property
        Public Property CustomPrefixDateTime() As Boolean
            Get
                Return m_CustomPrefixDateTime
            End Get
            Set(ByVal value As Boolean)
                m_CustomPrefixDateTime = value
            End Set
        End Property
        Private m_CustomPrefixDateTime As Boolean
        Public Property CustomLogToFile() As Boolean
            Get
                Return m_CustomLogToFile
            End Get
            Set(ByVal value As Boolean)
                m_CustomLogToFile = value
            End Set
        End Property
        Private m_CustomLogToFile As Boolean
#End Region

#Region "Fields"
        Private Shared _logFilePath As String
        Private Shared _streamWriter As StreamWriter

        Private _customStreamWriter As StreamWriter
        Private _customLogFilePath As String
#End Region

    End Class

End Namespace