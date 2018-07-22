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
using Microsoft.Win32;

namespace SearchImporter
{
    public class MainViewModel :INotifyPropertyChanged
    {

        private string _serverName;
        private string _database;
        private string _username;
        private string _password;
        private string _filePath;
        private string _searchText;
        private List<string> _output;

        public bool SqlCredentialsValid = true;
        public bool FileSelected;

        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
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
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        public List<string> OutputLog
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenDirectoryComamnd { get; set; }
        public ICommand PopulateDatabaseCommand { get; set; }
        public ICommand ConnectCommand { get; set; }

        //====================================================================================================
        //  Constructor
        //====================================================================================================

        public MainViewModel()
        {
            OpenDirectoryComamnd = new OpenDirectory(this);
            PopulateDatabaseCommand = new PopulateDatabase(this);
            ConnectCommand = new Connect(this);
            OutputLog = new List<string>();
        }

        //====================================================================================================
        //  Helper methods
        //====================================================================================================

        public bool VerifySqlCredentials()
        {
            string connectString = "Server=" + ServerName + ";" +
                    "Initial Catalog=" + Database + ";" +
                    "User Id=" + Username + ";" +
                    "Password = " + Password + ";";

            using (var connection = new SqlConnection(connectString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM sys.tables";

                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                OutputLog.Add(DateTime.Now + ": Connected To Server " + ServerName);
                                return true;
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        OutputLog.Add(DateTime.Now + ": Database Connection Failed: " + e);
                    }
                }
            }


            return false;
        }

        public void OpenDirectory()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            FilePath = fileDialog.FileName;
            OutputLog.Add(DateTime.Now +": New Path Added: " + FileSelected);
        }

        public void PopulateDatabase()
        {
            //MessageBox.Show(String.Format("{0}", reader["name"]));
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
