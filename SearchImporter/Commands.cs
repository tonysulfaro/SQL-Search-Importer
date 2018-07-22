using System;
using System.Windows.Input;

namespace SearchImporter
{
    public class OpenDirectory : ICommand
    {
        private readonly MainViewModel _mainVm;

        public OpenDirectory(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            
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
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            
        }
    }
}
