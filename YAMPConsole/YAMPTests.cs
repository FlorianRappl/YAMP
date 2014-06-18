/*
	Copyright (c) 2012-2014, Florian Rappl.
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
using System.Collections.Generic;
using System.Diagnostics;
using YAMP;
using YAMP.Help;

namespace YAMPConsole
{
    static class YAMPTests
    {
        delegate double CompareAction(double x);

        static int success = 0;
        static int total = 0;

        public static void Run()
        {
            Parser.UseScripting = true;

            Console.WriteLine("DEBUG MODE");
            Console.WriteLine("----------");

            Console.WriteLine();
            Console.WriteLine("Testing the engine . . .");
            TestEngine();

            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();

            Console.WriteLine();
            Console.WriteLine("Testing the core . . .");
            TestCore();

            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();

            Console.WriteLine();
            Console.WriteLine("Testing physics library . . .");
            TestPhysics();

            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();

            Console.WriteLine();
            Console.WriteLine("Testing examples . . .");
            TestExamples();
        }

        static void TestEngine()
        {
            success = 0;
            total = 0;

            var sw = Stopwatch.StartNew();

            Test("mandelbrot(-2,,-2,2,100,100)", true);
            Test("[1,2,3](2)", false);
            Test("2^-2", false);
            Test("2^^2", true);
            Test("function f(", true);
            Test("for() { }", true);
            Test("for() { ", true);
            Test("for(i=0 { }", true);
            Test("for(i=0, i != 2, i++) { }", true);
            Test("for(i = 0; i != 2; i++) { }", false);
            Test("newton(", true);
            Test("do{ ", true);
            Test("do{ }", false);
            Test("while() {}", true);
            Test("while(true) {}", false);
            Test("do {} while(0 > 2)", false);
            Test("a([4)", true);
            Test("f(", true);

            sw.Stop();

            Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
            Console.WriteLine("Time for the tests ... {0} ms", sw.ElapsedMilliseconds);
        }

        static void TestExamples()
        {
            success = 0;
            total = 0;
            var sw = Stopwatch.StartNew();

            var doc = Documentation.Create(Parser.PrimaryContext);

            Console.WriteLine();

            foreach (var section in doc.Sections)
            {
                if (section is HelpFunctionSection)
                {
                    var f = (HelpFunctionSection)section;

                    foreach (var usage in f.Usages)
                    {
                        foreach (var example in usage.Examples)
                        {
                            if (example.IsFile)
                                continue;

                            total++;

                            try
                            {
                                var parser = YAMP.Parser.Parse(example.Example);
                                parser.Execute();
                                success++;
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Caution:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                Console.WriteLine("At example: " + example.Example);
                                Console.Write("For usage: " + usage.Usage);
                                Console.WriteLine("In function: " + f.Name);
                                Console.WriteLine();
                            }

                            if (total % 20 == 0)
                                Console.WriteLine("{0} elements have been processed . . .", total);
                        }
                    }
                }
            }

            sw.Stop();

            Console.WriteLine();
            Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
            Console.WriteLine("Time for the tests ... {0} ms", sw.ElapsedMilliseconds);
        }

        static void TestCore()
        {
            success = 0;
            total = 0;
            var sw = Stopwatch.StartNew();

            Test("2-3-4", -5.0);
            Test("(10^6*27)/8/1024", (27000000.0 / 8.0) / 1024.0);
            Test("-pi", -Math.PI);
            Test("-2+3", 1.0);
            Test("2.2", 2.2);
            Test(".2+4-4+.2", 0.4);
            Test("3/3+1*4-5", 0.0);
            Test("7*3+5-13+2", 15.0);
            Test("(7*3+5-13+2)/3", 5.0);
            Test("15/3-5", 0.0);
            Test("(7*3+5-13+2)/3-5", 0.0);
            Test("2^3+3", 11.0);
            Test("3+2^3", 11.0);
            Test("2*3^3+7/2+1-4/2", 56.5);
            Test("2*3^(3-1)+7/2+1-4/2", 20.5);
            Test("(2-1)*3^(3-1)+7/2+1-4/2", 11.5);
            Test("ceil(2.5)", 3.0);
            Test("floor(2.5)", 2.0);
            Test("exp(0)*10-5", 5.0);
            Test("exp(3)*10", 10.0 * Math.Exp(3));
            Test("3*|-2|*5", 30.0);
            Test("cos(pi)", -1.0);
            Test("sin(pi/2)", 1.0);
            Test("cos(0)*exp(0)*sin(3*pi/2)", -1.0);
            Test("e^3-pi^3", Math.Pow(Math.E, 3.0) - Math.Pow(Math.PI, 3.0));
            Test("5!-1000/5+4*20", 0.0);
            Test("2*x-4", "x", 2.0, 0.0);
            Test("2*x-4", -2.0, 2.0, x => 2 * x - 4.0);
            Test("x!", "x", 5.0, 120.0);
            Test("x^2-3*x/4", "x", 0.75, 0.0);
            Test("2^2^2^2", 65536.0);
            Test("2-(3*5)^2+7/(2-8)*2", -225.0 - 1.0 / 3.0);
            Test("|[2,3,1]-[1,3,1]|", 1.0);
            Test("|[2^2,2+3,-2,-2]|", 7.0);
            Test("|4*(2i-5)/3i|", 4.0 * Math.Sqrt(29.0) / 3.0);
            Test("|[3,2,1]*[1;2;3]|", 10.0);
            Test("|[1;2;3]|-|[1,2,3]|", 0.0);
            Test("|(2+3i)^2|", 13.0, 1e-8);
            Test("|1^(i+5)|", 1.0);
            Test("|1^(i+5)|", 1.0);
            Test("|(5+8i)^(i+1)|", 3.4284942595728127, 1e-8);
            Test("|(2+3i)/(1+8i)|", 0.447213595499958, 1e-8);
            Test("|2*[1,2;1,2]|", 0.0);
            Test("|max([1,5,7,9,8+5i])|", Math.Sqrt(89.0));
            Test("|[2,1;3,5]-[2,1;3,5]'|", 4.0);
            Test("[2,1,0]*[5;2;1]", 12.0);
            Test("det([1;2]*[2,1])", 0.0);
            Test("X=(Y=5)+2", 7.0);
            Test("Y", 5.0);
            Test("2^-2", 0.25);
            Test("[1,2,3;4,5,6;7,8,9](2,3)", 6.0);
            Test("|cos(1+i)|", 1.2934544550420957, 1e-8);
            Test("|arccos(0.83373002513-0.988897705i)|", 1.4142135618741407, 1e-8);
            Test("|(x=[1,2,3;4,5,6;7,8,9])(:,1)|", Math.Sqrt(66.0));
            Test("17>12", 1.0);
            Test("7<-1.5", 0.0);
            Test("|[1,2,3,4,5,6,7] < 5|", 2.0);
            Test("2+i==2-i", 0.0);
            Test("3-i~=4", 1.0);
            Test("abs([1,2,3;4,5,6;7,8,9](1,:))", Math.Sqrt(14.0));
            Test("(-25)^2", 625.0);
            Test("length(help()) > 0", 1.0);
            Test("abs(3+4i)", 5.0);
            Test("eig([1,2;4,5])(1)(1)", 3.0 - Math.Sqrt(12));
            Test("eigval([1,2;4,5])(1)", 3.0 - Math.Sqrt(12));
            Test("abs(eigval([1,2;4,5])(1:2))", 6.48074069840786, 1e-8);
            Test("abs(eigvec([1,2;4,5])(1:2))", 1.0);
            Test("|[2 3 4]|", Math.Sqrt(29.0));
            Test("[2 3 4\n1 2 3](2, 2)", 2.0);
            Test(" [2 2 * 2 - 2^3 4](1, 2) ", -4.0);
            Test("sum([0:10, 2^(0:2:20), 2^(1:2:21)](:,1))", 55.0);
            Test("length(-pi/4:0.1:pi/4)", 16.0);
            Test("polar(-pi/4:0.1:pi/4, [sin(-pi/4:0.1:pi/4), cos(-pi/4:0.1:pi/4), tan(-pi/4:0.1:pi/4)])", new PlotSeries(3));
            Test("plot([0:10, 2^(0:2:20), 2^(1:2:21)])", new PlotSeries(2));
            Test("|1:1:3|", Math.Sqrt(1 + 4 + 9));
            Test("|[1,2,3]|", Math.Sqrt(1 + 4 + 9));
            Test("|[1;2;3]|", Math.Sqrt(1 + 4 + 9));
            Test("-sin([1,2,3])(2)", -Math.Sin(2));
            Test("f = x => x.^2; f(2)", 4.0);
            Test("f = (x, y) => x*y'; f([1,2,3],[1,2,3])", 14.0);
            Test("[a, b, c] = 12.0;b", 12.0);
            Test("round(sum(randn(10000, 1)) / 1000)", 0.0);
            Test("round(sum(rand(10000, 1)) / 1000)", 5.0);
            Test("round(sum(randi(10000, 1, 1, 9)) / 10000)", 5.0);
            Test("2+3//This is a line-comment!\n-4", 1.0);
            Test("1-8* /* this is another comment */ 0.25", -1.0);
            Test("1-8* /* this is \nanother comment\nwith new lines */ 0.5+4", 1.0);
            Test("ode((t, x) => -x, 0:0.01:1, 1.0)(101, 2)", 0.36818409421192455, 1e-8);
            Test("root(x => x.^2-4, 3)", 2.0, 1e-8);
            Test("sort([25, 1, 0, 29, 105, 0, -5])(4)", 1.0);
            Test("(-5)^2", 25.0);
            Test("(5)^2", 25.0);
            Test("(-75)^2", Math.Pow(75.0, 2.0));
            Test("real(2 + 5i)", 2.0);
            Test("imag(2 + 5i)", 5.0);
            Test("bessel(2, 4.5)", 0.21784898358785076, 1e-8);
            Test("erf(1.4)", 0.9522851197626383, 1e-8);
            Test("x = round(sum(sum([1, 0; 0, 100] * Jackknife([3 + randn(1000, 1), 10 + 2 * randn(1000, 1)], 10, avg)))); sum([x < 29, x > 18]) / 2", 1.0);
            Test("x = round(sum(sum([1, 0; 0, 10] * Jackknife([3 + randn(1000, 1), 10 + 2 * randn(1000, 1)], 10, var)*[10,0;0,1]))); sum([x < 24, x > 16]) / 2", 1.0);
            Test("sum(sum(round(cor([3 + randn(100, 1), 10 + 2 * randn(100, 1)]))))", 2.0);
            Test("sum(round(acor(3 + randn(100, 1))))", 1.0);
            Test("x = []; y = 0; for(i = 1; i <= 10; i+=1) { y+=i; x(i) = y; } x(10) - x(9)", 10.0);
            Test("x = 9; y = 5; if(x > y) { t = x; x = y; y = t; } y - x", 4.0);
            Test("x = [3 + randn(100, 1), 10 + 2 * randn(100, 1)]; sum(sum(Bootstrap(x, 200, avg) - Jackknife(x, 20, avg))) < 0.1", 1.0);
            Test("sum(size([\n1 2 3 4\n5 6 7 8]))", 6.0);
            Test("zeta(0.5)", -1.4603545088095868, 1e-8);
            Test("zeta(0)", -0.5, 1e-8);
            Test("eval(\"2+3\")", 5.0);
            Test("eval(\"|[1,2,3]|\")", Math.Sqrt(14.0));
            Test("cast(\"3.5\")", 3.5);
            Test("bin2dec(\"1011\")", 11.0);
            Test("bin2dec(\"0011\")", 3.0);
            Test("hex2dec(\"Ff\")", 255.0);
            Test("hex2dec(\"123\")", 291.0);
            Test("oct2dec(\"1627\")", 919.0);
            Test("oct2dec(\"77\")", 63.0);
            Test("trace(inv([1, 2, 3; 4, 5, 6; 7, 8, 10]))", 4.0, 1e-8);
            Test("polyval([1 2 3], 5)", 86.0, 1e-8);
            Test("sum(polyfit([0, 1, 2, 3, 4, 5], [3, 3, 5, 9, 15, 23], 2))", 3.0, 1e-8);
            Test("([3 2 1] - [1 2 3])(3)", -2.0);
            Test("([3 2 1] + [1 2 3])(2)", 4.0);
            Test("2^-1^-1", 0.5);
            Test("(2^-1)^-1", 2.0);
            Test("det(chol([1, 1i; -1i, pi]))", Math.Sqrt(Math.PI - 1.0), 1e-8);
            Test("A=[4, 3; 6, 3]; [L, U, p] = lu(A); sum(sum(A - p * L * U))", 0.0, 1e-8);
            Test("A=[12,-51,4;6,167,-68;-4,24,-41]; [q, r] = qr(A); det(q)", -1.0, 1e-8);
            Test("any([1, 0; 0, 0])", 1.0);
            Test("all([1, 0; 0, 0])", 0.0);
            Test("any([0, 0; 0, 0])", 0.0);
            Test("all([1, 1; 5, 3])", 1.0);
            Test("length(fft(ones(31,1)))", 31.0);
            Test("1 == 1", 1.0);
            Test("1 == 0", 0.0);
            Test("1 ~= 0", 1.0);
            Test("1 ~= 1", 0.0);
            Test("1 && 1", 1.0);
            Test("1 && 0", 0.0);
            Test("0 && 1", 0.0);
            Test("0 && 0", 0.0);
            Test("1 || 1", 1.0);
            Test("0 || 1", 1.0);
            Test("0 || 0", 0.0);
            Test("1 || 0", 1.0);
            Test("A = rand(3); A(2, 2) = 0.5i; x = solve(A, eye(3,1)); abs(A * x - eye(3,1))", 0.0, 1e-8);
            Test("-2^2", -4.0);
            Test("-2-2", -4.0);
            Test("-2*2", -4.0);
            Test("-2^2+4", 0.0);

            sw.Stop();

            Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
            Console.WriteLine("Time for the tests ... {0} ms", sw.ElapsedMilliseconds);
        }

        static void TestPhysics()
        {
            success = 0;
            total = 0;

            var sw = Stopwatch.StartNew();

            Test("convert(1, \"m / s\", \"km / h\")", 3.6);
            Test("convert(1, \"eV\", \"J\") - Q * unit(1, \"V\")", 0.0);
            Test("convert(1, \"m\", \"ft\")", 3.2808);
            Test("ylm(0, 0, 0, 0)", 0.5 / Math.Sqrt(Math.PI));
            Test("ylm(0, 0, 10, 0)", 0.5 / Math.Sqrt(Math.PI));
            Test("ylm(0, 0, 0, 5)", 0.5 / Math.Sqrt(Math.PI));
            Test("ylm(1, 0, pi / 3, 0)", 0.5 * Math.Sqrt(3.0 / Math.PI) * Math.Cos(Math.PI / 3.0));
            Test("ylm(2, 2, pi / 3, 0)", 0.28970565151739147, 1e-8);
            Test("imag(ylm(2, 1, pi / 3, 0.5))", -0.16037899974811717, 1e-8);
            Test("clebsch(0.5, 0.5)(1, 5)", 1.0);
            Test("clebsch(0.5, 0.5)(5, 5)", 1.0 / Math.Sqrt(2.0));
            Test("legendre(3, 1)", 1.0);
            Test("hermite(3, 2)", 40.0);
            Test("laguerre(2, 2)", -1.0, 1e-8);
            Test("zernike(1, 1, 0.5)", 0.5);
            Test("zernike(2, 0, 0.5)", 2.0 * 0.25 - 1.0);
            Test("gegenbauer(1, 0.5, 0.25)", 2.0 * 0.5 * 0.25);
            Test("polylog(0, 3)", -1.5);
            Test("polylog(1, 0)", 0.0);
            Test("polylog(2, 1)", Math.PI * Math.PI / 6.0);
            Test("polylog(3, 1)", 1.2020569031595945, 1e-8);
            Test("polylog(-3, 2)", 26.0);
            Test("polylog(-5, 2)", 1082.0000000231821, 1e-8);
            Test("polylog(-9, 0.1)", 86.621357524537643, 1e-8);
            Test("hzeta(3, 1)", 1.20205690315959428, 1e-8);

            sw.Stop();

            Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
            Console.WriteLine("Time for the tests ... {0} ms", sw.ElapsedMilliseconds);
        }

        #region Helpers

        static bool Test(string query, double xmin, double xmax, CompareAction compare)
        {
            var parser = Parser.Parse(query);
            Console.WriteLine("Testing: {0} = ...", query);
            var x = xmin;
            var success = 0;
            var total = 0;

            while (x < xmax)
            {
                total++;
                var value = parser.Execute(new { x });

                if (value.Equals(compare(x)))
                    success++;

                x += 0.1;
            }

            Console.WriteLine("with x = {2}..{3} ;\n{0}\n-> correct: {1}", success, total, xmin, xmax);
            return Assert(success, total);
        }

        static bool Test(string query, string name, double val, double result, double prec = 0.0)
        {
            var parser = Parser.Parse(query);
            Console.WriteLine("Testing: {0} = ...", query);
            var arg = new Dictionary<string, Value>();
            arg.Add(name, new ScalarValue(val));
            var value = parser.Execute(arg);
            Console.WriteLine("with {3} = {2};\n{0}\n-> correct: {1}", value, result, val, name);
            return Assert(value, result, prec);
        }

        static bool Test(string query, PlotSeries series)
        {
            var parser = Parser.Parse(query);
            Console.WriteLine("Testing: {0} = ...", query);
            var value = (YAMP.PlotValue)parser.Execute();
            Console.WriteLine("{0}\n-> correct: {1}", value.Count, series.Number);
            return Assert(value.Count, series.Number);
        }

        static bool Test(string query, bool hasErrors)
        {
            var parser = Parser.Parse(query);
            Console.WriteLine("Testing: {0} = ...", query);
            var value = parser.Context.Parser.HasErrors;
            Console.WriteLine("{0}\n-> correct: {1}", value, hasErrors);
            return Assert(value ? 1.0 : 0.0, hasErrors ? 1.0 : 0.0);
        }

        static bool Test(string query, double result, double prec = 0.0)
        {
            var parser = Parser.Parse(query);
            Console.WriteLine("Testing: {0} = ...", query);
            var value = parser.Execute();
            Console.WriteLine("{0}\n-> correct: {1}", value, result);
            return Assert(value, result, prec);
        }

        static bool Assert(int total, int result, double prec = 0.0)
        {
            return Assert((double)total, (double)result, prec);
        }

        static bool Assert(Value value, double result, double prec = 0.0)
        {
            return Assert(((ScalarValue)value).Re, result, prec);
        }

        static bool Assert(double value, double result, double prec = 0.0)
        {
            var isSuccess = true;
            total++;

            if (Math.Abs(value - result) <= prec)
            {
                success++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Test successful!");
            }
            else
            {
                isSuccess = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test failed!");
            }

            Console.ResetColor();
            Console.WriteLine("---");
            return isSuccess;
        }

        class PlotSeries
        {
            public PlotSeries(int num)
            {
                Number = num;
            }

            public int Number { get; private set; }
        }

        #endregion
    }
}
