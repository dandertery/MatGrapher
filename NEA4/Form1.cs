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
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using System.Diagnostics;
using System.Runtime;
//https://www.youtube.com/watch?v=TTsyUclt-XU
namespace NEA4
{
    public partial class MatGrapher : Form
    {
        private int functionListNumber = 0; //counting functions 
        private double pitch;
        private double bounds;
        private double fBounds;
        private double renderBounds;
        private double renderFBounds;
        private double aniPitch;
        private double aniPitch2;
        private int steps;
        private bool unitSquareDisplay = false;
        private bool displayGrid = false;
        private bool displayTriangle = false;
        private bool ShearX = true; //Shear X, or Shear Y
        private bool isAnimating;
        private string animationType = null;
        private Matrix StartAniMatrix = new Matrix(1, 0, 0, 1);
        private Matrix AniMatrix;
        private Matrix lMat = new Matrix(-4, 3, 1, -2);
        private Matrix rMat = new Matrix(-4, 3, 1, -2);
        private Matrix QueueMatrix = new Matrix(1, 0, 0, 1);
        private Variable k; // kpq for function input use
        private Variable p;
        private Variable q; 

        private Variable V; // for matrix value input use
        private ObservablePoint[] UnitSquare = new ObservablePoint[45]; // initialising Unit Square graph construct

        private string[] functionArray = { "cos", "sin", "log", "ln", "abs" }; //to determine nature of parsed function tokens
        private string[] operationArray = { "^", "*", "/", "-", "+" }; // to determine nature of parsed operation tokens

        private Stack<Function> fs = new Stack<Function>(); // Stack of functions
        private Queue<Matrix> ms = new Queue<Matrix>(); //FIFO structure for matrix transformations


        struct Coordinate
        {
            public double x;
            public double y;
        }

        struct NamedCoordArray
        {
            public Coordinate[] coordinateArray;
            public string name;
        }
        struct Variable
        {
            public string letter;
            public double value;
        }
        
        struct Function
        {
            public int breakpoints;
            public List<ObservablePoint[]> fSections;
            public string name;
        }
        public MatGrapher()
        {
            InitializeComponent();
            bounds = 10;
            pitch = 0.1;
            V.letter = "V";
            fBounds = bounds / pitch;    
            cartesianChart1.EasingFunction = null; //prevents bouncing animation when graph is initialised
            checkMatrixTimer.Start(); //Clock for updating displayed matrix values
            InitialiseKPQ();          
            DefineUnitSquare();
            UpdateFunctions();
           
        }

        private void InitialiseKPQ() //Initialising variables for use in function definition
        {
            k = new Variable();
            k.letter = "K";
            k.value = double.Parse(kTextbox.Text);
            p = new Variable();
            p.letter = "P";
            p.value = double.Parse(pTextbox.Text);
            q = new Variable();
            q.letter = "Q";
            q.value = double.Parse(qTextbox.Text);
            
        }

        private void DefineUnitSquare()
        {
            UnitSquare[0] = new ObservablePoint(0, 0);
            int b = 1;
            for (double i = 0; i < 10; i++)
            {
                UnitSquare[b] = new ObservablePoint(0, i / 10 + 0.1);
                b++;
            }
            UnitSquare[b] = new ObservablePoint(0, 1);
            b++;
            for (double i = 0; i < 10; i++)
            {
                UnitSquare[b] = new ObservablePoint(i / 10 + 0.1, 1);
                b++;
            }

            UnitSquare[b] = new ObservablePoint(1, 1);
            b++;
            for (double i = 10; i > 0; i--)
            {
                UnitSquare[b] = new ObservablePoint(1, i / 10);
                b++;
            }
            UnitSquare[b] = new ObservablePoint(1, 0);
            b++;
            for (double i = 10; i > 0; i--)
            {
                UnitSquare[b] = new ObservablePoint(i / 10, 0);
                b++;
            }
            UnitSquare[b] = new ObservablePoint(0, 0);
        }

