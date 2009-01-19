Imports System.Windows.Media.Media3D

Public Class Particle
    Public position As Point3D
    Public velocity As Vector3D
    Public pbestx As Point3D
    Public pbest As Double = Double.NegativeInfinity

    Private Shared rand As Random = New Random()

    Public Sub New(p As Point3D)
        position = p
        pbest = p.Z
        pbestx = p

        velocity = New Point3D(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5, 0)
    End Sub

End Class

Public Class PSO

    Public Shared instance As PSO

    Public particles() As Particle

    Public gbest As Double = Double.NegativeInfinity
    Public gbestx As Point3D = New Point3D()

    Private rand As Random = New Random()

    Public Sub New(ByVal qty As Integer, ByVal expr As MathExpression, ByVal xmin As Double, ByVal xmax As Double, ByVal ymin As Double, ByVal ymax As Double)
        instance = Me

        ReDim particles(qty - 1)
        For i As Integer = 0 To particles.Length - 1
            Dim x As Double = xmin + rand.NextDouble() * (xmax - xmin)
            Dim y As Double = ymin + rand.NextDouble() * (ymax - ymin)
            particles(i) = New Particle(New Point3D(x, y, expr.eval(x, y)))
        Next
    End Sub

End Class
