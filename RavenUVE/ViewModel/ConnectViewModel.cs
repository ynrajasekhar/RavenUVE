using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RavenUVE.Model;
using RavenUVE.Views.Utils;
using System.Diagnostics.Contracts;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Raven.Client;
using Raven.Client.Document;
using GalaSoft.MvvmLight.Messaging;
using RavenUVE.Views;

namespace RavenUVE.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ConnectViewModel : ViewModelBase, IRequestCloseViewModel
    {

        #region Fields

        /// <summary>
        /// The <see cref="SelectedServerItem" /> property's name.
        /// </summary>
        public const string SelectedServerItemPropertyName = "SelectedServerItem";
        private const string className = "ConnectViewModel";

        private DatabaseConnection selectedServer;
        private IMessenger messenger;
        private readonly IConfiguration configuration;
        private readonly ILog logger;
        
        #endregion

        #region Events

        public event EventHandler RequestClose;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ConnectViewModel class.
        /// </summary>
        public ConnectViewModel(ILog logger, IMessenger messenger, IConfiguration configuration)
        {
            Contract.Requires(null != logger);
            Contract.Requires(null != messenger);
            Contract.Requires(null != configuration);

            this.logger = logger;
            this.messenger = messenger;
            this.configuration = configuration;

            logger.Trace(m => m("{0}: Creating.", className));

            Servers = new ObservableCollection<DatabaseConnection>();

            if (IsInDesignMode)
            {
                Servers.Add(new DatabaseConnection { Name = "ADataBase", Url = "localhost:8080" });
            }
            else
            {
                foreach (var database in configuration.Servers)
                {
                    Servers.Add(database);
                }
            }

            logger.Trace(m => m("{0}: Created.", className));
        }

        #endregion

        #region Properties

        public ObservableCollection<DatabaseConnection> Servers { get; set; }

        public ICommand ConnectCommand { get { return new RelayCommand(ConnectionDialogConnect, ConnectionDialogCanConnect); } }

        public ICommand RemoveCommand { get { return new RelayCommand(Remove, CanRemove); } }

        public ICommand EditCommand { get { return new RelayCommand(Edit, CanEdit); } }

        public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }

        public ICommand AddCommand { get { return new RelayCommand(Add); } }

        /// <summary>
        /// Sets and gets the SelectedServerItem property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DatabaseConnection SelectedServerItem
        {
            get
            {
                return selectedServer;
            }

            set
            {
                if (selectedServer == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedServerItemPropertyName);
                selectedServer = value;
                RaisePropertyChanged(SelectedServerItemPropertyName);
            }
        }

        #endregion

        #region Methods

        private void Cancel()
        {
            logger.Trace(m => m("{0}: Entering Cancel method.", className));
            Close();
            logger.Trace(m => m("{0}: Exiting Cancel method.", className));
        }

        private void Close()
        {
            logger.Trace(m => m("{0}: Entering Close method.", className));
            configuration.Save();
            if (RequestClose != null)
            {
                RequestClose(this, new EventArgs());
            }
            logger.Trace(m => m("{0}: Exiting Close method.", className));
        }

        private bool ConnectionDialogCanConnect()
        {
            logger.Trace(m => m("{0}: Entering CanConnect method.", className));
            return selectedServer != null;
        }

        private void ConnectionDialogConnect()
        {
            logger.Trace(m => m("{0}: Entering Connect method.", className));
            var documentStore = new DocumentStore { Url = selectedServer.Url };
            messenger.Send<GenericMessage<IDocumentStore>>(new GenericMessage<IDocumentStore>(this, documentStore));
            Close();
            logger.Trace(m => m("{0}: Leaving Connect method.", className));
        }

        private void Edit()
        {
            logger.Trace(m => m("{0}: Entering Edit method.", className));
            if (selectedServer != null)
            {
                var copyOfSelectedServer = new DatabaseConnection(selectedServer);
                var editConnectionDialog = new EditServerView(copyOfSelectedServer, logger);
                editConnectionDialog.ShowDialog();
                if (!String.IsNullOrEmpty(copyOfSelectedServer.Name))
                {
                    logger.Debug(m => m("{0}: Setting new value\n\tName: {1}\n\tURL: {2}", 
                        className, 
                        copyOfSelectedServer.Name, 
                        copyOfSelectedServer.Url));
                    selectedServer.Name = copyOfSelectedServer.Name;
                    selectedServer.Url = copyOfSelectedServer.Url;
                }
            }

            logger.Trace(m => m("{0}: Leaving Connect method.", className));
        }

        private bool CanEdit()
        {
            logger.Trace(m => m("{0}: Entering CanEdit method.", className));
            return selectedServer != null;
        }

        private bool CanRemove()
        {
            logger.Trace(m => m("{0}: Entering CanRemove method.", className));
            return selectedServer != null;
        }

        private void Remove()
        {
            logger.Trace(m => m("{0}: Entering Remove method.", className));

            if (selectedServer != null)
            {
                if (!Servers.Remove(selectedServer))
                {
                    logger.Warn(m => m("{0}: Could not remove the selected server item.", className));
                }
                else
                {
                    configuration.Servers.Remove(selectedServer);
                }
            }

            logger.Trace(m => m("{0}: Leaving Remove method.", className));
        }

        private void Add()
        {
            logger.Trace(m => m("{0}: Entering Remove method.", className));

            var dbConnection = new DatabaseConnection();
            var editConnectionDialog = new EditServerView(dbConnection, logger);
            editConnectionDialog.ShowDialog();
            if (!String.IsNullOrEmpty(dbConnection.Name))
            {
                Servers.Add(dbConnection);
                configuration.Servers.Add(dbConnection);
            }

            logger.Trace(m => m("{0}: Leaving Remove method.", className));
        }

        #endregion

    }
}