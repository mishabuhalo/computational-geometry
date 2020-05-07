using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Hermite_interpolation
{
    public partial class Form1 : Form
    {
        const int n = 9;
        double[] x = new double[n] {1,2,3,4,5,6,7,9,10};
        double[] y = new double[n] {4,3,2,4,1,4,3,1,2};
        double[] derivative = new double[n];

        List<double> t = new List<double>();
        List<double> result = new List<double>();

        public void computeDerivative()
        {
            derivative[0] = derivative[n - 1] = 0;
            for (int i =1; i < n-1; i++)
            {
                derivative[i] = 0.5 * (((y[i+1]-y[i])/(x[i+1]-x[i]))+((y[i]-y[i-1])/(x[i]-x[i-1])));
            }
        }

        public void computeHermitfunction()
        {
            for(int i = 0; i < n-1; i ++)
            {
                for(double j = x[i]; j<x[i+1]; j+=0.2)
                {
                    double temp = (j - x[i]) / (x[i + 1] - x[i]);
                    
                    t.Add(temp);

                    double res = (2 * Math.Pow(temp, 3) - 3 * temp * temp + 1) * y[i] + (Math.Pow(temp, 3) - 2 * temp * temp+ temp) * (x[i + 1] - x[i]) * derivative[i]
                        + (-2 * Math.Pow(temp, 3) + 3 * temp * temp) * y[i + 1] + (Math.Pow(temp, 3) - temp * temp) * (x[i + 1] - x[i]) * derivative[i + 1];

                    result.Add(res);
                }
            }

            // Видаляю елементи які не потрібні, вони додались, через те що тип double неточний
            result.RemoveAt(5);
            result.RemoveAt(40);
        }

        public Form1()
        {
            InitializeComponent();

            computeDerivative();

            computeHermitfunction();

            DrawGraph();
        }

        public void DrawGraph()
        {
            GraphPane pane = zedGraphControl1.GraphPane;

            pane.CurveList.Clear();

            PointPairList list = new PointPairList();
            int j = 0;
            for(double i = x[0]; i <=x[n-1]; i+=0.2)
            {
                list.Add(i, result[j]);
                j++;
            }

            LineItem myCurve = pane.AddCurve("Hermite", list, Color.Blue, SymbolType.None);

            PointPairList linear = new PointPairList();

            for(int i = 0; i <n; i ++)
            {
                linear.Add(x[i], y[i]);
            }

            LineItem linearCurve = pane.AddCurve("Linear", linear, Color.Red, SymbolType.None);

            pane.AddCurve("", linear, Color.Red, SymbolType.Circle);


            zedGraphControl1.AxisChange();

            zedGraphControl1.Invalidate();
        }

    }
}
