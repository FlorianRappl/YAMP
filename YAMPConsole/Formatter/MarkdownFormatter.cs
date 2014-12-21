namespace YAMPConsole.Formatter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    sealed class MarkdownFormatter : IFormatter
    {
        readonly String outputDirectory;
        readonly List<MarkdownFile> files;
        readonly Dictionary<String, List<String>> topics;
        MarkdownFile current;

        public MarkdownFormatter(String outputDirectory)
        {
            this.outputDirectory = outputDirectory;
            this.files = new List<MarkdownFile>();
            this.topics = new Dictionary<String, List<String>>();
        }

        public void AddSection(String name, String topic)
        {
            current = new MarkdownFile(name);
            current.AddTitle(name);

            if (!topics.ContainsKey(topic))
                topics.Add(topic, new List<String>());

            topics[topic].Add(name);
            files.Add(current);
        }

        public void AddLink(String link)
        {
            current.AddCaption("References");
            current.AddLink(link);
        }

        public void AddDescription(String description)
        {
            current.AddDescription(description);
        }

        public void AddUsage(String usage)
        {
            current.AddHeading(usage);
        }

        public void AddArgument(String name)
        {
            current.AddVariable("Argument", name);
        }

        public void AddReturn(String name)
        {
            current.AddVariable("Returns", name);
        }

        public void AddExample(String code)
        {
            current.AddCaption("Example");
            current.AddCode(code);
        }

        void IDisposable.Dispose()
        {
            var index = new MarkdownFile("Home");
            files.Add(index);
            index.AddTitle("Documentation");

            foreach (var topic in topics)
            {
                index.AddHeading(topic.Key);

                foreach (var link in topic.Value)
                    index.AddLink(link);
            }

            foreach (var file in files)
                file.WriteTo(outputDirectory);

            files.Clear();
        }

        class MarkdownFile
        {
            readonly StringBuilder _content;

            public MarkdownFile(String fileName)
            {
                FileName = fileName;
                _content = new StringBuilder();
            }

            public String FileName
            {
                get;
                private set;
            }

            public void AddTitle(String title)
            {
                _content.Append("# ").AppendLine(title).AppendLine();
            }

            public void AddHeading(String title)
            {
                _content.AppendLine().Append("## ").AppendLine(title).AppendLine();
            }

            public void AddCaption(String title)
            {
                _content.AppendLine().Append("### ").AppendLine(title).AppendLine();
            }

            public void AddDescription(String description)
            {
                _content.AppendLine(description).AppendLine();
            }

            public void AddLink(String link)
            {
                _content.Append("* [").Append(link).Append("](").Append(link).AppendLine(")");
            }

            public void AddVariable(String type, String name)
            {
                _content.Append("**").Append(type).Append("** *").Append(name).AppendLine("*").AppendLine();
            }

            public void AddCode(String code)
            {
                var intend = "".PadLeft(4);
                var lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (var line in lines)
                    _content.Append(intend).AppendLine(line);

                _content.AppendLine();
            }

            public void WriteTo(String outputDirectory)
            {
                var path = Path.Combine(outputDirectory, FileName + ".md");
                File.WriteAllText(path, _content.ToString());
            }
        }
    }
}
