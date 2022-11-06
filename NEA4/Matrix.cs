namespace NEA4
{
    public class Matrix
    {
        private double a;
        private double b;
        private double c;
        private double d;

        private double determinant;
        public Matrix(double aInput, double bInput, double cInput, double dInput)
        {
            a = aInput;
            b = bInput;
            c = cInput;
            d = dInput;
            determinant = (a*d) - (b*c);

        }

        public double getDet()
        {
            return determinant;
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
                return true;
            }
            else
            {
                return false;
            }


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