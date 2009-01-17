Imports System.Windows.Media.Media3D

Public Class Particle
    Public position As Point3D
    Public velocity As Vector3D

    Private Shared rand As Random = New Random()

    Public Sub New(p As Point3D)
        position = p

        velocity = New Point3D(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5, rand.NextDouble() - 0.5)
    End Sub

End Class

Public Class PSO

    Public Shared instance As PSO

    Public particles() As Particle

    Private rand As Random = New Random()

    Public Sub New(qty As Integer, xmin As Double, xmax As Double, ymin As Double, ymax As Double)
        instance = Me

        ReDim particles(15)
        For i As Integer = 0 To particles.Length - 1
            particles(i) = New Particle(New Point3D(rand.NextDouble() * 10 - 5, rand.NextDouble() * 10 - 5, -5))
        Next
    End Sub

End Class
