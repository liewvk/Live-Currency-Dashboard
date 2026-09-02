Imports System.Threading
Imports System.Threading.Tasks

Public Class CurrencyDashboardService
    Implements IDisposable

    Private ReadOnly _currencyService _
        As New CurrencyService()

    Private ReadOnly _viewModel _
        As CurrencyDashboardViewModel

    Private ReadOnly _refreshLock _
        As New SemaphoreSlim(1, 1)

    Private ReadOnly _stateLock _
        As New Object()

    Private _timer As System.Threading.Timer

    Private _baseCurrency As String =
        "USD"

    Private _disposed As Boolean = False


    Public Event DataUpdated As EventHandler

    Public Event DataError _
        As EventHandler(Of CurrencyErrorEventArgs)


    Public Sub New(
        viewModel As CurrencyDashboardViewModel)

        _viewModel = viewModel

    End Sub


    ' ============================================================
    ' Base Currency
    ' ============================================================

    Public Property BaseCurrency As String

        Get

            SyncLock _stateLock
                Return _baseCurrency
            End SyncLock

        End Get

        Set(value As String)

            SyncLock _stateLock

                _baseCurrency =
                    value.Trim().ToUpperInvariant()

            End SyncLock

        End Set

    End Property


    ' ============================================================
    ' Start background polling
    ' ============================================================

    Public Sub Start()

        If _timer IsNot Nothing Then
            Return
        End If

        ' First refresh immediately,
        ' then every 60 seconds.
        _timer =
            New System.Threading.Timer(
                AddressOf TimerTick,
                Nothing,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(60))

    End Sub


    ' ============================================================
    ' ThreadPool timer callback
    ' ============================================================

    Private Async Sub TimerTick(
        state As Object)

        Await RefreshAsync()

    End Sub


    ' ============================================================
    ' Manual or automatic refresh
    ' ============================================================

    Public Async Function RefreshAsync() As Task

        ' Prevent overlapping downloads
        Dim lockObtained As Boolean =
            Await _refreshLock.WaitAsync(0)

        If Not lockObtained Then
            Return
        End If

        Try

            Dim currentBase As String =
                BaseCurrency

            Dim rates As Dictionary(
                Of String, Double) =
                Await _currencyService _
                    .GetRatesAsync(currentBase) _
                    .ConfigureAwait(False)

            _viewModel.UpdateRates(
                currentBase,
                rates)

            ' This event is normally raised
            ' from a ThreadPool thread.
            RaiseEvent DataUpdated(
                Me,
                EventArgs.Empty)

        Catch ex As Exception

            RaiseEvent DataError(
                Me,
                New CurrencyErrorEventArgs(
                    ex.Message))

        Finally

            _refreshLock.Release()

        End Try

    End Function


    ' ============================================================
    ' Dispose
    ' ============================================================

    Public Sub Dispose() _
        Implements IDisposable.Dispose

        If _disposed Then
            Return
        End If

        _disposed = True

        If _timer IsNot Nothing Then

            _timer.Dispose()
            _timer = Nothing

        End If

        _refreshLock.Dispose()

    End Sub

End Class


' ==============================================================
' Error Event Arguments
' ==============================================================

Public Class CurrencyErrorEventArgs
    Inherits EventArgs

    Public ReadOnly Property Message As String

    Public Sub New(
        message As String)

        Me.Message = message

    End Sub

End Class
