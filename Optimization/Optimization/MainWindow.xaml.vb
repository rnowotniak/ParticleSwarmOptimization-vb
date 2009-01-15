Imports System.Windows.Media.Media3D

Partial Public Class MainWindow

    Private trackball As _3DTools.Trackball = New _3DTools.Trackball()

    Public Sub New()
        InitializeComponent()
        debugTextBox.Text = "Application started" & vbCrLf
        trackball.EventSource = view3dBorder
        camera.Transform = trackball.Transform

    End Sub

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
        mesh.TriangleIndices.Add(0)
        mesh.TriangleIndices.Add(1)
        mesh.TriangleIndices.Add(2)
        Dim normal As Vector3D = calculateNormal(p0, p1, p2)
        mesh.Normals.Add(normal)
        mesh.Normals.Add(normal)
        mesh.Normals.Add(normal)
        Dim material As Material = New DiffuseMaterial( _
            New SolidColorBrush(Colors.Yellow))
        Dim model As GeometryModel3D = New GeometryModel3D( _
            mesh, material)
        Dim group As Model3DGroup = New Model3DGroup()
        group.Children.Add(model)

        If wireframeCheckBox.IsChecked Then
            Dim wireframe As _3DTools.ScreenSpaceLines3D = New _3DTools.ScreenSpaceLines3D
            wireframe.Points.Add(p0)
            wireframe.Points.Add(p1)
            wireframe.Points.Add(p2)
            wireframe.Points.Add(p0)
            wireframe.Color = Colors.Red
            wireframe.Thickness = 3
            view3d.Children.Add(wireframe)
        End If

        Return group
    End Function

    Private Function calculateNormal(ByVal p0 As Point3D, ByVal p1 As Point3D, ByVal p2 As Point3D) As Vector3D
        Dim v0 As Vector3D = New Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z)
        Dim v1 As Vector3D = New Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z)
        Return Vector3D.CrossProduct(v0, v1)
    End Function

    Private Function generateTophography(ByVal expr As MathExpression, _
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

        Dim maxheight = Integer.MinValue

        For i As Integer = 0 To density - 1
            y = ymin
            For j As Integer = 0 To density - 1
                points(counter) = New Point3D(i * scale - 5, j * scale - 5, expr.eval(x, y))
                If Math.Abs(points(counter).Z) > maxheight Then
                    maxheight = Math.Abs(points(counter).Z)
                End If
                y = y + (ymax - ymin) / (density - 1)
                counter = counter + 1
            Next
            x = x + (xmax - xmin) / (density - 1)
        Next

        logLine("Maximum value on the plot: " & Str(maxheight))

        ' Rescale height
        For v = 0 To points.Length - 1
            points(v).Z = 4.0 * points(v).Z / maxheight
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

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles startButton.Click
        logLine("Starting optimization...")

    End Sub

    Private Sub presetsComboBox_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles presetsComboBox.SelectionChanged
        Select Case presetsComboBox.SelectedIndex
            Case 0
                functionTextBox.Text = " -(100.0*(y-x*x)*(y-x*x) + (1.0-x)*(1.0-x)) "
                xminTextBox.Text = "-2.5"
                xmaxTextBox.Text = "-2.5"
                yminTextBox.Text = "2.5"
                ymaxTextBox.Text = "2.5"
        End Select
    End Sub

    Private Sub view3d_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        'logLine(camera.Transform.ToString())
    End Sub

    Private Sub resetButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles resetButton.Click
        clearPlot()
    End Sub

    Private Sub clearPlot()
        logLine("Viewport area has been cleared")
        Dim i As Integer = view3d.Children.Count - 1
        While i >= 0
            Dim m As ModelVisual3D = view3d.Children(i)
            If Not TypeOf m.Content Is DirectionalLight Then
                view3d.Children.RemoveAt(i)
            End If
            i = i - 1
        End While
    End Sub

    Private Sub plotButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles plotButton.Click
        clearPlot()

        Dim mesh As Model3DGroup = New Model3DGroup()

        Dim expr As MathExpression = New MathExpression(functionTextBox.Text)
        Dim points() As Point3D = generateTophography(expr, Int(densityTextBox.Text))

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

        'mesh.Children.Add(createTriangle(0, 0, 0, 1, 1, 0, 0, 1.2, 0))

        Dim model As ModelVisual3D = New ModelVisual3D()
        model.Content = mesh
        view3d.Children.Add(model)
    End Sub
End Class
