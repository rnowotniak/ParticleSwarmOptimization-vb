Partial Public Class MainWindow

    Public Sub New()
        InitializeComponent()
        debugTextBox.Text = "Application started" & vbCrLf
    End Sub

    Private Sub quitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles quitMenuItem.Click
        Environment.Exit(0)
    End Sub

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles startButton.Click
        debugTextBox.Text += "fasdf" & vbCrLf
    End Sub

    Private Sub presetsComboBox_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles presetsComboBox.SelectionChanged
        MsgBox(presetsComboBox.SelectedIndex)
    End Sub
End Class
