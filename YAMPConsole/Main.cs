namespace YAMPConsole
{
    using System;
    using System.Reflection;
    using YAMP;
    using Formatter;

    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Command Line Test Tool of YAMP");
            Console.WriteLine();
            Console.WriteLine("--------------------------");
            Console.WriteLine();
            Console.WriteLine(" YAMP VERSION " + YAMP.Parser.Version);
            Console.WriteLine();
            Console.WriteLine("--------------------------");
            Console.WriteLine();

            LoadCore();
            LoadPhysics();
#if DEBUG
            YAMPTests.Run();
#elif BENCHMARKS
            Benchmarks.Run();
#elif HELP
            var output = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var formatter = new MarkdownFormatter(output);
            HelpPrinter.Run(formatter);
#elif CONSOLE
			YAMPConsole.Run();
#endif
        }

        static void LoadCore()
        {
            Console.Write("Loading core library ... ");
            Parser.Load();
            Console.WriteLine("loaded !");
        }

		static void LoadPhysics()
		{
            Console.Write("Loading physics library ... ");
		    Parser.LoadPlugin(Assembly.LoadFile(Environment.CurrentDirectory + "\\YAMP.Physics.dll"));
            Console.WriteLine("loaded !");
		}
	}
}