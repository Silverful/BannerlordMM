namespace BL.API.Services.Extensions
{
    static public class NumericExtensions
    {
        static public decimal SafeDivision(this decimal Numerator, decimal Denominator)
        {
            return (Denominator == 0) ? 0 : Numerator / Denominator;
        }

        static public decimal SafeDivisionToDecimal(this int Numerator, int Denominator)
        {
            return (Denominator == 0) ? 0 : (decimal)Numerator / Denominator;
        }
    }
}
