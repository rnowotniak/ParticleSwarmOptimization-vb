Imports System.Windows.Media.Media3D
Imports System.Windows.Threading

Partial Public Class MainWindow

    Private rand As Random = New Random()

    Private prevMousePoint As Point

    Private wireframe As _3DTools.ScreenSpaceLines3D = New _3DTools.ScreenSpaceLines3D()
    Private axes As _3DTools.ScreenSpaceLines3D = New _3DTools.ScreenSpaceLines3D()

    Private expr As MathExpression
    Private zfactor As Double = 1.0

    Public maxVelocity As Double = 0.3

    Private autoRotationTimer As DispatcherTimer = New DispatcherTimer()
    Private psoTimer As DispatcherTimer = New DispatcherTimer()

    ' textures
    Dim plotTexture As Material = New DiffuseMaterial( _
        New SolidColorBrush(Colors.White))
    Dim particlesTexture As Material = New DiffuseMaterial( _
        New SolidColorBrush(Colors.Red))


    ' Constructor
    Public Sub New()
        InitializeComponent()
        For Each kvp In Application.instance.presets
            Dim mi As MenuItem = New MenuItem()
            mi.Header = kvp.Key
            mi.Command = New RoutedUICommand()
            mi.Tag = kvp.Value
            CommandBindings.Add(New CommandBinding(mi.Command, AddressOf presetsMenuItem_Click))
            presetsMenuItem.Items.Add(mi)
            presetsComboBox.Items.Add(kvp.Value)
        Next

        makeAxes()

        view3d.Children.Add(axes)
        view3d.Children.Add(wireframe)

        ' set timers
        autoRotationTimer.Interval = New TimeSpan(10000000 / 20)
        AddHandler autoRotationTimer.Tick, AddressOf autoRotationTimer_tick
        psoTimer.Interval = New TimeSpan(10000000 / 5)
        AddHandler psoTimer.Tick, AddressOf psoTimer_tick

        debugTextBox.Text = "Application started" & vbCrLf
    End Sub

    Private Sub psoTimer_tick(ByVal sender As Object, ByVal e As EventArgs)
        psoStep()
    End Sub

    Private Sub autoRotationTimer_tick(ByVal sender As Object, ByVal e As EventArgs)
        xrot.Angle += 1
    End Sub

#Region "axes"
    Private Sub makeAxes()
        axes.Points.Add(New Point3D(-5, -5, -5))
        axes.Points.Add(New Point3D(5, -5, -5))
        axes.Points.Add(New Point3D(5, -5, -5))
        axes.Points.Add(New Point3D(4.5, -4.5, -5))
        axes.Points.Add(New Point3D(5, -5, -5))
        axes.Points.Add(New Point3D(4.5, -5.5, -5))

        axes.Points.Add(New Point3D(-5, -5, -5))
        axes.Points.Add(New Point3D(-5, 5, -5))
        axes.Points.Add(New Point3D(-5, 5, -5))
        axes.Points.Add(New Point3D(-5.5, 4.5, -5))
        axes.Points.Add(New Point3D(-5, 5, -5))
        axes.Points.Add(New Point3D(-4.5, 4.5, -5))

        axes.Points.Add(New Point3D(-5, -5, -5))
        axes.Points.Add(New Point3D(-5, -5, 5))
        axes.Points.Add(New Point3D(-5, -5, 5))
        axes.Points.Add(New Point3D(-5.5, -5, 4.5))
        axes.Points.Add(New Point3D(-5, -5, 5))
        axes.Points.Add(New Point3D(-4.5, -5, 4.5))


        axes.Thickness = 3
    End Sub
#End Region

