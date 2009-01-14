Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Public Sub New()
        Dim mep As MathExpressionParser = New MathExpressionParser()
        mep.init("x+yfsdf")
        MsgBox(Str(mep.eval(9, 15)))
        Environment.Exit(0)
    End Sub

End Class
