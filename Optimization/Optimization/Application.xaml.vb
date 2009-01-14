Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Public Sub New()
        If True Then
            Dim mep As MathExpressionParser = New MathExpressionParser()
            mep.init("1+pow(x,y) / 3")
            Try
                MsgBox(Str(mep.eval(9, 15)))
            Catch ex As Exception
                Environment.Exit(0)
            End Try
            Environment.Exit(0)
        End If
        Dim o As New Preset("Rosenbrock's vallye", "-(100.0 * (y-x*x)*(y-x*x) + (1-x)*(1-x))", _
            -2, 2, -2, 2)
    End Sub

End Class