#Region "triangles"
    Private Function createTriangle( _
        ByVal x0 As Double, ByVal y0 As Double, ByVal z0 As Double, _
        ByVal x1 As Double, ByVal y1 As Double, ByVal z1 As Double, _
        ByVal x2 As Double, ByVal y2 As Double, ByVal z2 As Double) As Model3DGroup
        Return createTriangle( _
            New Point3D(x0, y0, z0), _
            New Point3D(x1, y1, z1), _
            New Point3D(x2, y2, z2))
    End Function

    Private Function createTriangle(ByVal p0 As Point3D, ByVal p1 As Point3D, ByVal p2 As Point3D) As Model3DGroup
        Dim mesh As MeshGeometry3D = New MeshGeometry3D()
        mesh.Positions.Add(p0)
        mesh.Positions.Add(p1)
        mesh.Positions.Add(p2)
        If meshCheckBox.IsChecked Then
            mesh.TriangleIndices.Add(0)
            mesh.TriangleIndices.Add(1)
            mesh.TriangleIndices.Add(2)
            Dim normal As Vector3D = calculateNormal(p0, p1, p2)
            mesh.Normals.Add(normal)
            mesh.Normals.Add(normal)
            mesh.Normals.Add(normal)
        End If

        Dim model As GeometryModel3D = New GeometryModel3D( _
            mesh, Nothing)
        If meshCheckBox.IsChecked Then
            model.Material = plotTexture
            wireframe.Color = Colors.Blue
        Else
            wireframe.Color = Colors.Red
        End If
        If doubleSideCheckBox.IsChecked Then
            model.BackMaterial = plotTexture
        End If
        Dim group As Model3DGroup = New Model3DGroup()
        group.Children.Add(model)

        Return group
    End Function

    Private Function calculateNormal(ByVal p0 As Point3D, ByVal p1 As Point3D, ByVal p2 As Point3D) As Vector3D
        Dim v0 As Vector3D = New Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z)
        Dim v1 As Vector3D = New Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z)
        Return Vector3D.CrossProduct(v0, v1)
    End Function
