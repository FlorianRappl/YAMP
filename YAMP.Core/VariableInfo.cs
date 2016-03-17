namespace YAMP
{
    using System;

    public class VariableInfo
    {
        private readonly String _name;
        private readonly Boolean _assigned;
        private readonly ParseContext _context;

        public VariableInfo(String name, Boolean assigned, ParseContext context)
        {
            _name = name;
            _assigned = assigned;
            _context = context;
        }

        public Boolean IsAssigned
        {
            get { return _assigned; }
        }

        public ParseContext Context
        {
            get { return _context; }
        }

        public String Name
        {
            get { return _name; }
        }
    }
}
