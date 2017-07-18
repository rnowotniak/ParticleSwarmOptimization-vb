Imports System.Text.RegularExpressions

Public MustInherit Class ExpressionBaseClass
    Public MustOverride Function eval(ByVal x As Double, ByVal y As Double) As Double
End Class

Public Class MathExpression

    Private expressionObject As ExpressionBaseClass

    Public Sub New()
    End Sub

    Public Sub New(ByVal expr As String)
        init(expr)
    End Sub

    Private Function translate(ByVal expr As String) As String
        expr = expr.ToLower()
        expr = Regex.Replace(expr, "(^|[^.])(pow|sin|cos|sqrt|abs|pi|exp)", "$1Math.$2", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "pow", "Pow", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "sin", "Sin", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "cos", "Cos", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "sqrt", "Sqrt", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "abs", "Abs", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "exp", "Exp", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "pi", "PI", RegexOptions.IgnoreCase)
        expr = Regex.Replace(expr, "rand", "r.NextDouble()", RegexOptions.IgnoreCase)
        Return expr
    End Function

    Public Sub init(ByVal expr As String)
        expr = translate(expr)
        Dim cp As Microsoft.CSharp.CSharpCodeProvider _
            = New Microsoft.CSharp.CSharpCodeProvider()
        Dim ic As System.CodeDom.Compiler.ICodeCompiler = cp.CreateCompiler()
        Dim cpar As System.CodeDom.Compiler.CompilerParameters = _
            New System.CodeDom.Compiler.CompilerParameters()
        cpar.GenerateInMemory = True
        cpar.GenerateExecutable = False
        cpar.ReferencedAssemblies.Add("system.dll")
        cpar.ReferencedAssemblies.Add("Optimization.exe")
        Dim src = "using System;" & _
            "class ExpressionClass : Optimization.ExpressionBaseClass {" & _
            "  public override double eval(double x, double y) {" & _
            "  Random r = new Random();" & _
            "  return " & expr & " ;" & _
            "  }" & _
            "}"
        Dim cr As System.CodeDom.Compiler.CompilerResults = _
            ic.CompileAssemblyFromSource(cpar, src)
        For Each ce As System.CodeDom.Compiler.CompilerError In cr.Errors
            MsgBox(ce.ErrorText)
        Next
        If cr.Errors.Count = 0 Then
            Dim o As Object = Activator.CreateInstance( _
                cr.CompiledAssembly.GetType("ExpressionClass"))
            expressionObject = o
        Else
            MsgBox("Wrong syntax in mathematical expression")
        End If
    End Sub

    Public Function eval(ByVal x As Double, ByVal y As Double) As Double
        If expressionObject Is Nothing Then
            Throw New Exception("expression parser not initialised")
        End If
        Return expressionObject.eval(x, y)
    End Function

End Class
