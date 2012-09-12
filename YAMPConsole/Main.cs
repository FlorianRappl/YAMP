using System;
using System.IO;
using System.Diagnostics;

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
			Console.WriteLine ("Command Line Test Tool of YAMP");			

#if DEBUG
			Console.WriteLine("DEBUG MODE");
			Tests();
#elif BENCHMARKS
			Console.WriteLine("BENCHMARK MODE");
			Benchmarks();
#elif CONSOLE
			Console.WriteLine("CONSOLE MODE");
			Console.WriteLine("Enter your own statements now (exit with empty line):");
			var query = string.Empty;

			YAMP.Parser.AddCustomFunction("G", v => new YAMP.ScalarValue((v as YAMP.ScalarValue).Value * Math.PI) );
			YAMP.Parser.AddCustomConstant("R", 2.53);
			
			while(true)
			{
				Console.Write(">> ");
				query = Console.ReadLine();

                if (query.Equals("exit"))
                    break;
                else if (query.Replace(" ", string.Empty).Equals(string.Empty))
                    continue;
                else
                {
                    try
                    {
                        var parser = YAMP.Parser.Parse(query);
                        Console.WriteLine(parser.Execute());
						Console.WriteLine(parser);
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

#if BENCHMARKS

		static void Benchmarks()
		{
			if(!File.Exists(BMK_FILE))
				GenerateBenchmarks();
			
			YAMP.Parser.Load();
			var a = new MathParser.Parser();
			var b = new MathParserTK_NET.MathParserTK();
			var c = new MathParserNet.Parser();
			
			Console.WriteLine("Starting benchmarks ...");	
			Console.WriteLine("----------");

            //var lines = new string[0];
			// This is Benchmark #1
            //var lines = File.ReadAllLines(BMK_FILE);
			// This is Benchmark #2
            var lines = MakeTenK("2-3*5+7/2-8*2");
			// This is Benchmark #3
            //var lines = MakeTenK("2+3");
			// This is Benchmark #4
            //var lines = MakeTenK("2-(3*5)^2+7/(2-8)*2");
			
			//My own implementation
			// 13795 ms ; 1210 ms ; 348 ms ; 1582 ms
			Benchmark("YAMP", lines, query => YAMP.Parser.Parse(query).Execute());
			
			//http://www.codeproject.com/Articles/11164/Math-Parser
			// 547334 ms ; 3908 ms ; 625 ms ; 15212 ms
			Benchmark("MathParser", lines, query => a.Evaluate(query));
			
			//http://www.codeproject.com/Tips/381509/Math-Parser-NET-Csharp
			// FAILED ; 915 ms ; 193 ms ; 1407 ms
			Benchmark("MathParserTK", lines, query => b.Parse(query, false));
			
			//http://www.codeproject.com/Articles/274093/Math-Parser-NET
			// FAILED ; 15933 ms ; 15483 ms ; 16617 ms
			Benchmark("MathParserNet", lines, query => c.Simplify(query));
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
			Test("|(2,3,1)-(1,3,1)|", 1.0);
			Test("|(2^2,2+3,-2,-2)|", 7.0);
			Test("|4*(2i-5)/3i|", 4.0 * Math.Sqrt(29.0) / 3.0);
			Test("|(3,2,1)*(1;2;3)|", 10.0);
			Test("|(1;2;3)|-|(1,2,3)|", 0.0);
            Test("|(2+3i)^2|", 12.999999999999998);
            Test("|1^(i+5)|", 1.0);
            Test("|1^(i+5)|", 1.0);
            Test("|(5+8i)^(i+1)|", 3.4284942595728127);
            Test("|(2+3i)/(1+8i)|", 0.447213595499958);
            Test("|2*(1,2;1,2)|", 0.0);
            Test("|max(1,5,7,9,8+5i)|", Math.Sqrt(89.0));
            Test("|(2,1;3,5)-(2,1;3,5)'|", 4.0);
			Test("(2,1,0)*(5;2;1)", 12.0);
			Test("det((1;2)*(2,1))", 0.0);
			Test("X=(Y=5)+2", 7.0);
			Test("Y", 5.0);
			Test("(1,2,3;4,5,6;7,8,9)[2,3]", 6.0);
            Test("|cos(1+i)|", 1.2934544550420957);
            Test("|arccos(0.83373002513-0.988897705i)|", 1.4142135618741407);
			Test("|(x=(1,2,3;4,5,6;7,8,9))[:,1]|", Math.Sqrt(66.0));
			
			Console.WriteLine("{0} / {1} tests completed successfully ({2} %)", success, total, success * 100 / total);
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
			return Asset(success == total);
		}
		
		static bool Test(string query, double x, double result)
		{
			var parser = YAMP.Parser.Parse(query);
			Console.WriteLine("Testing: {0} = ...", query);
			var value = parser.Execute(new { x });
			Console.WriteLine("with x = {2};\n{0}\n-> correct: {1}", value, result, x);
			return Asset(value.Equals(result));
		}
		
		static bool Test(string query, double result)
		{
			var parser = YAMP.Parser.Parse(query);
			Console.WriteLine("Testing: {0} = ...", query);
			var value = parser.Execute();
			Console.WriteLine("{0}\n-> correct: {1}", value, result);
			return Asset(value.Equals(result));
		}
		
		static bool Asset(bool given)
		{			
			if(given)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Test successful!");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Test failed!");
			}
			
			Console.ResetColor();
			Console.WriteLine("---");
			total++;
			success += given ? 1 : 0;
			return given;
		}

#endif
	}
}