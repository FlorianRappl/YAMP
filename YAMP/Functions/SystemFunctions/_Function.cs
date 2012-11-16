using System;
using System.Collections.Generic;

namespace YAMP
{
    class _Function : ArgumentFunction
	{
		#region Members

		IHasIndex _variable;
        int[] _targetDimensions;
        IIsIndex[] _indices;
		int N;

		#endregion

		#region ctor

		public _Function(Value variable)
        {
            if (!(variable is IHasIndex))
                throw new OperationNotSupportedException("Index", variable);

            N = 1;
            _variable = variable as IHasIndex;
		}

		#endregion

		#region Methods

		public void Set(Value source)
        {
			if (source is IHasIndex)
			{
				var src = source as IHasIndex;
				var I = new VectorIndex { Entry = 1 };
				var i = 0;

				while (i < N)
				{
					_variable.Set(_indices[i], src.Get(I));
					I.Entry++;
					i++;
				}
			}
			else
			{
				for (var i = 0; i < N; i++)
					_variable.Set(_indices[i], source);
			}
        }

        public Value Get()
        {
            if (N == 1)
                return _variable.Get(_indices[0]);

            var m = _variable.Create(_targetDimensions);

			var I = new VectorIndex { Entry = 1 };
            var i = 0;

            while (i < N)
            {
                m.Set(I, _variable.Get(_indices[i]));
				I.Entry++;
                i++;
            }

            return m as Value;
        }

		[Arguments(0, 1)]
        public Value Function(ArgumentsValue arguments)
		{
			var _sourceDimensions = _variable.Dimensions;

            if (IsLogicalSubscripting(arguments))
            {
                LogicalSubscripting(arguments[1]);
            }
            else if(arguments.Length > 0)
            {
                var args = new List<int[]>();
                var dimensions = new List<int>();
				var argvals = arguments.Values;

				if (argvals.Length > _sourceDimensions.Length)
					throw new ArgumentsException("Index", _sourceDimensions.Length + 1);

				for (var j = 0; j < argvals.Length; j++)
				{
					var arg = argvals[j];
					var dim = _sourceDimensions[j];

					if (argvals.Length == 1)
						dim = _variable.Length;

                    var r = BuildIndices(arg, dim);
                    N = N * r.Length;
                    dimensions.Add(r.Length);
                    args.Add(r);
                }

                _targetDimensions = dimensions.ToArray();
                _indices = new IIsIndex[N];
                var I = CreateIndexArray(args.Count);
                var i = 0;

                while (i < N)
                {
					if (args.Count == 1)
					{
						_indices[i] = new VectorIndex { Entry = args[0][I[0] - 1] };
					}
					else if (args.Count == 2)
					{
						_indices[i] = new MatrixIndex
						{
							Row = args[0][I[0] - 1],
							Column = args[1][I[1] - 1]
						};
					}
					else
					{
						var t = new List<int>();

						for (var j = 0; j < args.Count; j++)
							t.Add(args[j][I[j] - 1]);

						_indices[i] = new MultipleIndex { Indices = t.ToArray() };
					}

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

        int[] BuildIndices(Value arg, int length)
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
				var maxLength = r.All ? length : (int)r.End;

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
            var idx = new List<IIsIndex>();

            for (var i = 1; i <= m.DimensionX; i++)
			{
                for (var j = 1; j <= m.DimensionY; j++)
				{
					if (m[j, i].Value != 0.0)
					{
						idx.Add(new MatrixIndex
						{
							Row = j,
							Column = i
						});
					}
				}
			}

            N = idx.Count;
            _targetDimensions = new int[] { 1, idx.Count };
            _indices = idx.ToArray();
		}

		#endregion
    }
}
