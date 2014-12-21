namespace YAMPConsole.Formatter
{
    using System;

    interface IFormatter : IDisposable
    {
        void AddSection(String name, String topic);

        void AddLink(String link);

        void AddDescription(String description);

        void AddUsage(String usage);

        void AddArgument(String name);

        void AddReturn(String name);

        void AddExample(String code);
    }
}
