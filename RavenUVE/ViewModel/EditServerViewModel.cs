using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Common.Logging;
using GalaSoft.MvvmLight.Command;
using RavenUVE.Model;
using RavenUVE.Views.Utils;

namespace RavenUVE.ViewModel
{
    public class EditServerViewModel : INotifyPropertyChanged, IRequestCloseViewModel
    {

        #region Fields

        private readonly DatabaseConnection dbConnection;
        private readonly ILog logger;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;

        #endregion

        #region Constructor

        public EditServerViewModel(DatabaseConnection dbConnection, ILog logger)
        {
            Contract.Requires(null != dbConnection);
            Contract.Requires(null != logger);

            this.dbConnection = dbConnection;
            this.logger = logger;

        }

        #endregion

        #region Properties

        public ICommand OkCommand { get { return new RelayCommand(Ok, CanOk); } }

        public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }

        public String Name
        {
            get { return dbConnection.Name; }
            set
            {
                if (value == dbConnection.Name)
                {
                    return;
                }

                dbConnection.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public String Url
        {
            get { return dbConnection.Url; }
            set
            {
                if (value == dbConnection.Url)
                {
                    return;
                }

                dbConnection.Url = value;
                OnPropertyChanged("Url");
            }
        }

        #endregion

        #region Methods

        private void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool CanOk()
        {
            return !String.IsNullOrEmpty(Name);
        }

        private void Ok()
        {
            Close();
        }

        private void Cancel()
        {
            Contract.Ensures(dbConnection.Name == String.Empty);
            dbConnection.Name = String.Empty;
            Close();
        }

        private void Close()
        {
            var handler = RequestClose;
            if (null != handler)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion





        
    }
}
