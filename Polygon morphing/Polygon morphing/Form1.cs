using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Threading;

namespace Polygon_morphing
{

    
    public partial class Form1 : Form
    {
        const int size = 3;
        const int steps = 100;
        Graphics draw;
        PointF[] firstPolygon = new PointF[size]{new PointF(100,150), new PointF(200, 300), new PointF(145, 75) };
        PointF[] secondPolygon = new PointF[size] { new PointF(450, 200), new PointF(500, 150), new PointF(450, 400)};

        List<PointF> vectors = new List<PointF>();

        List<double> lengths = new List<double>();

        public void computeLengths()
        {
            for(int i = 0;i < size; i ++)
            {
                lengths.Add(Math.Sqrt(Math.Pow((secondPolygon[i].X - firstPolygon[i].X), 2)+ Math.Pow((secondPolygon[i].Y- firstPolygon[i].Y),2)));
            }
        }

        public void computeNormalizedVectors()
        {
            for(int i = 0; i < size; i ++)
            {
                vectors.Add(new PointF(((float)((secondPolygon[i].X - firstPolygon[i].X)/lengths[i])),(float)(((secondPolygon[i].Y - firstPolygon[i].Y) / lengths[i]))));
            }
        }

        public void drawing()
        {
            Pen blackPen = new Pen(Color.Black);
            Pen redPen = new Pen(Color.Red);

            PointF[] tempPolygon = new PointF[size];
            Array.Copy(firstPolygon, tempPolygon, size);

            for(int i = 0; i<= steps; i++)
            {
                Thread.Sleep(10);
                draw.Clear(Color.White);

                draw.DrawPolygon(blackPen, firstPolygon);
                draw.DrawPolygon(blackPen, secondPolygon);
                draw.DrawPolygon(redPen, tempPolygon);

                for (int j = 0; j < size; j++)
                {
                    tempPolygon[j].X = (float)(tempPolygon[j].X + vectors[j].X * (lengths[j]/steps));
                    tempPolygon[j].Y = (float)(tempPolygon[j].Y + vectors[j].Y * (lengths[j]/steps));
                }
            }
        }

        public void preComputing()
        {
            computeLengths();
            computeNormalizedVectors();
        }

        public Form1()
        {
            InitializeComponent();

            draw = drawingArea.CreateGraphics();
            preComputing();

        }


        private void startBtn_Click(object sender, EventArgs e)
        {
            drawing();
        }
    }
}
