namespace YAMP.Help
{
    using System;
    using System.Collections.Generic;

    public static class Localization
    {
        public static readonly IDictionary<String, String> Default = new Dictionary<String, String>
        {
            { "NoDescription", "No description available." },
            { "AlphaConstantDescription", "The Feigenbaum constant alpha is the ratio between the width of a tine and the width of one of its two subtines (except the tine closest to the fold)." },
            { "AlphaConstantLink", "http://en.wikipedia.org/wiki/Feigenbaum_constant" },
            { "BernoulliConstantDescription", "In mathematics, the Bernoulli numbers Bn are a sequence of rational numbers with deep connections to number theory. The first 21 numbers are given in this vector. The Bernoulli numbers appear in the Taylor series expansions of the tangent and hyperbolic tangent functions, in formulas for the sum of powers of the first positive integers, in the Euler–Maclaurin formula, and in expressions for certain values of the Riemann zeta function." },
            { "BernoulliConstantLink", "http://en.wikipedia.org/wiki/Bernoulli_number" },
            { "CatalanConstantDescription", "In mathematics, Catalan's constant G, which occasionally appears in estimates in combinatorics, is defined by G = beta(2), where beta is the Dirichlet beta function." },
            { "CatalanConstantLink", "http://en.wikipedia.org/wiki/Catalan_constant" },
            { "ContainerConstantDescription", "A custom constant defined by you." },
            { "IConstantDescription", "In mathematics, the imaginary unit or unit imaginary number allows the real number system to be extended to the complex number system." },
            { "IConstantLink", "http://en.wikipedia.org/wiki/Imaginary_unit" },
            { "GaussConstantDescription", "In mathematics, Gauss's constant, denoted by G, is defined as the reciprocal of the arithmetic-geometric mean of 1 and the square root of 2." },
            { "Gamma1ConstantDescription", "The Euler–Mascheroni constant (also called Euler's constant) is a mathematical constant recurring in analysis and number theory." },
            { "Gamma1ConstantLink", "http://en.wikipedia.org/wiki/Euler–Mascheroni_constant" },
            { "FalseConstantDescription", "False can be used in all logical expressions or for calculations. False is numerically represented by 0, however, that does not necessarily mean that everything else is true." },
            { "EConstantDescription", "The number e is an important mathematical constant, approximately equal to 2.71828, that is the base of the natural logarithm." },
            { "EConstantLink", "http://en.wikipedia.org/wiki/E_(mathematical_constant)" },
            { "DeltaConstantDescription", "The Feigenbaum constant delta is the limiting ratio of each bifurcation interval to the next between every period doubling, of a one-parameter map." },
            { "DeltaConstantLink", "http://en.wikipedia.org/wiki/Feigenbaum_constant" },
            { "DegConstantDescription", "A degree (in full, a degree of arc, arc degree, or arcdegree), usually denoted by ° (the degree symbol), is a measurement of plane angle, representing 1⁄360 of a full rotation; one degree is equivalent to π/180 radians." },
            { "DegConstantLink", "http://en.wikipedia.org/wiki/Degree_(angle)" },
            { "TrueConstantDescription", "True can be used in all logical expressions or for calculations. True is numerically represented by 1, however, that does not necessarily mean that everything else is false." },
            { "PiConstantDescription", "The mathematical constant Pi is the ratio of a circle's circumference to its diameter." },
            { "PiConstantLink", "http://en.wikipedia.org/wiki/Pi" },
            { "PhiConstantDescription", "The golden ratio: two quantities are in the golden ratio if the ratio of the sum of the quantities to the larger quantity is equal to the ratio of the larger quantity to the smaller one." },
            { "PhiConstantLink", "http://en.wikipedia.org/wiki/Golden_ratio" },
            { "OmegaConstantDescription", "The omega constant is the value of W(1) where W is Lambert's W function. The name is derived from the alternate name for Lambert's W function, the omega function." },
            { "OmegaConstantLink", "http://en.wikipedia.org/wiki/Omega_constant" },
            { "DiagFunctionDescription", "Creates a diagonal matrix that has the given numeric values on the diagonal." },
            { "DiagFunctionDescriptionForMatrix", "Creates a diagonal matrix with the values from the given matrix." },
            { "DiagFunctionExampleForMatrix1", "Creates a matrix with dimension 25 x 25, containing random values on the diagonal." },
            { "DiagFunctionExampleForMatrix2", "Creates a matrix with dimension 5 x 5, containing random values on the diagonal." },
            { "DiagFunctionDescriptionForScalar", "Creates a diagonal matrix with the given value, i.e. just returns the value (1 x 1 diagonal matrix = scalar)." },
            { "DiagFunctionExampleForScalar1", "Returns the given value, which is 3." },
            { "DiagFunctionDescriptionForArguments", "Tries to create a diagonal matrix with the given arguments, which must be of numeric nature, i.e. scalars or matrices." },
            { "DiagFunctionExampleForArguments1", "Creates the unit matrix with dimension 4." },
            { "DiagFunctionExampleForArguments2", "Creates a matrix that is close to the unit matrix, except that one block has been rotated in the middle." },
            { "EyeFunctionDescription", "Generates an identity matrix. In linear algebra, the identity matrix or unit matrix of size n is the n x n square matrix with ones on the main diagonal and zeros elsewhere." },
            { "EyeFunctionLink", "http://en.wikipedia.org/wiki/Identity_matrix" },
            { "EyeFunctionDescriptionForVoid", "Generates the 1x1 identity matrix, which is just 1." },
            { "EyeFunctionDescriptionForScalar", "Generates an n-dimensional identity matrix." },
            { "EyeFunctionExampleForScalar1", "Returns a 5x5 matrix with 1 on the diagonal and 0 else." },
            { "EyeFunctionDescriptionForScalarScalar", "Generates an n x m-dimensional identity matrix." },
            { "EyeFunctionExampleForScalarScalar1", "Returns a 5x3 matrix with 1 on the diagonal and 0 else." },
        };

        public static IDictionary<String, String> Current = Default;
    }
}
