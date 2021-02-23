using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.Services;
using Dependencies.Viewer.Wpf.Controls.ViewModels.Settings;
using Dependencies.Viewer.Wpf.Controls.Views;
using Dependencies.Viewer.Wpf.Controls.Views.About;
using Dragablz;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AnalyserViewModel : ObservableObject
    {
        private const string OpenFileFilter =
            "Software (*.exe)|*.exe|" +
            "Assembly (*.dll)|*.dll|" +
            "All Files|*.*";

        private bool isDragFile;
        private IInterTabClient interTabClient;
        private readonly AnalyserProvider analyserProvider;
        private readonly IServiceFactory serviceFactory;
        private readonly AppLoggerService<AnalyserViewModel> logger;
        private readonly IList<IImportAssembly> importServices;
        private readonly IList<IExportAssembly> exportServices;

        private AnalyseResultViewModel? selectedItem;
        private bool isSettingsOpen;

        [SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Main view model resolve by ioc")]
        public AnalyserViewModel(AnalyserProvider analyserProvider,
                                 MainBusyService busyService,
                                 IServiceFactory serviceFactory,
                                 IInterTabClient interTabClient,
                                 SettingsViewModel settingsViewModel,
                                 IEnumerable<IImportAssembly> importServices,
                                 IEnumerable<IExportAssembly> exportServices,
                                 AppLoggerService<AnalyserViewModel> logger)
        {
            this.analyserProvider = analyserProvider;
            BusyService = busyService;
            this.serviceFactory = serviceFactory;
            this.interTabClient = interTabClient;
            SettingsViewModel = settingsViewModel;
            this.logger = logger;
            this.exportServices = exportServices.ToList();
            this.importServices = importServices.ToList();

            Title = $"Dependencies Viewer {typeof(AnalyserViewModel).Assembly.GetName().Version?.ToString(3)}";

            SettingsCommand = new Command(() => IsSettingsOpen = true);
            CloseCommand = new Command(() => Application.Current.Shutdown());
            AboutCommand = new Command(async () => await OpenAboutAsync());

            OpenFileCommand = new Command(async () => await BusyService.RunActionAsync(OpenFileAsync).ConfigureAwait(false), () => !BusyService.IsBusy);
            OnDragOverCommand = new Command<DragEventArgs>(OnDragOver);
            OnDropCommand = new Command<DragEventArgs>(async (x) => await BusyService.RunActionAsync(async () => await OnDrop(x).ConfigureAwait(false)).ConfigureAwait(false), _ => !BusyService.IsBusy);

            OnDragEnterCommand = new Command<DragEventArgs>((x) => IsDragFile = true, CanDrag);
            OnDragLeaveCommand = new Command<DragEventArgs>((x) => IsDragFile = false, CanDrag);

            ExportCommands = GenerateExportCommand();
            ImportCommands= GenerateImportCommand();

            CloseResultCommand = new Command<AnalyseResultViewModel>(CloseResult);

        }

        public string Title { get; }
        
        public ISnackbarMessageQueue MessageQueue => logger.MessageQueue;

        public ObservableCollection<AnalyseResultViewModel> AnalyseDetailsViewModels { get; } = new ObservableCollection<AnalyseResultViewModel>();

        public AnalyseResultViewModel? SelectedItem
        {
            get => selectedItem;
            set => Set(ref selectedItem, value);
        }

        public bool IsSettingsOpen
        {
            get => isSettingsOpen;
            set => Set(ref isSettingsOpen, value);
        }

        public IInterTabClient InterTabClient
        {
            get => interTabClient;
            private set => Set(ref interTabClient, value);
        }
        public MainBusyService BusyService { get; }
        public SettingsViewModel SettingsViewModel { get; }

        public ICommand OpenFileCommand { get; }
        public ICommand SettingsCommand { get; }

        public ICommand AboutCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand OnDragOverCommand { get; }
        public ICommand OnDropCommand { get; }
        public ICommand OnDragEnterCommand { get; }
        public ICommand OnDragLeaveCommand { get; }
        public ICommand CloseResultCommand { get; }

        public IList<ExchangeCommand> ExportCommands { get; }
        public IList<ExchangeCommand> ImportCommands { get; }

        public Func<DragEventArgs, bool> CanDragFunc => (x) => CanDrag(x);

        public bool IsDragFile
        {
            get => isDragFile;
            set => Set(ref isDragFile, value);
        }

        private async Task OpenFileAsync()
        {
            var openFileDialog = new OpenFileDialog { Title = "Open File", Filter = OpenFileFilter, Multiselect = false };
            if (openFileDialog.ShowDialog() != true) return;

            await AnalyseAsync(openFileDialog.FileName).ConfigureAwait(false);
        }

        private async Task OnDrop(DragEventArgs e)
        {
            e.Handled = true;
            IsDragFile = false;

            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            if (filenames.Length == 0) return;

            await AnalyseAsync(filenames[0]).ConfigureAwait(false);
        }

        public async Task InitialiseAsync(string? filePath = null)
        {
            if (filePath == null)
                return;

            await BusyService.RunActionAsync(async () => await AnalyseAsync(filePath).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private async Task AnalyseAsync(string filePath)
        {
            var analyser = analyserProvider.CurrentAnalyserFactory?.GetAnalyser();

            if (analyser is null) return;

            var (assembly, links) = await analyser.AnalyseAsync(filePath).ConfigureAwait(false);

            AddAssemblyResult(assembly.ToAssemblyModel(links.Values));
        }

        internal void AddAssemblyResult(AssemblyModel assembly)
        {
            var newViewModel = serviceFactory.Create<AnalyseResultViewModel>();
            newViewModel.AssemblyResult = assembly;

            new Action(() =>
            {
                AnalyseDetailsViewModels.Add(newViewModel);
                SelectedItem = newViewModel;
            }).InvokeUiThread();
        }

        private IList<ExchangeCommand> GenerateExportCommand()
        {
            return exportServices.Select(x => new ExchangeCommand
            (
                x.Name,
                new Command(async () => await BusyService.RunActionAsync(async () => await ExportAsync(x).ConfigureAwait(false)).ConfigureAwait(false), () => x.IsReady && !BusyService.IsBusy && SelectedItem?.AssemblyResult is not null)
            )).ToList();
        }

        private IList<ExchangeCommand> GenerateImportCommand()
        {
            return importServices.Select(x => new ExchangeCommand
            (
                x.Name,
                new Command(async () => await BusyService.RunActionAsync(async () => await ImportAsync(x).ConfigureAwait(false)).ConfigureAwait(false), () => x.IsReady && !BusyService.IsBusy)
            )).ToList();
        }

        private async Task ExportAsync(IExportAssembly exportAssembly)
        {
            if (selectedItem?.AssemblyResult is null) return;

            var (assembly, dependencies) = selectedItem.AssemblyResult.ToExchangeModel();

            await exportAssembly.ExportAsync(assembly, dependencies, CreateExchangeView).ConfigureAwait(false);
        }

        private async Task ImportAsync(IImportAssembly importAssembly)
        {
            var result = await importAssembly.ImportAsync(CreateExchangeView).ConfigureAwait(false);

            if (result == default)
                return;

            AddAssemblyResult(result.Assembly.ToAssemblyModel(result.Dependencies));
        }

        private async Task<T> CreateExchangeView<T>(UserControl view, IExchangeViewModel<T> viewModel)
        {
            var exchangeView = new ExchangeView();
            var exchangeViewModel = new ExchangeViewModel<T>((x) => CloseExchangeDialog(x), viewModel, logger.Logger);

            exchangeView.DataContext = exchangeViewModel;
            exchangeView.Control.Content = view;
            view.DataContext = viewModel;

            var result = await DialogHost.Show(exchangeView).ConfigureAwait(false);

            if (result is null)
                return default(T)!;

            return (T)result;
        }

        private static void CloseExchangeDialog<T>(T x) => new Action(() => DialogHost.CloseDialogCommand.Execute(x, null)).InvokeUiThread();

        private void OnDragOver(DragEventArgs e)
        {
            e.Effects = CanDrag(e) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private bool CanDrag(DragEventArgs? e) => e is not null && !BusyService.IsBusy && e.Data.GetDataPresent(DataFormats.FileDrop);

        private void CloseResult(AnalyseResultViewModel x) => TabablzControl.CloseItem(x);

        private async Task OpenAboutAsync()
        {
            var view = new AboutView
            {
                DataContext = serviceFactory.Create<AboutViewModel>()
            };

            await DialogHost.Show(view).ConfigureAwait(false);
        }

    }
}