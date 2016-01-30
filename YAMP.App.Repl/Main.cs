namespace YAMPConsole
{
    using System;
    using YAMPConsole.Formatter;

    class MainClass
    {
        public static void Main(String[] args)
        {
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
	}
}