        private void UpdateFunctions() //called upon changes
        {
            functionListNumber = fs.Count;
            UpdateQueueMatrix();
            fs = new Stack<Function>();
            for (int i = 0; i < FunctionList.Items.Count; i++)
            {
                try //try catch prevents crashes, and whilst function is still being typed out by user
                {
                    fs.Push(ToFunction(ApplyMatrix(ProcessInput(FunctionList.Items[i].ToString().Substring(4)), QueueMatrix)));
                }
                catch (Exception ex)
                {

                }

            }
            DisplayFunctions(fs);


        }
        private NamedCoordArray ProcessInput(string input)
        {
            Parsing TreeInput = new Parsing(input);
            TreeNode abstractSyntaxTree = FindRoot(TreeInput.GetTree());

            pitch = 0.1;
            fBounds = bounds / pitch;
            renderBounds = bounds * 10;
            renderFBounds = renderBounds / pitch;
            Coordinate[] coordinates = new Coordinate[Convert.ToInt32(renderFBounds * 2)];


            List<Variable> variableArray = new List<Variable>();
            Variable xTemp = new Variable();
            xTemp.value = 0;
            xTemp.letter = "x";
            variableArray.Add(xTemp);
            KPQ();
            Variable[] kpqArray = new Variable[3];
            kpqArray[0] = k;
            kpqArray[1] = p;
            kpqArray[2] = q;
            for (int i = 0; i < kpqArray.Length; i++)
            {
                variableArray.Add(kpqArray[i]);
            }

            Variable E = new Variable();
            Variable PI = new Variable();
            E.letter = "e";
            E.value = Math.E;
            PI.letter = "𝜋";
            PI.value = Math.PI;
            variableArray.Add(E);
            variableArray.Add(PI);
            double xValue = -renderBounds;

            for (int i = 0; i < (renderFBounds * 2); i++)
            {
                coordinates[i].x = checkForBinaryError(xValue, 6);
                xTemp.value = coordinates[i].x;
                xTemp.letter = "x";
                variableArray[0] = xTemp;

                coordinates[i].y = checkForBinaryError(ProcessTree(abstractSyntaxTree, variableArray), 6); //calculating f(x) for every x
                xValue = xValue + pitch;
            }
            NamedCoordArray output = new NamedCoordArray();
            output.coordinateArray = coordinates;
            output.name = input;
            return output;
        }
        private  Function ToFunction(NamedCoordArray input) //creates Function datatype from coordinates and their function name
        {
            Function function;
            function.breakpoints = 0;
            function.name = "y = " + input.name;
            function.fSections = new List<ObservablePoint[]>();


            List<int> tempLengthList = new List<int>();
            tempLengthList.Add(0);
            int listIndexer = 0;
            bool previousNaN = false;
            Coordinate[] coordinates = input.coordinateArray;

            for (int i = 0; i < (coordinates.Length); i++) //removing undefined coordinates and creating an array of number to  represent consecutive coordinate sequence lengths
            {

                if (double.NaN.Equals(coordinates[i].y) || coordinates[i].y == double.PositiveInfinity || coordinates[i].y == double.NegativeInfinity) //checks if coordinate is undefined
                {

                    
                    coordinates = RemoveCoordinate(coordinates, i);
                    i--;
                    if (!previousNaN)
                    {
                        function.breakpoints++;
                        listIndexer++;
                        tempLengthList.Add(0); // creating new list array for contiguous coordinates to be added to

                    }
                    previousNaN = true;
                }
                else
                {
                    tempLengthList[listIndexer]++;
                    previousNaN = false;
                }
            }



            for (int i = 0; i < tempLengthList.Count; i++)
            {
                if (!(tempLengthList[i] > 0)) //removes empty sequence length counters
                {
                    tempLengthList.Remove(i);
                    function.breakpoints--;
                }

            }
            List<int> arrayLengths = tempLengthList;


            Coordinate[][] CoordinatesArrays = new Coordinate[function.breakpoints + 1][]; //array of coordinate arrays

            CoordinatesArrays[0] = new Coordinate[arrayLengths[0]];
            CoordinatesArrays[0][0] = coordinates[0];
            int c = 1;

            Matrix InverseQueueMatrix = QueueMatrix.Inverse(QueueMatrix);

            for (int i = 0; i < (function.breakpoints + 1); i++)
            {
                if (i > 0)
                {
                    CoordinatesArrays[i] = new Coordinate[arrayLengths[i]];
                }
                while (c < Convert.ToInt32(coordinates.Length) && (lMat.checkForBinaryError(ApplyToCoordinate(coordinates[c], InverseQueueMatrix).x, 3) == lMat.checkForBinaryError(ApplyToCoordinate(coordinates[c - 1], InverseQueueMatrix).x + pitch, 3))) //checks for consecutive x coordinates, seperated by pitch
                {
                    CoordinatesArrays[i][c] = coordinates[c]; //here
                    c++;
                }


            }
            ObservablePoint[][] ValuesArrays = new ObservablePoint[function.breakpoints + 1][];

            for (int i = 0; i < (function.breakpoints + 1); i++)
            {
                ValuesArrays[i] = new ObservablePoint[Convert.ToInt32(CoordinatesArrays[i].Length)];
                for (int z = 0; z < CoordinatesArrays[i].Length; z++)
                {
                    ValuesArrays[i][z] = new ObservablePoint(lMat.checkForBinaryError(CoordinatesArrays[i][z].x, 5), lMat.checkForBinaryError(CoordinatesArrays[i][z].y,5));
                }
            }

            for (int i = 0; i < (function.breakpoints + 1); i++)
            {
                function.fSections.Add(ValuesArrays[i]); // creating each function 'sectionn'
            }
            return function;
        }
        
        private void AddButton_Click(object sender, EventArgs e)
        {
            
            if (RPNTextBox.Text != "")
            {

                RPNTextBox.Text = "";
                functionListNumber++;
            }
            
        }

        private void KPQ()
        {
            try
            {
                k = new Variable();
                k.letter = "K";
                k.value = double.Parse(kTextbox.Text);
            }
            catch
            {

            }
            try
            {
                p = new Variable();
                p.letter = "P";
                p.value = double.Parse(pTextbox.Text);
            }
            catch
            {

            }
            try
            {
                q = new Variable();
                q.letter = "Q";
                q.value = double.Parse(qTextbox.Text);
            }
            catch
            {

            }

        }

