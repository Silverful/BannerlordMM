namespace BL.API.Services.Extensions
{
    static public class NumericExtensions
    {
        static public double SafeDivision(this double Numerator, double Denominator)
        {
            return (Denominator == 0) ? 0 : Numerator / Denominator;
        }

        static public double SafeDivisionToDouble(this int Numerator, int Denominator)
        {
            return (Denominator == 0) ? 0 : (double)Numerator / Denominator;
        }
    }
}
