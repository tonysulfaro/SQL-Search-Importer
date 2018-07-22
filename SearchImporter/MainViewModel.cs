using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
//performance monitoring
using System.Diagnostics;
using System.Threading;
//sql connection stuff
using System.Data;
using System.Data.SqlClient;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace SearchImporter
{
    public class MainViewModel :INotifyPropertyChanged
    {

        private string _host;
        private string _database;
        private string _username;
        private string _password;

        public string Host
        {
            get => _host;
            set
            {
                _host = value;
                OnPropertyChanged();
            }
        }
        public string Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenDirectory { get; set; }
        public ICommand PopulateDatabase { get; set; }

        public MainViewModel()
        {

        }

        //====================================================================================================
        //  INotifiyPropertyChanged event handling.
        //====================================================================================================

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
