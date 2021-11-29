using System.ComponentModel;
using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.ViewModels;
using Dependencies.Viewer.Wpf.IoC;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Dependencies.Viewer.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly Scope scope;

    public MainWindow() : this(null)
    {
    }


    public MainWindow(string? initialFile)
    {

        InitializeComponent();

        scope = AsyncScopedLifestyle.BeginScope(SimpleInjectorConfig.Container);

        var analyserViewModel = scope.GetInstance<AnalyserViewModel>();

        DataContext = analyserViewModel;

        if (initialFile is not null)
            Task.Run(async () => await analyserViewModel.InitialiseAsync(initialFile).ConfigureAwait(false));
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        scope?.Dispose();
    }
}

