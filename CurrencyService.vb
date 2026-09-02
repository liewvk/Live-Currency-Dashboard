Imports System.Net.Http
Imports System.Text.Json

Public Class CurrencyService

    Private Shared ReadOnly _http As New HttpClient()

    Private Const API As String =
        "https://open.er-api.com/v6/latest/"


    Public Async Function GetRatesAsync(
        baseCurrency As String) _
        As Task(Of Dictionary(Of String, Double))

        Dim currency As String =
            baseCurrency.Trim().ToUpperInvariant()

        Dim url As String =
            API & Uri.EscapeDataString(currency)

        Using response As HttpResponseMessage =
            Await _http.GetAsync(url).ConfigureAwait(False)

            response.EnsureSuccessStatusCode()

            Dim json As String =
                Await response.Content _
                    .ReadAsStringAsync() _
                    .ConfigureAwait(False)

            Using doc As JsonDocument =
                JsonDocument.Parse(json)

                Dim root As JsonElement =
                    doc.RootElement

                ' Check whether API returned success
                If root.TryGetProperty(
                    "result",
                    Nothing) Then

                    Dim result As String =
                        root.GetProperty(
                            "result").GetString()

                    If Not String.Equals(
                        result,
                        "success",
                        StringComparison.OrdinalIgnoreCase) Then

                        Throw New Exception(
                            "The exchange-rate service returned an error.")

                    End If

                End If


                Dim ratesElement As JsonElement =
                    root.GetProperty("rates")


                Dim rates As New Dictionary(
                    Of String, Double)(
                    StringComparer.OrdinalIgnoreCase)


                For Each item As JsonProperty In
                    ratesElement.EnumerateObject()

                    rates(item.Name) =
                        item.Value.GetDouble()

                Next

                Return rates

            End Using

        End Using

    End Function

End Class
