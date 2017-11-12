Imports System.ComponentModel
Imports System.Data.Common
Imports System.Data.OleDb

Public Class Statement
    Inherits Databasic.Statement






    ''' <summary>
    ''' Currently prepared and executed MySQL/MariaDB command.
    ''' </summary>
    Public Overrides Property Command As DbCommand
        Get
            Return Me._cmd
        End Get
        Set(value As DbCommand)
            Me._cmd = value
        End Set
    End Property
	Private _cmd As OleDbCommand
	''' <summary>
	''' Currently executed MySQL/MariaDB data reader from MySQL/MariaDB command.
	''' </summary>
	Public Overrides Property Reader As DbDataReader
        Get
            Return Me._reader
        End Get
        Set(value As DbDataReader)
            Me._reader = value
        End Set
    End Property
	Private _reader As OleDbDataReader






	''' <summary>
	''' Empty SQL statement constructor.
	''' </summary>
	''' <param name="sql">SQL statement code.</param>
	''' <param name="connection">Connection instance.</param>
	Public Sub New(sql As String, connection As OleDbConnection)
		MyBase.New(sql, connection)
		Me._cmd = New OleDbCommand(sql, connection)
		Me._cmd.Prepare()
	End Sub
	''' <summary>
	''' Empty SQL statement constructor.
	''' </summary>
	''' <param name="sql">SQL statement code.</param>
	''' <param name="transaction">SQL transaction instance with connection instance inside.</param>
	Public Sub New(sql As String, transaction As OleDbTransaction)
		MyBase.New(sql, transaction)
		Me._cmd = New OleDbCommand(sql, transaction.Connection, transaction)
		Me._cmd.Prepare()
	End Sub





	''' <summary>
	''' Set up all sql params into internal Command instance.
	''' </summary>
	''' <param name="sqlParams">Anonymous object with named keys as MySQL/MariaDB statement params without any '@' chars in object keys.</param>
	Protected Overrides Sub addParamsWithValue(sqlParams As Object)
        If (Not sqlParams Is Nothing) Then
            Dim sqlParamValue As Object
            For Each prop As PropertyDescriptor In TypeDescriptor.GetProperties(sqlParams)
                sqlParamValue = prop.GetValue(sqlParams)
                If (sqlParamValue Is Nothing) Then
                    sqlParamValue = DBNull.Value
                Else
                    sqlParamValue = Me.getPossibleUnderlyingEnumValue(sqlParamValue)
                End If
                Me._cmd.Parameters.AddWithValue(
                    prop.Name, sqlParamValue
                )
            Next
        End If
    End Sub
    ''' <summary>
    ''' Set up all sql params into internal Command instance.
    ''' </summary>
    ''' <param name="sqlParams">Dictionary with named keys as MySQL/MariaDB statement params without any '@' chars in dictionary keys.</param>
    Protected Overrides Sub addParamsWithValue(sqlParams As Dictionary(Of String, Object))
        Dim sqlParamValue As Object
        If (Not sqlParams Is Nothing) Then
            For Each pair As KeyValuePair(Of String, Object) In sqlParams
                If (pair.Value Is Nothing) Then
                    sqlParamValue = DBNull.Value
                Else
                    sqlParamValue = Me.getPossibleUnderlyingEnumValue(pair.Value)
                End If
                Me._cmd.Parameters.AddWithValue(
                    pair.Key, sqlParamValue
                )
            Next
        End If
    End Sub





End Class