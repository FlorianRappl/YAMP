Yet Another Math Parser
============================================================

Version History
---------------

**1.3.0:**
- Last version of YAMPv1
- Added linear fitting function
- Added distribution estimation function
- Fixed typos in documentation
- Added automated tests for documentation
- Improved FFT (fixed some bugs)
- Improvement in `min()` and `max()` functions
- Improved the `load()` function
- Added possibility for extern functions defined in *.ys files
- Additional boolean operators && and || available
- Better expression and scripting blocks
- Some new functions (`cumprod`, `cumsum`, ...) available
- Added `chol` function for accessing the Cholesky decomposition

**1.2.3:**
- Added documentation tests
- Included more tests
- Improved QR, LU, Cholesky decomposition
- Added conversion functions for cartesian, spherical and polar coordinates
- Added functions for `all` and `any`
- Added the `find` function for getting indices
- Renamed the `product` function to `prod`
- Added `triu` and `tril` functions
- Added a `magic` function to shuffle new matrices
- Renamed the `printf` function to `notify`
- Added a `printf` function to format strings
- Changed the (unary) operator concept
- Added a function `Cholesky` to access the cholesky algorithm
- Fixed a bug in the assignment operator

**1.2.2:**
- Improved stability of the parser
- Included more tests
- Fixed some bugs (e.g. inverse, )
- Added the `let` keyword for explicitly local defined variables
- Added conversion functions, `bin2dec`, `hex2dec` and `oct2dec`
- Added fit function `polyfit`, and polynomial evaluation `polyval`
- Added function for convolution `convn`
- Curly brackets do no longer give scope - only for functions
- Functions require curly brackets and cannot access workspace variables
- Improved the matrix properties
- Added Hurwitz Zeta function
- Added Polylog and Polygamma (Psi) function
- Added Struve function and improved Ylm function
- Improved matrix operations (speedup at least 10)
- Improved the Gegenbauer function
- Added the inverse tangent function
- Added the `sphere` function to generate a sphere

**1.2.1:**
- Improved stability of the parser
- Included more tests
- Fixed some bugs (e.g. gamma function, zeta function)
- Added pre- and post increment / decrement operators
- Introduced functions for evaluating and casting user content
- Improved the event handling
- Added the method `setdef` to set default values for plots
- Added `SurfacePlot` with possibilities as Meshgrid and Surface plot
- Added the function `meshgrid` to generate a 2D grid (2 matrices)
- Introduced lots of new random number distributions
- Added wikipedia links in help
- Added several new trigonometric functions like sec, csc, ...
- Improved the numeric package
- Extended the existing "kinds" enumeration for naming classes of functions
- Introduced notifications with an event driven API

**1.2.0:**
- Rewrote the parser; it is now twice as fast and returns accurate errors
- Included the Zeta function
- Improved the Gamma function
- Completed a lot of the documentation work
- Physics plugin reached version 0.8 (includes Psi_0(x), working unit converter)
- Improved the operator system by allowing arbitrary operations through mapping
- Added new visualization features like `subplot()`, `errorbars()` or `hist()`
- Added lots of new value types for plotting
- Rewrote the complete Exception handling - most important feature is the difference
  between runtime and parsing
- Scripting is now fully functional and can be enabled by using `Parser.UseScripting = true`
- Improved the console demo application, as well as the benchmarks and tests
- Added the possibility of annotations in plots
- A complex plot is available by using `cplot()`
- A function plot can also be done by using `fplot()`
- Added `Block` and `Statement` classes to distinguish between atomic blocks and
  whole statements
- `Expression` and `Operator` instances are blocks, with `Keywords` being of type `Expression`

**1.1.2:**
- Improved physics plugin (unit conversations done)
- Extended `contourplot`, `polarplot` and fixed the color palette
- Added the `sinc()` function to the physics plugin
- Improved the output of scalars
- Added logic function `isinfinite()`, `isnan()`
- Added the `digamma()` function, i.e. psi(x) = psi_0(x)
- Improved documentation
- Added the `format()` function to define the output representation
- Added constants `true`, `false`
- Added statistic functions `acor()`, `cor()`, `cov()`, `hmean()`
- Added statistic functions `jackknife()`, `xcor()` and `lsq()`
- Added statistic function `bootstrap()`
- Added the complex plot with `ComplexPlotValue` and the `cplot()` function
- Added the bar plot with `BarPlotValue` and the `bar()` function
- Added the error plot with `ErrorPlotValue` and the `errorbar()` function
- Added the possibility to activate scripting by using `Parser.EnableScripting`
- Added prelimary scripting with `if`, `for`, `do`, `while`, `function`
- Added the possibility of having scopes with curly brackets
- Improved `load()` function
- Improved the parse trees and introduced keyword parse statements
- Extended the `QueryContext` and `ParseContext` objects
- Changed the syntax for lambda expressions from `@x => ...` to `x => ...`, i.e.
  the @ sign is now obsolete

