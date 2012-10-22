using System;
using System.Collections.Generic;

namespace YAMP
{
    class _Function : ArgumentFunction
    {
        ISupportsIndex _variable;
        int[] _dimensions;
        int[][] _indices;
        int N;

        public _Function(Value variable) : base(1)
        {
            if (!(variable is ISupportsIndex))
                throw new OperationNotSupportedException("Index", variable);

            N = 1;
            _variable = variable as ISupportsIndex;
        }

        public void Set(Value source)
        {
            if (source is ISupportsIndex)
            {
                var src = source as ISupportsIndex;
                var I = new int[] { 1 };
                var i = 0;

                while (i < N)
                {
                    _variable.Set(_indices[i], src.Get(I));
                    I[0]++;
                    i++;
                }
            }
            else if (_indices.Length == 1)
                _variable.Set(_indices[0], source);
            else
                throw new OperationNotSupportedException("Index", source);
        }

        public Value Get()
        {
            if (N == 1)
                return _variable.Get(_indices[0]);

            var m = _variable.Create(_dimensions);

            var I = new int[] { 1 };
            var i = 0;

            while (i < N)
            {
                m.Set(I, _variable.Get(_indices[i]));
                I[0]++;
                i++;
            }

            return m as Value;
        }

        public Value Function(ArgumentsValue arguments)
        {
            if (IsLogicalSubscripting(arguments))
            {
                LogicalSubscripting(arguments[1]);
            }
            else if(arguments.Length > 0)
            {
                var args = new List<int[]>();
                var i = 0;
                var dimensions = new List<int>();

                foreach (var arg in arguments.Values)
                {
                    var r = BuildIndices(arg, i++);
                    N = N * r.Length;
                    dimensions.Add(r.Length);
                    args.Add(r);
                }

                _dimensions = dimensions.ToArray();
                _indices = new int[N][];
                var t = new List<int>();
                var I = CreateIndexArray(args.Count);
                i = 0;

                while (i < N)
                {
                    t.Clear();

                    for (var j = 0; j < args.Count; j++)
                        t.Add(args[j][I[j] - 1]);

                    int k = 0;

                    do
                    {
                        I[k]++;

                        if (I[k] <= args[k].Length)
                            break;

                        I[k] = 1;
                        k++;
                    }
                    while (k < args.Count);

                    _indices[i] = t.ToArray();
                    i++;
                }
            }

            return arguments;
        }

        int[] CreateIndexArray(int length)
        {
            var array = new int[length];

            for (var i = 0; i < length; i++)
                array[i] = 1;

            return array;
        }

        int[] BuildIndices(Value arg, int dim)
        {
            if (!(arg is NumericValue))
                throw new OperationNotSupportedException("Index", arg);

            var z = new List<int>();

            if (arg is ScalarValue)
                z.Add((arg as ScalarValue).IntValue);
            else if (arg is RangeValue)
            {
                var r = arg as RangeValue;
                var step = (int)r.Step;
                var maxLength = r.All ? _variable.GetDimension(dim) : (int)r.End;

                for(var j = (int)r.Start; j <= maxLength; j += step)
                    z.Add(j);
            }
            else if (arg is MatrixValue)
            {
                var m = arg as MatrixValue;

                for(var j = 1; j <= m.Length; j++)
                    z.Add(m[j].IntValue);
            }

            return z.ToArray();
        }

        bool IsLogicalSubscripting(ArgumentsValue arguments)
        {
            if(_variable is MatrixValue)
            {
                if (arguments.Length != 1)
                    return false;

                if (arguments[1] is MatrixValue)
                {
                    var n = _variable as MatrixValue;
                    var m = arguments[1] as MatrixValue;
                    return m.DimensionX == n.DimensionX && m.DimensionY == n.DimensionY;
                }
            }

            return false;
        }

        void LogicalSubscripting(Value v)
        {
            var m = v as MatrixValue;
            var idx = new List<int[]>();

            for (var i = 1; i <= m.DimensionX; i++)
                for (var j = 1; j <= m.DimensionY; j++)
                    if (m[j, i].Value != 0.0)
                        idx.Add(new int[] { j, i });

            N = idx.Count;
            _dimensions = new int[] { 1, idx.Count };
            _indices = idx.ToArray();
        }
    }
}
