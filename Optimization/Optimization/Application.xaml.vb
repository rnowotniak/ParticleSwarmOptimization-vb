Imports System.Threading
Imports System.Globalization

Class Application

    Public presets As Dictionary(Of String, Preset) = New Dictionary(Of String, Preset)

    Public Shared instance As Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Public Sub New()
        instance = Me

        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US", False)

        addPresets()

    End Sub

    Private Sub addPreset(ByVal name As String, ByVal func As String, _
          Optional ByVal xmin As Double = -5, Optional ByVal xmax As Double = 5, _
          Optional ByVal ymin As Double = -5, Optional ByVal ymax As Double = 5, _
          Optional ByVal density As Integer = 10)

        presets.Add(name, _
            New Preset(name, func, xmin, xmax, ymin, ymax, density))
    End Sub

    Private Sub addPresets()
        addPreset("De Jong's function", "-(x*x+y*y)")
        addPreset("Axis parallel hyper-ellipsoid function", "-(x*x+2*y*y)")
        addPreset("Rotated hyper-ellipsoid function", "-(x*x+x*x+y*y)")
        addPreset("Moved axis parallel hyper-ellipsoid function", "-(5*x*x + 5*2*y*y)")
        addPreset("Rosenbrock's valley", "-(100*(y-x*x)*(y-x*x)+(1-x)*(1-x))", -2, 2, -2, 2)
        addPreset("Rastrigin's function", "-(10*2+ (x*x-10*cos(2*pi*x)) + (y*y-10*cos(2*pi*y)))", _
                  -1, 1, -1, 1, 20)
        addPreset("Schwefel's function", "-(-x*sin(sqrt(abs(x))) -y*sin(sqrt(abs(y))))", 100, 600, 400, 800, 20)
        addPreset("Griewangk's function", "-(x*x/4000+y*y/4000 - cos(x)*cos(y/sqrt(2)) +1)", -5, 5, -5, 5, 20)
        addPreset("Sum of different power function", "-(abs(x)*abs(x) + abs(y)*abs(y)*abs(y))", -1, 1, -1, 1)
        addPreset("Ackley's Path function", _
                  "-(-20.0*exp(-0.2*sqrt(1.0/2.0*(x*x+y*y)))-exp(1.0/2.0*(cos(2.0*pi*x)+cos(2.0*pi*y)))+20.0+exp(1))", _
                  -5, 5, -5, 5, 24)
        addPreset("Michalewicz's function", "-(-(sin(x)*pow(sin(x*x/pi),(2*10)) +sin(y)*(pow(sin(2.0*y*y/pi),2*10) )))", _
                  1.5, 2.5, 1, 2, 20)
        addPreset("Branins's rcos function", "-(1*(y-5.1/4/pi*i*x*x +5/pi*x - 6)*(y-5.1/4/pi*i*x*x +5/pi*x - 6) + 10*(1-1/8/pi)*cos(x)+10)")
        addPreset("Easom's function", "-(-cos(x)*cos(y)*exp(-( (x-pi)*(x-pi) + (y-pi)*(y-pi)  )))")
        addPreset("Easom's function 2", "-(-cos(x+5)*cos(y+4)*exp(-( (x+5-pi)*(x+5-pi) + (y+4-pi)*(y+4-pi)  )))")
        addPreset("Six-hump camel back function", "-((4-2.1*x*x+pow(x,(4.0/3.0)))*x*x+x*y+(-4+4*y*y)*y*y)")
        addPreset("Multi criteria function (Rosenbrock+Michalewicz)", " -(100*(y-x*x)*(y-x*x)+(1-x)*(1-x)) + " _
            & " -2000*(-(sin(x/30)*pow(sin((x/30)*(x/30)/pi),(2*10)) +sin(y)*pow(sin(2*(y)*(y)/pi),(2*10)) ))", _
            -2, 2, -2, 2)
        addPreset("Multi criteria function2 (Ackley + Michalewicz)", _
        "-(-(sin(x)*pow(sin(x*x/pi),(2*10)) +sin(y)*pow(sin(2*y*y/pi),(2*10)) ))" _
            & "+  -(-20*exp(-0.2*sqrt(1/2*(x*x+y*y)))-exp(1/2*(cos(2*pi*x)+cos(2*pi*y)))+20+exp(1))", _
            -4, 4, -4, 4)
    End Sub

End Class
