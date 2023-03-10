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
        private int functionListNumber = 1; //counting functions 
        private int steps; //for animation
        private double pitch;
        private double bounds;
        private double fBounds;
        private double renderBounds;
        private double renderFBounds;
        private double aniPitch;
        private double aniPitch2; // for combined X/Y stretch animations
        

        private bool unitSquareDisplay = false;
        private bool displayGrid = false;
        private bool displayTriangle = false;
        private bool ShearX = true; //Shear X, or Shear Y
        private bool isAnimating = false;
        private bool useDegrees = false;
        private bool showEigen = false;

        private string animationType = null;

        private Matrix StartAniMatrix = new Matrix(1, 0, 0, 1); //starts as identity matrix
        private Matrix AniMatrix;
        private Matrix lMat = new Matrix(-4, 3, 1, -2);
        private Matrix rMat = new Matrix(-4, 3, 1, -2);
        private Matrix QueueMatrix = new Matrix(1, 0, 0, 1);

        private Variable k; // kpq for function input use
        private Variable p;
        private Variable q; 
        private Variable V; // for matrix value input use

        Variable E = new Variable();
        Variable PI = new Variable();

        private ObservablePoint[] unitSquare = new ObservablePoint[45];// initialising graph constructs of the Unit Square, Triangle and Grid
        private ObservablePoint[] triangle = new ObservablePoint[34]; //
        private ObservablePoint[][] grid = new ObservablePoint[8][]; //
        private ObservablePoint[] eigenVector1OP = new ObservablePoint[1]; //Eigenvectors to be transformed by the eigenvalues 
        private ObservablePoint[] eigenVector2OP = new ObservablePoint[1]; //
        
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
            pitch = 0.05;
            cartesianChart1.TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.CompareAll;
            fBounds = bounds / pitch;    
            cartesianChart1.EasingFunction = null; //prevents bouncing animation when graph is initialised
            checkMatrixTimer.Start(); //Clock for updating displayed matrix values
            InitialiseVariables();          
            DefineUnitSquare();
            UpdateFunctions(); //called upon every relevant change

           
        }

        private void InitialiseVariables() //Initialising variables for use in function/matrix definition
        {
            k.letter = "K";
            k.value = double.Parse(kTextbox.Text);
            p.letter = "P";
            p.value = double.Parse(pTextbox.Text);
            q.letter = "Q";
            q.value = double.Parse(qTextbox.Text);

            E.letter = "e";
            E.value = Math.E;
            PI.letter = "π";
            PI.value = Math.PI;
            V.letter = "V";

        }
        private void DefineEigenVectors()
        {
            if(showEigen)
            {
                try
                {
                    eigenVector1OP[0] = new ObservablePoint(QueueMatrix.GetEV1A(), QueueMatrix.GetEV1B());

                }
                catch
                {

                }
                try
                {
                    eigenVector2OP[0] = new ObservablePoint(QueueMatrix.GetEV2A(), QueueMatrix.GetEV2B());
                }
                catch
                {

                }
            }
            else
            {
                eigenVector1OP[0] = new ObservablePoint();
                eigenVector2OP[0] = new ObservablePoint();
            }

        }
        private void DefineTriangle()
        {

            
            double x = -4;
            double y = -5;
            triangle[0] = new ObservablePoint(x,y);
            double dx = -2 - x;
            double dy = 3 - y;
            int m = 1;
            for (int i = 0; i < 10; i++)
            {
                x = x + (dx / 10);
                y = y + (dy / 10);
                triangle[m] = new ObservablePoint(x, y);
                m++;
            }
            x = -2;
            y = 3;
            triangle[11] = new ObservablePoint(x,y);
            dx = 6 - x;
            dy = -1 - y;
            m = 12;
            for (int i = 0; i < 10; i++)
            {
                x = x + (dx / 10);
                y = y + (dy / 10);
                triangle[m] = new ObservablePoint(x, y);
                m++;
            }
            x = 6;
            y = -1;
            triangle[22] = new ObservablePoint(x, y);
            dx = -4 - x;
            dy = -5 - y;
            m = 23;
            for (int i = 0; i < 10; i++)
            {
                x = x + (dx / 10);
                y = y + (dy / 10);
                triangle[m] = new ObservablePoint(x, y);
                m++;
            }
            triangle[33] = new ObservablePoint(-4, -5);

        }
        private void DefineUnitSquare()
        {
            unitSquare[0] = new ObservablePoint(0, 0);
            int b = 1;
            for (double i = 0; i < 10; i++)
            {
                unitSquare[b] = new ObservablePoint(0, i / 10 + 0.1);
                b++;
            }
            unitSquare[b] = new ObservablePoint(0, 1);
            b++;
            for (double i = 0; i < 10; i++)
            {
                unitSquare[b] = new ObservablePoint(i / 10 + 0.1, 1);
                b++;
            }

            unitSquare[b] = new ObservablePoint(1, 1);
            b++;
            for (double i = 10; i > 0; i--)
            {
                unitSquare[b] = new ObservablePoint(1, i / 10);
                b++;
            }
            unitSquare[b] = new ObservablePoint(1, 0);
            b++;
            for (double i = 10; i > 0; i--)
            {
                unitSquare[b] = new ObservablePoint(i / 10, 0);
                b++;
            }
            unitSquare[b] = new ObservablePoint(0, 0);
        }
        private void DefineGrid()
        {
            double width = (bounds) / 3;
            double x;
            double y;
            double[] widthArray = new double[] { -width * 2, -width, width, width * 2 }; //defining away from each axis
            for (int i = 0; i < (grid.Length / 2); i++)
            {
                x = widthArray[i];
                y = -bounds * 10;
                grid[i] = new ObservablePoint[100];
                for (int z = 0; z < grid[i].Length; z++)
                {
                    grid[i][z] = new ObservablePoint(x, y);
                    y = y + (bounds / 5);
                    
                }
                
                
            }           
            for (int i = 4; i < (grid.Length); i++)
            {
                x = -bounds * 10;
                y = widthArray[i-4];
                grid[i] = new ObservablePoint[100];
                for (int z = 0; z < grid[i].Length; z++)
                {
                    grid[i][z] = new ObservablePoint(x, y);
                    x = x + (bounds / 5);
                   
                }               
            }
        }

        private void UpdateFunctions() //called upon relevant changes
        {
            
            UpdateQueueMatrix();
            fs = new Stack<Function>();
            for (int i = 0; i < FunctionList.Items.Count; i++)
            {
                try 
                {
                    fs.Push(ToFunction(ApplyMatrix(ProcessInput(FunctionList.Items[i].ToString().Substring(4)), QueueMatrix)));
                }
                catch
                {

                }

            }
            DisplayFunctions(fs);


        }
        private List<Variable> DefineVariableArray()
        {
            List<Variable> vArray = new List<Variable>(); //creating variable array for use in parsing
            Variable x = new Variable();
            x.value = 0;
            x.letter = "x";
            vArray.Add(x);
            KPQ();
            Variable[] kpqArray = new Variable[3];
            kpqArray[0] = k;
            kpqArray[1] = p;
            kpqArray[2] = q;
            for (int i = 0; i < kpqArray.Length; i++)
            {
                vArray.Add(kpqArray[i]);
            }
            vArray.Add(E);
            vArray.Add(PI);
            return vArray;
        }
        private NamedCoordArray ProcessInput(string input) //processing a function
        {
            Parsing TreeInput = new Parsing(input);
            TreeNode abstractSyntaxTree = FindRoot(TreeInput.GetTree());

            pitch = 0.05; 
            fBounds = bounds / pitch;
            renderBounds = bounds * 10;
            renderFBounds = renderBounds / pitch;
            Coordinate[] coordinates = new Coordinate[Convert.ToInt32(renderFBounds * 2)];

            Variable xTemp = new Variable();
            xTemp.value = 0;
            xTemp.letter = "x";

            List<Variable> variableArray = DefineVariableArray();
            double xValue = -renderBounds;

            for (int i = 0; i < (renderFBounds * 2); i++)
            {
                coordinates[i].x = checkForBinaryError(xValue, 6);
                xTemp.value = coordinates[i].x; 
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
                        tempLengthList.Add(0); // creating new counter in the list to count the coordinates that have to be added in each section

                    }
                    previousNaN = true;
                }
                else
                {
                    tempLengthList[listIndexer]++; //adding to the counter
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
            CoordinatesArrays[0][0] = coordinates[0]; //adding first coordinate to first section
            int c = 1;

            Matrix InverseQueueMatrix = QueueMatrix.Inverse(QueueMatrix);
            int f = 1;
            for (int i = 0; i < (function.breakpoints + 1); i++)
            {
                bool startOfSection = true;
                if (i > 0)
                {
                    CoordinatesArrays[i] = new Coordinate[arrayLengths[i]];
                    f = 0;
                }
                while (c < Convert.ToInt32(coordinates.Length) &&(startOfSection || (lMat.checkForBinaryError(ApplyToCoordinate(coordinates[c], InverseQueueMatrix).x, 3) == lMat.checkForBinaryError(ApplyToCoordinate(coordinates[c - 1], InverseQueueMatrix).x + pitch, 3)))) //checks for consecutive x coordinates, seperated by pitch
                {
                    startOfSection = false;
                    CoordinatesArrays[i][f] = coordinates[c]; //here
                    c++;
                    f++;
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
                function.fSections.Add(ValuesArrays[i]); // creating each function 'section'
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

        private void KPQ() //updating KPQ
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
            
            double value;
            
            if(inputTree.leftChild == null && inputTree.rightChild == null) //indicating double value / variable
            {
                
                if(Double.TryParse(inputTree.token.contents, out value))
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
            else if (inputTree.leftChild == null) //indicating function
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
            else //indicating operation
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

        private NamedCoordArray ApplyMatrix(NamedCoordArray cInput, Matrix input) //applying matrix transformation to each coordinate
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
            

            pitch = 0.1;
            fBounds = bounds / pitch;
            int implementedMemory = 64; //number of functions allowed
            ObservablePoint[] displayUnitSquare = new ObservablePoint[1];
            Stack<Function> fscopy = new Stack<Function>(fsinput);
            int bpcount = 0;

            for (int i = 0; i < fscopy.Count; i++)
            {
                Function f = fscopy.Pop();
                bpcount = bpcount + f.breakpoints+1;

            }
            

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
                
                if(unitSquareDisplay) //Defining / transforming Unit Square
                {
                    displayUnitSquare = new ObservablePoint[unitSquare.Length];
                    for (int i = 0; i < unitSquare.Length; i++)
                    {
                        displayUnitSquare[i] = unitSquare[i];
                    }
                    for (int i = 0; i < unitSquare.Length; i++)
                    {
                        displayUnitSquare[i] = ApplyToObservablePoint(displayUnitSquare[i], QueueMatrix);
                    }
                    displayUnitSquare = CutFunctionToBounds(displayUnitSquare)[0];
                }
                ObservablePoint[] displayEigenVector1 = new ObservablePoint[1];
                ObservablePoint[] displayEigenVector2 = new ObservablePoint[1];
                if (showEigen) //Defining / transforming Eigenvectors
                {
                    DefineEigenVectors();
                    displayEigenVector1[0] = ApplyToObservablePoint(eigenVector1OP[0], QueueMatrix);
                    displayEigenVector2[0] = ApplyToObservablePoint(eigenVector2OP[0], QueueMatrix);
                    displayEigenVector1 = CutFunctionToBounds(displayEigenVector1)[0];
                    displayEigenVector2 = CutFunctionToBounds(displayEigenVector2)[0];

                    eigenVector1OP = CutFunctionToBounds(eigenVector1OP)[0];
                    eigenVector2OP = CutFunctionToBounds(eigenVector2OP)[0];
                }
                ObservablePoint[][] displayGridOP = new ObservablePoint[10][]; //Defining / transforming Grid
                if (displayGrid)
                {
                    DefineGrid();
                    for (int b = 0; b < grid.Length; b++)
                    {
                        for (int i = 0; i < grid[b].Length; i++)
                        {
                            grid[b][i] = ApplyToObservablePoint(grid[b][i], QueueMatrix);
                        }
                         displayGridOP[b] = CutFunctionToBounds(grid[b])[0];
                    }
                    
                }
                ObservablePoint[] displayTriangleOP = new ObservablePoint[34]; //Defining / transforming Triangle
                if(displayTriangle)
                {
                    DefineTriangle();
                    for (int i = 0; i <triangle.Length; i++)
                    {
                        displayTriangleOP[i] = ApplyToObservablePoint(triangle[i], QueueMatrix);
                    }                  
                    displayTriangleOP = CutFunctionToBounds(displayTriangleOP)[0];
                }

                //defining y and x axes
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

                cartesianChart1.Series = new ISeries[] //64 function section 'LineSeries' + others due to limitations of LiveCharts
                {
                    new LineSeries<ObservablePoint> // point at (bounds, bounds) to display axes up to that point
                    {
                        Values = UpperBoundSet,
                        Fill = null,
                        GeometrySize = 0.1f,

                        Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 0 },
                        TooltipLabelFormatter = (chartPoint) => ""
                    },
                    new LineSeries<ObservablePoint> // point at (-bounds, -bounds) to display axes down to that point
                    {
                        Values =  LowerBoundSet,
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 0 },
                        TooltipLabelFormatter = (chartPoint) => $""

                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = XAxis,
                        Fill = null,
                        GeometrySize = 0.1f,

                        Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 3 },
                        TooltipLabelFormatter = (chartPoint) => $"X Axis"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = YAxis,
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 3 },
                        TooltipLabelFormatter = (chartPoint) => $"Y Axis"

                    },
                    new LineSeries<ObservablePoint> //Displaying Grid, if enabled
                    {
                        Values = displayGridOP[0],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[1],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[2],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[3],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[4],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[5],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[6],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[7],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[8],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayGridOP[9],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.DarkGoldenrod) { StrokeThickness = 2 }
                    },
                    new LineSeries<ObservablePoint> //displaying functions that have successfully parsed - 64 slots
                    {
                        Values = displayLines[0],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[1],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[2],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[3],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[4],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"

                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[5],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[6],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[7],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[8],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[9],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[10],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[11],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[12],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[13],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[14],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[15],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[16],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[17],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[18],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[19],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[20],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[21],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[22],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[23],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[24],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[25],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[26],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[27],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[28],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[29],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[30],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[31],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[32],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[33],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[34],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[35],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[36],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[37],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[38],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[39],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[40],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[41],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[42],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[43],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[44],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
       
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[45],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[46],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[47],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[48],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[49],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[50],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[51],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[52],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[53],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"

                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[54],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[55],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[56],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[57],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[58],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[59],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[60],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[61],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[62],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"

                    },
                    new LineSeries<ObservablePoint>
                    {
                        Values = displayLines[63],
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 5 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"
                    },
                    new LineSeries<ObservablePoint> // displaying unit square
                    {
                        Values =  displayUnitSquare,
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 3 }

                    },
                    new LineSeries<ObservablePoint> //displaying Triangle
                    {
                        Values =  displayTriangleOP,
                        Fill = null,
                        GeometrySize = 0.1f,
                        Stroke = new SolidColorPaint(SKColors.Orange) { StrokeThickness = 3 },
                        LineSmoothness = 0

                    },
                    new LineSeries<ObservablePoint> //displaying eigenvector 1 not transformed
                    {
                        Values =  eigenVector1OP,
                        Fill = null,
                        GeometryFill = new SolidColorPaint(SKColors.Pink),
                        GeometryStroke = new SolidColorPaint(SKColors.DeepPink) { StrokeThickness = 4 },
                        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 0 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"

                    },
                    new LineSeries<ObservablePoint> //displaying eigenvector 2 not transformed
                    {
                        Values =  eigenVector2OP,
                        Fill = null,
                        GeometryFill = new SolidColorPaint(SKColors.LightGreen),
                        GeometryStroke = new SolidColorPaint(SKColors.LightSeaGreen) { StrokeThickness = 4 },
                        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 0 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"

                    },
                    new LineSeries<ObservablePoint> //displaying eigenvector1 transformed
                    {
                        Values =  displayEigenVector1,
                        Fill = null,
                        GeometryFill = new SolidColorPaint(SKColors.Red),
                        GeometryStroke = new SolidColorPaint(SKColors.Crimson) { StrokeThickness = 4 },
                        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 0 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"

                    },
                    new LineSeries<ObservablePoint> //displaying eigenvector2 transformed
                    {
                        Values =  displayEigenVector2,
                        Fill = null,
                        GeometryFill = new SolidColorPaint(SKColors.Green),
                        GeometryStroke = new SolidColorPaint(SKColors.DarkGreen) { StrokeThickness = 4 },
                        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 0 },
                        TooltipLabelFormatter = (chartPoint) => $"({TruncateText(chartPoint.PrimaryValue.ToString(), 4)}, {TruncateText(chartPoint.SecondaryValue.ToString(), 4)})"

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
            catch
            {
                return double.NegativeInfinity; //used to return that the parsing of V was unsuccessful
            }
        }
        private void TextboxChanged(TextBox textBox, string letter, Matrix inputMatrix)
        {
            double value = ParseV(textBox.Text);
            if (value == double.NegativeInfinity || CheckForFloatingPoint(textBox.Text) || (textBox.Text == TruncateText(inputMatrix.Get(letter).ToString(), 6)))
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
        private void MultiplyRightButton_Click(object sender, EventArgs e) //multiply the main and reserve matrices, store result in the main matrix
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
            if (!CheckForFloatingPoint(textBox.Text) && !CheckForNegativeSign(textBox.Text))
            {
                if(TruncateText(ParseV(textBox.Text).ToString(), 6) != TruncateText(inputMatrix.Get(letter).ToString(), 6))
                {
                    textBox.Text = ErrorRounder(Double.Parse(TruncateText(inputMatrix.Get(letter).ToString(), 5)), 2).ToString();
                }
                
            }
        }
        private bool CheckForNegativeSign(string text)
        {
            if(text == "-")
            {
                return true;
            }
            return false;
        }

        private void CheckMatrix() //called many times per second on timer
        {

            if (lMat.getDet() == 0) //if determinant is 0 a matrix has no inverse
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
            string transType = rMat.GetStringType();
            TransformationTypeLabel.Text = "Transformation Type: " + transType;
            if (transType == "unknown" || transType == "reflection" || isAnimating) //reflection cannot be animated
            {
                AnimateButton.Enabled = false;
            }
            else
            {
                if (transType == "rotation")
                {
                    if (a2.Text != "cos(V)" || b2.Text != "-sin(V)" || c2.Text != "sin(V)" || d2.Text != "cos(V)")
                    {
                        AnimateButton.Enabled = false;
                    }
                    else
                    {
                        AnimateButton.Enabled = true;
                    }
                }
                else
                {
                    AnimateButton.Enabled = true;
                }
            }


            try
            {
                detA.Text = ErrorRounder(Double.Parse(TruncateText(lMat.getDet().ToString(), 5)), 2).ToString();

                detB.Text = ErrorRounder(Double.Parse(TruncateText(rMat.getDet().ToString(), 5)), 2).ToString();
            }
            catch
            {

            }



            QueueMatrixLabel.Text = "(" + ErrorRounder(Double.Parse(TruncateText(QueueMatrix.Get("a").ToString(), 4)), 2) + ", " + ErrorRounder(Double.Parse(TruncateText(QueueMatrix.Get("b").ToString(), 4)), 2) + ", " + ErrorRounder(Double.Parse(TruncateText(QueueMatrix.Get("c").ToString(), 4)), 2) + ", " + ErrorRounder(Double.Parse(TruncateText(QueueMatrix.Get("d").ToString(), 4)), 2) + ")";


            if (MatrixList.Items.Count == 0)
            {
                InvLine1TextBox.Text = "";
                InvLine2TextBox.Text = "";
                LOfInvPointsTextBox.Text = "";
                EigenValue1TextBox.Text = "";
                EigenValue2TextBox.Text = "";
                EV1A.Text = "";
                EV1B.Text = "";
                EV2A.Text = "";
                EV2B.Text = "";
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
        private string TruncateText(string input, int sigFig) //truncating values for display
        {
            bool foundPoint = false;
            int z = sigFig;
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
        private void Clear() //clearing function stack
        {
            FunctionList.Items.Clear();
            UpdateFunctions();
        }

        private void Remove() //removing function
        {
            if (FunctionList.SelectedItems.Count == 1)
            { 
                FunctionList.Items.Remove(FunctionList.SelectedItem);
            }
            
            UpdateFunctions();
        }
        private TreeNode FindRoot(TreeNode input) //Finding root of tree produced by Parser. Prevents a possible erroneous output from Parsing
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
        public double checkForBinaryError(double input, int sigFig) //checks for minute differences by scaling numbers up
        {
            string counter = input.ToString();
            if (counter.Length > sigFig)
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
        public double ErrorRounder(double input, int sigFig) //More aggressive rounder than checkForBinaryError(), checks for repeated 9s or 0's
        {
            
            double tempDouble = input;
            string counter = input.ToString();
            if(counter.Length > sigFig)
            {

                double scalar = Math.Pow(10, sigFig);
                double check = 1 / scalar;
                double floor = Math.Floor(tempDouble);
                double ceiling = Math.Ceiling(tempDouble);
                if(tempDouble - floor < check)
                {
                    bool oneCondition = false;

                    bool zeroCondition = true;
                    int i = counter.Length - 1;
                    while (i > -1 && counter[i].ToString() != "." )
                    {
                        if(i == counter.Length - 1)
                        {
                            if(counter[i].ToString() == "1")
                            {
                                oneCondition = true;
                            }
                            
                        }
                        else
                        {
                            if(counter[i].ToString() != "0")
                            {
                                zeroCondition = false;
                            }
                        }
                        i--;
                    }
                    if(zeroCondition && !oneCondition)
                    {
                        return floor;
                    }
                }
                else if (ceiling - tempDouble < check)
                {

                    

                    bool nineCondition = true;
                    int i = counter.Length - 1;
                    while (counter[i].ToString() != ".")
                    {
                        if (counter[i].ToString() != "9")
                        {
                            nineCondition = false;
                        }


                        i--;
                    }
                    if(nineCondition)
                    {
                        return ceiling;
                    }
                }

                return input;
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


        private ObservablePoint[] OPListToArray(List<ObservablePoint> input) // List To Array converter for Observable Point Lists/Arrays
        {
            ObservablePoint[] output = new ObservablePoint[input.Count];
            for (int i = 0; i < input.Count; i++)
            {
                output[i] = input[i];
            }
            return output;
        }
        private void ApplyMatrixButton_Click(object sender, EventArgs e)
        {
            ms.Enqueue(new Matrix(rMat.Get("a"), rMat.Get("b"), rMat.Get("c"), rMat.Get("d")));

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
                pitch = bounds / 200;
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
                    if(functionListNumber == 0)
                    {
                        functionListNumber = 1;
                    }
                    string RPNinput = RPNTextBox.Text;
                    bool update = true;
                    try
                    {
                        Parsing test = new Parsing(RPNinput);
                    }
                    catch
                    {
                        update = false;
                    }

                    if (update)
                    {
                        if (FunctionList.Items.Count < functionListNumber)
                        {
                            FunctionList.Items.Add("y = " + RPNinput);

                        }
                        else
                        {
                            FunctionList.Items[functionListNumber - 1] = ("y = " + RPNinput);
                        }
                        
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
                    steps = 100; //less steps to make rotation faster relative to other animations
                    aniPitch = V.value / steps;  //V taken as radians or degrees           
                                                       
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



        private void kTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateFunctions();
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
        private double RadiansToDegrees(double input)
        {
            return (input * 180) / Math.PI;
        }
        private double DegreesToRadians(double input)
        {
            return (input * Math.PI) / 180;
        }

        private void ReflectionButton_Click(object sender, EventArgs e)
        {
            V.value = 0.785; // default value will reflection in y=x
            
            if (useDegrees)
            {
                vTextBox.Text = RadiansToDegrees(0.785).ToString();
            }
            else
            {
                vTextBox.Text = 0.785.ToString();
            }
            
            a2.Text = "cos(2V)";
            b2.Text = "sin(2V)";
            c2.Text = "sin(2V)";
            d2.Text = "-cos(2V)";
        }

        private void RotationButton_Click(object sender, EventArgs e)
        {
            V.value = 1.57; //default value in radians will rotate by 90 degrees
            if (useDegrees)
            {
                vTextBox.Text = RadiansToDegrees(1.57).ToString();
            }
            else
            {
                vTextBox.Text = 1.57.ToString();
            }
            a2.Text = "cos(V)";
            b2.Text = "-sin(V)";
            c2.Text = "sin(V)";
            d2.Text = "cos(V)";
        }

        private void EnlargementButton_Click(object sender, EventArgs e)
        {
            if (useDegrees)
            {
                DegreesRadiansSwap();
            }
            V.value = 2;
            vTextBox.Text = 2.ToString();
            a2.Text = "V";
            b2.Text = "0";
            c2.Text = "0";
            d2.Text = "V";

        }

        private void ShearingButton_Click(object sender, EventArgs e)
        {
            if(useDegrees)
            {
                DegreesRadiansSwap();
            }
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



        private void InvariantLinesButton_Click(object sender, EventArgs e) //adds invariant lines to function list
        {
            try
            {
                UpdateQueueMatrix();
                string InvLine1 = QueueMatrix.GetInvLine1();
                string InvLine2 = QueueMatrix.GetInvLine2();
                if(InvLine1 != "x axis" && InvLine1 != "y axis")
                {
                    FunctionList.Items.Add(InvLine1);
                    functionListNumber++;
                }
                if(InvLine2 != "x axis" && InvLine2 != "y axis")
                {
                    FunctionList.Items.Add(InvLine2);
                    functionListNumber++;
                }
                
                UpdateFunctions();
                
            }
            catch
            {

            }

        }

        private void LinesOfInvariantPointsButton_Click(object sender, EventArgs e)//adds the line of invariant points to function list
        {
            try
            {
                UpdateQueueMatrix();
                string InvPointLine = QueueMatrix.GetInvPointLine();
                if(InvPointLine != "x axis" && InvPointLine != "y axis")
                {
                    FunctionList.Items.Add(InvPointLine);
                    functionListNumber++;
                }
                

                UpdateFunctions();
            }
            catch
            {

            }

        }

        private void cosButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "cos()";
        }



        private void piButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "π";
        }

        private void eButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "e";
        }





        private void sinButton_Click(object sender, EventArgs e)
        {
            RPNTextBox.Text = RPNTextBox.Text + "sin()";
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
                if(useDegrees)
                {
                    V.value = DegreesToRadians(Double.Parse(vTextBox.Text));
                }
                else
                {
                    V.value = Double.Parse(vTextBox.Text);
                }
                

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
               
                V.value = V.value + aniPitch; //updating V
                
                if (useDegrees)
                {
                    vTextBox.Text = RadiansToDegrees(V.value).ToString();

                }
                else
                {
                    vTextBox.Text = V.value.ToString();
                }
                AniMatrix.requestChange("a", (Math.Cos(V.value).ToString()));
                AniMatrix.requestChange("b", (Math.Sin((V.value) * -1).ToString()));
                AniMatrix.requestChange("c", ((Math.Sin(V.value)).ToString()));
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
                if (AniMatrix.Get("b") != 0)
                {
                    
                    AniMatrix.requestChange("b", (AniMatrix.Get("b") + aniPitch).ToString());
                }
                else
                {
                    AniMatrix.requestChange("c", (AniMatrix.Get("c") + aniPitch).ToString());
                }
            }
            steps--;

            if(steps == 0) //ending the animation
            {
                AnimateTimer.Enabled = false;
                isAnimating = false;
            }
            rMat = AniMatrix; 
            ms = new Queue<Matrix>(); //clearing and added updated matrix transformation
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

        private void cartesianChart1_Load(object sender, EventArgs e)
        {

        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(QueueMatrixLabel.Text);
        }
        private void DegreesRadiansSwap()
        {
            useDegrees = !useDegrees;
            if (DegreesRadiansButton.Text == "Degrees")
            {

                DegreesRadiansButton.Text = "Radians";
            }
            else
            {
                DegreesRadiansButton.Text = "Degrees";
            }
            if (vTextBox.Text == "90" && !useDegrees) //accurate swap of default rotation values
            {
                vTextBox.Text = "1.57";
            }
            else if (vTextBox.Text == "1.57" && useDegrees)
            {
                vTextBox.Text = "90";
            }
            else if (useDegrees)
            {
                vTextBox.Text = RadiansToDegrees(Double.Parse(vTextBox.Text)).ToString();
            }
            else
            {
                vTextBox.Text = DegreesToRadians(Double.Parse(vTextBox.Text)).ToString();
            }
        }
        private void DegreesRadiansButton_Click(object sender, EventArgs e)
        {
            DegreesRadiansSwap();
        }

        private void EigenvectorsButton_Click(object sender, EventArgs e)
        {
            showEigen = !showEigen;
            DefineEigenVectors();
            UpdateFunctions();
        }
    }
}