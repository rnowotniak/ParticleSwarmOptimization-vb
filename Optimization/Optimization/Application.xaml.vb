Imports System.Threading
Imports System.Globalization

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Public Sub New()

        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)

        ' Some tests
        'Dim o As New Preset("Rosenbrock's valley", "-(100.0 * (y-x*x)*(y-x*x) + (1-x)*(1-x))", _
        '    -2, 2, -2, 2)
        If False Then
            Dim mep As MathExpression = New MathExpression()
            mep.init("1+rand")
            'mep.init(o.func)
            Try
                MsgBox(Str(mep.eval(1, 1)))
            Catch ex As Exception
                Environment.Exit(0)
            End Try
            Environment.Exit(0)
        End If

    End Sub

End Class
