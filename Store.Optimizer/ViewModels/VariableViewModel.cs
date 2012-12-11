using System;
using Caliburn.Micro;

namespace Store.Optimizer.ViewModels
{
    public class VariableViewModel : PropertyChangedBase
    {
        #region FIELDS

        private static int _counter;

        private string _name;
        private bool _hasLowerBound;
        private bool _hasUpperBound;
        private double _lowerBound;
        private double _upperBound;
        private double _value;

        #endregion

        #region CONSTRUCTORS

        public VariableViewModel(string name, double? lowerBound = null, double? upperBound = null)
        {
            Name = name;
            HasLowerBound = lowerBound.HasValue;
            HasUpperBound = upperBound.HasValue;
            LowerBound = lowerBound ?? 0.0;
            UpperBound = upperBound ?? 0.0;
            Value = 0.0;
        }

        public VariableViewModel() : this(String.Format("x_{0}", ++_counter))
        {
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

        public bool HasLowerBound
        {
            get { return _hasLowerBound; }
            set
            {
                if (value.Equals(_hasLowerBound)) return;
                _hasLowerBound = value;
                NotifyOfPropertyChange(() => HasLowerBound);
            }
        }

        public bool HasUpperBound
        {
            get { return _hasUpperBound; }
            set
            {
                if (value.Equals(_hasUpperBound)) return;
                _hasUpperBound = value;
                NotifyOfPropertyChange(() => HasUpperBound);
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

        public double Value
        {
            get { return _value; }
            set
            {
                if (value.Equals(_value)) return;
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        #endregion
    }
}