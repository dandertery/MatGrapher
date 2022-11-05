namespace NEA4
{
    public class Matrix
    {
        private float a;
        private float b;
        private float c;
        private float d;

        private float determinant;
        public Matrix(float aInput, float bInput, float cInput, float dInput)
        {
            a = aInput;
            b = bInput;
            c = cInput;
            d = dInput;
            determinant = (a*d) - (b*c);

        }

        public float getDet()
        {
            return determinant;
        }


        
    }
}