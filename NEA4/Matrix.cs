namespace NEA4
{
    public class Matrix
    {
        private double a;
        private double b;
        private double c;
        private double d;
        private double Eigenvalue1;
        private double Eigenvalue2;
        private double EV1A; //Top Eigenvector 1 value
        private double EV1B; //Bottom Eigenvector 1 value
        private double EV2A; //Top Eigenvector 2 value
        private double EV2B; //Bottom Eigenvector 2 value
        private string invLine1;
        private string invLine2;
        private string invPointLine;
        private string type;

        private double determinant;
        public Matrix(double aInput, double bInput, double cInput, double dInput)
        {
            a = aInput;
            b = bInput;
            c = cInput;
            d = dInput;
            determinant = checkForBinaryError((a*d) - (b*c), 6);
            FindInvariantLines();
            FindLinesOfInvariantPoints();
            FindEigenvectors();
            type = DetermineType();
        }

        public double getDet()
        {
            return determinant;
        }
        public string GetStringType()
        {
            return type;
        }
        public string GetInvLine1()
        {
            return invLine1;
        }
        public string GetInvLine2()
        {
            return invLine2;
        }
        public string GetInvPointLine()
        {
            return invPointLine;
        }
        public double GetEigenValue1()
        {
            return Eigenvalue1;
        }
        public double GetEigenValue2()
        {
            return Eigenvalue2;
        }
        public double GetEV1A()
        {
            return EV1A;
        }
        public double GetEV1B()
        {
            return EV1B;
        }
        public double GetEV2A()
        {
            return EV2A;
        }
        public double GetEV2B()
        {
            return EV2B;
        }

        public string DetermineType()
        {
            if (checkForBinaryError(Math.Pow(a, 2) + Math.Pow(b, 2), 2) == 1) //(sin^2 x + cos^2 x = 1)
            {
                if (a == d && b == -c)
                {
                    return "rotation";
                }
                else if (a == -d && b == c)
                {
                    return "reflection";
                }
            }
            if (b == 0 && c == 0)
            {
                return "stretchlargement";
            }
            else if ( a == d)
            {
                if(b==0 || c==0)
                {
                    return "shear";
                }
            }
            return "unknown";
        }
        public string getName()
        {
            string name = "(" + a + ", " + b + ", " + c + ", " + d + ")";
            return name;
        }

        public double Get(string letter)
        {
            if (letter == "a")
            {
                return a;
            }
            else if (letter == "b")
            {
                return b;
            }
            else if (letter == "c")
            {
                return c;
            }
            else if (letter == "d")
            {
                return d;
            }
            return 0;
        }

        public bool requestChange(string intendedEdit, string value) // will handle string  / input error
        {
            double valueDouble;

            if(Double.TryParse(value, out valueDouble))
            {
                if (intendedEdit == "a")
                {
                    a=valueDouble;
                }
                else if (intendedEdit == "b")
                {
                    b=valueDouble;
                }
                else if (intendedEdit == "c")
                {
                    c=valueDouble;
                }
                else if (intendedEdit == "d")
                {
                    d=valueDouble;
                }
                determinant = checkForBinaryError((a * d) - (b * c), 6);
                type = DetermineType();
                return true;
            }
            else
            {
                return false;
            }



        }

        public Matrix Inverse(Matrix inputMat) //MAKE FRONT END THE DET CANNOT BE 0 WHEN REQUESTING AN INVERSE
        {
            double ia = inputMat.a;
            double ib = inputMat.b;
            double ic = inputMat.c;
            double id = inputMat.d;

            ia = checkForBinaryError(ia / inputMat.getDet(), 6);
            ib = checkForBinaryError(ib / inputMat.getDet(), 6);
            ic = checkForBinaryError(ic / inputMat.getDet(), 6);
            id = checkForBinaryError(id / inputMat.getDet(), 6);
            Matrix output = new Matrix(id, -ib, -ic, ia);
            return output;
        }

        public double checkForBinaryError(double input, int sigFig)
        {
            double scalar = Math.Pow(10,sigFig);
            double scaled = input * scalar;
            double floor = Math.Floor(scaled);
            double ceiling = Math.Ceiling(scaled);
            if(scaled - floor < 0.1)
            {
                return floor / scalar;
            }
            else if(ceiling- scaled < 0.1)
            {
                return ceiling / scalar;
            }
            double output = scaled / scalar;
            return output;

           
        }

        public Matrix Transpose(Matrix inputMat)
        {
            double ia = inputMat.Get("a");
            double ib = inputMat.Get("b");
            double ic = inputMat.Get("c");
            double id = inputMat.Get("d");

            Matrix output = new Matrix(ia, ic, ib, id);
            return output;

        }
        public Matrix Multiplication(Matrix inputMatA, Matrix inputMatB) //(A * B, not commmutative!)
        {
            double a1 = inputMatA.Get("a");
            double b1 = inputMatA.Get("b");
            double c1 = inputMatA.Get("c");
            double d1 = inputMatA.Get("d");

            double a2 = inputMatB.Get("a");
            double b2 = inputMatB.Get("b");
            double c2 = inputMatB.Get("c");
            double d2 = inputMatB.Get("d");

            double a3 = (a1 * a2) + (b1 * c2);
            double b3 = (a1 * b2) + (b1 * d2);
            double c3 = (c1 * a2) + (d1 * c2);
            double d3 = (c1 * b2) + (d1 * d2);

            Matrix output = new Matrix(a3, b3, c3, d3);

            return output;


        }


        private void FindEigenvalues()
        {
            double temp = SolveQuadratic(1, -a - d, (a * d) - (b * c), true);
            if(temp != double.NaN && temp != double.PositiveInfinity && temp != double.NegativeInfinity)
            {
                Eigenvalue1 = temp;
            }
            double temp2 = SolveQuadratic(1, -a - d, (a * d) - (b * c), false);
            if(temp2 != double.NaN && temp2 != double.PositiveInfinity && temp2 != double.NegativeInfinity)
            {
                Eigenvalue2 = temp2;
            }
            
        }
        private void FindEigenvectors()
        {
            FindEigenvalues();           
            if(b!= 0)
            {
                if(Eigenvalue1 != double.NaN &&  Eigenvalue1 != double.PositiveInfinity && Eigenvalue1 != double.NegativeInfinity)
                {
                    EV1A = 1;
                    EV2B = (-a + Eigenvalue1) / b;
                }
                if (Eigenvalue1 != double.NaN && Eigenvalue1 != double.PositiveInfinity && Eigenvalue1 != double.NegativeInfinity)
                {
                    EV2A = 1;
                    EV2B = (-a + Eigenvalue2) / b;
                }
                
                
            }


        }

        private void FindInvariantLines()
        {
            double m1 = SolveQuadratic(b, a - d, -c, true);
            double m2 = SolveQuadratic(b, a - d, -c, false);

            invLine1 = "y = " + SixFigText(m1.ToString()) + "x";
            invLine2 = "y = " + SixFigText(m2.ToString()) + "x";
        }
        private void FindLinesOfInvariantPoints()
        {
            double m = 0;
            if(((1-a)/b) == ((-c)/(d-1)))
            {
                m = (1 - a) / b;
                invPointLine = "y = " + SixFigText(m.ToString()) + "x";
            }
            
        }

        private double SolveQuadratic(double aInput, double bInput, double cInput, bool isPlus)
        {
            int quad = 1;
            if(!isPlus)
            {
                quad = -1;
            }

            return (-bInput + (quad * Math.Sqrt(Math.Pow(bInput, 2) - (4 * aInput * cInput)))) / (2*aInput);
        }
        private string SixFigText(string input)
        {
            bool foundPoint = false;
            int z = 6;
            int index = input.Length - 1;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '.')
                {
                    foundPoint = true;

                }
                if (foundPoint)
                {
                    z--;
                }
                if (z == 0)
                {
                    index = i;
                }
            }
            string output = input.Remove(index + 1);
            return output;
        }


    }


}


//
//
//
//
//
//
//
//
//