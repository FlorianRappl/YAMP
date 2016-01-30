namespace YAMPConsole
{
    using System;
    using System.Reflection;
    using YAMP;
    using YAMPConsole.Formatter;

    class MainClass
    {
        public static void Main(String[] args)
        {
            LoadCore();
            LoadPhysics();

            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-h":
                    case "--help":
                        HelpPrinter.Run();
                        return;

                    case "-b":
                    case "--benchmark":
                        Benchmarks.Run();
                        return;

                    default:
                        Console.WriteLine("Argument not found!");
                        Environment.Exit(-1);
                        return;
                }
            }

            Repl.Run();
        }

        static void PrintHelp()
        {
            var output = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var formatter = new MarkdownFormatter(output);
            HelpPrinter.Run(formatter);
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