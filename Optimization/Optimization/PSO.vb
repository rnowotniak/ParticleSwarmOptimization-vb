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

    Private rand As Random = New Random()

    Public particles() As Particle

    Private expr As MathExpression
    Private xmin, xmax, ymin, ymax As Double

    Public gbest As Double = Double.NegativeInfinity
    Public gbestx As Point3D = New Point3D()

    Private c1 As Double = 2, c2 As Double = 2
    Private maxVelocity As Double = 1

    Public iteration As Integer

    Public Sub New(ByVal qty As Integer, ByVal expr As MathExpression, ByVal xmin As Double, ByVal xmax As Double, ByVal ymin As Double, ByVal ymax As Double)
        instance = Me

        Me.xmin = xmin
        Me.xmax = xmax
        Me.ymin = ymin
        Me.ymax = ymax
        Me.expr = expr

        iteration = 0

        ReDim particles(qty - 1)
        For i As Integer = 0 To particles.Length - 1
            Dim x As Double = xmin + rand.NextDouble() * (xmax - xmin)
            Dim y As Double = ymin + rand.NextDouble() * (ymax - ymin)
            particles(i) = New Particle(New Point3D(x, y, expr.eval(x, y)))
        Next
    End Sub

    Public Sub doStep()
        iteration += 1

        ' learning factors
        Dim c1 As Double = 2
        Dim c2 As Double = 2

        For i As Integer = 0 To particles.Length - 1
            ' calculate fitness value of this particle
            If particles(i).position.X > xmin And particles(i).position.X < xmax And _
                    particles(i).position.Y > ymin And particles(i).position.Y < ymax Then
                particles(i).position.Z = expr.eval(particles(i).position.X, particles(i).position.Y)
            Else
                ' solution is out of constraints
                particles(i).position.Z = Double.NegativeInfinity
            End If

            If particles(i).position.Z > particles(i).pbest Then
                particles(i).pbestx = particles(i).position
                particles(i).pbest = particles(i).position.Z
            End If
            If particles(i).position.Z > gbest Then
                gbest = particles(i).position.Z
                gbestx = particles(i).position
            End If
        Next

        For i As Integer = 0 To particles.Length - 1
            ' update particles velocities and positions
            particles(i).velocity += c1 * rand.NextDouble() * (particles(i).pbestx - particles(i).position) _
                + c2 * rand.NextDouble() * (gbestx - particles(i).position)
            particles(i).velocity.Z = 0
            If particles(i).velocity.Length > maxVelocity Then
                particles(i).velocity /= particles(i).velocity.Length
                particles(i).velocity *= maxVelocity
            End If
            particles(i).position += particles(i).velocity
        Next
    End Sub

End Class
