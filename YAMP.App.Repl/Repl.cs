namespace YAMPConsole
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using YAMP;
    using YAMP.Physics;

    static class Repl
    {
        public static void Run()
        {
            var query = String.Empty;
            var buffer = new StringBuilder();
            var parser = new Parser();
            parser.LoadPlugin(typeof(UnitValue).Assembly);
            var exit = false;

            parser.InteractiveMode = true;
            parser.UseScripting = true;

            parser.AddCustomFunction("G", v => new ScalarValue(((ScalarValue)v).Re * Math.PI));
            parser.AddCustomConstant("R", 2.53);

            parser.NotificationReceived += OnNotified;
            parser.UserInputRequired += OnUserPrompt;
            parser.PauseDemanded += OnPauseDemanded;

            while (!exit)
            {
                buffer.Remove(0, buffer.Length);
                Console.Write("> ");

                while(true)
                {
                    query = Console.ReadLine();

                    if (query.Length == 0 || query[query.Length - 1] != '\\')
                    {
                        buffer.Append(query);
                        break;
                    }

                    buffer.Append(query.Substring(0, query.Length - 1)).Append("\n");
                }

                query = buffer.ToString();
                exit = query.Equals("exit");

                if (!exit)
                {
                    try
                    {
                        var value = parser.Evaluate(query);

                        if (value != null)
                        {
                            var result = value.ToString(parser.Context);
                            Console.WriteLine(result);
                        }
                    }
                    catch (YAMPParseException parseex)
                    {
                        Console.WriteLine(parseex.Message);
                        Console.WriteLine("---");
                        Console.Write(parseex.ToString());
                        Console.WriteLine("---");
                    }
                    catch (YAMPRuntimeException runex)
                    {
                        Console.WriteLine("An exception during runtime occured:");
                        Console.WriteLine(runex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        static void OnPauseDemanded(object sender, PauseEventArgs e)
        {
            Console.WriteLine("Press any key to continue . . . ");
            Console.ReadKey(true);
            e.Continue();
        }

        static void OnUserPrompt(object sender, UserInputEventArgs e)
        {
            Console.WriteLine();
            Console.Write(e.Message);
            Console.Write(": ");
            e.Continue(Console.ReadLine());
        }

        static void OnNotified(object sender, NotificationEventArgs e)
        {
            Console.WriteLine(e.Message);
            Trace.WriteLine(e.Message);
        }
    }
}
