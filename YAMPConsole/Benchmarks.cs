/*
	Copyright (c) 2012-2013, Florian Rappl.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
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
*/

using System;
using System.Diagnostics;
using System.IO;

namespace YAMPConsole
{
    static class Benchmarks
    {
        const string BMK_FILE = "benchmarks.data";

        public static void Run()
        {
            Console.WriteLine("BENCHMARK MODE");
            RunBenchmark(BenchmarkKind.Standard);
        }

        enum BenchmarkKind
        {
            File,
            Standard,
            Little,
            Thomson
        }

        static void RunBenchmark(BenchmarkKind which)
        {
            var yamp = YAMP.Parser.PrimaryContext;
            var mpparser = new MathParser.Parser();
            var mptk = new MathParserTK_NET.MathParserTK();
            var mpnet = new MathParserNet.Parser();
            var mfmp = new MathFunctions.MathParser();
            var llmp = new MathParserDataStructures.MathObj();
            var calcEngine = new CalcEngine.CalcEngine();
            calcEngine.CacheExpressions = false;

            var lines = new string[0];

            switch (which)
            {
                case BenchmarkKind.Standard:
                    //         UB
                    //YAMP  : 154 ms
                    //LLMP  : 108 ms
                    //MP    : 4134 ms
                    //MPTK  : 375 ms
                    //MPNET : 3054 ms
                    //MFP   : 88 ms
                    //CALEN : 33 ms
                    //NCALC : 420 ms
                    lines = MakeTenK("2-3*5+7/2-8*2");
                    break;

                case BenchmarkKind.File:
                    //         UB
                    //YAMP  : 2084 ms
                    //LLMP  : 1072 ms
                    //MP    : 372847 ms
                    //MPTK  : ---
                    //MPNET : ---
                    //MFP   : ---
                    //CALEN : 271 ms
                    //NCALC : ---
                    if (!File.Exists(BMK_FILE))
                        GenerateBenchmarks();

                    lines = File.ReadAllLines(BMK_FILE);
                    break;

                case BenchmarkKind.Little:
                    //         UB
                    //YAMP  : 71 ms
                    //LLMP  : 59 ms
                    //MP    : 1840 ms
                    //MPTK  : 87 ms
                    //MPNET : 3232 ms
                    //MFP   : 37 ms
                    //CALEN : 23 ms
                    //NCALC : 247 ms
                    lines = MakeTenK("2+3");
                    break;

                case BenchmarkKind.Thomson:
                    //         UB
                    //YAMP  : 193 ms
                    //LLMP  : 138 ms
                    //MP    : 11508 ms
                    //MPTK  : 647 ms
                    //MPNET : 3827 ms
                    //MFP   : ---
                    //CALEN : 41 ms
                    //NCALC : ---
                    lines = MakeTenK("2-(3*5)^2+7/(2-8)*2");
                    break;
            }

            Console.WriteLine("Starting benchmarks ...");
            Console.WriteLine("----------");

            // The implementation here... YAMP
            Benchmark("YAMP", lines, query => yamp.Run(query));

            //http://www.codeproject.com/Articles/53001/LL-Mathematical-Parser
            Benchmark("LLMathParser", lines, query => llmp.Evaluate(query, new char[0], new double[0]));

            //http://www.codeproject.com/Articles/11164/Math-Parser
            Benchmark("MathParser", lines, query => mpparser.Evaluate(query));

            //http://www.codeproject.com/Tips/381509/Math-Parser-NET-Csharp
            Benchmark("MathParserTK", lines, query => mptk.Parse(query, false));

            //http://www.codeproject.com/Articles/274093/Math-Parser-NET
            Benchmark("MathParserNet", lines, query => mpnet.Simplify(query));

            //http://www.codeproject.com/Articles/23061/MathParser-Math-Formula-Parser
            Benchmark("MathFormulaParser", lines, query => mfmp.Calculate(query));

            //http://www.codeproject.com/Articles/246374/A-Calculation-Engine-for-NET
            Benchmark("CalcEngine", lines, query => calcEngine.Evaluate(query));

            //http://ncalc.codeplex.com/
            //Benchmark("NCalc", lines, query => new NCalc.Expression(query, NCalc.EvaluateOptions.NoCache).Evaluate());
        }

        #region Helpers

        static string[] MakeTenK(string s)
        {
            var l = new string[10000];

            for (var i = 0; i < l.Length; i++)
                l[i] = s;

            return l;
        }

        static void Benchmark(string name, string[] lines, Action<string> parser)
        {
            var sw = new Stopwatch();
            Console.Write(name);
            Console.Write(" : Running benchmark ... ");
            sw.Start();

            foreach (var query in lines)
            {
                parser(query);
            }

            sw.Stop();
            Console.WriteLine("{0} ms", sw.ElapsedMilliseconds);
        }

        static void GenerateBenchmarks()
        {
            Console.Write("Generating benchmark file ... ");

            var r = new Random();
            var count = 100000;
            var operators = new string[] { "+", "-", "*", "^", "/" };
            int length;

            using (var fs = File.CreateText(BMK_FILE))
            {
                for (var i = 0; i < count; i++)
                {
                    length = r.Next(2, 14);
                    fs.Write(r.Next(-100, 100));

                    for (var j = 0; j < length; j++)
                    {
                        fs.Write(operators[r.Next(0, operators.Length)]);
                        fs.Write(r.Next(-100, 100));
                    }

                    if (i < count - 1)
                        fs.WriteLine();
                }
            }

            Console.WriteLine("done !");
        }

        #endregion
    }
}
