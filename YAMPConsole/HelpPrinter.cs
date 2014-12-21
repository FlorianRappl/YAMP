namespace YAMPConsole
{
    using Formatter;
    using System;
    using System.Collections.Generic;
    using YAMP;
    using YAMP.Help;

    class HelpPrinter
    {
        readonly IFormatter formatter;
        readonly Documentation doc;

        HelpPrinter(IFormatter formatter)
        {
            this.formatter = formatter;
            doc = Documentation.Create(Parser.PrimaryContext);
        }

        public static void Run(IFormatter formatter, Boolean flush = true)
        {
            var help = new HelpPrinter(formatter);
            help.PrintSections();

            if (flush)
                help.Flush();
        }

        void PrintSections()
        {
            foreach (var section in doc.Sections)
            {
                PrintSection(section);
                PrintUsages(section as HelpFunctionSection);
            }
        }

        void PrintUsages(HelpFunctionSection section)
        {
            if (section == null)
                return;

            foreach (var usage in section.Usages)
                PrintUsage(usage);
        }

        void PrintUsage(HelpFunctionUsage usage)
        {
            if (usage == null)
                return;

            formatter.AddUsage(usage.Usage);
            formatter.AddDescription(usage.Description);

            for (int i = 0; i < usage.ArgumentNames.Count; i++)
            {
                formatter.AddArgument(usage.ArgumentNames[i]);
                formatter.AddDescription(usage.Arguments[i]);
            }

            for (int i = 0; i < usage.Returns.Count; i++)
            {
                formatter.AddReturn((i + 1) + ". entry");
                formatter.AddDescription(usage.Returns[i]);
            }

            PrintExamples(usage.Examples);
        }

        void PrintExamples(List<HelpExample> examples)
        {
            if (examples == null)
                return;

            foreach (var example in examples)
                PrintExample(example);
        }

        void PrintExample(HelpExample example)
        {
            if (example == null)
                return;

            formatter.AddExample(example.Example);
            formatter.AddDescription(example.Description);
        }

        void PrintSection(HelpSection section)
        {
            if (section == null)
                return;

            formatter.AddSection(section.Name, section.Topic);
            formatter.AddDescription(section.Description);

            if (section.HasLink)
                formatter.AddLink(section.Link);
        }

        void Flush()
        {
            formatter.Dispose();
        }
    }
}