**1.1.1:**
- Added plugin project YAMP.Physics
- Added some special functions to YAMP.Physics
- Added several physical constants to YAMP.Physics
- YAMP.Physics will have a special unit type - still under construction
- Added `ContourPlotValue` type for contour plots
- Improved the Gamma function
- Added `arg()` function as well as `real()` and `imag()`
- Added the `ncr()` function (n choose r, binomial coefficient)
- Renamed some classes to improve the code
- Improved whitespace, newline and comment handling
- Added `lu()` and `qr()` functions for linear algebra
- Added `bessel()`, `bessel0()` and `bessel1()` functions
- Added `mv()` and `cp()` function
- Added function `contour()` for creating contour plots
- Added the error function `erf()` and `erfc()`
- Improved the `set()` function
- Added exponential distribution `rande()`
- Improved the string representation of the scalar value
- Added more constants
- Improved `IsPrime()`
- Fixed an issue with the `timer()`
- Improved the help
- Added the possibility for string literals @"a\bc" (i.e. "a\\bc")

**1.1.0:**
- Removed the functions `title()`, `xlabel()`, ... to focus on the more general `set()`
- Added the ability to write so called lambda expressions, i.e. functions in the code
- Changed the random number generator to the Mersenne Twister MT19937
- Added a function `diag()` to create diagonal matrices
- Added a function `sort()` to sort vectors in matrices
- Fixed some bugs regarding the SVD, Mandelbrot and other functions
- Added a function to talk to the SVD, called `svd()`
- Added the possibility to have multiple output arguments over `ArgumentValue`
- Improved the possibility to have multiple statements
- Added new tests and improved the test console application
- Added a new class for serializing and one for deserializing (helpers)
- Improved the documention and added a new `ReturnsAttribute` attribute (mult. outputs)
- Added a new type called `FunctionValue`, which stores lambda expressions
- Added a function `gcd()` to determine the greatest common divisor
- Added several new random distributions along with the new random generator
- Improved the way that indices work in YAMP - values can now act as functions as well
- Added new constants `deg`, `g` and `omega`.
- Moved the converter attributes to a different namespace
- Added a lot new classes for numerics, e.g. in Optimization, ODE, Interpolator, ...
- Added a new function `cd()` for changing the working directory
- Added comments, i.e. `//` is a line comment and `/*` to `*/` is a block comment
- Changed the `ToString()` of the parser / parse tree, expression and operators.
- Added the ability to document return parameters with comments
- Added functions for finding the root `root()` and for differential eq. `ode()`
- Added a function `date()` to get the current date as a `StringValue`
- Added more properties and functions to the `MatrixValue` class
- Improved the string output of a `FunctionValue` instance

**1.0.1:**
- Removed the `setData()` method to adjust plot series values
- Added the `set()` method to set arbitrary plot properties and series
- The `set()` function includes possibilities to specify various series at once
- Improved the documentation
- Latest merge included to improve the portability of the library
- Removed all Async functions - async. calls have to be done externally
- Added new attributes to declare properties of `Plot` values modifyable
- Changed the name of the `gamma` constant to `gamma1` (since this is the value of `Gamma(1)`)
- Added several new exceptions
- Added some missing help declarations
- Added a lot of new properties to the various `Plot` derivatives
- Fixed some bugs regarding `Range` and `Matrix` types
- Added improved serialization (for `Plot` derivatives, `Matrix` and `Range` values)
- Added new tests and fixed a misbehavior for the unary minus operator regarding matrices
- Added new functions `ls()` and `pwd()` for directory information

**1.0.0:**
- Improved the `load()` function (decides now if the source is text or binary)
- Removed the transforms, but kept the advantages
- Introduced new attributes for the kind of function / class and optional arguments.
- Added the modulo operator (%) and a `mod()` function
- Added a `product()` function, which works like the `sum()` function
- Extended the documentation and included some documentation on the constants
- Changed the way that constants are being built (now similar to functions)
- Improved the index operator (logical subscripting is now much better)
- Included various plot functions and helpers (including `polar()`, `semilogx()`, `semilogy()`
  and `loglog()`)
- Now a completely object oriented help is available
- Added the possibility of constructing own `ParseTree` derivatives
- Fixed a bug concerning multiple spaces
- Can handle function overloads and multiple optional arguments for `ArgumentFunction` types
- Replaced all calls that contain a `Hashtable` with strongly typed `Dictionary<string, Value>`
- Un-nested the enum `PointSymbol` and the class `Points` for more convenience

**0.9.7:**
- Changed the syntax to agree with MATLAB (e.g. matrices can only be defined in [])
- Improved the `ParseContext` with events
- Now returns the `QueryContext` in most cases to give the programmer more control
- Included several plot functions like `PlotFunction` (for 2D plotting)
- Renamed the `FactoryFunction` to `FactorialFunction` (corrected)
- Spaces and new lines can be used within the matrix environment (i.e. within []) to
  mark new columns and new rows
- Included new expressions and new exceptions
- Improved the speed of the parser by another 10%
- Rewrote the matrix internally to work better for sparse matrices and be more flexible
- Rewrote the range internally to only set user changed values explicitly
- Added more unit tests

