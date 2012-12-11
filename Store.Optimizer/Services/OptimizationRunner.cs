using System;
using System.Collections.Generic;
using Cureos.Numerics;
using Store.Optimizer.ViewModels;
using YAMP;

namespace Store.Optimizer.Services
{
    public class OptimizationRunner
    {
        private readonly ParseContext _parseContext;
        private readonly Parser _parser;

        private readonly int _n;
        private readonly string[] _names;
        private readonly double[] _lowerBounds;
        private readonly double[] _upperBounds;

        public OptimizationRunner(string objective, ICollection<VariableViewModel> variables)
        {
            _parseContext = new ParseContext();

            _n = variables.Count;
            _names = new string[_n];
            _lowerBounds = new double[_n];
            _upperBounds = new double[_n];

            var i = 0;
            foreach (var variable in variables)
            {
                Parser.AddVariable(_parseContext, variable.Name, new ScalarValue(variable.Value));

                _names[i] = variable.Name;
                _lowerBounds[i] = variable.HasLowerBound ? variable.LowerBound : Double.MinValue;
                _upperBounds[i] = variable.HasUpperBound ? variable.UpperBound : Double.MaxValue;
                ++i;
            }

            _parser = Parser.Parse(_parseContext, objective);
        }

        public double Run()
        {
            var x = new double[_n];
            var status = Bobyqa.FindMinimum(BobyqaCompute, _n, x, _lowerBounds, _upperBounds);
            return status == BobyqaExitStatus.Normal ? BobyqaCompute(_n, x) : Double.MaxValue;
        }

        private double BobyqaCompute(int n, double[] x)
        {
            for (var i = 0; i < n; ++i)
                _parseContext.AssignVariable(_names[i], new ScalarValue(x[i]));
            return ((ScalarValue)_parser.Execute()).Value;
        }
    }
}