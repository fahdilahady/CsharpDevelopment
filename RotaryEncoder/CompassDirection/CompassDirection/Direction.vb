Option Explicit On
Option Strict On

Imports Microsoft.SPOT
Imports System


Public Class Direction
    Private _direction As String
    Public Function Degreeto16Direction(ByVal _degree As Double) As String
        Try
            Select Case _degree
                Case Is <= 11.25
                    _direction = "N"
                Case Is <= 33.75
                    _direction = "NNE"
                Case Is <= 56.25
                    _direction = "NE"
                Case Is <= 78.75
                    _direction = "ENE"
                Case Is <= 101.25
                    _direction = "E"
                Case Is <= 123.75
                    _direction = "ESE"
                Case Is <= 146.25
                    _direction = "SE"
                Case Is <= 168.75
                    _direction = "SSE"
                Case Is <= 191.25
                    _direction = "S"
                Case Is <= 213.75
                    _direction = "SSW"
                Case Is <= 236.25
                    _direction = "SW"
                Case Is <= 258.75
                    _direction = "WSW"
                Case Is <= 281.25
                    _direction = "W"
                Case Is <= 303.75
                    _direction = "WNW"
                Case Is <= 326.25
                    _direction = "NW"
                Case Is <= 348.75
                    _direction = "NNW"
                Case Is <= 360
                    _direction = "N"
            End Select
            Return _direction
        Catch ex As Exception
            NullReferenceException()
            Return "Degree Not Defined"
        End Try
    End Function

    Public Function Degreeto8Direction(ByVal _degree As Double) As String
        Try
            Select Case _degree
                Case Is <= 22.5
                    _direction = "N"
                Case Is <= 67.5
                    _direction = "NE"
                Case Is <= 112.5
                    _direction = "E"
                Case Is <= 157.5
                    _direction = "SE"
                Case Is <= 202.5
                    _direction = "S"
                Case Is <= 247.5
                    _direction = "SW"
                Case Is <= 292.5
                    _direction = "W"
                Case Is <= 337.5
                    _direction = "NW"
                Case Is <= 360
                    _direction = "N"
            End Select
            Return _direction
        Catch ex As Exception
            NullReferenceException()
            Return "Degree Not Defined"
        End Try
    End Function

    Private Sub NullReferenceException()
        Throw New NotImplementedException
    End Sub

End Class




