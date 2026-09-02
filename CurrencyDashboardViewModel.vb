Imports System.Collections.Concurrent
Imports System.Threading

Public Class CurrencyDashboardViewModel

    Private _rates As New ConcurrentDictionary(
        Of String, Double)(
        StringComparer.OrdinalIgnoreCase)

    Private ReadOnly _syncRoot As New Object()

    Private _baseCurrency As String = "USD"

    Private _lastUpdated As DateTime =
        DateTime.MinValue


    ' ============================================================
    ' Current rates
    ' ============================================================

    Public ReadOnly Property Rates _
        As ConcurrentDictionary(Of String, Double)

        Get
            Return _rates
        End Get

    End Property


    ' ============================================================
    ' Base currency
    ' ============================================================

    Public ReadOnly Property BaseCurrency As String

        Get

            SyncLock _syncRoot
                Return _baseCurrency
            End SyncLock

        End Get

    End Property


    ' ============================================================
    ' Last update
    ' ============================================================

    Public ReadOnly Property LastUpdated As DateTime

        Get

            SyncLock _syncRoot
                Return _lastUpdated
            End SyncLock

        End Get

    End Property


    ' ============================================================
    ' Replace rates with a fresh snapshot
    ' ============================================================

    Public Sub UpdateRates(
        baseCurrency As String,
        newRates As Dictionary(Of String, Double))

        Dim freshRates As New ConcurrentDictionary(
            Of String, Double)(
            newRates,
            StringComparer.OrdinalIgnoreCase)

        ' Atomically replace the entire dictionary
        Interlocked.Exchange(
            _rates,
            freshRates)

        SyncLock _syncRoot

            _baseCurrency =
                baseCurrency.ToUpperInvariant()

            _lastUpdated =
                DateTime.Now

        End SyncLock

    End Sub

End Class