#End Region


    Private Function generateTopography(ByVal expr As MathExpression, _
                                         Optional ByVal density As Integer = 10) As Point3D()
        Dim points(density * density - 1) As Point3D
        Dim rand As New Random()
        Dim x, y As Double
        Dim xmin, ymin, xmax, ymax As Double

        xmin = Int(xminTextBox.Text)
        ymin = Int(yminTextBox.Text)
        xmax = Int(xmaxTextBox.Text)
        ymax = Int(ymaxTextBox.Text)

        Dim counter As Integer = 0
        x = xmin
        y = ymin

        Dim scale As Double = 10.0 / (density - 1)

        Dim maxheight As Double = Double.NegativeInfinity
        zfactor = Double.MinValue

        Try
            For i As Integer = 0 To density - 1
                y = ymin
                For j As Integer = 0 To density - 1
                    points(counter) = New Point3D(i * scale - 5, j * scale - 5, expr.eval(x, y))
                    If Math.Abs(points(counter).Z) > zfactor Then
                        zfactor = Math.Abs(points(counter).Z)
                    End If
                    If points(counter).Z > maxheight Then
                        maxheight = points(counter).Z
                    End If
                    y = y + (ymax - ymin) / (density - 1)
                    counter = counter + 1
                Next
                x = x + (xmax - xmin) / (density - 1)
            Next
        Catch ex As Exception
            Return Nothing
        End Try

        logLine("Maximum value on the plot: " & Str(maxheight))
        zfactor /= 4.0

        ' Rescale height
        For v = 0 To points.Length - 1
            points(v).Z = points(v).Z / zfactor
        Next

        Return points
    End Function


    Public Sub logLine(ByVal s As String)
        debugTextBox.AppendText(s & vbCrLf)
        debugTextBox.ScrollToEnd()
    End Sub

    Private Sub quitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles quitMenuItem.Click
        Environment.Exit(0)
    End Sub

    Private Function screenCoords(ByVal x As Double, ByVal y As Double, ByVal z As Double) As Double()
        Dim result(2) As Double
        Dim xmin, xmax, ymin, ymax As Double
        xmin = Int(xminTextBox.Text)
        ymin = Int(yminTextBox.Text)
        xmax = Int(xmaxTextBox.Text)
        ymax = Int(ymaxTextBox.Text)
        result(0) = x * 10.0 / (xmax - xmin) + -5 + -xmin * (10.0 / (xmax - xmin))
        result(1) = y * 10.0 / (ymax - ymin) + -5 + -ymin * (10.0 / (ymax - ymin))
        result(2) = z / zfactor + 0.01
        Return result
    End Function

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles startButton.Click
        logLine("Starting optimization...")
        Dim pso As PSO = New PSO(Int(particlesTextBox.Text), expr, _
            Int(xminTextBox.Text), Int(xmaxTextBox.Text), Int(yminTextBox.Text), Int(ymaxTextBox.Text))
        Dim mg3d As Model3DGroup = New Model3DGroup()
        Dim mg As MeshGeometry3D = New MeshGeometry3D()
        Dim size As Double = 0.1
        For i As Integer = 0 To pso.particles.Length - 1
            Dim coords() As Double = screenCoords(pso.particles(i).position.X, pso.particles(i).position.Y, pso.particles(i).position.Z)
            mg.Positions.Add(New Point3D(coords(0) - size, coords(1) - size, coords(2)))
            mg.Positions.Add(New Point3D(coords(0) + size, coords(1) - size, coords(2)))
            mg.Positions.Add(New Point3D(coords(0) + size, coords(1) + size, coords(2)))
            mg.Positions.Add(New Point3D(coords(0) - size, coords(1) + size, coords(2)))
            mg.TriangleIndices.Add(4 * i + 0)
            mg.TriangleIndices.Add(4 * i + 1)
            mg.TriangleIndices.Add(4 * i + 2)
            mg.TriangleIndices.Add(4 * i + 0)
            mg.TriangleIndices.Add(4 * i + 2)
            mg.TriangleIndices.Add(4 * i + 3)
            mg.Normals.Add(New Vector3D(0, 0, 1))
            mg.Normals.Add(New Vector3D(0, 0, 1))
            mg.Normals.Add(New Vector3D(0, 0, 1))
        Next
        Dim gm3 As GeometryModel3D = New GeometryModel3D(mg, particlesTexture)
        gm3.BackMaterial = particlesTexture
        mg3d.Children.Add(gm3)
        particlesModel.Content = mg3d

    End Sub

    Private Sub presetsComboBox_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles presetsComboBox.SelectionChanged
        Dim preset As Preset = CType(presetsComboBox.SelectedItem, Preset)
        If preset Is Nothing Then
            Return
        End If
        setPreset(preset)
    End Sub

    Private Sub setPreset(ByVal preset As Preset)
        functionTextBox.Text = preset.func
        xminTextBox.Text = Str(preset.xmin)
        xmaxTextBox.Text = Str(preset.xmax)
        yminTextBox.Text = Str(preset.ymin)
        ymaxTextBox.Text = Str(preset.ymax)
        densityTextBox.Text = Str(preset.density)
    End Sub

    Private Sub resetButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles resetButton.Click
        clearPlot()
    End Sub

    Private Sub clearPlot()
        plot.Content = Nothing
        wireframe.Points.Clear()
    End Sub

    Private Sub plotButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles plotButton.Click
        clearPlot()

        Dim mesh As Model3DGroup = New Model3DGroup()

        Dim expr As MathExpression = New MathExpression(functionTextBox.Text)
        Dim points() As Point3D = generateTopography(expr, Int(densityTextBox.Text))
        If points Is Nothing Then
            Return
        End If
        Me.expr = expr

        If Not PSO.instance Is Nothing Then
            PSO.instance.gbest = Double.NegativeInfinity
        End If

        Dim density = Int(densityTextBox.Text)

        Dim y = 0
        While y <= density * (density - 2)
            Dim x = 0
            While x < density - 1
                mesh.Children.Add( _
                    createTriangle(points(x + y), points(x + y + density), points(x + y + 1)))
                mesh.Children.Add( _
                    createTriangle(points(x + y + 1), points(x + y + density), points(x + y + density + 1)))
                x = x + 1
            End While
            y = y + density
        End While

        plot.Content = mesh
        If wireframeCheckBox.IsChecked Then
            wireframe.MakeWireframe(mesh)
        End If
    End Sub

    Private Sub hScrollBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles hScrollBar.ValueChanged
        Dim val As Double = hScrollBar.Value
        xrot.Angle = val
    End Sub

    Private Sub vScrollBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles vScrollBar.ValueChanged
        Dim val As Double = vScrollBar.Value
        If yrot Is Nothing Then
            Return
        End If
        yrot.Angle = val
    End Sub

    Private Sub aboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles aboutMenuItem.Click
        Dim w As Window = New AboutWindow()
        w.ShowDialog()
    End Sub

    Private Sub presetsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles presetsMenuItem.Click
        Dim mi As MenuItem
        Try
            mi = e.Source
        Catch ex As InvalidCastException
            Return
        End Try
        Dim p As Preset = mi.Tag
        setPreset(p)
    End Sub

    Private Sub resetViewButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles resetViewButton.Click
        vScrollBar.Value = 0
        hScrollBar.Value = 0
        distance.ScaleX = 1
        distance.ScaleY = 1
        distance.ScaleZ = 1
    End Sub


    Private Sub OnViewportMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        If e.MouseDevice.LeftButton = MouseButtonState.Pressed Then
            Dim vector As Vector = prevMousePoint - e.GetPosition(view3d)
            xrot.Angle += Math.Sign(vector.X)
            yrot.Angle += Math.Sign(vector.Y)
            prevMousePoint = e.GetPosition(view3d)
        ElseIf e.MouseDevice.RightButton = MouseButtonState.Pressed Then
            Dim vector As Vector = prevMousePoint - e.GetPosition(view3d)
            Dim factor As Double = 1.02
            If Math.Sign(vector.Y) < 0 Then
                distance.ScaleX *= factor
                distance.ScaleY *= factor
                distance.ScaleZ *= factor
            Else
                distance.ScaleX /= factor
                distance.ScaleY /= factor
                distance.ScaleZ /= factor
            End If
            prevMousePoint = e.GetPosition(view3d)
        End If
    End Sub

    Private Sub OnViewportMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        prevMousePoint = e.GetPosition(view3d)
    End Sub

    ' XXX: przeniesc do PSO.vb
    Private Sub psoStep()
        Dim pso As PSO = pso.instance
        Dim positions As Point3DCollection = New Point3DCollection()
        Dim size As Double = 0.1

        Dim xmin, xmax, ymin, ymax As Double
        xmin = Int(xminTextBox.Text)
        ymin = Int(yminTextBox.Text)
        xmax = Int(xmaxTextBox.Text)
        ymax = Int(ymaxTextBox.Text)

        ' learning factors
        Dim c1 As Double = 2
        Dim c2 As Double = 2

        For i As Integer = 0 To pso.particles.Length - 1
            ' calculate fitness value of this particle
            If pso.particles(i).position.X > xmin And pso.particles(i).position.X < xmax And _
                    pso.particles(i).position.Y > ymin And pso.particles(i).position.Y < ymax Then
                pso.particles(i).position.Z = expr.eval(pso.particles(i).position.X, pso.particles(i).position.Y)
            Else
                ' solution is out of constraints
                pso.particles(i).position.Z = Double.NegativeInfinity
            End If

            If pso.particles(i).position.Z > pso.particles(i).pbest Then
                pso.particles(i).pbestx = pso.particles(i).position
                pso.particles(i).pbest = pso.particles(i).position.Z
            End If
            If pso.particles(i).position.Z > pso.gbest Then
                pso.gbest = pso.particles(i).position.Z
                pso.gbestx = pso.particles(i).position
            End If
        Next

        For i As Integer = 0 To pso.particles.Length - 1
            ' update particles velocities and positions
            pso.particles(i).velocity += c1 * rand.NextDouble() * (pso.particles(i).pbestx - pso.particles(i).position) _
                + c2 * rand.NextDouble() * (pso.gbestx - pso.particles(i).position)
            pso.particles(i).velocity.Z = 0
            If pso.particles(i).velocity.Length > maxVelocity Then
                pso.particles(i).velocity /= pso.particles(i).velocity.Length
                pso.particles(i).velocity *= maxVelocity
            End If
            pso.particles(i).position += pso.particles(i).velocity
        Next

        logLine("Global best solution: " & String.Format("({0},{1})  Value: {2}", pso.gbestx.X, pso.gbestx.Y, pso.gbest))
        optimumXTextBox.Text = Str(pso.gbestx.X)
        optimumYTextBox.Text = Str(pso.gbestx.Y)
        optimumValueTextBox.Text = Str(pso.gbest)

        ' update goemetry
        For i As Integer = 0 To pso.particles.Length - 1
            Dim coords() As Double
            coords = screenCoords(pso.particles(i).position.X, pso.particles(i).position.Y, _
                                  expr.eval(pso.particles(i).position.X, pso.particles(i).position.Y))
            positions.Add(New Point3D(coords(0) - size, coords(1) - size, coords(2)))
            positions.Add(New Point3D(coords(0) + size, coords(1) - size, coords(2)))
            positions.Add(New Point3D(coords(0) + size, coords(1) + size, coords(2)))
            positions.Add(New Point3D(coords(0) - size, coords(1) + size, coords(2)))
        Next

        Dim mg3d As Model3DGroup = particlesModel.Content
        Dim gm3 As GeometryModel3D = mg3d.Children(0)
        Dim g As MeshGeometry3D = gm3.Geometry
        g.Positions = positions
    End Sub

    Private Sub stepButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles stepButton.Click
        psoStep()
    End Sub


    Private Sub autoRotateToggleButton_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles autoRotateToggleButton.Checked
        autoRotationTimer.IsEnabled = autoRotateToggleButton.IsChecked
    End Sub

    Private Sub runToggleButton_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles runToggleButton.Checked
        psoTimer.IsEnabled = runToggleButton.IsChecked
    End Sub
End Class