        private double ProcessTree(TreeNode inputTree, List<Variable> varinputs) //recursive, top down
        {
            
            int value;
            
            if(inputTree.leftChild == null && inputTree.rightChild == null)
            {
                
                if(int.TryParse(inputTree.token.contents, out value))
                {
                    return (double)value;
                }
                else
                {
                    for (int i = 0; i < varinputs.Count; i++)
                    {
                        if(inputTree.token.contents == varinputs[i].letter)
                        {
                            return varinputs[i].value;
                        }
                    }
                }
            }
            else if (inputTree.leftChild == null)
            {
                double rightValue = ProcessTree(inputTree.rightChild, varinputs);
                int index = -1;  
                for (int i = 0; i < functionArray.Length; i++)
                {
                    if(inputTree.token.contents == functionArray[i])
                    {
                        index = i;
                    }
                }
                if (index == -1)
                {
                    throw new Exception("function not found in list");
                }
                switch (index)
                {
                    case 0:
                        return Math.Cos(rightValue);

                    case 1:
                        return Math.Sin(rightValue);

                    case 2:
                        return Math.Log10(rightValue);

                    case 3:
                        return Math.Log(rightValue);
                    case 4:
                        return Math.Abs(rightValue);

                }   
            }
            else
            {
                double leftValue = ProcessTree(inputTree.leftChild, varinputs);
                double rightValue = ProcessTree(inputTree.rightChild, varinputs);
                int index = -1;
                for (int i = 0; i < operationArray.Length; i++)
                {
                    if (inputTree.token.contents == operationArray[i])
                    {
                        index = i;
                    }
                }

                switch (index)
                {
                    case 0:
                        return Math.Pow(leftValue, rightValue);

                    case 1:
                        return leftValue * rightValue;

                    case 2:
                        return leftValue / rightValue;

                    case 3:
                        return leftValue - rightValue;

                    case 4:
                        return leftValue + rightValue;

                }
            }
            return -1;
            
        }

        private NamedCoordArray ApplyMatrix(NamedCoordArray cInput, Matrix input)
        {
            Coordinate[] result = cInput.coordinateArray;
            for (int i = 0; i < cInput.coordinateArray.Length; i++)
            {
                result[i] = ApplyToCoordinate(result[i], input);
            }
            NamedCoordArray output = new NamedCoordArray();
            output.coordinateArray = result;
            output.name = cInput.name;
            return output;
        }
        
