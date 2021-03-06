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
using System.IO;
using System.Threading.Tasks;
// custom types
using SearchImporter.FileTypes;

namespace SearchImporter
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private string _serverName;
        private string _database;
        private string _username;
        private string _password;
        private string _filePath;
        private string _searchText;
        private ObservableCollection<string> _output;
        private FileTypes.File _file;

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
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> OutputLog
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConnectCommand { get; set; }
        public ICommand OpenDirectoryComamnd { get; set; }
        public ICommand PopulateDatabaseCommand { get; set; }
        public ICommand SearchByInputCommand { get; set; }

        //====================================================================================================
        //  Constructor
        //====================================================================================================

        public MainViewModel()
        {
            ConnectCommand = new Connect(this);
            OpenDirectoryComamnd = new OpenDirectory(this);
            PopulateDatabaseCommand = new PopulateDatabase(this);
            SearchByInputCommand = new SearchByInput(this);

            OutputLog = new ObservableCollection<string>();
        }

        //====================================================================================================
        //  Helper methods
        //====================================================================================================

        public async Task<bool> VerifySqlCredentials()
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
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();


                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                OutputLog.Add(DateTime.Now + ": Connected To Server " + ServerName);
                                SqlCredentialsValid = true;
                                return true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //OutputLog.Add(DateTime.Now + ": Database Connection Failed: " + e);
                    }
                }
            }
            return false;
        }

        public bool AuthenticateUser()
        {
            var x = Task.Run(() => VerifySqlCredentials()).Result;

            if (x)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    OutputLog.Add("Connected to host: " + ServerName);
                });
            }
            else
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    OutputLog.Add("Connection to " + ServerName + " Failed");
                });
            }
            SqlCredentialsValid = true;
            return x;
        }


        public void OpenDirectory()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            FilePath = fileDialog.FileName;
            OutputLog.Add(DateTime.Now + ": New Path Added: " + FileSelected);
            FileSelected = true;

            _file = new FileCSV("test.csv", FilePath);
        }

        public void PopulateDatabase()
        {
            string line;
            string connectString = "Server=" + ServerName + ";" +
                    "Initial Catalog=" + Database + ";" +
                    "User Id=" + Username + ";" +
                    "Password = " + Password + ";";

            var tableWindow = new TableCreationWindow();
            tableWindow.ShowDialog();

            using (var connection = new SqlConnection(connectString))
            {
                try
                {
                    connection.Open();

                    //insert into table
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        //command.CommandText = "INSERT INTO Table('field1','field2') VALUES('" + field1 + "','" + field2 + "')";

                        var reader = command.ExecuteNonQuery();
                    }
                }

                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    OutputLog.Add(DateTime.Now + ": Database Connection Failed: " + e.ToString());
                }

            }
            //when reading from table
            //MessageBox.Show(String.Format("{0}", reader["name"]));
        }

        public void showMessage(string message)
        {
            MessageBox.Show(message);
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
