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

The current version number is **1.3.0**. A NuGet package (current version 1.3.0) is
available [here](http://nuget.org/packages/YAMP). There are a lot of tests in the code -
since the package aims to be cross platform (created with Mono), no particular unit testing
framework has been chosen.

The console project (provided in the solution) gives you instant access to benchmarks, 
tests and your own trials. Parse equations as you want to. In the current release
exceptions from the parser are catched in the console application. Currently the following
builds are available:

- Debug: Contains the expression tests.
- Console: Contains a command line tool.
- Benchmark: Performs benchmarks for YAMP and three other C# only parsers.
- Release: Version to produce the NuGet library and other productive output.

With version 1.3 the project will drive into a new direction. The next release will carry
the version number 2. There will be major changes in the code, with only the functions
remaining. YAMP will get a new scripting language called M# (MathSharp). The architecture
of YAMP will also change dramatically. While YAMP was a kind of experimental project,
YAMPv2 will aim for a package that allows mathematicians who love C# to do rapide
prototyping. M# will be a scripting language that uses *some* parts of C# with a mix of
mathematical operators (^, !, ', ...) and dynamic typing.

Change log
-------------------------------------------------------

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

More changes can be found in the [version history][history.markdown].

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

Some legal stuff
------------------

Copyright (c) 2012-2013, Florian Rappl and collaborators.
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