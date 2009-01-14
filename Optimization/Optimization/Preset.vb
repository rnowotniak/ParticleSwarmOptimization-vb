Public Class Preset

    Public name As String
    Public func As String
    Public xmin, xmax, ymin, ymax As Double
    Public density As Integer

    Public Sub New(ByVal name As String, ByVal func As String, _
        ByVal xmin As Double, ByVal xmax As Double, ByVal ymin As Double, ByVal ymax As Double, _
        Optional ByVal density As Integer = 10)

        Me.name = name
        Me.func = func

        Me.xmin = xmin
        Me.xmax = xmax
        Me.ymin = ymin
        Me.ymax = ymax

        Me.density = density
    End Sub

End Class
