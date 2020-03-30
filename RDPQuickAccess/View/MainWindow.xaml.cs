using System.Windows;

namespace RDPQuickAccess.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal ViewModel.MainWindowViewModel ViewModel
        {
            get => DataContext as ViewModel.MainWindowViewModel;
            set => DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
