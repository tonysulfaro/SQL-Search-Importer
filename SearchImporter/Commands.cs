using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SearchImporter
{
    public class Connect : ICommand
    {
        private readonly MainViewModel _mainVm;

        public Connect(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public bool CanExecute(object parameter)
        {
            return !String.IsNullOrEmpty(_mainVm.ServerName) && !String.IsNullOrEmpty(_mainVm.Database) && !String.IsNullOrEmpty(_mainVm.Username) && !String.IsNullOrEmpty(_mainVm.Password);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            _mainVm.VerifySqlCredentials();
        }
    }

    public class OpenDirectory : ICommand
    {
        private readonly MainViewModel _mainVm;

        public OpenDirectory(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public bool CanExecute(object parameter)
        {
            return _mainVm.SqlCredentialsValid;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            _mainVm.OpenDirectory();
        }
    }

    public class PopulateDatabase : ICommand
    {
        private readonly MainViewModel _mainVm;

        public PopulateDatabase(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public bool CanExecute(object parameter)
        {
            return _mainVm.SqlCredentialsValid && _mainVm.FileSelected;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            _mainVm.PopulateDatabase();
        }
    }

    public class SearchByInput : ICommand
    {
        private readonly MainViewModel _mainVm;

        public SearchByInput(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public bool CanExecute(object parameter)
        {
            return !String.IsNullOrEmpty(_mainVm.ServerName) && !String.IsNullOrEmpty(_mainVm.Database) && !String.IsNullOrEmpty(_mainVm.Username) && !String.IsNullOrEmpty(_mainVm.Password);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            //search method
        }
    }
}
