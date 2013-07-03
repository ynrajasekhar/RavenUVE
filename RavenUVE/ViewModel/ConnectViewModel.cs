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

        private DatabaseConnection selectedServer;
        private readonly String className = "ConnectViewModel";
        private readonly ILog logger;
        
        #endregion

        #region Events

        public event EventHandler RequestClose;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ConnectViewModel class.
        /// </summary>
        public ConnectViewModel(ILog logger)
        {
            Contract.Requires(null != logger);

            this.logger = logger;
            logger.Trace(m => m("{0}: Creating.", className));

            Servers = new ObservableCollection<DatabaseConnection>();
            ConnectCommand = new RelayCommand(Connect, CanConnect);
            RemoveCommand = new RelayCommand(Remove, CanRemove);
            EditCommand = new RelayCommand(Edit, CanEdit);
            CancelCommand = new RelayCommand(Cancel);
            AddCommand = new RelayCommand(Add);


            if (IsInDesignMode)
            {
                Servers.Add(new DatabaseConnection { Name = "ADataBase", Url = "localhost:8080" });
            }
            else
            {
                Servers.Add(new DatabaseConnection { Name = "ADataBase", Url = "localhost:8080" });
                Servers.Add(new DatabaseConnection { Name = "Nicer Database", Url = "localhost:8080" });
            }

            logger.Trace(m => m("{0}: Created.", className));
        }

        #endregion

        #region Properties

        public ObservableCollection<DatabaseConnection> Servers { get; set; }

        public ICommand ConnectCommand { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        public ICommand EditCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand AddCommand { get; private set; }

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
            if (RequestClose != null)
            {
                RequestClose(this, new EventArgs());
            }
            logger.Trace(m => m("{0}: Exiting Close method.", className));
        }

        private bool CanConnect()
        {
            return selectedServer != null;
        }

        private void Connect()
        {
            var simpleIoc = ServiceLocator.Current as ISimpleIoc;
            if (simpleIoc != null)
            {
                if (simpleIoc.IsRegistered<IDocumentStore>())
                {
                    simpleIoc.Unregister<IDocumentStore>();
                }

                simpleIoc.Register<IDocumentStore>(() => new DocumentStore { Url = selectedServer.Url });
                Close();
            }
        }

        private void Edit()
        {
            throw new System.NotImplementedException();
        }

        private bool CanEdit()
        {
            return selectedServer != null;
        }

        private bool CanRemove()
        {
            return selectedServer != null;
        }

        private void Remove()
        {
            if (selectedServer != null)
            {
                if (!Servers.Remove(selectedServer))
                {
                    logger.Warn(m => m("{0}: Could not remove the selected server item.", className));
                }
            }
        }

        private void Add()
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}