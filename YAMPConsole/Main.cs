using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace YAMPConsole
{
	class MainClass
	{
		delegate double CompareAction(double x);

#if DEBUG

		static int success = 0;
		static int total = 0;

#endif
		
		const string BMK_FILE = "benchmarks.data";
		
		public static void Main (string[] args)
		{
            Console.WriteLine("Command Line Test Tool of YAMP");
            Console.WriteLine();
            Console.WriteLine("--------------------------");
            Console.WriteLine();
            Console.WriteLine(" YAMP VERSION " + YAMP.Parser.Version);
            Console.WriteLine();
            Console.WriteLine("--------------------------");
            Console.WriteLine();

#if DEBUG
			Console.WriteLine("DEBUG MODE");
            Console.WriteLine("----------");
            Console.WriteLine("Testing the core . . .");
			Tests();
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
            Console.WriteLine("Testing physics library . . .");
            TestPhysics();
#elif BENCHMARKS
			Console.WriteLine("BENCHMARK MODE");
			Benchmarks();
#elif CONSOLE
			Console.WriteLine("CONSOLE MODE");
			Console.WriteLine("Enter your own statements now (exit with the command 'exit'):");
            Console.WriteLine();
            var query = string.Empty;
            var context = YAMP.Parser.Load();
            LoadPhysics();

            YAMP.Parser.EnableScripting = true;
			YAMP.Parser.AddCustomFunction("G", v => new YAMP.ScalarValue((v as YAMP.ScalarValue).Value * Math.PI) );
			YAMP.Parser.AddCustomConstant("R", 2.53);

			//var history = new List<string>();
			
			while(true)
			{
				Console.Write(">> ");

				/*var historyIndex = history.Count;
				var position = 0;
				query = string.Empty;
				var cursorLeft = Console.CursorLeft;
				var cursorTop = Console.CursorTop;

				do
				{
					var key = Console.ReadKey(true);

					if (key.Key == ConsoleKey.Enter)
					{
						if (key.Modifiers == ConsoleModifiers.Shift)
							query = query.Insert(position++, "\n");
						else
							break;
					}
					else if (key.Key == ConsoleKey.Tab)
					{
						query = query.Insert(position++, "\t");
					}
					else if (key.Key == ConsoleKey.Backspace)
					{
						if (position > 0)
							query = query.Remove(--position, 1);
					}
					else if (key.Key == ConsoleKey.UpArrow)
					{
						if (historyIndex > 0)
						{
							historyIndex--;
							query = history[historyIndex];
						}
					}
					else if (key.Key == ConsoleKey.DownArrow)
					{
						if (historyIndex < history.Count)
						{
							historyIndex++;
							query = historyIndex == history.Count ? string.Empty : history[historyIndex];
						}
					}
					else if (key.Key == ConsoleKey.RightArrow)
					{
						if(position < query.Length)
							position++;
					}
					else if (key.Key == ConsoleKey.LeftArrow)
					{
						if (position > 0)
							position--;
					}
					else
					{
						query = query.Insert(position++, key.KeyChar.ToString());
					}

					Console.SetCursorPosition(cursorLeft, cursorTop);
					Console.Write(query);
				}
				while (true);

				Console.WriteLine();
				history.Add(query);*/
				query = Console.ReadLine();

				if (query.Equals("exit"))
					break;
				else
				{
					try
					{
                        var result = context.Run(query);

						if (result.Output != null)
						{
							Console.WriteLine(result.Result);
							Console.WriteLine(result.Statements);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}
#else
			Console.WriteLine("RELEASE MODE");
#endif

		}

        static void LoadPhysics()
        {
            YAMP.Parser.LoadPlugin(System.Reflection.Assembly.LoadFile(Environment.CurrentDirectory + "\\YAMP.Physics.dll"));
        }

#if BENCHMARKS

		static void Benchmarks()
		{
			if(!File.Exists(BMK_FILE))
				GenerateBenchmarks();
			
			YAMP.Parser.Load();
			var a = new MathParser.Parser();
			var b = new MathParserTK_NET.MathParserTK();
			var c = new MathParserNet.Parser();
			var d = new MathFunctions.MathParser();
			var e = new MathParserDataStructures.MathObj();
			
			Console.WriteLine("Starting benchmarks ...");	
			Console.WriteLine("----------");

            var lines = new string[0];
			// This is Benchmark #1
            //var lines = File.ReadAllLines(BMK_FILE);
			// This is Benchmark #2
            //var lines = MakeTenK("2-3*5+7/2-8*2");
			// This is Benchmark #3
			//var lines = MakeTenK("2+3");
			// This is Benchmark #4
			//var lines = MakeTenK("2-(3*5)^2+7/(2-8)*2");
			
			// The implementation here... YAMP
			// 4814 ms ; 279 ms ; 95 ms ; 413 ms
			Benchmark("YAMP", lines, query => YAMP.Parser.Parse(query).Execute());

			//http://www.codeproject.com/Articles/53001/LL-Mathematical-Parser
			// 2092 ms ; 108 ms ; 75 ms ; 155 ms
			Benchmark("LLMathParser", lines, query => e.Evaluate(query, new char[0], new double[0]));
			
			//http://www.codeproject.com/Articles/11164/Math-Parser
			// 372847 ms ; 3631 ms ; 2385 ms ; 11508 ms
			Benchmark("MathParser", lines, query => a.Evaluate(query));
			
			//http://www.codeproject.com/Tips/381509/Math-Parser-NET-Csharp
			// FAILED ; 417 ms ; 100 ms ; 647 ms
			Benchmark("MathParserTK", lines, query => b.Parse(query, false));
			
			//http://www.codeproject.com/Articles/274093/Math-Parser-NET
			// FAILED ; 3763 ms ; 3511 ms ; 3827 ms
			Benchmark("MathParserNet", lines, query => c.Simplify(query));

			//http://www.codeproject.com/Articles/23061/MathParser-Math-Formula-Parser
			// FAILED ; 161 ms ; 37 ms ; FAILED
			Benchmark("MathFormulaParser", lines, query => d.Calculate(query));
		}

		static string[] MakeTenK(string s)
		{
			var l = new string[10000];

			for(var i = 0; i < l.Length; i++)
				l[i] = s;

			return l;
		}
		
		static void Benchmark(string name, string[] lines, Action<string> parser)
		{
			var sw = new Stopwatch();
			Console.Write(name);
			Console.Write(" : Running benchmark ... ");
			sw.Start();
			
			foreach(var query in lines)
			{			
				parser(query);
			}
			
			sw.Stop();
			Console.WriteLine("{0} ms", sw.ElapsedMilliseconds);
		}
		
		static void GenerateBenchmarks ()
		{
			Console.Write("Generating benchmark file ... ");
			
			var r = new Random();
			var count = 100000;
			var operators = new string [] { "+", "-", "*", "^", "/" };
			int length;
			
			using(var fs = File.CreateText(BMK_FILE))
			{
				for(var i = 0; i < count; i++)
				{
					length = r.Next(2, 14);
					fs.Write(r.Next(-100, 100));
							 
					for(var j = 0; j < length; j++)
					{
						fs.Write(operators[r.Next(0, operators.Length)]);
						fs.Write(r.Next(-100, 100));
					}
					
					if(i < count - 1)
						fs.WriteLine();
				}
			}
			
			Console.WriteLine("done !");
		}

#endif

#if DEBUG

		static void Tests()
        {
            YAMP.Parser.EnableScripting = true;
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
			Test("2*x-4", 2.0, 0.0);
			Test("2*x-4", -2.0, 2.0, x => 2 * x - 4.0);
			Test("x!", 5.0, 120.0);
			Test("x^2-3*x/4", 0.75, 0.0);
			Test("2^2^2^2", 65536.0);
			Test("2-(3*5)^2+7/(2-8)*2", -225.0-1.0/3.0);
			Test("|[2,3,1]-[1,3,1]|", 1.0);
			Test("|[2^2,2+3,-2,-2]|", 7.0);
			Test("|4*(2i-5)/3i|", 4.0 * Math.Sqrt(29.0) / 3.0);
			Test("|[3,2,1]*[1;2;3]|", 10.0);
			Test("|[1;2;3]|-|[1,2,3]|", 0.0);
			Test("|(2+3i)^2|", 12.999999999999998);
			Test("|1^(i+5)|", 1.0);
			Test("|1^(i+5)|", 1.0);
			Test("|(5+8i)^(i+1)|", 3.4284942595728127);
			Test("|(2+3i)/(1+8i)|", 0.447213595499958);
			Test("|2*[1,2;1,2]|", 0.0);
			Test("|max([1,5,7,9,8+5i])|", Math.Sqrt(89.0));
			Test("|[2,1;3,5]-[2,1;3,5]'|", 4.0);
			Test("[2,1,0]*[5;2;1]", 12.0);
			Test("det([1;2]*[2,1])", 0.0);
			Test("X=(Y=5)+2", 7.0);
			Test("Y", 5.0);
			Test("[1,2,3;4,5,6;7,8,9](2,3)", 6.0);
			Test("|cos(1+i)|", 1.2934544550420957);
			Test("|arccos(0.83373002513-0.988897705i)|", 1.4142135618741407);
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
			Test("abs(ev([1,2;4,5])(1:2))", 1.0);
			Test("abs(eigvec([1,2;4,5])(1:2))", 1.0);
			Test("|[2 3 4]|", Math.Sqrt(29.0));
			Test("[2 3 4\n1 2 3](2, 2)", 2.0);
			Test(" [2 2 * 2 - 2^3 4](1, 2) ", -4.0);
			Test("sum([0:10, 2^(0:2:20), 2^(1:2:21)](:,1))", 55.0);
			Test("length(-pi/4:0.1:pi/4)", 16.0);
			Test("polar(-pi/4:0.1:pi/4, [sin(-pi/4:0.1:pi/4), cos(-pi/4:0.1:pi/4), tan(-pi/4:0.1:pi/4)])", 3);
			Test("plot([0:10, 2^(0:2:20), 2^(1:2:21)])", 2);
			Test("|1:1:3|", Math.Sqrt(1 + 4 + 9));
			Test("|[1,2,3]|", Math.Sqrt(1 + 4 + 9));
			Test("|[1;2;3]|", Math.Sqrt(1 + 4 + 9));
			Test("-sin([1,2,3])(2)", -Math.Sin(2));
			Test("f = x => x.^2; f(2)", 4.0);
			Test("f = (x, y) => x*y'; f([1,2,3],[1,2,3])", 14.0);
			Test("[a,b,c]=12.0;b", 12.0);
			Test("round(sum(randn(10000, 1)) / 1000)", 0.0);
			Test("round(sum(rand(10000, 1)) / 1000)", 5.0);
			Test("round(sum(randi(10000, 1, 1, 10)) / 10000)", 5.0);
            Test("2+3//This is a line-comment!\n-4", 1.0);
            Test("1-8* /* this is another comment */ 0.25", -1.0);
            Test("1-8* /* this is \nanother comment\nwith new lines */ 0.5+4", 1.0);
			Test("ode((t, x) => -x, 0:0.01:1, 1.0)(101, 2)", 0.36818409421192455);
			Test("root(x => x.^2-4, 3)", 2.0000000000519473);
			Test("sort([25,1,0,29,105,0,-5])(4)", 1.0);
			Test("(-5)^2", 25.0);
			Test("(5)^2", 25.0);
			Test("(-75)^2", Math.Pow(75.0, 2.0));
			Test("real(2+5i)", 2.0);
			Test("imag(2+5i)", 5.0);
			Test("bessel(2, 4.5)", 0.21784898358785076);
			Test("erf(1.4)", 0.95228511976264874);
            Test("x = round(sum(sum([1, 0; 0, 100] * Jackknife([3 + randn(1000, 1), 10 + 2 * randn(1000, 1)], 10, avg)))); sum([x < 29, x > 18]) / 2", 1.0);
            Test("x = round(sum(sum([1, 0; 0, 10] * Jackknife([3 + randn(1000, 1), 10 + 2 * randn(1000, 1)], 10, var)*[10,0;0,1]))); sum([x < 24, x > 16]) / 2", 1.0);
            Test("sum(sum(round(cor([3 + randn(100, 1), 10 + 2 * randn(100, 1)]))))", 2.0);
            Test("sum(round(acor(3 + randn(100, 1))))", 1.0);
            Test("x=[]; y= 0; for(i = 1; i <= 10; i+=1) { y+=i; x(i) = y; } x(10) - x(9)", 10.0);
            Test("x=9; y=5; if(x > y) { t = x; x = y; y = t; } y - x", 4.0);

			sw.Stop();
			
			Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
			Console.WriteLine("Time for the tests ... {0} ms", sw.ElapsedMilliseconds);
		}

        static void TestPhysics()
        {
            success = 0;
            total = 0;
            LoadPhysics();

            var sw = Stopwatch.StartNew();

            Test("convert(1, \"m / s\", \"km / h\")", 3.6);
            Test("convert(1, \"eV\", \"J\") - Q", 0.0);
            Test("convert(1, \"m\", \"ft\")", 3.2808);
            Test("ylm(0, 0, 0, 0)", 0.5 / Math.Sqrt(Math.PI));
            Test("ylm(0, 0, 10, 0)", 0.5 / Math.Sqrt(Math.PI));
            Test("ylm(0, 0, 0, 5)", 0.5 / Math.Sqrt(Math.PI));
            Test("ylm(1, 0, pi / 3, 0)", 0.5 * Math.Sqrt(3.0 / Math.PI) * Math.Cos(Math.PI / 3.0));
            Test("ylm(2, 2, pi / 3, 0)", 0.28970565151739147);
            Test("imag(ylm(2, 1, pi / 3, 0.5))", -0.16037899974811717);
            Test("clebsch(0.5, 0.5)(1, 5)", 1.0);
            Test("clebsch(0.5, 0.5)(5, 5)", 1.0 / Math.Sqrt(2.0));
            Test("legendre(3, 1)", 1.0);
            Test("hermite(3, 2)", 40.0);
            Test("laguerre(2, 2)", -0.99999999999999956);

            sw.Stop();

            Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
            Console.WriteLine("Time for the tests ... {0} ms", sw.ElapsedMilliseconds);
        }
		
		static bool Test(string query, double xmin, double xmax, CompareAction compare)
		{
			var parser = YAMP.Parser.Parse(query);
			Console.WriteLine("Testing: {0} = ...", query);
			var x = xmin;
			var success = 0;
			var total = 0;
			
			while(x < xmax)
			{
				total++;
				var value = parser.Execute(new { x });
				
				if(value.Equals(compare(x)))
					success++;
				
				x += 0.1;
			}
			
			Console.WriteLine("with x = {2}..{3} ;\n{0}\n-> correct: {1}", success, total, xmin, xmax);
			return Assert(success, total);
		}
		
		static bool Test(string query, double x, double result)
		{
			var parser = YAMP.Parser.Parse(query);
			Console.WriteLine("Testing: {0} = ...", query);
			var value = parser.Execute(new { x });
			Console.WriteLine("with x = {2};\n{0}\n-> correct: {1}", value, result, x);
			return Assert(value, result);
		}

		static bool Test(string query, int series)
		{
			var parser = YAMP.Parser.Parse(query);
			Console.WriteLine("Testing: {0} = ...", query);
			var value = (YAMP.PlotValue)parser.Execute();
			Console.WriteLine("{0}\n-> correct: {1}", value.Count, series);
			return Assert(value.Count, series);
		}
		
		static bool Test(string query, double result)
		{
			var parser = YAMP.Parser.Parse(query);
			Console.WriteLine("Testing: {0} = ...", query);
			var value = parser.Execute();
			Console.WriteLine("{0}\n-> correct: {1}", value, result);
			return Assert(value as YAMP.ScalarValue, result);
		}

		static bool Assert(int total, int result)
		{
			return Assert ((double)total, (double)result);
		}
		
		static bool Assert (YAMP.Value value, double result)
		{
			return Assert ((value as YAMP.ScalarValue).Value, result);
		}

		static bool Assert(double value, double result)
		{
			var isSuccess = true;
			total++;

			if(value == result)
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

#endif
    }
}