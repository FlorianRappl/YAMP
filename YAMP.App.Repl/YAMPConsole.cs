namespace YAMPConsole
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using YAMP;

    static class YAMPConsole
    {
        public static void Run()
        {
            var query = string.Empty;
            var buffer = new StringBuilder();
            var context = Parser.PrimaryContext;

            Console.WriteLine();
            Console.WriteLine("*****************************************************************");
            Console.WriteLine("*                                                               *");
            Console.WriteLine("*                                                               *");
            Console.WriteLine("* Enter your own statements now (exit with the command 'exit'). *");
            Console.WriteLine("*                                                               *");
            Console.WriteLine("*                                                               *");
            Console.WriteLine("* You are in interactive mode with scripting being enabled.     *");
            Console.WriteLine("*                                                               *");
            Console.WriteLine("*                                                               *");
            Console.WriteLine("*****************************************************************");
            Console.WriteLine();

            Parser.InteractiveMode = true;
            Parser.UseScripting = true;

            Parser.AddCustomFunction("G", v => new ScalarValue(((ScalarValue)v).Re * Math.PI));
            Parser.AddCustomConstant("R", 2.53);

            Parser.OnNotificationReceived += OnNotified;
            Parser.OnUserInputRequired += OnUserPrompt;
            Parser.OnPauseDemanded += OnPauseDemanded;

            while (true)
            {
                buffer.Remove(0, buffer.Length);
                Console.Write(">> ");

                while(true)
                {
                    query = Console.ReadLine();

                    if (query.Length != 0 && query[query.Length - 1] == '\\')
                    {
                        buffer.Append(query.Substring(0, query.Length - 1)).Append("\n");
                        continue;
                    }
                    else
                        buffer.Append(query);

                    break;
                }

                query = buffer.ToString();

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
                            Console.Write(result.Parser);
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
