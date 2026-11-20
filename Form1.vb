Public Class Form1
    Private ReadOnly weightInput As New TextBox()
    Private ReadOnly unitSelector As New ComboBox()
    Private ReadOnly calculateButton As New Button()
    Private ReadOnly quitButton As New Button()
    Private ReadOnly resultsList As New ListView()
    Private ReadOnly statusLabel As New Label()
    Private backgroundMusic As System.Media.SoundPlayer

    Private Const PoundsPerKilogram As Decimal = 2.20462262185D

    Private Shared ReadOnly PlanetGravity As (Name As String, RelativeGravity As Decimal)() = {
        ("Mercury", 0.378D),
        ("Venus", 0.907D),
        ("Earth", 1D),
        ("Mars", 0.377D),
        ("Jupiter", 2.36D),
        ("Saturn", 0.916D),
        ("Uranus", 0.889D),
        ("Neptune", 1.12D),
        ("Pluto", 0.063D)
    }

    Public Sub New()
        InitializeComponent()
        BuildCalculator()
        backgroundMusic = New System.Media.SoundPlayer(
            IO.Path.Combine(Application.StartupPath, "Cring.wav"))
        AddHandler Me.Load, AddressOf Form1_Load
        AddHandler Me.FormClosed, AddressOf Form1_FormClosed
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs)
        backgroundMusic.PlayLooping()
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs)
        backgroundMusic.Stop()
        backgroundMusic.Dispose()
    End Sub

    Private Sub BuildCalculator()
        BackColor = Color.FromArgb(245, 248, 252)
        Font = New Font("Segoe UI", 10.0F)
        Text = "Solar System Weight Calculator"
        ControlBox = False
        MinimumSize = New Size(540, 560)
        ClientSize = New Size(640, 680)
        StartPosition = FormStartPosition.CenterScreen

        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(28),
            .ColumnCount = 1,
            .RowCount = 6,
            .BackColor = BackColor
        }
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 72))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 92))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 52))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 36))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 48))
        Controls.Add(layout)

        Dim heading As New Label With {
            .Text = "Your weight across the solar system",
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI Semibold", 18.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(28, 48, 74),
            .TextAlign = ContentAlignment.MiddleLeft
        }
        layout.Controls.Add(heading, 0, 0)

        Dim entryPanel As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .WrapContents = False,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(0, 10, 0, 0)
        }
        layout.Controls.Add(entryPanel, 0, 1)

        Dim weightLabel As New Label With {
            .Text = "Your weight",
            .AutoSize = True,
            .Margin = New Padding(0, 8, 10, 0),
            .ForeColor = Color.FromArgb(55, 70, 90)
        }
        entryPanel.Controls.Add(weightLabel)

        weightInput.Width = 150
        weightInput.Height = 34
        weightInput.Font = New Font("Segoe UI", 12.0F)
        weightInput.Margin = New Padding(0, 0, 12, 0)
        weightInput.MaxLength = 9
        weightInput.TextAlign = HorizontalAlignment.Right
        AddHandler weightInput.KeyPress, AddressOf WeightInput_KeyPress
        AddHandler weightInput.TextChanged, AddressOf WeightInput_TextChanged
        entryPanel.Controls.Add(weightInput)

        unitSelector.DropDownStyle = ComboBoxStyle.DropDownList
        unitSelector.Width = 80
        unitSelector.Height = 34
        unitSelector.Font = New Font("Segoe UI", 11.0F)
        unitSelector.Items.AddRange(New Object() {"lb", "kg"})
        unitSelector.SelectedIndex = 0
        unitSelector.Margin = New Padding(0)
        AddHandler unitSelector.SelectedIndexChanged, AddressOf UnitSelector_SelectedIndexChanged
        entryPanel.Controls.Add(unitSelector)

        calculateButton.Text = "Calculate"
        calculateButton.Dock = DockStyle.Fill
        calculateButton.FlatStyle = FlatStyle.Flat
        calculateButton.FlatAppearance.BorderSize = 0
        calculateButton.BackColor = Color.FromArgb(38, 104, 173)
        calculateButton.ForeColor = Color.White
        calculateButton.Font = New Font("Segoe UI Semibold", 11.0F, FontStyle.Bold)
        calculateButton.Cursor = Cursors.Hand
        calculateButton.Margin = New Padding(0, 0, 0, 12)
        AddHandler calculateButton.Click, AddressOf CalculateButton_Click
        layout.Controls.Add(calculateButton, 0, 2)

        resultsList.Dock = DockStyle.Fill
        resultsList.View = View.Details
        resultsList.FullRowSelect = True
        resultsList.GridLines = True
        resultsList.HideSelection = False
        resultsList.Font = New Font("Segoe UI", 11.0F)
        resultsList.Columns.Add("Planet", 180)
        resultsList.Columns.Add("Weight (lb)", 170, HorizontalAlignment.Right)
        resultsList.Columns.Add("Weight (kg)", 170, HorizontalAlignment.Right)
        layout.Controls.Add(resultsList, 0, 3)

        statusLabel.Text = "Enter a whole-number weight to see the results."
        statusLabel.Dock = DockStyle.Fill
        statusLabel.ForeColor = Color.FromArgb(85, 100, 120)
        statusLabel.TextAlign = ContentAlignment.MiddleLeft
        layout.Controls.Add(statusLabel, 0, 4)

        quitButton.Text = "Quit"
        quitButton.Width = 110
        quitButton.Height = 36
        quitButton.Anchor = AnchorStyles.Right
        quitButton.FlatStyle = FlatStyle.Flat
        quitButton.FlatAppearance.BorderSize = 0
        quitButton.BackColor = Color.FromArgb(85, 100, 120)
        quitButton.ForeColor = Color.White
        quitButton.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        quitButton.Cursor = Cursors.Hand
        AddHandler quitButton.Click, AddressOf QuitButton_Click
        layout.Controls.Add(quitButton, 0, 5)
    End Sub

    Private Sub QuitButton_Click(sender As Object, e As EventArgs)
        Close()
    End Sub

    Private Sub WeightInput_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub WeightInput_TextChanged(sender As Object, e As EventArgs)
        UpdateResults()
    End Sub

    Private Sub UnitSelector_SelectedIndexChanged(sender As Object, e As EventArgs)
        UpdateResults()
    End Sub

    Private Sub CalculateButton_Click(sender As Object, e As EventArgs)
        UpdateResults()

        Dim enteredWeight As Integer
        If Not Integer.TryParse(weightInput.Text, enteredWeight) OrElse enteredWeight <= 0 Then
            statusLabel.Text = "Enter a whole number greater than zero."
            statusLabel.ForeColor = Color.Firebrick
            weightInput.Focus()
        End If
    End Sub

    Private Sub UpdateResults()
        Dim enteredWeight As Integer
        If Not Integer.TryParse(weightInput.Text, enteredWeight) OrElse enteredWeight <= 0 Then
            resultsList.Items.Clear()
            statusLabel.ForeColor = Color.FromArgb(85, 100, 120)
            statusLabel.Text = "Enter a whole-number weight to see the results."
            Return
        End If

        Dim inputUnit As String = CStr(unitSelector.SelectedItem)
        Dim inputIsPounds As Boolean = inputUnit = "lb"
        resultsList.Columns(1).Text = If(inputIsPounds, "Weight (lb)", "Weight (kg)")
        resultsList.Columns(2).Text = If(inputIsPounds, "Weight (kg)", "Weight (lb)")
        resultsList.BeginUpdate()
        resultsList.Items.Clear()

        For Each planet In PlanetGravity
            Dim weightInPounds As Decimal
            Dim weightInKilograms As Decimal
            If inputIsPounds Then
                weightInPounds = enteredWeight * planet.RelativeGravity
                weightInKilograms = weightInPounds / PoundsPerKilogram
            Else
                weightInKilograms = enteredWeight * planet.RelativeGravity
                weightInPounds = weightInKilograms * PoundsPerKilogram
            End If

            Dim roundedPounds As Integer = CInt(Math.Round(weightInPounds, MidpointRounding.AwayFromZero))
            Dim roundedKilograms As Integer = CInt(Math.Round(weightInKilograms, MidpointRounding.AwayFromZero))
            Dim row As New ListViewItem(planet.Name)
            row.SubItems.Add($"{If(inputIsPounds, roundedPounds, roundedKilograms):N0}")
            row.SubItems.Add($"{If(inputIsPounds, roundedKilograms, roundedPounds):N0}")
            resultsList.Items.Add(row)
        Next

        resultsList.EndUpdate()
        statusLabel.ForeColor = Color.FromArgb(85, 100, 120)
        statusLabel.Text = $"Input: {inputUnit}. Results show rounded whole-number pounds and kilograms."
    End Sub
End Class
