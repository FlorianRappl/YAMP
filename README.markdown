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
the code within a few lines), it is easily possible. This could allow operator
overloading as well.

Current status
-------------------------------------------------------

The current version number is **0.5.0**. This version is still an incomplete alpha build.
A NuGet package will be available quite soon. There are a lot of tests in the code - since
the package aims to be cross platform (created with Mono), no particular unit testing
framework has been chosen.

The console project (provided in the solution) gives you instant access to benchmarks, 
tests and your own trials. Parse equations as you want to. In the current release
exceptions from the parser are catched in the console application. Currently the following
builds are available:

- Debug: Contains the expression tests.
- Release: Contains a command line tool.
- Benchmark: Performs benchmarks for YAMP and three other C# only parsers.

Version history
-------------------------------------------------------
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

This project is licensed under the Code Project Open License
([CPOL](http://www.codeproject.com/info/cpol10.aspx)) 1.02.

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the license
is respected in every way.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
