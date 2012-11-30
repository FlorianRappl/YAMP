using System;
using Caliburn.Micro;

namespace Store.Optimizer.ViewModels
{
    public class VariableViewModel : PropertyChangedBase
    {
        #region FIELDS

        private static int _counter;

        private string _name;
        private double _lowerBound;
        private double _upperBound;

        #endregion

        #region CONSTRUCTORS

        public VariableViewModel()
        {
            Name = string.Format("x_{0}", ++_counter);
            LowerBound = Double.MinValue;
            UpperBound = Double.MaxValue;
        }

        #endregion

        #region PROPERTIES

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public double LowerBound
        {
            get { return _lowerBound; }
            set
            {
                if (value.Equals(_lowerBound)) return;
                _lowerBound = value;
                NotifyOfPropertyChange(() => LowerBound);
            }
        }

        public double UpperBound
        {
            get { return _upperBound; }
            set
            {
                if (value.Equals(_upperBound)) return;
                _upperBound = value;
                NotifyOfPropertyChange(() => UpperBound);
            }
        }

        #endregion
    }
}