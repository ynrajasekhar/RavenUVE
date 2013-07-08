using System.Windows;
using Common.Logging;
using RavenUVE.Model;
using RavenUVE.ViewModel;
using RavenUVE.Views.Utils;

namespace RavenUVE.Views
{
    /// <summary>
    /// Interaction logic for EditServerView.xaml
    /// </summary>
    public partial class EditServerView : Window
    {
        public EditServerView(DatabaseConnection dbConnection, ILog logger)
        {
            this.DataContextChanged += OnDataContextChanged;
            DataContext = new EditServerViewModel(dbConnection, logger);
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
