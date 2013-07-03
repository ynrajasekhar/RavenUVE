using System.Windows;
using RavenUVE.Views.Utils;

namespace RavenUVE.Views
{
    /// <summary>
    /// Description for ConnectView.
    /// </summary>
    public partial class ConnectView : Window
    {
        /// <summary>
        /// Initializes a new instance of the ConnectView class.
        /// </summary>
        public ConnectView()
        {
            this.DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var request = e.NewValue as IRequestCloseViewModel;

            if (request != null)
            {
                request.RequestClose += (caller, arguments) => this.Close();
            }
        }

    }
}