using System;
using System.Text;

namespace MathParserNet
{
    [Serializable]
    public class SimplificationReturnValue
    {
        private const int FactorDefault = 100000;

        public enum ReturnTypes
        {
            Float,
            Integer
        }

        /// <summary>
        /// Gets or sets the original equation passed into the parser
        /// </summary>
        public string OriginalEquation
        {
            get;
            set; 
        }

        /// <summary>
        /// Gets or sets the value, if the return type is ReturnTypes.Float
        /// </summary>
        public double DoubleValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value, if the return type is ReturnTypes.Integer
        /// </summary>
        public int IntValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the return type. Use this to determine if the value is a float, integer or an error occured
        /// </summary>
        public ReturnTypes ReturnType
        {
            get;
            set;
        }

        /// <summary>
        /// Converts the return type to a fraction. Uses a slash "/" as the default numerator/denominator seperator.
        /// Uses a space " " as the default whole number/fracational seperator.
        /// </summary>
        /// <returns>The fraction</returns>
        public string ToFraction()
        {
            return ToFraction("/", " ", FactorDefault);
        }

        /// <summary>
        /// Converts the return type to a fraction. You must specify a default numerator/denomiator seperator character.
        /// </summary>
        /// <param name="fractionSeperator">The default numerator/denominator to use as a seperator</param>
        /// <param name="factor">Set to the highest precision you want. The numerator and/or denominator will never be higher than this number.</param>
        /// <returns>The Fraction</returns>
        public string ToFraction(string fractionSeperator, int factor)
        {
            return ToFraction(fractionSeperator, " ", factor);
        }

        public string ToFraction(string fractionSeperator)
        {
            return ToFraction(fractionSeperator, " ", FactorDefault);
        }

        public string ToFraction(string fractionSeperator, string wholeNumberSeperator)
        {
            return ToFraction(fractionSeperator, wholeNumberSeperator, FactorDefault);
        }

        public string ToFraction(string fractionSeperator, string wholeNumberSeperator, int factor)
        {
            var sb = new StringBuilder();
            double d = 0f;

            if (ReturnType == ReturnTypes.Float)
                d = DoubleValue;
            if (ReturnType == ReturnTypes.Integer)
                d = IntValue;

            if (d<0)
            {
                sb.Append("-");
                d = -d;
            }
            var l = (long)d;
            if (l != 0)
            {
                sb.Append(l);
                sb.Append(wholeNumberSeperator);
            }
            d -= l;
            double error = Math.Abs(d);
            int bestDenominator = 1;
            for (int i=2;i<=factor;i++)
            {
                double error2 = Math.Abs(d - Math.Round(d*i)/i);
                if (error2<error)
                {
                    error = error2;
                    bestDenominator = i;
                }
            }
            if (bestDenominator > 1)
                sb.Append(Math.Round(d*bestDenominator)).Append(fractionSeperator).Append(bestDenominator);
            return sb.ToString().Trim();
        }
    }
}
