using Caliburn.Micro;

namespace Store.Optimizer.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region FIELDS

        private IObservableCollection<VariableViewModel> _variables;

        #endregion

        #region CONSTRUCTORS

        public ShellViewModel()
        {
            Variables = new BindableCollection<VariableViewModel>();
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

        #endregion

        #region METHODS

        public void AddVariable()
        {
            Variables.Add(new VariableViewModel());
        }

        #endregion
    }
}