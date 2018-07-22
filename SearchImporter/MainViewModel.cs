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

        private string _serverName;
        private string _database;
        private string _username;
        private string _password;
        private string _filePath;

        public bool SqlCredentialsValid;
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
        public List<string> Output { get; set; }

        public ICommand OpenDirectoryComamnd { get; set; }
        public ICommand PopulateDatabaseCommand { get; set; }
        public ICommand ConnectCommand { get; set; }

        public MainViewModel()
        {
            OpenDirectoryComamnd = new OpenDirectory(this);
            PopulateDatabaseCommand = new PopulateDatabase(this);
            ConnectCommand = new Connect(this);
        }

        //====================================================================================================
        //  Helper methods
        //====================================================================================================

        public bool VerifySqlCredentials()
        {
            using(var connection = new SqlConnection())
            {
      
                string connectString = "Server="+ServerName+";" +
                    "+Database="+Database+";"+
                    "+User Id="+Username+";"+
                    "+Password = "+Password+";";
                Console.Write(connectString);
                connection.ConnectionString = connectString;

                connection.Open();

                using(var command = new SqlCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Logins";

                    var reader = command.ExecuteReader();

                    try
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(String.Format("{0}, {1}",
                            reader["tPatCulIntPatIDPk"], reader["tPatSFirstname"]));
                        }
                    }
                    catch(Exception e)
                    {
                        Output.Add(e.ToString());
                    }
                }
            }


            return false;
        }

        public void OpenDirectory()
        {

        }

        public void PopulateDatabase()
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
