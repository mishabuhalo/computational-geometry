using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nevron.GraphicsCore;
using Nevron.Chart;
using Nevron.Chart.Windows;
using Nevron.Chart.WinForm;
using Nevron.Editors;

namespace Bezier_surface
{
    public partial class Form1 : Form
    {
        List<double> x = new List<double>() { 1, 2, 3, 4, 5, 6, 7, 9 };
        List<double> y = new List<double>() { 4, 3, 2, 4, 1, 4, 3, 1 };
        List<double> z = new List<double>() { 1, 0, 2, 1, 2, 0, 3, 3 };

        List<double> resultX = new List<double>();
        List<double> resultY = new List<double>();
        List<double> resultZ = new List<double>();
        const int n = 2;
        const int m = 4;

        public double factorial(int number)
        {
            double result = 1;
            while (number > 1)
            {
                result = result * number;
                number = number - 1;
            }
            return result;
        }

        public double Bernstein(int first, int second, double third)
        {
            return (factorial(first) / (factorial(second) * factorial(first - second))) * Math.Pow(third, second) * Math.Pow(1 - third, first - second);
        }

        public List<double> Bezier(double value)
        {
            List<double> result = new List<double>();
            for (double u = 0.1; u < 1; u += 0.05)
            {
                for (double v = 0.1; v < 1; v += 0.05)
                {
                    double temp = 0;
                    for (int i = 0; i <= n; i++)
                    {
                        for (int j = 0; j <= m; j++)
                        {
                            temp += Bernstein(n, i, u) * Bernstein(m, j, v) * value;
                        }
                    }
                    result.Add(temp);
                }
            }
            return result;
        }

        public void computation()
        {
            for (int i = 0; i < 8; i++)
                resultX.AddRange(Bezier(x[i]));

            for (int i = 0; i < 8; i++)
                resultY.AddRange(Bezier(y[i]));

            for (int i = 0; i < 8; i++)
                resultZ.AddRange(Bezier(z[i]));
        }

        public void generateSurface()
        {
            NCartesianChart chart;

            //nChartControl1.Clear();
            NTriangulatedSurfaceSeries surface;

            NZoomTool zt = new NZoomTool();
            zt.BeginDragMouseCommand = new NMouseCommand(MouseAction.Wheel, MouseButton.Middle, 0);
            zt.ZoomStep = 20;


            nChartControl1.Controller.Tools.Add(zt);

            nChartControl1.Controller.Tools.Add(new NSelectorTool());
            nChartControl1.Controller.Tools.Add(new NTrackballTool());

            nChartControl1.Settings.JitterMode = JitterMode.Enabled;
            nChartControl1.Settings.JitteringSteps = 1;

            chart = (NCartesianChart)nChartControl1.Charts[0];

            chart.Enable3D = true;
            chart.Width = chart.Height = chart.Depth = 50;
            chart.Projection.SetPredefinedProjection(PredefinedProjection.PerspectiveTilted);

            surface = new NTriangulatedSurfaceSeries();

            chart.Series.Add(surface);

            surface.FrameMode = SurfaceFrameMode.Mesh;

            surface.FillMode = SurfaceFillMode.Zone;

            for(int i = 0; i < resultX.Count(); i ++)
            {
                surface.XValues.Add(resultX[i]);
                surface.ZValues.Add(resultZ[i]);
                surface.Values.Add(resultY[i]);
            }
        }
        public Form1()
        {
            InitializeComponent();

            computation();
            generateSurface();
        }
    }
}
