using Caliburn.Micro;
using Store.Optimizer.Services;

namespace Store.Optimizer.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region FIELDS

        private IObservableCollection<VariableViewModel> _variables;
        private string _objective;
        private double _optimalValue;

        #endregion

        #region CONSTRUCTORS

        public ShellViewModel()
        {
            Variables = new BindableCollection<VariableViewModel>(new[] { new VariableViewModel("x"), new VariableViewModel("y")  });
        }

        #endregion

        #region PROPERTIES

        public IObservableCollection<VariableViewModel> Variables
        {
            get { return _variables; }
            private set
            {
                if (Equals(value, _variables)) return;
                _variables = value;
                NotifyOfPropertyChange(() => Variables);
            }
        }

        public string Objective
        {
            get { return _objective; }
            set
            {
                if (value == _objective) return;
                _objective = value;
                NotifyOfPropertyChange(() => Objective);
            }
        }

        public double OptimalValue
        {
            get { return _optimalValue; }
            set
            {
                if (value.Equals(_optimalValue)) return;
                _optimalValue = value;
                NotifyOfPropertyChange(() => OptimalValue);
            }
        }

        #endregion

        #region METHODS

        public void AddVariable()
        {
            Variables.Add(new VariableViewModel());
        }

        public void Optimize()
        {
            var optimizer = new OptimizationRunner(Objective, Variables);
            OptimalValue = optimizer.Run();
        }

        #endregion
    }
}