        private Coordinate ApplyToCoordinate(Coordinate inputCoordinate, Matrix inputMatrix)
        {
            double x;
            double y;
            x = ((double)inputCoordinate.x * inputMatrix.Get("a")) + ((double)inputCoordinate.y * inputMatrix.Get("b"));
            y = ((double)inputCoordinate.x * inputMatrix.Get("c")) + ((double)inputCoordinate.y * inputMatrix.Get("d"));
            Coordinate outputCoordinate = new Coordinate();
            outputCoordinate.x = x;
            outputCoordinate.y = y;
            return outputCoordinate;
        }
        private ObservablePoint ApplyToObservablePoint(ObservablePoint inputCoordinate, Matrix inputMatrix)
        {
            double x;
            double y;
            x = ((double)inputCoordinate.X * inputMatrix.Get("a")) + ((double)inputCoordinate.Y * inputMatrix.Get("b"));
            y = ((double)inputCoordinate.X * inputMatrix.Get("c")) + ((double)inputCoordinate.Y * inputMatrix.Get("d"));
            ObservablePoint outputCoordinate = new ObservablePoint();
            outputCoordinate.X = x;
            outputCoordinate.Y = y;
            return outputCoordinate;
        }
        private ObservablePoint[][] CutFunctionToBounds(ObservablePoint[] input)
        {
            List<ObservablePoint[]> result = new List<ObservablePoint[]>();
            ObservablePoint[] temp = input;
            List<ObservablePoint> addTo = new List<ObservablePoint>();
            int w = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if(temp[i].X > bounds || temp[i].X < -bounds || input[i].Y > bounds || temp[i].Y < -bounds)
                {
                    if(addTo.Count > 0)
                    {
                        result.Add(OPListToArray(addTo));
                        addTo = new List<ObservablePoint>();
                    }
                    
                }
                else
                {
                    addTo.Add(temp[i]);
                }


            }
            result.Add(OPListToArray(addTo));
            ObservablePoint[][] output = new ObservablePoint[result.Count][];
            for (int i = 0; i < result.Count; i++)
            {
                output[i] = result[i];
            }
            return output;
        }
        private void DisplayFunctions(Stack<Function> fsinput) //64 functions! (WHY CAN'T I FIX THIS) all rage about this expressed here -> AHHG=GGGHGHGHGHGHHG ahahdhfgfggfgfgfggdf eilfjh\esklfjhhnse\lkj adhhshshdshdh hdidkifhfghgeh ahaujsidfhfidhe aoaoaoaoao
        {
            //unit Square

            pitch = 0.1;
            fBounds = bounds / pitch;

            int implementedMemory = 64;
            ObservablePoint[] displayUnitSquare = new ObservablePoint[1];
            Stack<Function> fscopy = new Stack<Function>(fsinput);
            int bpcount = 0;

            for (int i = 0; i < fscopy.Count; i++)
            {
                Function f = fscopy.Pop();
                bpcount = bpcount + f.breakpoints+1;

            }
            //bpcount--;

            if (bpcount < implementedMemory)
            {
                ObservablePoint[][] displayLines = new ObservablePoint[implementedMemory][];
                fscopy = new Stack<Function>(fsinput);
                int w = 0;
                
                for (int i = 0; i < fsinput.Count; i++)
                {
                    Function f = fscopy.Pop();
                    for (int z = 0; z < (f.breakpoints + 1); z++)
                    {
                        ObservablePoint[][] temp = CutFunctionToBounds(f.fSections[z]);
                        for (int d = 0; d < temp.Length; d++)
                        {
                            displayLines[w] = temp[d];
                            w++;
                        }
                        
                    }
                }
                
                if(unitSquareDisplay)
                {
                    displayUnitSquare = new ObservablePoint[UnitSquare.Length];
                    for (int i = 0; i < UnitSquare.Length; i++)
                    {
                        displayUnitSquare[i] = UnitSquare[i];
                    }
                    for (int i = 0; i < UnitSquare.Length; i++)
                    {
                        displayUnitSquare[i] = ApplyToObservablePoint(displayUnitSquare[i], QueueMatrix);
                    }
                    displayUnitSquare = CutFunctionToBounds(displayUnitSquare)[0];
                }
                
                
                double[] xcoordinates = new double[Convert.ToInt32(fBounds * 2)];
                double[] ycoordinates = new double[Convert.ToInt32(fBounds * 2)];


                double xValue = -bounds;
                double yValue = -bounds;

                for (int i = 0; i < xcoordinates.Length; i++)
                {
                    xcoordinates[i] = xValue;
                    xValue = xValue + pitch;
                }
                for (int i = 0; i < ycoordinates.Length; i++)
                {
                    ycoordinates[i] = yValue;
                    yValue = yValue + pitch;
                }
                ObservablePoint[] XAxis = new ObservablePoint[Convert.ToInt32(fBounds * 2)];
                for (int i = 0; i < fBounds * 2; i++)
                {
                    XAxis[i] = new ObservablePoint(xcoordinates[i], 0);
                }
                ObservablePoint[] YAxis = new ObservablePoint[Convert.ToInt32(fBounds * 2)];
                for (int i = 0; i < fBounds * 2; i++)
                {
                    YAxis[i] = new ObservablePoint(0, ycoordinates[i]);
                }
                ObservablePoint[] UpperBoundSet = new ObservablePoint[1];
                UpperBoundSet[0] = new ObservablePoint(bounds, bounds);
                ObservablePoint[] LowerBoundSet = new ObservablePoint[1];
                LowerBoundSet[0] = new ObservablePoint(-bounds, -bounds);
                //unit Square

                cartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<ObservablePoint>
                    {
                        Values = UpperBoundSet,
                        Fill = null,
                        GeometrySize = 0.1f,

                        Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 0 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values =  LowerBoundSet,
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 0 }

                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values =  displayUnitSquare,
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 3 }

                    },
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
                        Values = displayLines[0],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[1],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[2],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[3],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[4],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[5],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[6],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[7],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[8],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[9],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[10],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[11],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[12],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[13],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[14],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[15],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[16],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[17],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[18],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[19],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[20],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[21],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[22],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[23],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[24],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[25],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[26],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[27],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[28],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[29],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[30],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[31],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[32],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[33],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[34],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[35],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[36],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[37],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[38],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[39],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[40],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[41],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[42],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[43],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[44],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[45],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[46],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[47],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[48],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[49],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[50],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[51],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[52],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[53],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[54],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[55],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[56],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[57],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[58],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[59],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[60],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[61],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[62],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[63],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 }
                    },
                }; //64


            }
            else
            {
                MessageBox.Show("The number of function sections has exceeded the memory", "Function Memory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }
        
        

        private Coordinate[] RemoveCoordinate(Coordinate[] input, int index)
        {
            Coordinate[] output = new Coordinate[input.Length-1];
            for (int i = 0; i < index; i++)
            {
                output[i] = input[i];
            }
            for (int i = index; i < (input.Length-1); i++)
            {
                output[i] = input[i + 1];
            }
            return output;
        }
        private ObservablePoint[] RemoveObservablePoint(ObservablePoint[] input, int index)
        {
            ObservablePoint[] output = new ObservablePoint[input.Length - 1];
            for (int i = 0; i < index; i++)
            {
                output[i] = input[i];
            }
            for (int i = index; i < (input.Length - 1); i++)
            {
                output[i] = input[i + 1];
            }
            return output;
        }

        private void a1_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(a1, "a", lMat);

        }

        private void b1_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(b1, "b", lMat);
        }

        private void c1_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(c1, "c", lMat);
        }

        private void d1_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(d1, "d", lMat);
        }

        private void a2_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(a2, "a", rMat);
        }

        private void b2_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(b2, "b", rMat);
        }

        private void c2_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(c2, "c", rMat);
        }

        private void d2_TextChanged(object sender, EventArgs e)
        {
            TextboxChanged(d2, "d", rMat);
        }
        private double ParseV(string text)
        {
            try
            {
                List<Variable> variableArray = new List<Variable>();
                variableArray.Add(V);
                Parsing valueParser = new Parsing(text);
                TreeNode abstractSyntaxTree = FindRoot(valueParser.GetTree());
                double value = checkForBinaryError(ProcessTree(abstractSyntaxTree, variableArray), 6);
                return value;
            }
            catch(Exception ex)
            {
                return double.NegativeInfinity;
            }
        }
        private void TextboxChanged(TextBox textBox, string letter, Matrix inputMatrix)
        {
            double value = ParseV(textBox.Text);
            if (value == double.NegativeInfinity || CheckForFloatingPoint(textBox.Text) || (textBox.Text == SixFigText(inputMatrix.Get(letter).ToString())))
            {

            }
            else
            {
                inputMatrix.requestChange(letter, value.ToString());
                if (!ContainsV(textBox.Text))
                {
                    textBox.Text = checkForBinaryError(inputMatrix.Get(letter), 6).ToString();
                }
            }






        }
        private bool ContainsV(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if(input[i].ToString() == "V")
                {
                    return true;
                }
            }
            return false;
        }
        private void MultiplyRightButton_Click(object sender, EventArgs e)
        {
            rMat = rMat.Multiplication(lMat, rMat);
        }

        private void fTimer_Tick(object sender, EventArgs e)
        {

        }

        private bool CheckForFloatingPoint(string input)
        {
            if(input.Length == 0 || input[input.Length - 1] == '.')
            {
                return true;
            }
            return false;
        }

        private void checkMatrixTimer_Tick(object sender, EventArgs e)
        {
            CheckMatrix();
        }
        private void CheckMatrixValue(TextBox textBox, string letter, Matrix inputMatrix)
        {
            if (!CheckForFloatingPoint(textBox.Text))
            {
                if(SixFigText(ParseV(textBox.Text).ToString()) != SixFigText(inputMatrix.Get(letter).ToString()))
                {
                    textBox.Text = SixFigText(inputMatrix.Get(letter).ToString());
                }
                
            }
        }
        private void CheckMatrix()
        {
            if(lMat.getDet() == 0) //if determinant is 0 a matrix has no inverse
            {
                InverseLeft.Enabled = false;
            }
            else
            {
                InverseLeft.Enabled = true;
            }
            if (rMat.getDet() == 0)
            {
                InverseRight.Enabled = false;
            }
            else
            {
                InverseLeft.Enabled = true;
            }
            CheckMatrixValues();
            if (rMat.GetStringType() == "unknown" || rMat.GetStringType() == "reflection" || isAnimating) //reflection cannot be animated
            {
                AnimateButton.Enabled = false;
            }
            else
            {
                AnimateButton.Enabled = true;
            }

            if(rMat.GetStringType() == "rotation")
            {
                if(a2.Text != "cos(V)" || b2.Text != "-sin(V)" || c2.Text != "sin(V)" || d2.Text != "cos(V)")
                {
                    AnimateButton.Enabled = false; 
                }
            }

            detA.Text = SixFigText(lMat.getDet().ToString());
            detB.Text = SixFigText(rMat.getDet().ToString());




            if (MatrixList.Items.Count == 0)
            {
                InvLine1TextBox.Text = "";
                InvLine2TextBox.Text = "";
                LOfInvPointsTextBox.Text = "";
            }
            else
            {
                InvLine1TextBox.Text = QueueMatrix.GetInvLine1();
                InvLine2TextBox.Text = QueueMatrix.GetInvLine2();
                LOfInvPointsTextBox.Text = QueueMatrix.GetInvPointLine();
                EigenValue1TextBox.Text = QueueMatrix.GetEigenValue1().ToString();
                EigenValue2TextBox.Text = QueueMatrix.GetEigenValue2().ToString();
                EV1A.Text = QueueMatrix.GetEV1A().ToString();
                EV1B.Text = QueueMatrix.GetEV1B().ToString();
                EV2A.Text = QueueMatrix.GetEV2A().ToString();
                EV2B.Text = QueueMatrix.GetEV2B().ToString();
            }
        }

        private void CheckMatrixValues()
        {
            CheckMatrixValue(a1, "a", lMat);
            CheckMatrixValue(b1, "b", lMat);
            CheckMatrixValue(c1, "c", lMat);
            CheckMatrixValue(d1, "d", lMat);

            CheckMatrixValue(a2, "a", rMat);
            CheckMatrixValue(b2, "b", rMat);
            CheckMatrixValue(c2, "c", rMat);
            CheckMatrixValue(d2, "d", rMat);
        }
        private string SixFigText(string input)
        {
            bool foundPoint = false;
            int z = 6;
            int index = input.Length - 1;
            for (int i = 0; i < input.Length; i++)
            {
                if(input[i] == '.')
                {
                    foundPoint = true;
                    
                }
                if(foundPoint)
                {
                    z--;
                }
                if(z == 0)
                {
                    index = i;
                }
            }
            string output = input.Remove(index + 1);
            return output;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Clear()
        {
            FunctionList.Items.Clear();
            fs = new Stack<Function>();
            UpdateFunctions();
        }

        private void Remove()
        {
            if (FunctionList.SelectedItems.Count == 1)
            {
                string functionName = FunctionList.SelectedItem.ToString();
                FunctionList.Items.Remove(FunctionList.SelectedItem);

                Stack<Function> stackTemp = new Stack<Function>();
                for (int i = 0; i < fs.Count; i++)
                {
                    Function f = fs.Pop();
                    if (f.name == functionName) //could improve by only removing one of a function, but why would someone input two of a function?
                    {

                    }
                    else
                    {
                        stackTemp.Push(f);
                    }
                }
                for (int i = 0; i < stackTemp.Count; i++)
                {
                    fs.Push(stackTemp.Pop());
                }
            }
            functionListNumber--;
            UpdateFunctions();
        }
        private TreeNode FindRoot(TreeNode input)
        {
            TreeNode output = input;
            if(input.leftChild == null && input.rightChild == null)
            {
                if(input.token.tree != null)
                {
                    output = FindRoot(input.token.tree[0]);
                    return output;
                }
                else
                {
                    return output;
                }

                
            }
            else
            {
                
                return output;
            }
        }
        private void switchMatrices()
        {
            Matrix temp = rMat;
            rMat = lMat;
            lMat = temp;
        }
        public double checkForBinaryError(double input, int sigFig)
        {
            string counter = input.ToString();
            if(counter.Length > sigFig)
            {
                double scalar = Math.Pow(10, sigFig);
                double scaled = input * scalar;
                double floor = Math.Floor(scaled);
                double ceiling = Math.Ceiling(scaled);
                if (scaled - floor < 0.1)
                {
                    return floor / scalar;
                }
                else if (ceiling - scaled < 0.1)
                {
                    return ceiling / scalar;
                }
                double output = scaled / scalar;
                return output;
            }
            else
            {
                return input;
            }



        }

        private Matrix OnInverseClick(Matrix input)
        {
            return input.Inverse(input);
        }
        private Matrix OnTransposeClick(Matrix input)
        {
            return input.Transpose(input);
        }

        private void SwitchButton_Click(object sender, EventArgs e)
        {
            switchMatrices();
        }

        private void InverseLeft_Click(object sender, EventArgs e)
        {
            if(lMat.getDet() != 0)
            {
                lMat = OnInverseClick(lMat);

            }
            
        }

        private void InverseRight_Click(object sender, EventArgs e)
        {
            if (rMat.getDet() != 0)
            {
                rMat = OnInverseClick(rMat);

            }
            
        }

        private void TransposeLeft_Click(object sender, EventArgs e)
        {
            lMat = OnTransposeClick(lMat);
            string temp = c1.Text;
            c1.Text = b1.Text;
            b1.Text = temp;
            CheckMatrixValues();
        }

        private void TransposeRight_Click(object sender, EventArgs e)
        {
            rMat = OnTransposeClick(rMat);
            string temp = c2.Text;
            c2.Text = b2.Text;
            b2.Text = temp;
            CheckMatrixValues();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            Remove();
        }

        private void cartesianChart1_Load(object sender, EventArgs e)
        {

        }
        private ObservablePoint[] OPListToArray(List<ObservablePoint> input) // List To Array converter for Observable Point Lists/Arrays
        {
            ObservablePoint[] output = new ObservablePoint[input.Count];
            for (int i = 0; i < input.Count; i++)
            {
                output[i] = input[i];
            }
            return output;
        }
        private Variable[] ListToArray(List<Variable> input)
        {
            Variable[] output = new Variable[input.Count];
            for (int i = 0; i < input.Count; i++)
            {
                output[i] = input[i];
            }
            return output;
        }

        private List<Variable> ArrayToList(Variable[] input)
        {
            List<Variable> output = new List<Variable>();
            for (int i = 0; i < input.Length; i++)
            {
                output.Add(input[i]);
            }
            return output;
        }
        private void ApplyMatrixButton_Click(object sender, EventArgs e)
        {
            ms.Enqueue(rMat);

            MatrixList.Items.Add(rMat.getName());
            UpdateFunctions();
        }

        private void UpdateQueueMatrix()
        {
            QueueMatrix = new Matrix(1, 0, 0, 1);
            Queue<Matrix> msCopy = new Queue<Matrix>(ms);
            for (int i = 0; i < ms.Count; i++)
            {
                QueueMatrix = QueueMatrix.Multiplication(msCopy.Dequeue(), QueueMatrix);
            }

        }

        private void BoundsTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bounds = double.Parse(BoundsTextBox.Text);
                pitch = bounds / 100;
                fBounds = bounds / pitch;
                UpdateFunctions();
            }
            catch (Exception)
            {

                BoundsTextBox.Text = "";
            }
        }

        private void RPNTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (RPNTextBox.Text != "")
                {
                    string RPNinput = RPNTextBox.Text;
                    if (FunctionList.Items.Count < functionListNumber + 1)
                    {
                        FunctionList.Items.Add("y = " + RPNinput);
                    }
                    else
                    {
                        FunctionList.Items[functionListNumber] = ("y = " + RPNinput);
                    }                  
                    UpdateFunctions();
                }
            }
            catch (Exception)
            {


            }
        }

        private void RemoveMatrix_Click(object sender, EventArgs e)
        {
            if (MatrixList.SelectedItems.Count == 1)
            {
                string matrixName = MatrixList.SelectedItem.ToString();
                MatrixList.Items.Remove(MatrixList.SelectedItem);
                Queue<Matrix> queueTemp = new Queue<Matrix>();
                for (int i = 0; i < ms.Count; i++)
                {
                    Matrix m = ms.Dequeue();
                    if (m.getName() == matrixName) //could improve by only removing one of a function, but why would someone input two of a function?
                    {

                    }
                    else
                    {
                        queueTemp.Enqueue(m);
                    }
                }
                ms = queueTemp;
            }
            UpdateFunctions();
        }

        private void ClearMatrix_Click(object sender, EventArgs e)
        {
            MatrixList.Items.Clear();
            ms = new Queue<Matrix>();
            UpdateFunctions();
        }

        private void AnimateButton_Click(object sender, EventArgs e)
        {
            AnimateButton.Enabled = false;
            if(MatrixList.Items.Count == 0)
            {

                AniMatrix = StartAniMatrix;
                
                animationType = rMat.GetStringType();
                steps = 150;
                
                if (animationType == "rotation")
                {
                    steps = 100;
                    aniPitch = V.value / steps;
                                                       
                    V.value = 0;
                }
                else if (animationType == "reflection")
                {

                }
                else if (animationType == "stretchlargement")
                {
                    aniPitch = ((rMat.Get("a") - 1) / steps); //stretch animation in x
                    aniPitch2 = ((rMat.Get("d") - 1) / steps); //stretch animation in y
                }
                else if (animationType == "shear") //determining shear nature
                {
                    if(rMat.Get("b") == 0)
                    {
                        aniPitch = (rMat.Get("c") / steps);
                    }
                    else
                    {
                        aniPitch = (rMat.Get("b") / steps);
                    }
                }
                isAnimating = true;
                AnimateTimer.Start();


            }
            else
            {
                MessageBox.Show( "Please empty the Matrix List to animate", "Animation cannot be shown", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void kRadio_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void kTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateFunctions();
        }

        private void RPNInputLabel_Click(object sender, EventArgs e)
        {

        }

        private void pTextbox_TextChanged(object sender, EventArgs e)
        {
            UpdateFunctions();
        }



        private void FunctionList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void qTextbox_TextChanged(object sender, EventArgs e)
        {
            UpdateFunctions();
        }
        private void pRadio_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void qRadio_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void ReflectionButton_Click(object sender, EventArgs e)
        {
            V.value = 0.785;
            vTextBox.Text = 0.785.ToString();
            a2.Text = "cos(2V)";
            b2.Text = "sin(2V)";
            c2.Text = "sin(2V)";
            d2.Text = "-cos(2V)";
        }

        private void RotationButton_Click(object sender, EventArgs e)
        {
            V.value = 1.57;
            vTextBox.Text = 1.57.ToString();
            a2.Text = "cos(V)";
            b2.Text = "-sin(V)";
            c2.Text = "sin(V)";
            d2.Text = "cos(V)";
        }

        private void EnlargementButton_Click(object sender, EventArgs e)
        {
            V.value = 2;
            vTextBox.Text = 2.ToString();
            a2.Text = "V";
            b2.Text = "0";
            c2.Text = "0";
            d2.Text = "V";

        }

        private void ShearingButton_Click(object sender, EventArgs e)
        {
            if(ShearX)
            {
                ShearX = !ShearX;
                V.value = 3;
                vTextBox.Text = 3.ToString();
                a2.Text = "1";
                b2.Text = "V";
                c2.Text = "0";
                d2.Text = "1";
            }
            else
            {
                ShearX = !ShearX;
                V.value = 3;
                vTextBox.Text = 3.ToString();
                a2.Text = "1";
                b2.Text = "0";
                c2.Text = "V";
                d2.Text = "1";
            }

        }



        private void GridButton_Click(object sender, EventArgs e)
        {
            displayGrid = !displayGrid;
            UpdateFunctions();
        }

        private void FunctionButton_Click(object sender, EventArgs e)
        {

        }

        private void BitmapButton_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void InvariantLinesButton_Click(object sender, EventArgs e)
        {

            UpdateQueueMatrix();
            string InvLine1 = QueueMatrix.GetInvLine1();
            string InvLine2 = QueueMatrix.GetInvLine2();
            FunctionList.Items.Add(InvLine1);
            FunctionList.Items.Add(InvLine2);
            functionListNumber = functionListNumber + 2;
            UpdateFunctions();
        }

        private void LinesOfInvariantPointsButton_Click(object sender, EventArgs e)
        {
            UpdateQueueMatrix();
            string InvPointLine = QueueMatrix.GetInvPointLine();
            FunctionList.Items.Add(InvPointLine);
            functionListNumber++;
            UpdateFunctions();
        }

        private void cosButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "cos()";
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void piButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "𝜋";
        }

        private void eButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "e";
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void sinButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "sin()";
        }



        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void UnitSquareButton_Click(object sender, EventArgs e)
        {
            unitSquareDisplay = !unitSquareDisplay;
            UpdateFunctions();
        }

        private void vTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                V.value = Double.Parse(vTextBox.Text);

                TextboxChanged(a1, "a", lMat);
                TextboxChanged(b1, "b", lMat);
                TextboxChanged(c1, "c", lMat);
                TextboxChanged(d1, "d", lMat);
                TextboxChanged(a2, "a", rMat);
                TextboxChanged(b2, "b", rMat);
                TextboxChanged(c2, "c", rMat);
                TextboxChanged(d2, "d", rMat);
            }
            catch
            {

            }


        }

        private void AnimateTimer_Tick(object sender, EventArgs e)
        {
            if (animationType == "rotation")
            {
               
                V.value = V.value + aniPitch;
                vTextBox.Text = V.value.ToString();
                AniMatrix.requestChange("a", (Math.Cos(V.value).ToString()));
                AniMatrix.requestChange("b", (Math.Sin(V.value).ToString()));
                AniMatrix.requestChange("c", ((Math.Sin(V.value)*-1).ToString()));
                AniMatrix.requestChange("d", (Math.Cos(V.value).ToString()));
            }
            else if (animationType == "reflection")
            {

            }
            else if (animationType == "stretchlargement")
            {
                AniMatrix.requestChange("a", (AniMatrix.Get("a") + aniPitch).ToString());
                AniMatrix.requestChange("d", (AniMatrix.Get("d") + aniPitch2).ToString());
            }
            else if (animationType == "shear")
            {
                if (AniMatrix.Get("b") == 0)
                {
                    AniMatrix.requestChange("c", (AniMatrix.Get("c") + aniPitch).ToString());
                }
                else
                {
                    AniMatrix.requestChange("b", (AniMatrix.Get("b") + aniPitch).ToString());
                }
            }
            steps--;

            if(steps == 0)
            {
                AnimateTimer.Enabled = false;
                isAnimating = false;
            }
            rMat = AniMatrix;
            ms = new Queue<Matrix>();
            ms.Enqueue(AniMatrix);
            UpdateFunctions();
        }

        private void TriangleButton_Click(object sender, EventArgs e)
        {
            displayTriangle = !displayTriangle;
            UpdateFunctions();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        //private double ProcessRPN(string input, double xInput)
        //{
        //    Stack variableStack = new Stack();

        //    foreach (char c in input)
        //    {
        //        if ((int)c == 120)
        //        {
        //            variableStack.Push(xInput);
        //        }
        //        else if ((int)c > 47 && (int)c < 58) // 0 - 9
        //        {
        //            variableStack.Push((double)(c));
        //        }
        //        else // if((int)c > 96 && (int)c <= 122
        //        {
        //            double temp;
        //            double temp2;
        //            switch (c.ToString())
        //            {
        //                case "a":
        //                    temp = (double)variableStack.Pop();
        //                    variableStack.Push(abs(temp));
        //                    break;
        //                case "c":+;
        //                    variableStack.Push(cos(temp));
        //                    break;
        //                case "s":
        //                    temp = (double)variableStack.Pop();
        //                    variableStack.Push(sin(temp));
        //                    break;
        //                case "^":
        //                    temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
        //                    temp2 = (double)variableStack.Pop();
        //                    variableStack.Push(power(temp2, temp));
        //                    break;
        //                case "+":
        //                    temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
        //                    temp2 = (double)variableStack.Pop();
        //                    variableStack.Push(add(temp, temp2));
        //                    break;
        //                case "-":
        //                    temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
        //                    temp2 = (double)variableStack.Pop();
        //                    variableStack.Push(sub(temp2, temp));
        //                    break;
        //                case "*":
        //                    temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
        //                    temp2 = (double)variableStack.Pop();
        //                    variableStack.Push(mult(temp, temp2));
        //                    break;
        //                case "/":

        //                    temp = (double)variableStack.Pop(); // NEED TO check theres enough variables (2)
        //                    temp2 = (double)variableStack.Pop();
        //                    variableStack.Push(div(temp2, temp));
        //                    break;
        //                case "p":
        //                    variableStack.Push(Math.PI);
        //                    break;
        //                case "e":
        //                    variableStack.Push(Math.E);
        //                    break;

        //            }
        //        }

        //    }

        //    return (double)variableStack.Pop();

        //}
    }
}