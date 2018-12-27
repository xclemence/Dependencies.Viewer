using System.Threading.Tasks;
using CommonServiceLocator;
using Dependencies.Viewer.Wpf.Controls.ViewModels;

namespace Dependencies.Viewer.Wpf.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow() : this(null)
        {
        }


        public MainWindow(string initialFile)
        {
            InitializeComponent();

             var analyserViewModel = ServiceLocator.Current.GetInstance<AnalyserViewModel>();

            DataContext = analyserViewModel;

            if (initialFile != null)
                Task.Run(async () => await analyserViewModel.InitialiseAsync(initialFile));
        }
    }
}
