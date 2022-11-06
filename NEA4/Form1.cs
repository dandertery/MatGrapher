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
        private double forceYBounds;
        struct Coordinate
        {
            public double x;
            public double y;

        }
        public Form1()
        {
            InitializeComponent();
        }

        private double abs(double v) //absolute
        {
            return Math.Abs(v);
        }
        private double cos(double v) 
        {
            return Math.Cos(v);
        }
        private double sin(double v) 
        {
            return Math.Sin(v);
        }
        private double add(double v, double u)
        {
            return v + u;
        }
        private double sub(double v, double u)
        {
            return v - u;
        }
        private double mult(double v, double u)
        {
            return v * u;
        }
        private double div(double v, double u)
        {
            return v / u;
        }

        private double power(double v, double u ) //v^u
        {
            double temp = 1;
            for (int i = 0; i < u; i++)
            {
                temp = temp * v;
            }
            return temp;
            //return Math.Pow(v,u);
        }

        private void DisplayButton_Click(object sender, EventArgs e)
        {
            if(RPNTextBox.Text != "")
            {
                string RPNinput = RPNTextBox.Text;
                pitch = 0.1;
                bounds = 10;
                fBounds = bounds / pitch;
                Coordinate[] coordinates = new Coordinate[Convert.ToInt32(fBounds * 2)];
                double min = -bounds;
                double xValue = min;
                double yMin = 0;
                double yMax = 0;
                forceYBounds = 100000000;

                for (int i = 0; i < (fBounds * 2); i++)
                {
                    coordinates[i].x = xValue;
                    coordinates[i].y = ProcessRPN(RPNinput, coordinates[i].x);
                    if (double.NaN.Equals(coordinates[i].y) || coordinates[i].y == double.PositiveInfinity || coordinates[i].y == double.NegativeInfinity || abs(coordinates[i].y) > forceYBounds )
                    {
                        RemoveCoordinate(coordinates, i);
                        //if (i == 0)
                        //{
                        //    coordinates[i].y = 0;
                        //}
                        //else
                        //{
                        //    coordinates[i].y = coordinates[i - 1].y;
                        //}

                    }
                    if (coordinates[i].y < yMin)
                    {
                        yMin = coordinates[i].y;
                    }
                    if (coordinates[i].y > yMax)
                    {
                        yMax = coordinates[i].y;
                    }
                    xValue = xValue + pitch;


                }


                ObservablePoint[] ValueArray = new ObservablePoint[Convert.ToInt32(fBounds * 2)];
                for (int i = 0; i < fBounds * 2; i++)
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
        }
        
        private double ProcessRPN(string input, double xInput)
        {
            Stack functionStack = new Stack();
            Stack variableStack = new Stack();
            
            foreach (char c in input)
            {
                if((int)c == 120)
                {
                    variableStack.Push(xInput);
                }
                else if((int)c >47 && (int)c <58) // 0 - 9
                {
                    variableStack.Push((double)(c));
                }
                else // if((int)c > 96 && (int)c <= 122
                {
                    double temp;
                    double temp2;
                    switch (c.ToString())
                    {
                        case "a":
                            temp = (double)variableStack.Pop();
                            variableStack.Push(abs(temp));
                            break;
                        case "c":
                            temp = (double)variableStack.Pop();
                            variableStack.Push(cos(temp));
                            break;
                        case "s":
                            temp = (double)variableStack.Pop();
                            variableStack.Push(sin(temp));
                            break;
                        case "^":
                            temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
                            temp2 = (double)variableStack.Pop();
                            variableStack.Push(power(temp2, temp));
                            break;
                        case "+":
                            temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
                            temp2 = (double)variableStack.Pop();
                            variableStack.Push(add(temp, temp2));
                            break;
                        case "-":
                            temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
                            temp2 = (double)variableStack.Pop();
                            variableStack.Push(sub(temp2, temp));
                            break;
                        case "*":
                            temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
                            temp2 = (double)variableStack.Pop();
                            variableStack.Push(mult(temp, temp2));
                            break;
                        case "/":
                            temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
                            temp2 = (double)variableStack.Pop();
                            variableStack.Push(div(temp2, temp));
                            break;
                        case "p":
                            variableStack.Push(Math.PI);
                            break;
                        case "e":
                            variableStack.Push(Math.E);
                            break;

                    }
                }
                
            }

            return (double)variableStack.Pop();

        }

        private Coordinate[] RemoveCoordinate(Coordinate[] input, int index)
        {
            for (int i = index; i < (input.Length-1); i++)
            {
                input[i] = input[i + 1];
            }
            return input;
        }
        
    }
}