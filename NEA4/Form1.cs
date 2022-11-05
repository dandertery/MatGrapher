using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp.Views;
using LiveChartsCore.Defaults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
namespace NEA4
{
    public partial class Form1 : Form
    {
        private double pitch;
        private double bounds;
        private double fBounds;
        struct Coordinate
        {
            public double x;
            public double y;

        }
        public Form1()
        {
            InitializeComponent();

            
            pitch = 0.1;
            bounds = 10;
            fBounds = bounds / pitch;
            Coordinate[] coordinates = new Coordinate[Convert.ToInt32(fBounds * 2)];
            double min = -bounds;
            double xValue = min;
            double yMin = 0;
            double yMax = 0;
            
            for (int i = 0; i < (fBounds * 2); i++)
            {
                coordinates[i].x = xValue;
                coordinates[i].y = Function(coordinates[i].x);
                if (double.NaN.Equals(coordinates[i].y) || coordinates[i].y == double.PositiveInfinity || coordinates[i].y == double.NegativeInfinity)
                {
                    if(i == 0)
                    {
                        coordinates[i].y = 0;
                    }
                    else
                    {
                        coordinates[i].y = coordinates[i - 1].y;
                    }
                    
                }
                if(coordinates[i].y < yMin)
                {
                    yMin = coordinates[i].y;
                }
                if (coordinates[i].y > yMax)
                {
                    yMax = coordinates[i].y;
                }
                xValue = xValue + pitch;


            }
            

            ObservablePoint[] ValueArray = new ObservablePoint[Convert.ToInt32(fBounds*2)];
            for (int i = 0; i < fBounds*2; i++)
            {
                ValueArray[i] = new ObservablePoint(coordinates[i].x, coordinates[i].y);
            }
            ObservablePoint[] XAxis = new ObservablePoint[Convert.ToInt32(fBounds * 2)];
            for (int i = 0; i < fBounds * 2; i++)
            {
                XAxis[i] = new ObservablePoint(coordinates[i].x, 0);
            }
            ObservablePoint[] YAxis = new ObservablePoint[Convert.ToInt32(fBounds * 2)];
            for (int i = 0; i < fBounds * 2; i++)
            {
                YAxis[i] = new ObservablePoint(0, coordinates[i].y);
            }

            cartesianChart1.Series = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = XAxis,
                    Fill = null,
                    GeometrySize = 0.1f,

                    Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 3 }



                },
                new LineSeries<ObservablePoint>
                {
                    Values = YAxis,
                    Fill = null,
                    GeometrySize = 0.1f,
                    Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 3 }




                },
                new LineSeries<ObservablePoint>
                {
                    Values = ValueArray,
                    Fill = null,
                    GeometrySize = 0.1f,
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }


                },


            };

        }

        private double Function(double x)
        {
            //return A(((x-5)*(x+3))); //change later with user input
            //return Math.Pow(Math.E, x);
            //return Math.Sin(x);
            //return Math.Sin(x) / x;
            //double function = Math.Sin(x) / x;
            //double function = Math.Sqrt(x);
            double function = sin(x);
            return function;

            //if (x == 0)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return function;
            //}



        }
        
        private double abs(double v) //absolute
        {
            return Math.Abs(v);
        }
        private double sin(double x) //absolute
        {
            return Math.Sin(x);
        }
        private double cos(double x) //absolute
        {
            return Math.Cos(x);
        }
        private double power(double v, double u ) //v^u
        {
            return Math.Pow(v,u);
        }

        private void DisplayButton_Click(object sender, EventArgs e)
        {
            if(RPNTextBox.Text != "")
            {
                ProcessRPN(RPNTextBox.Text);
            }
        }
        
        private void ProcessRPN(string input)
        {
            Stack st = new Stack();

        }
    }
}