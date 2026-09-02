Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class Form1

    ' ============================================================
    ' Architecture objects
    ' ============================================================

    Private ReadOnly _viewModel _
        As New CurrencyDashboardViewModel()

    Private ReadOnly _service _
        As CurrencyDashboardService


    ' ============================================================
    ' Rate history for chart
    ' ============================================================

    Private ReadOnly _history _
        As New List(Of (Time As DateTime, Rate As Double))


    ' ============================================================
    ' Controls
    ' ============================================================

    Private ReadOnly cboBase As New ComboBox()

    Private ReadOnly cboTarget As New ComboBox()

    Private ReadOnly lblBase As New Label()

    Private ReadOnly lblTarget As New Label()

    Private ReadOnly lblRate As New Label()

    Private ReadOnly lblStatus As New Label()

    Private ReadOnly lblAlert As New Label()

    Private ReadOnly lblLow As New Label()

    Private ReadOnly lblHigh As New Label()

    Private ReadOnly nudAlertLow As New NumericUpDown()

    Private ReadOnly nudAlertHigh As New NumericUpDown()

    Private ReadOnly btnRefresh As New Button()

    Private ReadOnly picChart As New PictureBox()


    ' ============================================================
    ' Constructor
    ' ============================================================

    Public Sub New()

        InitializeComponent()

        ConfigureForm()

        ConfigureControls()

        _service =
            New CurrencyDashboardService(
                _viewModel)

        AddHandler _service.DataUpdated,
            AddressOf Service_DataUpdated

        AddHandler _service.DataError,
            AddressOf Service_DataError

    End Sub


    ' ============================================================
    ' Form Setup
    ' ============================================================

    Private Sub ConfigureForm()

        Text =
            "Live Currency Dashboard"

        ClientSize =
            New Size(800, 560)

        StartPosition =
            FormStartPosition.CenterScreen

        BackColor =
            Color.White

    End Sub


    ' ============================================================
    ' Create User Interface
    ' ============================================================

    Private Sub ConfigureControls()

        ' --------------------------------------------------------
        ' Base Currency
        ' --------------------------------------------------------

        lblBase.Text =
            "Base currency:"

        lblBase.Location =
            New Point(30, 25)

        lblBase.AutoSize =
            True


        cboBase.Location =
            New Point(130, 20)

        cboBase.Width =
            100

        cboBase.DropDownStyle =
            ComboBoxStyle.DropDownList

        cboBase.Items.AddRange(
            New Object() {
                "USD",
                "EUR",
                "GBP",
                "CHF"
            })

        cboBase.SelectedItem =
            "USD"


        ' --------------------------------------------------------
        ' Target Currency
        ' --------------------------------------------------------

        lblTarget.Text =
            "Target currency:"

        lblTarget.Location =
            New Point(260, 25)

        lblTarget.AutoSize =
            True


        cboTarget.Location =
            New Point(370, 20)

        cboTarget.Width =
            100

        cboTarget.DropDownStyle =
            ComboBoxStyle.DropDownList

        cboTarget.Items.AddRange(
            New Object() {
                "EUR",
                "GBP",
                "CHF",
                "JPY",
                "USD"
            })

        cboTarget.SelectedItem =
            "EUR"


        ' --------------------------------------------------------
        ' Refresh button
        ' --------------------------------------------------------

        btnRefresh.Text =
            "Refresh Now"

        btnRefresh.Location =
            New Point(500, 19)

        btnRefresh.Size =
            New Size(120, 30)


        ' --------------------------------------------------------
        ' Current Rate
        ' --------------------------------------------------------

        lblRate.Text =
            "Waiting for exchange rates..."

        lblRate.Location =
            New Point(30, 75)

        lblRate.AutoSize =
            True

        lblRate.Font =
            New Font(
                "Segoe UI",
                18,
                FontStyle.Bold)


        ' --------------------------------------------------------
        ' Status
        ' --------------------------------------------------------

        lblStatus.Text =
            "Starting..."

        lblStatus.Location =
            New Point(32, 120)

        lblStatus.AutoSize =
            True

        lblStatus.ForeColor =
            Color.DimGray


        ' --------------------------------------------------------
        ' Alerts
        ' --------------------------------------------------------

        lblLow.Text =
            "Low alert:"

        lblLow.Location =
            New Point(32, 160)

        lblLow.AutoSize =
            True


        nudAlertLow.Location =
            New Point(105, 155)

        nudAlertLow.Width =
            100

        nudAlertLow.DecimalPlaces =
            4

        nudAlertLow.Minimum =
            0

        nudAlertLow.Maximum =
            1000

        nudAlertLow.Value =
            0.8D


        lblHigh.Text =
            "High alert:"

        lblHigh.Location =
            New Point(235, 160)

        lblHigh.AutoSize =
            True


        nudAlertHigh.Location =
            New Point(315, 155)

        nudAlertHigh.Width =
            100

        nudAlertHigh.DecimalPlaces =
            4

        nudAlertHigh.Minimum =
            0

        nudAlertHigh.Maximum =
            1000

        nudAlertHigh.Value =
            1.1D


        lblAlert.Location =
            New Point(450, 160)

        lblAlert.AutoSize =
            True

        lblAlert.Font =
            New Font(
                "Segoe UI",
                10,
                FontStyle.Bold)


        ' --------------------------------------------------------
        ' Chart
        ' --------------------------------------------------------

        picChart.Location =
            New Point(30, 205)

        picChart.Size =
            New Size(735, 310)

        picChart.BorderStyle =
            BorderStyle.FixedSingle

        picChart.BackColor =
            Color.White


        ' --------------------------------------------------------
        ' Events
        ' --------------------------------------------------------

        AddHandler btnRefresh.Click,
            AddressOf btnRefresh_Click

        AddHandler cboBase.SelectedIndexChanged,
            AddressOf CurrencySelectionChanged

        AddHandler cboTarget.SelectedIndexChanged,
            AddressOf CurrencySelectionChanged

        AddHandler picChart.Paint,
            AddressOf picChart_Paint


        ' --------------------------------------------------------
        ' Add controls
        ' --------------------------------------------------------

        Controls.Add(lblBase)
        Controls.Add(cboBase)

        Controls.Add(lblTarget)
        Controls.Add(cboTarget)

        Controls.Add(btnRefresh)

        Controls.Add(lblRate)
        Controls.Add(lblStatus)

        Controls.Add(lblLow)
        Controls.Add(nudAlertLow)

        Controls.Add(lblHigh)
        Controls.Add(nudAlertHigh)

        Controls.Add(lblAlert)

        Controls.Add(picChart)

    End Sub


    ' ============================================================
    ' Start Dashboard
    ' ============================================================

    Private Sub Form1_Shown(
        sender As Object,
        e As EventArgs) Handles MyBase.Shown

        _service.BaseCurrency =
            cboBase.Text

        lblStatus.Text =
            "Connecting to exchange-rate service..."

        _service.Start()

    End Sub


    ' ============================================================
    ' Currency selection changed
    ' ============================================================

    Private Async Sub CurrencySelectionChanged(
        sender As Object,
        e As EventArgs)

        If _service Is Nothing Then
            Return
        End If

        _history.Clear()

        picChart.Invalidate()

        _service.BaseCurrency =
            cboBase.Text

        lblStatus.Text =
            "Updating rates..."

        Await _service.RefreshAsync()

    End Sub


    ' ============================================================
    ' Manual Refresh
    ' ============================================================

    Private Async Sub btnRefresh_Click(
        sender As Object,
        e As EventArgs)

        btnRefresh.Enabled =
            False

        lblStatus.Text =
            "Refreshing..."

        Try

            _service.BaseCurrency =
                cboBase.Text

            Await _service.RefreshAsync()

        Finally

            btnRefresh.Enabled =
                True

        End Try

    End Sub


    ' ============================================================
    ' DataUpdated
    '
    ' IMPORTANT:
    ' This event can arrive from a ThreadPool thread.
    ' ============================================================

    Private Sub Service_DataUpdated(
        sender As Object,
        e As EventArgs)

        InvokeIfRequired(
            AddressOf RefreshDashboard)

    End Sub


    ' ============================================================
    ' Data Error
    ' ============================================================

    Private Sub Service_DataError(
        sender As Object,
        e As CurrencyErrorEventArgs)

        InvokeIfRequired(
            Sub()

                lblStatus.Text =
                    "Error: " & e.Message

                lblStatus.ForeColor =
                    Color.Crimson

            End Sub)

    End Sub


    ' ============================================================
    ' Marshal work to UI thread
    ' ============================================================

    Private Sub InvokeIfRequired(
        action As Action)

        If IsDisposed Then
            Return
        End If

        If Not IsHandleCreated Then
            Return
        End If

        If InvokeRequired Then

            BeginInvoke(action)

        Else

            action()

        End If

    End Sub


    ' ============================================================
    ' Update UI
    ' ============================================================

    Private Sub RefreshDashboard()

        Dim target As String =
            cboTarget.Text

        Dim rate As Double

        If Not _viewModel.Rates.TryGetValue(
            target,
            rate) Then

            lblStatus.Text =
                "Rate not available for " &
                target

            Return

        End If


        ' --------------------------------------------------------
        ' Display current rate
        ' --------------------------------------------------------

        lblRate.Text =
            $"1 {_viewModel.BaseCurrency} = " &
            $"{rate:F4} {target}"


        lblStatus.Text =
            "Updated: " &
            _viewModel.LastUpdated.ToString(
                "HH:mm:ss")

        lblStatus.ForeColor =
            Color.DarkGreen


        ' --------------------------------------------------------
        ' Add rate to history
        ' --------------------------------------------------------

        _history.Add(
            (DateTime.Now, rate))


        ' Keep only latest 60 readings
        If _history.Count > 60 Then
            _history.RemoveAt(0)
        End If


        ' Redraw chart
        picChart.Invalidate()


        ' --------------------------------------------------------
        ' Alert
        ' --------------------------------------------------------

        Dim lowLimit As Double =
            CDbl(nudAlertLow.Value)

        Dim highLimit As Double =
            CDbl(nudAlertHigh.Value)


        If rate < lowLimit Then

            lblAlert.Text =
                $"LOW ALERT: {rate:F4}"

            lblAlert.ForeColor =
                Color.Crimson

        ElseIf rate > highLimit Then

            lblAlert.Text =
                $"HIGH ALERT: {rate:F4}"

            lblAlert.ForeColor =
                Color.Crimson

        Else

            lblAlert.Text =
                "Rate within alert range"

            lblAlert.ForeColor =
                Color.DarkGreen

        End If

    End Sub


    ' ============================================================
    ' Draw Live Rate Chart
    ' ============================================================

    Private Sub picChart_Paint(
        sender As Object,
        e As PaintEventArgs)

        Dim g As Graphics =
            e.Graphics

        g.SmoothingMode =
            SmoothingMode.AntiAlias


        ' --------------------------------------------------------
        ' Chart title
        ' --------------------------------------------------------

        Using titleFont As New Font(
            "Segoe UI",
            11,
            FontStyle.Bold)

            g.DrawString(
                $"{cboBase.Text}/{cboTarget.Text} Rate History",
                titleFont,
                Brushes.Black,
                12,
                10)

        End Using


        If _history.Count < 2 Then

            Using messageFont As New Font(
                "Segoe UI",
                10)

                g.DrawString(
                    "Waiting for rate history...",
                    messageFont,
                    Brushes.Gray,
                    20,
                    60)

            End Using

            Return

        End If


        ' --------------------------------------------------------
        ' Chart rectangle
        ' --------------------------------------------------------

        Dim chart As New RectangleF(
            55,
            50,
            picChart.ClientSize.Width - 80,
            picChart.ClientSize.Height - 90)


        g.DrawRectangle(
            Pens.Gray,
            Rectangle.Round(chart))


        ' --------------------------------------------------------
        ' Find minimum and maximum
        ' --------------------------------------------------------

        Dim minimumRate As Double =
            _history.Min(
                Function(item)
                    Return item.Rate
                End Function)


        Dim maximumRate As Double =
            _history.Max(
                Function(item)
                    Return item.Rate
                End Function)


        If Math.Abs(
            maximumRate - minimumRate) < 0.000001 Then

            minimumRate -= 0.01
            maximumRate += 0.01

        End If


        ' --------------------------------------------------------
        ' Draw rate labels
        ' --------------------------------------------------------

        Using axisFont As New Font(
            "Segoe UI",
            8)

            g.DrawString(
                maximumRate.ToString("F4"),
                axisFont,
                Brushes.Gray,
                3,
                chart.Top - 5)

            g.DrawString(
                minimumRate.ToString("F4"),
                axisFont,
                Brushes.Gray,
                3,
                chart.Bottom - 12)

        End Using


        ' --------------------------------------------------------
        ' Convert history into chart points
        ' --------------------------------------------------------

        Dim points(
            _history.Count - 1) As PointF


        For i As Integer =
            0 To _history.Count - 1

            Dim x As Single

            If _history.Count = 1 Then

                x = chart.Left

            Else

                x =
                    chart.Left +
                    CSng(
                        i /
                        CDbl(_history.Count - 1) *
                        chart.Width)

            End If


            Dim normalized As Double =
                (_history(i).Rate - minimumRate) /
                (maximumRate - minimumRate)


            Dim y As Single =
                chart.Bottom -
                CSng(normalized * chart.Height)


            points(i) =
                New PointF(x, y)

        Next


        ' --------------------------------------------------------
        ' Draw line
        ' --------------------------------------------------------

        Using linePen As New Pen(
            Color.Black,
            2)

            g.DrawLines(
                linePen,
                points)

        End Using


        ' --------------------------------------------------------
        ' Draw last point
        ' --------------------------------------------------------

        Dim lastPoint As PointF =
            points(points.Length - 1)

        g.FillEllipse(
            Brushes.Black,
            lastPoint.X - 4,
            lastPoint.Y - 4,
            8,
            8)

    End Sub


    ' ============================================================
    ' Close application
    ' ============================================================

    Private Sub Form1_FormClosed(
        sender As Object,
        e As FormClosedEventArgs) _
        Handles MyBase.FormClosed

        If _service IsNot Nothing Then
            _service.Dispose()
        End If

    End Sub

End Class