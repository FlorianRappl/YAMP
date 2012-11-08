Yet Another Math Parser
============================================================

YAMP may be really helpful or really useless depending on your needs, dependencies
and software stack. YAMP is completely built in C# and provides an easy, yet powerful
approach to parse expressions in a syntax, that is comfortable and quite close to
industry standards.

The parser is currently very stable. The drawback of the current release is the speed
of the parser, which is faster than most other implementations, but slower than some.
The good side is that everything is parsed in a standard way, so that you can easily
perform (parse and interpret) thousands of queries within a second.

Features of YAMP is (complex) (matrix) numerics (scalars, vectors, matrices) with
symbolic terms (constants, variables, functions) that can be customized. Even though
the current release does not support adding your own operators (they can be added in
the code within a few lines), it is easily possible. This allows operator
overloading as well.

Current status
-------------------------------------------------------

The current version number is **0.9.6**. This version is still an incomplete beta build.
A NuGet package is available [here](http://nuget.org/packages/YAMP). There are a lot of
tests in the code - since the package aims to be cross platform (created with Mono), no
particular unit testing framework has been chosen.

The console project (provided in the solution) gives you instant access to benchmarks, 
tests and your own trials. Parse equations as you want to. In the current release
exceptions from the parser are catched in the console application. Currently the following
builds are available:

- Debug: Contains the expression tests.
- Console: Contains a command line tool.
- Benchmark: Performs benchmarks for YAMP and three other C# only parsers.
- Release: Version to produce the NuGet library and other productive output.

Version history
-------------------------------------------------------
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

Where this parser is useful
-------------------------------------------------------

- C# only projects
- Lightweight projects with a sophisticated, yet small and fast parser
- Customized mathematics that is regularly updated
- Easily plug-and extensible architecture for (numerical) mathematics

Participating in the project
-------------------------------------------------------

If you know some feature that YAMP is currently missing, and you are willing to implement
the feature, then your contribution is more than welcome! Also if you have a really cool
idea - do not be shy, I'd like to hear it.

What the project could possibly achieve
-------------------------------------------------------

From my current point of view there is no reason why YAMP should not be able to do more
sophisticated (analytical) mathematics. Things like derivations or integrals with
symbols could be possible. I've already implemented derivations before, with the parser
already present this is highly possible. Including integrals is a different matter: I've
spent some time in the past, thinking about a proper implementations. What I think right
now is that the parser matters a lot. Once you normalize this process the integral
implemention is just a big database (integral table), with some (only basic) rules defined.

What is more likely is that the project goes in direction of a C# kind of MATLAB (more
lightweight). But time will tell!

Some legal stuff
------------------

Copyright (c) 2012, Florian Rappl.
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

*	Redistributions of source code must retain the above copyright
	notice, this list of conditions and the following disclaimer.

*	Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

*	Neither the name of the YAMP team nor the names of its contributors
	may be used to endorse or promote products derived from this
	software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.