**0.9.6:**
- Added a `PlotValue` type with derived types like `Plot2DValue` etc.
- Created a function called plot(), which takes one or more arguments
- Created a special abstract class `PropertyFunction` to derive from
- Only numeric types will be assigned to the $
- An assignment to the $ will only happen if there is no user assignment
- Every property changed in a `PlotValue` instance invokes a callback
- Functions can return multiple values
- The last returned `PlotValue` is saved in the corresponding `ParseContext`

**0.9.5:**
- Added a `ParseContext` class that manages the (local and global) contexts
- Applied the changes to all operators, expressions and functions
- Improved the waterfall of global to local scope; one can have several independent branches
- Added the ability to work purely in one context
- Changed the derivation for each exception to a new class `YAMPException`
- Included the Mandelbrot set as an example for fractals
- Added a new abstract base class `SystemFunction` that makes use of the local context
- Altered the overview of the help
- Changed the `Tokens` class to only contain `Operator` and `Expression` instances
- Changed the definition of the `IFunction` interface
- Included a `ContainerFunction` class that can wrap any functions with the `FunctionDelegate`
  signature
- Improved the performance of the parser by another 5%
- Added more async methods, mainly due to the `ParseContext` specification
- Added new `Interpolation` base class with `SplineInterpolation` implemention

**0.9.1:**
- Added the "who()" command to view the current workspace
- Added a method called `LoadPlugin()` to the Parser - now it should be easily possible
  to load libraries full of functions, operators and other YAMPy stuff
- Added more functions for async operations
- Improved the structure of the code, so that it is somehow more clear what must be done
  to write own YAMP libraries / plugins
- Added more tests

**0.9.0:**
- Changed the license from the CPOL to the New BSD License
- Underscores can now also be used in symbol or function expressions
- Added a new help() method to provide help for included methods
- Added `DescriptionAttribute` and `ExampleAttribute` classes to write help messages
- `ArgumentFunction` derived types are now type-safe
- Included help for most non-trivial functions
- Fixed a bug concerning the `trace()` method of the `MatrixValue` class
- Included a class `AsyncTask` that is used for async invoking
- Provided a method `ExecuteAsync()` for async execution
- Added some simple integrators
- Added the GMRES(k) method for solving non-SPD matrices

**0.8.0:**
- Introduced combined assignment operators (+=, -=, *=, ^=, /=, \=, ...)
- Added a function for setting the output precision (precision())
- Added the CG method and a proper (prelimary) solve() method
- Improved the ArgumentsFunction class to support optional arguments
- Added the possibility to specify the variables to load, save and clear in those functions
- Small changes in the code for more elegance
- Improved the performance of the parser

**0.7.0:**
- Introduced logic operators (>, <, ==, ~=)
- Included logical subscripting (like x[x>0]=...)
- Improved the parser performance (factor 1.5)
- Improved the bracket expressions
- Added new linear algebra functions (inv(), trace())
- Added new Spectroscopy function (FFT)
- Added new tests
- Added new parsers for comparison (LL MP, MFP)

**0.6.0:**

- Resorted the YAMP code (directory changes)
- Added new functions (trace, eig, ev, ...)
- Added some basic linear algebra functions
- Added dot operators (.^, ./, .*, .&#92;)
- Added new exceptions
- Improved the exception handling in ArgumentFunctions
- Improved the parser by a factor of 5
- Fixed a small bug in the parser
- Introduced the YAMP.Numerics namespace with some stuff for numerical mathematics
- Added new tests

**0.5.0:**

- Added new functions (load, save, eye, dim, length, ...)
- The power function can now work with matrices
- Added a datatype for Strings
- Added the range operator (including special notations like ":" and "end")
- The index operator now supports the range operator
- Added new random number generator (gaussian) and integer number generator
- Improved the parser (Unary operators)
- Added new tests

**0.4.0:**

- Added new functions (arsinh, arcosh, artanh, arcoth, sinh, cosh, tanh, coth, ...)
- Improved the power functions
- Added an operator for left divisons (usual operator is for right divisions)
- Added new matrix functions (eye, zeros, ones, ...)
- Added random number generator (uniform)
- Assignment operator now works with indices
- Added new tests

**0.3.0:**

- Functions can now either perform operations on whole matrices or on each sub-element
- Assignment operator has been introduced
- Fixed a bug concerning powers of purely imaginary numbers
- MatrixValue numerics have been improved
- VectorValue has been replaced completely by MatrixValue
- Improved the performance of the parser (factor 2)

**0.2.0:**

- More sophisticated operators (transpose, faculty, ...) have been integrated
- Custom constants (symbols) and functions are now possible
- More sophisticated functions are now available
- VectorValue and MatrixValue numerics have been included
- Improved the stability of the parser

**0.1.0:**

- The project has been created
- First version of the parser, Stack based
- First test cases have been stated
- Main operators and expressions have been included
- Constants have been introduced
- ScalarValue numerics and some basic functions (exp, sin, cos, ...) have been implemented