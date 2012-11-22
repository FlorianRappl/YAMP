using System;
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
    public class ParseTreeCollection
    {
        #region Members

        List<StatementParseTree> statements;
        QueryContext query;
        int count;

        #endregion

        #region ctor

        public ParseTreeCollection(QueryContext _query)
        {
            count = 1;
            statements = new List<StatementParseTree>();
            query = _query;
        }

        #endregion

        #region Properties

        public QueryContext Query
        {
            get 
            {
                return query; 
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        #endregion

        #region Methods

        internal void AddStatement(string input)
        {
            var line = count++;
            var statement = new StatementParseTree(query, input, line);
            statements.Insert(0, statement);
        }

        internal Value Run(Dictionary<string, Value> values)
        {
            Value value = null;

            foreach (var statement in statements)
            {
                if (statement.HasContent)
                {
                    value = statement.Interpret(values);

                    if (!statement.IsAssignment)
                    {
                        if (value is ArgumentsValue)
                            value = ((ArgumentsValue)value).First();

                        Query.Context.AssignVariable("$", value);
                    }
                }
                else
                    value = null;
            }

            return value;
        }

        public override string ToString()
        {
            var str = new string[statements.Count];

            for (var i = 0; i != statements.Count; i++)
            {
                str[i] = statements[i].ToString();
            }

            return string.Join(Environment.NewLine, str);
        }

        #endregion
    }
}
